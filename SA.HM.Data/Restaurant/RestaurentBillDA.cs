using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurentBillDA : BaseService
    {
        public int SaveRestaurantBill(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> restaurentBillDetailBOList, List<GuestBillPaymentBO> guestPaymentDetailList, List<RestaurantBillBO> categoryWisePercentageDiscountBOList, string categoryIdList, bool isBillWillSettle, bool isAmountWillDistribution, out int billID)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            //dbSmartAspects.AddInParameter(command, "@BillPaymentDate", DbType.DateTime, restaurentBillBO.BillPaymentDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                            //dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, restaurentBillBO.PayMode);
                            //dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, restaurentBillBO.BankId);
                            //dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, restaurentBillBO.CardType);
                            //dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, restaurentBillBO.CardNumber);
                            //if (restaurentBillBO.PayMode == "Card")
                            //{
                            //    dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, restaurentBillBO.ExpireDate);
                            //}
                            //dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, restaurentBillBO.CardHolderName);
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.String, "0");
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);

                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            billID = Convert.ToInt32(command.Parameters["@BillId"].Value);

                        }

                        //Save Kot Information ------------------------------------
                        if (restaurentBillDetailBOList != null && status > 0)
                        {
                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                row.BillId = billID;
                                using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                    if (restaurentBillBO.BearerId > 0)
                                    {
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                    }

                                    dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);

                                    //free = dbSmartAspects.ExecuteNonQuery(commandBillDetails) > 0 ? true : false;
                                    //tmpPKId = Convert.ToInt32(commandBillDetails.Parameters["@DetailId"].Value);
                                }
                            }
                        }

                        if (status > 0 && categoryWisePercentageDiscountBOList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (RestaurantBillBO percentageDiscountBO in categoryWisePercentageDiscountBOList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && guestPaymentDetailList != null)
                        {
                            foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            {
                                if (guestBillPaymentBO.PaymentMode != "Other Room")
                                {
                                    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        //dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);
                                        dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                        //countGuestBillPaymentList += 1;
                                    }
                                }

                                if (guestBillPaymentBO.PaymentMode == "Other Room")
                                {
                                    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                                    {
                                        if (guestBillPaymentBO.PaidServiceId == 0)
                                        {
                                            commandGuestBillPayment.Parameters.Clear();
                                            guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;

                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                            dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                            //dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);
                                            dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);

                                            ////countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);
                                            //countGuestBillPaymentList += 1;
                                        }
                                        else
                                        {
                                            //countGuestBillPaymentList += 1;
                                        }
                                    }
                                }
                            }

                            //using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            //{
                            //    foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            //    {
                            //        if (guestBillPaymentBO.PaidServiceId == 0)
                            //        {
                            //            commandGuestBillPayment.Parameters.Clear();

                            //            guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                            //            guestBillPaymentBO.PaymentDate = DateTime.Now;

                            //            int companyId = 0;
                            //            if (guestBillPaymentBO.CompanyId != null)
                            //            {
                            //                companyId = guestBillPaymentBO.CompanyId;
                            //            }

                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                            //            if (guestBillPaymentBO.PaymentMode == "Card")
                            //            {
                            //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                            //            }

                            //            //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                            //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                            //            dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                            //            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            //            //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                            //            status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                            //        }
                            //    }
                            //}
                        }

                        if (isAmountWillDistribution)
                        {
                            if (status > 0)
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, billID);

                                    if (!string.IsNullOrEmpty(categoryIdList))
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList.Trim());
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                    }

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }
                        }

                        if (isAmountWillDistribution && isBillWillSettle)
                        {
                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billID);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }

                            }
                        }

                        if (status > 0)
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
                        billID = 0;
                        throw ex;

                    }
                }
            }

            return billID;
        }
        public int SaveRestaurantBillForHoldUp(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> restaurentBillDetailBOList, List<GuestBillPaymentBO> guestPaymentDetailList, List<RestaurantBillBO> categoryWisePercentageDiscountBOList, out int billID)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            //dbSmartAspects.AddInParameter(command, "@BillPaymentDate", DbType.DateTime, restaurentBillBO.BillPaymentDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                            //dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, restaurentBillBO.PayMode);
                            //dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, restaurentBillBO.BankId);
                            //dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, restaurentBillBO.CardType);
                            //dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, restaurentBillBO.CardNumber);
                            //if (restaurentBillBO.PayMode == "Card")
                            //{
                            //    dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, restaurentBillBO.ExpireDate);
                            //}
                            //dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, restaurentBillBO.CardHolderName);
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.String, "0");
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                            //dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, restaurentBillBO.DealId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            billID = Convert.ToInt32(command.Parameters["@BillId"].Value);

                        }

                        //Save Kot Information ------------------------------------
                        if (restaurentBillDetailBOList != null && status > 0)
                        {
                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                row.BillId = billID;
                                using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetailForHoldUp_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                    dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && categoryWisePercentageDiscountBOList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (RestaurantBillBO percentageDiscountBO in categoryWisePercentageDiscountBOList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && guestPaymentDetailList != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                            billID = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        billID = 0;
                        throw ex;
                    }
                }
            }

            return billID;
        }
        public bool UpdateRestaurantBillForPayfirstPOS(RestaurantBillBO restaurentBillBO, List<GuestBillPaymentBO> paymentAdded, bool isBillWillSettle)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 1);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);


                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }

                        if (status > 0 && paymentAdded.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }
                        if (isBillWillSettle)
                        {

                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }

                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateRestaurantBill(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> restaurentBillDetailBOList, List<GuestBillPaymentBO> paymentAdded, List<GuestBillPaymentBO> paymentUpdate, List<GuestBillPaymentBO> paymentDelete, List<RestaurantBillBO> categoryWisePercentageDiscountBOList, string categoryIdList, bool isBillWillSettle)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 0);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);


                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);


                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);
                            
                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }

                        if (status > 0 && categoryWisePercentageDiscountBOList.Count > 0)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (RestaurantBillBO percentageDiscountBO in categoryWisePercentageDiscountBOList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentAdded.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentUpdate.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentUpdate)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentDelete.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (GuestBillPaymentBO pd in paymentDelete)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.PaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }

                        if (restaurentBillDetailBOList.Count > 0 && status > 0)
                        {
                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillDetail_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && restaurentBillDetailBOList.Count > 0)
                        {
                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                using (DbCommand commandUpdateReceipeCost = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotBillItemRecipeCost_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@KotId", DbType.Int32, row.KotId);
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandUpdateReceipeCost, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, false);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (isBillWillSettle)
                        {
                            if (status > 0)
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                    if (!string.IsNullOrEmpty(categoryIdList.Trim()))
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                    }

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }

                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateRestaurantBillForHoldUp(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> restaurentBillDetailBOList, List<GuestBillPaymentBO> paymentAdded, List<GuestBillPaymentBO> paymentUpdate, List<GuestBillPaymentBO> paymentDelete, List<RestaurantBillBO> categoryWisePercentageDiscountBOList)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 0);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);


                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);


                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }

                        if (status > 0 && categoryWisePercentageDiscountBOList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (RestaurantBillBO percentageDiscountBO in categoryWisePercentageDiscountBOList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentAdded != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentUpdate != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentUpdate)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentDelete != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (GuestBillPaymentBO pd in paymentDelete)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.PaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                using (DbCommand commandUpdateReceipeCost = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotBillItemRecipeCost_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@KotId", DbType.Int32, row.KotId);
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandUpdateReceipeCost, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, true);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool DistributionRestaurantBill(int billId, string categoryIdList, string discountType, decimal discountAmount, int costCenterId)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, billId);

                            if (!string.IsNullOrEmpty(categoryIdList))
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList.Trim());
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                            }

                            dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, discountType);
                            dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, discountAmount);
                            dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, costCenterId);

                            status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            status = 0;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transction.Rollback();
                        throw ex;

                    }
                }
            }

            return (status > 0 ? true : false);
        }
        public int UpdateRestaurantBillPayment(int billId, List<GuestBillPaymentBO> guestPaymentDetailList, int createdBy)
        {
            int status = 0;
            int countGuestBillPaymentList = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                if (guestPaymentDetailList != null)
                {
                    foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                    {
                        if (guestBillPaymentBO.PaymentMode != "Other Room")
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                commandGuestBillPayment.Parameters.Clear();
                                guestBillPaymentBO.CreatedBy = createdBy;
                                guestBillPaymentBO.PaymentDate = DateTime.Now;

                                int companyId = 0;
                                if (guestBillPaymentBO.CompanyId != null)
                                {
                                    companyId = guestBillPaymentBO.CompanyId;
                                }

                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, string.Empty);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                if (guestBillPaymentBO.PaymentMode == "Card")
                                {
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                }

                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);
                                countGuestBillPaymentList += 1;
                            }
                        }

                        if (guestBillPaymentBO.PaymentMode == "Other Room")
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                            {
                                if (guestBillPaymentBO.PaidServiceId == 0)
                                {
                                    commandGuestBillPayment.Parameters.Clear();
                                    guestBillPaymentBO.CreatedBy = createdBy;

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                    dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                    dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);
                                    //countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);
                                    countGuestBillPaymentList += 1;
                                }
                                else
                                {
                                    countGuestBillPaymentList += 1;
                                }
                            }
                        }
                    }

                    //if (guestPaymentDetailList.Count == countGuestBillPaymentList)
                    //{
                    //    status = 1;
                    //}
                }
            }

            if (guestPaymentDetailList.Count == countGuestBillPaymentList)
            {
                status = 1;
            }

            return status;
            /*
            int status = 0;
            int countGuestBillPaymentList = 0;
            int countGuestExtraBillPaymentList = 0;
            Boolean isPaidServiceExist = false;

            foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
            {
                if (guestBillPaymentBO.PaidServiceId > 0)
                {
                    isPaidServiceExist = true;
                }
            }

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                if (guestPaymentDetailList != null)
                {
                    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                    {
                        foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                        {
                            if (isPaidServiceExist == false)
                            {
                                if (guestBillPaymentBO.PaidServiceId == 0)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    guestBillPaymentBO.CreatedBy = createdBy;
                                    guestBillPaymentBO.PaymentDate = DateTime.Now;

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, guestBillPaymentBO.CompanyId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                    if (guestBillPaymentBO.PaymentMode == "Card")
                                    {
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                    }

                                    //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                    dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                    //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                    //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                    countGuestBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);

                                    //int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                                }
                                else
                                {
                                    countGuestBillPaymentList += 1;
                                }
                            }
                            else
                            {
                                countGuestBillPaymentList += 1;
                            }
                        }

                    }

                    if (isPaidServiceExist)
                    {
                        using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                        {
                            foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            {
                                if (guestBillPaymentBO.PaymentMode == "Other Room")
                                {
                                    if (isPaidServiceExist == true)
                                    {
                                        if (guestBillPaymentBO.PaidServiceId == 0)
                                        {
                                            commandGuestBillPayment.Parameters.Clear();

                                            guestBillPaymentBO.CreatedBy = createdBy;

                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billId);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                            dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                            //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                            //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                            countGuestExtraBillPaymentList += dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment);

                                            //int tmpApprovedId = Convert.ToInt32(commandRoomGuestList.Parameters["@ApprovedId"].Value);
                                        }
                                        else
                                        {
                                            countGuestExtraBillPaymentList += 1;
                                        }
                                    }
                                    else
                                    {
                                        countGuestExtraBillPaymentList += 1;
                                    }
                                }
                            }

                        }

                        if (guestPaymentDetailList.Count == countGuestExtraBillPaymentList)
                        {
                            status = 1;
                        }
                    }
                }

                if (guestPaymentDetailList.Count == countGuestBillPaymentList)
                {
                    status = 1;
                }
            }

            return status;
             */
        }
        public int SaveUpdateRestaurantBill(DateTime restaurantBillDate, List<GuestBillPaymentBO> guestPaymentDetailList, string deletedPaymentIds, string deletedTransferedPaymentIds, string deletedAchievementPaymentIds, int billID)
        {
            int status = 0;
            bool isWorkAny = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    status = 1;

                    if (guestPaymentDetailList != null)
                    {
                        using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        {
                            foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                            {
                                if (guestBillPaymentBO.PaymentMode != "Other Room")
                                {
                                    if (guestBillPaymentBO.ServiceBillId == null) // guestBillPaymentBO.PaidServiceId == 0
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, restaurantBillDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, string.Empty);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);

                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            if (guestBillPaymentBO.ExpireDate != null)
                                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        if (status > 0)
                                        {
                                            status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                            isWorkAny = true;
                                        }
                                    }
                                }

                                if (guestBillPaymentBO.PaymentMode == "Other Room" && status > 0)
                                {
                                    using (DbCommand commandGuestBillPayment2 = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                                    {
                                        if (guestBillPaymentBO.ServiceBillId == null)
                                        {
                                            commandGuestBillPayment.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@ModuleName", DbType.String, "Restaurant");
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@ServiceBillId", DbType.Int32, billID);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment2, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                            dbSmartAspects.AddOutParameter(commandGuestBillPayment2, "@PaymentId", DbType.Int32, sizeof(Int32));

                                            if (status > 0)
                                            {
                                                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment2);
                                                isWorkAny = true;
                                            }
                                        }
                                    }
                                }

                                using (DbCommand commandGuestBillPaymentDesc = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillPaymentDescriptionInfo_SP"))
                                {
                                    commandGuestBillPaymentDesc.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandGuestBillPaymentDesc, "@BillId", DbType.Int32, billID);
                                    dbSmartAspects.AddInParameter(commandGuestBillPaymentDesc, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);

                                    if (status > 0)
                                    {
                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPaymentDesc);
                                        isWorkAny = true;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(deletedPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKId", DbType.String, paymentId[i]);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    isWorkAny = true;
                                }
                            }
                        }

                        //-----------------------------------Delete from Room Payment------------------------------------------
                        if (!string.IsNullOrEmpty(deletedTransferedPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedTransferedPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TableName", DbType.String, "HotelGuestExtraServiceBillApproved");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKField", DbType.String, "ApprovedId");
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@TablePKId", DbType.String, paymentId[i]);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    isWorkAny = true;
                                }
                            }
                        }

                        // -----------------------------------------Delete Paid Service Achievement ----------------------------------------
                        if (!string.IsNullOrEmpty(deletedAchievementPaymentIds) && status > 0)
                        {
                            string[] paymentId = deletedAchievementPaymentIds.Split(',');

                            using (DbCommand commandDeletePayment = dbSmartAspects.GetStoredProcCommand("UpdateHotelGuestServiceBillApprovedForNotAchievement_SP"))
                            {
                                for (int i = 0; i < paymentId.Count(); i++)
                                {
                                    commandDeletePayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDeletePayment, "@ApprovedId", DbType.Int32, Convert.ToInt32(paymentId[i]));
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePayment, transction);
                                    isWorkAny = true;
                                }
                            }
                        }
                    }

                    if (status > 0 && isWorkAny)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        if (isWorkAny)
                            transction.Rollback();
                    }
                }
            }

            return status;
        }
        public Boolean UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment(int billId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public RestaurantBillBO GetBillInfoByBillId(int billID)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillInfoByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.BillId = billID;
                                billBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                billBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"].ToString());
                                billBO.CustomerName = reader["CustomerName"].ToString();
                                billBO.CardNumber = reader["CardNumber"].ToString();
                                billBO.BillDate = Convert.ToDateTime(reader["BillDate"].ToString());
                                billBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                billBO.CreatedDate = reader["CreatedDate"].ToString();
                                billBO.DiscountType = reader["DiscountType"].ToString();
                                billBO.Remarks = reader["Remarks"].ToString();
                                billBO.InvoiceServiceRate = Convert.ToDecimal(reader["InvoiceServiceRate"].ToString());
                                billBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"].ToString());
                                //billBO.InvoiceServiceCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                billBO.IsInvoiceCitySDChargeEnable = Convert.ToBoolean(reader["IsInvoiceCitySDChargeEnable"].ToString());
                                //billBO.InvoiceCitySDCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                billBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"].ToString());
                                //billBO.InvoiceVatAmount = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                billBO.IsInvoiceAdditionalChargeEnable = Convert.ToBoolean(reader["IsInvoiceAdditionalChargeEnable"].ToString());
                                //billBO.InvoiceAdditionalCharge = Convert.ToString(reader["CostCenterId"].ToString());
                                billBO.CompanyType = reader["CompanyType"].ToString();
                                billBO.AccountCompany = reader["AccountCompany"].ToString();
                                billBO.ProjectCode = reader["ProjectCode"].ToString();
                                billBO.ProjectName = reader["ProjectName"].ToString();
                                billBO.BinNumber = reader["BinNumber"].ToString();
                                billBO.TinNumber = reader["TinNumber"].ToString();
                                billBO.BillDeclaration = reader["BillDeclaration"].ToString();
                                billBO.UserSignature = reader["UserSignature"].ToString();
                                billBO.DeliveredByInfo = reader["DeliveredByInfo"].ToString();
                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public RestaurantBillBO GetSTBillInfoByBillId(int billID)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSTBillInfoByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.BillId = billID;
                                billBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                //billBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"].ToString());
                                billBO.CustomerName = reader["CustomerName"].ToString();
                                billBO.CardNumber = reader["CardNumber"].ToString();
                                billBO.BillDate = Convert.ToDateTime(reader["BillDate"].ToString());
                                billBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                billBO.CreatedDate = reader["CreatedDate"].ToString();
                                billBO.DiscountType = reader["DiscountType"].ToString();
                                billBO.Remarks = reader["Remarks"].ToString();
                                billBO.InvoiceServiceRate = Convert.ToDecimal(reader["InvoiceServiceRate"].ToString());
                                //billBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"].ToString());
                                ////billBO.InvoiceServiceCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                //billBO.IsInvoiceCitySDChargeEnable = Convert.ToBoolean(reader["IsInvoiceCitySDChargeEnable"].ToString());
                                ////billBO.InvoiceCitySDCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                //billBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"].ToString());
                                ////billBO.InvoiceVatAmount = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                //billBO.IsInvoiceAdditionalChargeEnable = Convert.ToBoolean(reader["IsInvoiceAdditionalChargeEnable"].ToString());
                                ////billBO.InvoiceAdditionalCharge = Convert.ToString(reader["CostCenterId"].ToString());

                                billBO.AccountCompany = reader["AccountCompany"].ToString();
                                billBO.ProjectCode = reader["ProjectCode"].ToString();
                                billBO.ProjectName = reader["ProjectName"].ToString();
                                billBO.BinNumber = reader["BinNumber"].ToString();
                                billBO.TinNumber = reader["TinNumber"].ToString();
                                billBO.BillDescription = reader["BillDescription"].ToString();
                                billBO.BillDeclaration = reader["BillDeclaration"].ToString();
                                billBO.UserSignature = reader["UserSignature"].ToString();
                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public RestaurantBillBO GetATBillInfoByBillId(int billID)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetATBillInfoByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.BillId = billID;
                                billBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                //billBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"].ToString());
                                billBO.CustomerName = reader["CustomerName"].ToString();
                                billBO.CardNumber = reader["CardNumber"].ToString();
                                billBO.BillDate = Convert.ToDateTime(reader["BillDate"].ToString());
                                billBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                billBO.CreatedDate = reader["CreatedDate"].ToString();
                                billBO.DiscountType = reader["DiscountType"].ToString();
                                billBO.Remarks = reader["Remarks"].ToString();
                                billBO.InvoiceServiceRate = Convert.ToDecimal(reader["InvoiceServiceRate"].ToString());
                                //billBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"].ToString());
                                ////billBO.InvoiceServiceCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                //billBO.IsInvoiceCitySDChargeEnable = Convert.ToBoolean(reader["IsInvoiceCitySDChargeEnable"].ToString());
                                ////billBO.InvoiceCitySDCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                //billBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"].ToString());
                                ////billBO.InvoiceVatAmount = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                //billBO.IsInvoiceAdditionalChargeEnable = Convert.ToBoolean(reader["IsInvoiceAdditionalChargeEnable"].ToString());
                                ////billBO.InvoiceAdditionalCharge = Convert.ToString(reader["CostCenterId"].ToString());

                                billBO.GLCompanyId = Convert.ToInt32(reader["GLCompanyId"].ToString());
                                billBO.ProjectId = Convert.ToInt32(reader["ProjectId"].ToString());

                                billBO.AccountCompany = reader["AccountCompany"].ToString();
                                billBO.ProjectCode = reader["ProjectCode"].ToString();
                                billBO.ProjectName = reader["ProjectName"].ToString();
                                billBO.BinNumber = reader["BinNumber"].ToString();
                                billBO.TinNumber = reader["TinNumber"].ToString();
                                billBO.BillDescription = reader["BillDescription"].ToString();
                                billBO.BillDeclaration = reader["BillDeclaration"].ToString();
                                billBO.UserSignature = reader["UserSignature"].ToString();
                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public List<RestaurantBillBO> GetRestaurantBillInfoBySearchCriteria(DateTime FormDate, DateTime ToDate, string BillNo, string CustomerInfo, string Remarks, int userInfoId, int costCenterId)
        {
            string Where = GenarateWhereConditionstring(FormDate, ToDate, BillNo, CustomerInfo, Remarks, userInfoId, costCenterId);
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.CustomerName = reader["CustomerName"].ToString();
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public List<RestaurantBillBO> GetRestaurantBillInfoBySearchCriteriaForpagination(DateTime FormDate, DateTime ToDate, string BillNo, string CustomerInfo, string Remarks, int userInfoId, int costCenterId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string Where = GenarateWhereConditionstring(FormDate, ToDate, BillNo, CustomerInfo, Remarks, userInfoId, costCenterId);
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillInfoBySearchCriteriaForPagination_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);
                                entityBO.CustomerName = reader["CustomerName"].ToString();
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.BillStatus = reader["BillStatus"].ToString();
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                                entityBO.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                entityBO.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                                entityBO.IsLocked = Convert.ToBoolean(reader["IsLocked"]);
                                entityBO.CheckOutDate = reader["CheckOutDate"].ToString();
                                entityBO.IsDayClosed = Convert.ToInt32(reader["IsDayClosed"]);
                                entityBO.Remarks = Convert.ToString(reader["Remarks"]);
                                entityBO.CostCenterType = Convert.ToString(reader["CostCenterType"]);
                                entityBO.CompanyName = Convert.ToString(reader["CompanyName"]);
                                entityBO.ProjectName = Convert.ToString(reader["ProjectName"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return entityBOList;
        }
        public List<RestaurantBillBO> GetBillNoByText(string searchTerm)
        {
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillNoByText_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.CustomerName = reader["CustomerName"].ToString();
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                                entityBO.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                entityBO.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                                entityBO.IsLocked = Convert.ToBoolean(reader["IsLocked"]);
                                entityBO.Remarks = Convert.ToString(reader["Remarks"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public string CostcenterProcessing(DateTime? FormDate, DateTime? ToDate, int userInfo, int costCenterId)
        {
            string costCenterIdList = string.Empty;

            if (userInfo != 1)
            {
                if (FormDate != null && ToDate != null)
                {
                    RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                    List<RestaurantBearerBO> costCenterInfoBOList = new List<RestaurantBearerBO>();
                    costCenterInfoBOList = restaurantBearerDA.GetRestaurantInfoForBearerByEmpIdNIsBearer(userInfo, 0);

                    if (costCenterId != 0)
                    {
                        costCenterIdList = costCenterId.ToString();
                    }
                    else
                    {
                        foreach (RestaurantBearerBO row in costCenterInfoBOList)
                        {
                            if (!string.IsNullOrWhiteSpace(costCenterIdList))
                            {
                                costCenterIdList += "," + row.CostCenterId.ToString();
                            }
                            else
                            {
                                costCenterIdList = row.CostCenterId.ToString();
                            }
                        }
                    }
                }
            }
            else
            {
                if (FormDate != null && ToDate != null)
                {
                    if (@costCenterId != 0)
                    {
                        costCenterIdList = costCenterId.ToString();
                    }
                }
            }

            return costCenterIdList;
        }
        public string GenarateWhereConditionstring(DateTime FormDate, DateTime ToDate, string BillNo, string CustomerInfo, string Remarks, int userInfo, int costCenterId)
        {
            string Where = string.Empty;

            if (userInfo != 1)
            {
                if (!string.IsNullOrEmpty(FormDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()))
                {
                    string strCostCenterIdList = string.Empty;
                    if (costCenterId != 0)
                    {
                        strCostCenterIdList = costCenterId.ToString();
                    }
                    else
                    {
                        RestaurantBearerDA restaurantBearerDA = new RestaurantBearerDA();
                        List<RestaurantBearerBO> costCenterInfoBOList = new List<RestaurantBearerBO>();
                        costCenterInfoBOList = restaurantBearerDA.GetRestaurantInfoForBearerByEmpIdNIsBearer(userInfo, 0);

                        foreach (RestaurantBearerBO row in costCenterInfoBOList)
                        {
                            if (string.IsNullOrWhiteSpace(strCostCenterIdList))
                            {
                                strCostCenterIdList = row.CostCenterId.ToString();
                            }
                            else
                            {
                                strCostCenterIdList = strCostCenterIdList + "," + row.CostCenterId.ToString();
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(strCostCenterIdList))
                    {
                        Where += "ISNULL(rb.IsBillSettlement, 0) <> 0 AND rb.CostCenterId IN (" + strCostCenterIdList + ") AND dbo.FnDate(rb.BillDate) BETWEEN dbo.FnDate('" + FormDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "')";
                    }
                    else
                    {
                        Where += "ISNULL(rb.IsBillSettlement, 0) <> 0 AND dbo.FnDate(rb.BillDate) BETWEEN dbo.FnDate('" + FormDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "')";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(FormDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()))
                {
                    if (@costCenterId == 0)
                    {
                        Where += "ISNULL(rb.IsBillSettlement, 0) <> 0 AND dbo.FnDate(rb.BillDate) BETWEEN dbo.FnDate('" + FormDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "')";
                    }
                    else
                    {
                        Where += "ISNULL(rb.IsBillSettlement, 0) <> 0 AND rb.CostCenterId IN (" + costCenterId.ToString() + ") AND dbo.FnDate(rb.BillDate) BETWEEN dbo.FnDate('" + FormDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "')";
                    }
                }
            }

            if (!string.IsNullOrEmpty(BillNo.Trim()))
            {
                if (string.IsNullOrEmpty(Where))
                {
                    Where += "rb.BillNumber LIKE '%" + BillNo.Trim() + "%'";
                }
                else
                {
                    Where += " AND rb.BillNumber LIKE '%" + BillNo.Trim() + "%'";
                }
            }

            if (!string.IsNullOrEmpty(CustomerInfo.Trim()))
            {
                if (string.IsNullOrEmpty(Where))
                {
                    Where += "(ISNULL(rb.CustomerName, '') + ' ' + ISNULL(rb.CustomerMobile, '')  + ' ' + ISNULL(rb.CustomerAddress, '')) LIKE '%" + CustomerInfo.Trim() + "%'";
                }
                else
                {
                    Where += " AND (ISNULL(rb.CustomerName, '') + ' ' + ISNULL(rb.CustomerMobile, '')  + ' ' + ISNULL(rb.CustomerAddress, '')) LIKE '%" + CustomerInfo.Trim() + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Remarks.Trim()))
            {
                if (string.IsNullOrEmpty(Where))
                {
                    Where += "rb.Remarks LIKE '%" + Remarks.Trim() + "%'";
                }
                else
                {
                    Where += " AND rb.Remarks LIKE '%" + Remarks.Trim() + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where + " ORDER BY rb.BillId DESC";
            }
            return Where;
        }
        public RestaurantBillBO GetLastRestaurantBillInfoByCostCenterIdNTable(string sourceName, int costCenterId, int tableId)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLastRestaurantBillInfoByCostCenterIdNTable_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.Int32, tableId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.BillId = Convert.ToInt32(reader["BillId"]);
                                billBO.CustomerName = reader["CustomerName"].ToString();
                                billBO.DiscountType = reader["DiscountType"].ToString();
                                billBO.DiscountTransactionId = Convert.ToInt32(reader["DiscountTransactionId"]);
                                billBO.BusinessPromotionIdNPercentAmount = reader["BusinessPromotionIdNPercentAmount"].ToString();
                                billBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                                billBO.CalculatedDiscountAmount = Convert.ToDecimal(reader["CalculatedDiscountAmount"]);

                                if (reader["BearerId"] != null)
                                    billBO.BearerId = Convert.ToInt32(reader["BearerId"]);

                                if (reader["IsComplementary"] != DBNull.Value)
                                    billBO.IsComplementary = Convert.ToBoolean(reader["IsComplementary"]);
                                else
                                    billBO.IsComplementary = false;

                                billBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"]);
                                billBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"]);

                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public List<RestaurantBillBO> GetRestaurantBillClassificationDiscountInfo(int billId)
        {
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillClassificationDiscountInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public List<RestaurantBillBO> GetRestaurantBillDetailInfoByBillId(int billId)
        {
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillDetailInfoByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public Boolean UpdateRestaurantCostCenterTableMappingInfo(int costCenterId, string tableIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantCostCenterTableMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, costCenterId);
                    dbSmartAspects.AddInParameter(command, "@TableIdList", DbType.String, tableIdList);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean RestaurantBillSettlementInfoByBillId(int billId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billId);
                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateRestaurantBillSummaryAmount(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> restaurentBillDetailBOList, List<RestaurantBillBO> categoryWisePercentageDiscountBOList, string categoryIdList, bool isBillWillSettle, bool isAmountWillDistribution, Int32 userInfoId)
        {
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillSummaryAmount_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                        dbSmartAspects.AddInParameter(command, "@BillPaymentDate", DbType.DateTime, restaurentBillBO.BillPaymentDate);
                        dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                        dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                        dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                        dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                        dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                        dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                        dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                        dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                        dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                        dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                        dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                        dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                        dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                        dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);

                        if (restaurentBillBO.BearerId != 0)
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                        else
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, DBNull.Value);

                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                    }


                    //Save Kot Information ------------------------------------
                    if (restaurentBillDetailBOList != null && status > 0)
                    {
                        if (restaurentBillDetailBOList.Count > 0)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBillDetailInfoByBillId_SP"))
                            {
                                commandPercentageDiscount.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);

                                if (status < 0)
                                    status = 1;
                            }

                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                if (row.KotId > 0)
                                {
                                    row.BillId = restaurentBillBO.BillId;
                                    using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        if (restaurentBillBO.BearerId > 0)
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                        }

                                        dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }
                    }

                    if (status > 0 && categoryWisePercentageDiscountBOList.Count > 0)
                    {
                        using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                        {
                            foreach (RestaurantBillBO percentageDiscountBO in categoryWisePercentageDiscountBOList)
                            {
                                if (percentageDiscountBO.ClassificationId > 0)
                                {
                                    commandPercentageDiscount.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                    dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                }
                            }
                        }
                    }

                    if (status > 0 && categoryWisePercentageDiscountBOList.Count == 0)
                    {
                        using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBillClassificationDiscount_SP"))
                        {
                            commandPercentageDiscount.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);

                            if (status < 0)
                                status = 1;
                        }
                    }

                    if (isAmountWillDistribution)
                    {
                        if (status > 0)
                        {
                            using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                if (!string.IsNullOrEmpty(categoryIdList))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList.Trim());
                                }
                                else
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                }

                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                            }
                        }
                    }

                    if (isBillWillSettle && isAmountWillDistribution)
                    {
                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }
        public bool UpdateRestaurantBill(RestaurantBillBO restaurentBillBO)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(cmd, "@BillStatus", DbType.String, restaurentBillBO.BillStatus);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@BillVoidBy", DbType.Int32, restaurentBillBO.BillVoidBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transction);
                        }

                        if (status > 0 && restaurentBillBO.BillStatus == "Void")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillItemRollBackAtReSettlementTime_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@CostcenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, "Void");

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0 && restaurentBillBO.BillStatus == "Active")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, "Void");

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
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

            if (status > 0)
                return true;
            else
                return false;
        }
        public List<RestaurantBillBO> GetRestaurantCancelBill(string costcenterIdList, DateTime fromDate, DateTime toDate)
        {
            List<RestaurantBillBO> cancelBillList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantCancelBillInfo_SP"))
                {
                    if (!string.IsNullOrEmpty(costcenterIdList))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, costcenterIdList);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantBill");
                    DataTable Table = ds.Tables["RestaurantBill"];

                    cancelBillList = Table.AsEnumerable().Select(r => new RestaurantBillBO
                    {
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        StringBillDate = r.Field<string>("StringBillDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        SalesAmount = r.Field<decimal>("SalesAmount"),
                        GrandTotal = r.Field<decimal>("GrandTotal"),
                        Remarks = r.Field<string>("Remarks"),
                        BillVoidByName = r.Field<string>("BillVoidByName"),
                        VoidDate = r.Field<string>("VoidDate"),
                        VoidTime = r.Field<string>("VoidTime"),
                        KotIds = r.Field<string>("KotIds")
                    }).ToList();
                }
            }

            return cancelBillList;
        }
        public List<KotBillDetailBO> GetRestaurantSetteledByForddl()
        {
            List<KotBillDetailBO> kotBill = new List<KotBillDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantSetteledByForddl_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                KotBillDetailBO rBill = new KotBillDetailBO()
                                {
                                    LastModifiedUser = (reader["ModifiedByUser"]).ToString(),
                                    LastModifiedBy = Convert.ToInt32(reader["UserInfoId"])
                                };
                                kotBill.Add(rBill);
                            }
                        }
                    }
                }
            }

            return kotBill;
        }
        public List<RestaurantBillBO> GetRestaurantKotDetailsInfo(DateTime? fromDate, DateTime? toDate, int costCenterId, string billNumber, string kotNumber, int userInfoId)
        {
            List<RestaurantBillBO> cancelBillList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKotDetailsInfo_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);
                    }
                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);
                    }
                    if (costCenterId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    }
                    if (userInfoId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, userInfoId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(kotNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, Convert.ToInt32(kotNumber));
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(billNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantBill");
                    DataTable Table = ds.Tables["RestaurantBill"];

                    cancelBillList = Table.AsEnumerable().Select(r => new RestaurantBillBO
                    {
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        KotDateString = r.Field<string>("KotDateString"),
                        KotId = r.Field<Int32>("KotId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CreatedByUser = r.Field<string>("CreatedByUser"),
                        CreatedDateString = r.Field<string>("CreatedDateString"),
                        LastModifiedBy = r.Field<Int32>("LastModifiedBy"),
                        ModifiedByUser = r.Field<string>("ModifiedByUser"),
                        LastModifiedDateString = r.Field<string>("LastModifiedDateString"),
                        KotStatus = r.Field<string>("KotStatus")
                    }).ToList();
                }
            }

            return cancelBillList;
        }
        public int SaveUpdateRestaurantBillDetail(string bill, string kotSave, string kotDelete)
        {
            int status = 0, row = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetailOnly_SP"))
                    {
                        if (!string.IsNullOrEmpty(kotSave))
                        {
                            string[] saveKotBill = kotSave.Split(',');

                            for (row = 0; row < saveKotBill.Count(); row++)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, Convert.ToInt32(bill));
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, Convert.ToInt32(saveKotBill[row]));
                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                        else if (!string.IsNullOrEmpty(kotDelete))
                            status = 1;
                    }

                    if (status > 0)
                    {
                        if (!string.IsNullOrEmpty(kotDelete))
                        {
                            status = 0;

                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBillDetailOnly_SP"))
                            {
                                string[] deleteKotBill = kotDelete.Split(',');

                                for (row = 0; row < deleteKotBill.Count(); row++)
                                {
                                    commandBillDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, Convert.ToInt32(bill));
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, Convert.ToInt32(deleteKotBill[row]));
                                    status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                }
                            }
                        }
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }

                return status;
            }
        }
        public List<RestaurantBillReportBO> GetRestaurantBillReport(int billId)
        {
            List<RestaurantBillReportBO> restaurantBill = new List<RestaurantBillReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBill"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantBill");
                    DataTable Table = ds.Tables["RestaurantBill"];
                    restaurantBill = Table.AsEnumerable().Select(r => new RestaurantBillReportBO
                    {
                        BillId = r.Field<int?>("BillId"),
                        BillDate = r.Field<string>("BillDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        SourceName = r.Field<string>("SourceName"),
                        TableNumber = r.Field<string>("TableNumber"),
                        PaxQuantity = r.Field<int?>("PaxQuantity"),
                        CostCenterId = r.Field<int>("CostCenterId"),
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
                        AdditionalChargeType = r.Field<string>("AdditionalChargeType"),
                        RestaurantSDCharge = r.Field<string>("RestaurantSDCharge"),
                        RestaurantAdditionalCharge = r.Field<string>("RestaurantAdditionalCharge"),

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
                        CustomerId = r.Field<int?>("CustomerId"),
                        UnitHead = r.Field<string>("UnitHead"),
                        IsInclusiveBill = r.Field<int?>("IsInclusiveBill"),
                        IsVatServiceChargeEnable = r.Field<int?>("IsVatServiceChargeEnable"),
                        PaymentInformation = r.Field<string>("PaymentInformation"),
                        RestaurantVatString = r.Field<string>("RestaurantVatString"),
                        RestaurantServiceChargeString = r.Field<string>("RestaurantServiceChargeString"),
                        TransactionType = r.Field<string>("TransactionType"),
                        DiscountTitle = r.Field<string>("DiscountTitle"),
                        MultipleTableAddedNumbers = r.Field<string>("MultipleTableAddedNumbers"),
                        ClassificationWiseDiscount = r.Field<string>("ClassificationWiseDiscount")

                    }).ToList();
                }
            }

            return restaurantBill;
        }
        public RestaurantBillBO GetItemCostCenterMappingInfo(int? costcenterId, int? itemId)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemCostCenterMappingInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costcenterId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.CitySDCharge = Convert.ToDecimal(reader["SDCharge"]);
                                billBO.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public RestaurantBillBO GetSDCInfoviceInformation(int billId)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSDCInfoviceInformation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.BillId = Convert.ToInt32(reader["BillId"]);
                                billBO.SDCInvoiceNumber = reader["SDCInvoiceNumber"].ToString();
                                billBO.QRCode = reader["QRCode"].ToString();
                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public RestaurantBillBO GetRestaurantBillInfoForCompanyBalance(int billID)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillInfoForCompanyBalance_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                billBO.BillId = billID;
                                billBO.PaySourceCurrentBalance = Convert.ToDecimal(reader["PaySourceCurrentBalance"].ToString());
                            }
                        }
                    }
                }
            }
            return billBO;
        }
        public Boolean DeleteRecipeDetailsAndUpdateDefaultPrice(int kotId, int itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRecipeDetailsAndUpdateDefaultPrice_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@kotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(command, "@itemId", DbType.Int32, itemId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public RestaurantBillBO GetRestaurantBillByKotId(int kotId, string sourceName)
        {
            RestaurantBillBO kotBill = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    kotBill = Table.AsEnumerable().Select(r => new RestaurantBillBO
                    {
                        BillId = r.Field<Int32>("BillId"),
                        RoundedAmount = r.Field<decimal>("RoundedAmount"),
                        RoundedGrandTotal = r.Field<decimal>("RoundedGrandTotal"),
                        ProjectId = r.Field<Int32>("ProjectId"),
                        PaymentInstructionId = r.Field<Int32>("PaymentInstructionId"),
                        ContactId = r.Field<Int32>("ContactId"),
                        BillType = r.Field<string>("BillType"),
                        BillingType = r.Field<string>("BillingType"),
                        Remarks = r.Field<string>("Remarks"),
                        Subject = r.Field<string>("Subject"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        IsBillSettlement = r.Field<bool>("IsBillSettlement"),
                        IsComplementary = r.Field<bool>("IsComplementary"),
                        SourceName = r.Field<string>("SourceName"),
                        BillPaidBySourceId = r.Field<Int32>("BillPaidBySourceId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        PaxQuantity = r.Field<Int32>("PaxQuantity"),
                        CustomerName = r.Field<string>("CustomerName"),
                        PayMode = r.Field<string>("PayMode"),
                        BankId = r.Field<Int32>("BankId"),
                        CardType = r.Field<string>("CardType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        ExpireDate = r.Field<DateTime?>("ExpireDate"),
                        CardHolderName = r.Field<string>("CardHolderName"),
                        RegistrationId = r.Field<Int32>("RegistrationId"),
                        SalesAmount = r.Field<decimal>("SalesAmount"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountTransactionId = r.Field<Int32>("DiscountTransactionId"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        CalculatedDiscountAmount = r.Field<decimal>("CalculatedDiscountAmount"),
                        IsInvoiceServiceChargeEnable = r.Field<bool>("IsInvoiceServiceChargeEnable"),
                        ServiceCharge = r.Field<decimal>("ServiceCharge"),
                        IsInvoiceVatAmountEnable = r.Field<bool>("IsInvoiceVatAmountEnable"),
                        VatAmount = r.Field<decimal>("VatAmount"),
                        IsInvoiceCitySDChargeEnable = r.Field<bool>("IsInvoiceCitySDChargeEnable"),
                        CitySDCharge = r.Field<decimal>("CitySDCharge"),
                        IsInvoiceAdditionalChargeEnable = r.Field<bool>("IsInvoiceAdditionalChargeEnable"),
                        AdditionalCharge = r.Field<decimal>("AdditionalCharge"),
                        AdditionalChargeType = r.Field<string>("AdditionalChargeType"),
                        GrandTotal = r.Field<decimal>("GrandTotal"),
                        IsBillPreviewButtonEnable = r.Field<bool>("IsBillPreviewButtonEnable"),
                        TransactionType = r.Field<string>("TransactionType"),
                        TransactionId = r.Field<Int64?>("TransactionId")
                    }).FirstOrDefault();
                }
            }
            return kotBill;
        }
        public List<GuestBillPaymentBO> GetBillPaymentByBillId(int billId, string moduleName)
        {
            List<GuestBillPaymentBO> kotBillPayment = new List<GuestBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillPaymentByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceBillId", DbType.Int32, billId);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, moduleName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    kotBillPayment = Table.AsEnumerable().Select(r => new GuestBillPaymentBO
                    {
                        PaymentId = r.Field<Int32>("PaymentId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentType = r.Field<string>("PaymentType"),
                        RegistrationId = r.Field<int>("RegistrationId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        ServiceBillId = r.Field<int?>("ServiceBillId"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        BankId = r.Field<int>("BankId"),
                        BranchName = r.Field<string>("BranchName"),
                        ChecqueNumber = r.Field<string>("ChecqueNumber"),
                        CardType = r.Field<string>("CardType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        CardHolderName = r.Field<string>("CardHolderName"),
                        CardReference = r.Field<string>("CardReference")
                    }).ToList();
                }
            }
            return kotBillPayment;
        }
        public int GetBillPaymentByBillId(string billNumber)
        {
            int billId = 0;

            string query = string.Format("SELECT BillId FROM RestaurantBill WHERE BillNumber = '{0}'", billNumber);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    var v = Table.AsEnumerable().Select(r => new RestaurantBillReportBO
                    {
                        BillId = r.Field<int?>("BillId")

                    }).FirstOrDefault();

                    if (v != null)
                    {
                        billId = Convert.ToInt32(v.BillId);
                    }
                }
            }
            return billId;
        }
        public bool UpdateBillForVatAdjustment(List<string> billList, string type)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillForVatAdjustment_SP"))
                {
                    foreach (var v in billList)
                    {
                        cmd.Parameters.Clear();

                        dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, Convert.ToInt32(v));
                        dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
                        status = dbSmartAspects.ExecuteNonQuery(cmd);
                    }
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }
        public List<KotWiseVatNSChargeNDiscountNComplementaryBO> GetKotWiseVatNSChargeNDiscountNComplementary(RestaurantBillBO RestaurantBill, string kotIdList, string categoryIdList)
        {
            List<KotWiseVatNSChargeNDiscountNComplementaryBO> kotBillSummary = new List<KotWiseVatNSChargeNDiscountNComplementaryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetKotWiseVatNSChargeNDiscountNComplementary_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotIdList", DbType.String, kotIdList);

                    if (!string.IsNullOrWhiteSpace(categoryIdList))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryIdList", DbType.String, categoryIdList);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryIdList", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, RestaurantBill.DiscountType);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountPercent", DbType.Decimal, RestaurantBill.DiscountAmount);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, RestaurantBill.CostCenterId);

                    dbSmartAspects.AddInParameter(cmd, "@IsInvoiceServiceChargeEnable", DbType.Boolean, RestaurantBill.IsInvoiceServiceChargeEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, RestaurantBill.IsInvoiceCitySDChargeEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsInvoiceVatAmountEnable", DbType.Boolean, RestaurantBill.IsInvoiceVatAmountEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, RestaurantBill.IsInvoiceAdditionalChargeEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsComplementary", DbType.Boolean, RestaurantBill.IsComplementary);
                    dbSmartAspects.AddInParameter(cmd, "@IsNonChargeable", DbType.Boolean, RestaurantBill.IsNonChargeable);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    kotBillSummary = Table.AsEnumerable().Select(r => new KotWiseVatNSChargeNDiscountNComplementaryBO
                    {
                        KotId = r.Field<int?>("KotId"),
                        KotDetailId = r.Field<int>("KotDetailId"),
                        ItemId = r.Field<int?>("ItemId"),
                        Amount = r.Field<decimal?>("Amount"),
                        ItemTotalAmount = r.Field<decimal>("ItemTotalAmount"),
                        InvoiceTotalAmount = r.Field<decimal?>("InvoiceTotalAmount"),
                        ItemUnit = r.Field<decimal?>("ItemUnit"),
                        IsInvoiceVatAmountEnable = r.Field<bool?>("IsInvoiceVatAmountEnable"),
                        IsInvoiceServiceChargeEnable = r.Field<bool?>("IsInvoiceServiceChargeEnable"),
                        IsInvoiceCitySDChargeEnable = r.Field<bool?>("IsInvoiceCitySDChargeEnable"),
                        IsInvoiceAdditionalChargeEnable = r.Field<bool?>("IsInvoiceAdditionalChargeEnable"),
                        ServiceChargeConfig = r.Field<decimal>("ServiceChargeConfig"),
                        SDChargeConfig = r.Field<decimal>("SDChargeConfig"),
                        VatAmountConfig = r.Field<decimal>("VatAmountConfig"),
                        AdditionalChargeConfig = r.Field<decimal>("AdditionalChargeConfig"),
                        BillWiseDiscountType = r.Field<string>("BillWiseDiscountType"),
                        BillWiseDiscountAmount = r.Field<decimal?>("BillWiseDiscountAmount"),
                        ItemWiseDiscountType = r.Field<string>("ItemWiseDiscountType"),
                        ItemWiseDiscountAmount = r.Field<decimal>("ItemWiseDiscountAmount"),
                        ClassificationId = r.Field<int?>("ClassificationId"),
                        ClassificationWiseDiscountAmount = r.Field<decimal?>("ClassificationWiseDiscountAmount"),
                        IsComplementary = r.Field<bool?>("IsComplementary"),
                        IsNonChargeable = r.Field<bool?>("IsNonChargeable"),
                        ItemWiseDiscount = r.Field<decimal?>("ItemWiseDiscount"),
                        ClassificationWiseDiscount = r.Field<decimal?>("ClassificationWiseDiscount"),
                        BillWiseDiscount = r.Field<decimal?>("BillWiseDiscount"),
                        FixedDiscountRatio = r.Field<decimal?>("FixedDiscountRatio"),
                        ActualDiscountType = r.Field<string>("ActualDiscountType"),
                        ActualDiscount = r.Field<decimal?>("ActualDiscount"),
                        ActualDiscountAmount = r.Field<decimal?>("ActualDiscountAmount"),
                        AmountAfterDiscount = r.Field<decimal?>("AmountAfterDiscount"),
                        AmountForDistribution = r.Field<decimal?>("AmountForDistribution"),
                        TransactionId = r.Field<Int64?>("TransactionId"),
                        RackRate = r.Field<decimal?>("RackRate"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        SDCityCharge = r.Field<decimal?>("SDCityCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        CalculatedAmount = r.Field<decimal?>("CalculatedAmount")

                    }).ToList();
                }
            }

            return kotBillSummary;
        }
        public List<RestaurantBillBO> GetRestaurantBillInfo()
        {
            string query = string.Format("SELECT BillId, BillNumber, ISNULL(IsBillSettlement, 0) IsBillSettlement FROM RestaurantBill WHERE IsBillSettlement = 1 AND " +
                                          " dbo.FnDate(BillDate) = '{0}'", DateTime.Now.Date);

            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    entityBOList = Table.AsEnumerable().Select(r => new RestaurantBillBO
                    {
                        BillId = r.Field<Int32>("BillId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        IsBillSettlement = r.Field<bool>("IsBillSettlement")

                    }).ToList();
                }
            }

            return entityBOList;
        }
        public List<RestaurantBillBO> GetRestaurantBillInfoByBillId(Int64 billId)
        {
            string query = string.Format("SELECT BillId, BillNumber, ISNULL(IsBillSettlement, 0) IsBillSettlement FROM RestaurantBill WHERE " +
                                          " BillId = {0}", billId);

            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    entityBOList = Table.AsEnumerable().Select(r => new RestaurantBillBO
                    {
                        BillId = r.Field<Int32>("BillId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        IsBillSettlement = r.Field<bool>("IsBillSettlement")

                    }).ToList();
                }
            }

            return entityBOList;
        }
        public List<RestaurantBillBO> GetRoomBillSettlmentPending(string searchType, int registrationId, string roomNumber)
        {
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomBillSettlmentPending_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchType);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.BillDate = Convert.ToDateTime(reader["BillDate"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.CostCenter = reader["CostCenter"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public Boolean SaveMachineTestInfo(MachineTestBO entityBO, out int tmpPkId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMachineTestInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TestDate", DbType.DateTime, entityBO.TestDate);
                    dbSmartAspects.AddInParameter(command, "@MachineId", DbType.Int32, entityBO.MachineId);
                    dbSmartAspects.AddInParameter(command, "@TestQuantity", DbType.Decimal, entityBO.TestQuantity);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, entityBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TestId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpPkId = Convert.ToInt32(command.Parameters["@TestId"].Value);
                }
            }
            return status;
        }
        public List<MachineTestBO> GetMachineTestInformationBySearchCriteriaForPaging(int costCenterId, DateTime fromDate, DateTime toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<MachineTestBO> bankList = new List<MachineTestBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMachineTestInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                MachineTestBO machineBO = new MachineTestBO();
                                machineBO.TestId = Convert.ToInt32(reader["TestId"]);
                                machineBO.BeforeMachineReadNumber = reader["BeforeMachineReadNumber"].ToString();
                                machineBO.TestQuantity = Convert.ToDecimal(reader["TestQuantity"].ToString());
                                machineBO.AfterMachineReadNumber = reader["AfterMachineReadNumber"].ToString();
                                machineBO.Remarks = reader["Remarks"].ToString();
                                bankList.Add(machineBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return bankList;
        }
        public bool UpdatePOSMachineReaderUnitInfo(RestaurantBillBO restaurentBillBO)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePOSMachineReaderUnitInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateRestaurantBillInfoForCompany(RestaurantBillBO restaurentBillBO)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillInfoForCompany_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, restaurentBillBO.PayMode);
                            dbSmartAspects.AddInParameter(command, "@PayModeSourceId", DbType.Int32, restaurentBillBO.PayModeSourceId);
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        //--------------------New TOuch Screen Methods
        public List<RestaurantBillDetailBO> GetRestaurantBillDetailsByBillId(int billId)
        {
            List<RestaurantBillDetailBO> entityBOList = new List<RestaurantBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillDetailsByBillId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillDetailBO entityBO = new RestaurantBillDetailBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.DetailId = Convert.ToInt32(reader["DetailId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }
        public int SaveRestaurantBillForAll(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> BillDetail, List<GuestBillPaymentBO> guestPaymentDetailList,
                                            List<ItemClassificationBO> AddedClassificationList, string categoryIdList, bool isBillWillSettle,
                                            bool isAmountWillDistribution, out int billID)
        {
            int status = 0;
            string query = string.Format(@"
                                        IF(EXISTS(SELECT KotId FROM dbo.RestaurantKotPendingList WHERE KotId = {0}))
                                        BEGIN
                                        DELETE FROM dbo.RestaurantKotPendingList WHERE KotId = {0}
                                        END
                                      ", restaurentBillBO.KotId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        var isRoomWisePayment = guestPaymentDetailList.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                        var isOtherPayment = guestPaymentDetailList.Where(p => p.PaymentMode != "Other Room").FirstOrDefault();

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);

                            if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            else
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, 0);

                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                            dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                            dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);

                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            billID = Convert.ToInt32(command.Parameters["@BillId"].Value);
                            restaurentBillBO.BillId = billID;
                        }

                        //Save Kot & Bill Mapping Information ------------------------------------
                        if (BillDetail != null && status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                int billIdForDetails = 0;

                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    commandBillDetails.Parameters.Clear();
                                    billIdForDetails = row.BillId == 0 ? restaurentBillBO.BillId : row.BillId;

                                    if (row.DetailId == 0)
                                    {
                                        row.BillId = billID;

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billIdForDetails); //row.BillId
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        if (restaurentBillBO.BearerId > 0)
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                        }

                                        dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);

                                        row.MainBillId = billID;
                                    }
                                    else { row.MainBillId = billID; }
                                }
                            }
                        }

                        if (BillDetail != null && status > 0)
                        {
                            var addedBill = BillDetail.Where(d => d.DetailId > 0 && d.BillId != restaurentBillBO.BillId).ToList();

                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillDetailNForMultipleTableAdded_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in addedBill)
                                {
                                    if (row.DetailId > 0)
                                    {
                                        commandBillDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@DetailId", DbType.Int32, row.DetailId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@MainBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }


                        if (status > 0 && AddedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && guestPaymentDetailList != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList.Where(p => p.PaymentMode != "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                            if (guestBillPaymentBO.PaymentMode == "Company")
                                            {
                                                guestBillPaymentBO.PaymentType = "Company";
                                            }
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);

                                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        else
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, 0);

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (isAmountWillDistribution)
                        {
                            if (status > 0)
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, billID);

                                    if (!string.IsNullOrEmpty(categoryIdList))
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList.Trim());
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                    }

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }
                        }

                        if (isAmountWillDistribution && isBillWillSettle)
                        {
                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billID);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }

                            }

                            if (status > 0 && restaurentBillBO.SourceName == "RestaurantTable")
                            {
                                string tableIdLst = string.Empty;

                                foreach (RestaurantBillDetailBO dt in BillDetail)
                                {
                                    if (tableIdLst != string.Empty)
                                        tableIdLst += "," + dt.TableId.ToString();
                                    else
                                        tableIdLst = dt.TableId.ToString();
                                }

                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantCostCenterTableMappingInfo_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, restaurentBillBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@TableIdList", DbType.String, tableIdLst);
                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }

                            if (status > 0 && restaurentBillBO.SourceName == "RestaurantToken")
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billID);
                                    dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, false);
                                    dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);
                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList.Where(p => p.PaymentMode == "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        //restaurentBillBO.SourceName == "GuestRoom" && 

                        //if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment_SP"))
                        //    {
                        //        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billID);
                        //        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        //    }
                        //}

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                if (status < 0) { status = 1; }
                            }
                        }

                        if (status > 0)
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
                        billID = 0;
                        throw ex;

                    }
                }
            }

            return billID;
        }
        public bool UpdateRestauranBillGenerationNewSettlement(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> BillDetail,
                                                               List<RestaurantBillDetailBO> BillDeletedDetail, List<GuestBillPaymentBO> paymentAdded,
                                                               List<GuestBillPaymentBO> paymentUpdate, List<GuestBillPaymentBO> paymentDelete,
                                                               List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList,
                                                               string categoryIdList, bool isBillWillSettle
                                                               )
        {
            int status = 0;
            bool retVal = false;
            string queryPendingKot = string.Format(@"
                                        IF(EXISTS(SELECT KotId FROM dbo.RestaurantKotPendingList WHERE KotId = {0}))
                                        BEGIN
                                        DELETE FROM dbo.RestaurantKotPendingList WHERE KotId = {0}
                                        END
                                      ", restaurentBillBO.KotId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        var isRoomWisePayment = paymentAdded.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                        var isOtherPayment = paymentAdded.Where(p => p.PaymentMode != "Other Room").FirstOrDefault();

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 0);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            else
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, 0);

                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                            dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                            dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);

                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);


                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0)
                        {
                            string query = string.Format(@"UPDATE RestaurantBill SET CreatedBy = {1} WHERE BillId = {0}", restaurentBillBO.BillId, restaurentBillBO.LastModifiedBy);
                            using (DbCommand cmdUpdate = dbSmartAspects.GetSqlStringCommand(query))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(cmdUpdate, transction);
                            }
                        }

                        if (status > 0 && DeletedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantClassificationDiscountByDiscountId_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in DeletedClassificationList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, percentageDiscountBO.DiscountId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && AddedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList.Where(d => d.DiscountId == 0))
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }

                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList.Where(d => d.DiscountId > 0))
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, percentageDiscountBO.DiscountId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentAdded.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded.Where(p => p.PaymentMode != "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);

                                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        else
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, 0);

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentUpdate.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentUpdate.Where(p => p.PaymentMode != "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        else
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, 0);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentDelete.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (GuestBillPaymentBO pd in paymentDelete)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.PaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }

                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded.Where(p => p.PaymentMode == "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        //if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment_SP"))
                        //    {
                        //        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                        //        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        //    }
                        //}

                        if (BillDetail != null && status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    if (row.DetailId == 0)
                                    {
                                        commandBillDetails.Parameters.Clear();
                                        row.BillId = restaurentBillBO.BillId;

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        if (restaurentBillBO.BearerId > 0)
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                        }

                                        dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }

                        if (BillDetail != null && status > 0)
                        {
                            var addedBill = BillDetail.Where(d => d.DetailId > 0 && d.BillId != restaurentBillBO.BillId).ToList();

                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillDetailNForMultipleTableAdded_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in addedBill)
                                {
                                    if (row.DetailId > 0)
                                    {
                                        commandBillDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@DetailId", DbType.Int32, row.DetailId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@MainBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && BillDetail != null)
                        {
                            using (DbCommand commandUpdateReceipeCost = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotBillItemRecipeCost_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    if (row.DetailId == 0)
                                    {
                                        commandUpdateReceipeCost.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@KotId", DbType.Int32, row.KotId);
                                        dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandUpdateReceipeCost, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && BillDeletedDetail != null)
                        {
                            using (DbCommand commandDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBillDetailsByDetailsNKotId_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDeletedDetail)
                                {
                                    commandDeleteDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeleteDetails, "@DetailId", DbType.Int32, row.DetailId);
                                    dbSmartAspects.AddInParameter(commandDeleteDetails, "@KotId", DbType.Int32, row.KotId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeleteDetails, transction);
                                    if (status < 0) status = 1;
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, false);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (isBillWillSettle)
                        {
                            if (status > 0)
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                    if (!string.IsNullOrEmpty(categoryIdList.Trim()))
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                    }

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }

                            if (status > 0 && restaurentBillBO.SourceName == "RestaurantTable")
                            {
                                string tableIdLst = string.Empty;

                                foreach (RestaurantBillDetailBO dt in BillDetail)
                                {
                                    if (tableIdLst != string.Empty)
                                        tableIdLst += "," + dt.TableId.ToString();
                                    else
                                        tableIdLst = dt.TableId.ToString();
                                }

                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantCostCenterTableMappingInfo_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, restaurentBillBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@TableIdList", DbType.String, tableIdLst);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetSqlStringCommand(queryPendingKot))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                if (status < 0) status = 1;
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public int SaveRestaurantBillForNewHoldUp(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> BillDetail, List<GuestBillPaymentBO> guestPaymentDetailList,
                                                  List<ItemClassificationBO> AddedClassificationList, out int billID)
        {
            int status = 0;
            string categoryIdList = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            //dbSmartAspects.AddInParameter(command, "@BillPaymentDate", DbType.DateTime, restaurentBillBO.BillPaymentDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                            //dbSmartAspects.AddInParameter(command, "@PayMode", DbType.String, restaurentBillBO.PayMode);
                            //dbSmartAspects.AddInParameter(command, "@BankId", DbType.Int32, restaurentBillBO.BankId);
                            //dbSmartAspects.AddInParameter(command, "@CardType", DbType.String, restaurentBillBO.CardType);
                            //dbSmartAspects.AddInParameter(command, "@CardNumber", DbType.String, restaurentBillBO.CardNumber);
                            //if (restaurentBillBO.PayMode == "Card")
                            //{
                            //    dbSmartAspects.AddInParameter(command, "@ExpireDate", DbType.DateTime, restaurentBillBO.ExpireDate);
                            //}
                            //dbSmartAspects.AddInParameter(command, "@CardHolderName", DbType.String, restaurentBillBO.CardHolderName);
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                            dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                            dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);

                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            //dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, restaurentBillBO.DealId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            billID = Convert.ToInt32(command.Parameters["@BillId"].Value);
                            restaurentBillBO.BillId = billID;
                        }

                        //Save Kot & Bill Mapping Information ------------------------------------
                        if (BillDetail != null && status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    commandBillDetails.Parameters.Clear();

                                    if (row.DetailId == 0)
                                    {
                                        row.BillId = billID;

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID); //row.BillId
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        if (restaurentBillBO.BearerId > 0)
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                        }

                                        dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));
                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);

                                        row.MainBillId = billID;
                                    }
                                    else { row.MainBillId = billID; }
                                }
                            }
                        }

                        if (BillDetail != null && status > 0)
                        {
                            var addedBill = BillDetail.Where(d => d.DetailId > 0 && d.BillId != restaurentBillBO.BillId).ToList();

                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillDetailNForMultipleTableAdded_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in addedBill)
                                {
                                    if (row.DetailId > 0)
                                    {
                                        commandBillDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@DetailId", DbType.Int32, row.DetailId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@MainBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && AddedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0 && percentageDiscountBO.DiscountId == 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, billID);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);

                                        categoryIdList += categoryIdList != string.Empty ? ("," + percentageDiscountBO.ClassificationId.ToString()) : percentageDiscountBO.ClassificationId.ToString();
                                    }
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, billID);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, true);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                if (!string.IsNullOrEmpty(categoryIdList.Trim()))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList);
                                }
                                else
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                }

                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                            billID = 0;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        billID = 0;
                        throw ex;
                    }
                }
            }

            return billID;
        }

        public bool UpdateRestaurantBillForNewHoldUp(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> BillDetail, List<RestaurantBillDetailBO> BillDeletedDetail,
                                                     List<GuestBillPaymentBO> paymentAdded, List<GuestBillPaymentBO> paymentUpdate, List<GuestBillPaymentBO> paymentDelete,
                                                     List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList)
        {
            int status = 0;
            bool retVal = false;
            string categoryIdList = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 0);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);

                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                            dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                            dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);

                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);


                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }

                        if (status > 0 && DeletedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantClassificationDiscountByDiscountId_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in DeletedClassificationList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, percentageDiscountBO.DiscountId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && AddedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList.Where(d => d.DiscountId == 0))
                                {
                                    categoryIdList += categoryIdList != string.Empty ? ("," + percentageDiscountBO.ClassificationId.ToString()) : percentageDiscountBO.ClassificationId.ToString();

                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }

                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList.Where(d => d.DiscountId > 0))
                                {
                                    categoryIdList += categoryIdList != string.Empty ? ("," + percentageDiscountBO.ClassificationId.ToString()) : percentageDiscountBO.ClassificationId.ToString();

                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, percentageDiscountBO.DiscountId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        //if (status > 0 && paymentAdded != null)
                        //{
                        //    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        //    {
                        //        foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded)
                        //        {
                        //            if (guestBillPaymentBO.PaidServiceId == 0)
                        //            {
                        //                commandGuestBillPayment.Parameters.Clear();

                        //                guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                        //                guestBillPaymentBO.PaymentDate = DateTime.Now;

                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                        //                //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, guestBillPaymentBO.CompanyId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                        //                if (guestBillPaymentBO.PaymentMode == "Card")
                        //                {
                        //                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                        //                }

                        //                //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                        //                dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                        //                //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //                //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                        //                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                        //            }
                        //        }
                        //    }
                        //}

                        //if (status > 0 && paymentUpdate != null)
                        //{
                        //    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoModarate_SP"))
                        //    {
                        //        foreach (GuestBillPaymentBO guestBillPaymentBO in paymentUpdate)
                        //        {
                        //            if (guestBillPaymentBO.PaidServiceId == 0)
                        //            {
                        //                commandGuestBillPayment.Parameters.Clear();

                        //                guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                        //                guestBillPaymentBO.PaymentDate = DateTime.Now;

                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                        //                //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                        //                if (guestBillPaymentBO.PaymentMode == "Card")
                        //                {
                        //                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                        //                }

                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                        //                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                        //            }
                        //        }
                        //    }
                        //}

                        //if (status > 0 && paymentDelete != null)
                        //{
                        //    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        //    {
                        //        foreach (GuestBillPaymentBO pd in paymentDelete)
                        //        {
                        //            commandGuestBillPayment.Parameters.Clear();

                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "PaymentId");
                        //            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.PaymentId);

                        //            status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                        //        }
                        //    }
                        //}

                        if (BillDetail != null && status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    if (row.DetailId == 0)
                                    {
                                        commandBillDetails.Parameters.Clear();
                                        row.BillId = restaurentBillBO.BillId;

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        if (restaurentBillBO.BearerId > 0)
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                        }

                                        dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }

                        if (BillDetail != null && status > 0)
                        {
                            var addedBill = BillDetail.Where(d => d.DetailId > 0 && d.BillId != restaurentBillBO.BillId).ToList();

                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillDetailNForMultipleTableAdded_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in addedBill)
                                {
                                    if (row.DetailId > 0)
                                    {
                                        commandBillDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@DetailId", DbType.Int32, row.DetailId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@MainBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && BillDetail != null)
                        {
                            using (DbCommand commandUpdateReceipeCost = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotBillItemRecipeCost_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    commandUpdateReceipeCost.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@KotId", DbType.Int32, row.KotId);
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandUpdateReceipeCost, transction);
                                }
                            }
                        }

                        if (status > 0 && BillDeletedDetail != null)
                        {
                            using (DbCommand commandDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBillDetailsByDetailsNKotId_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDeletedDetail)
                                {
                                    commandDeleteDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeleteDetails, "@DetailId", DbType.Int32, row.DetailId);
                                    dbSmartAspects.AddInParameter(commandDeleteDetails, "@KotId", DbType.Int32, row.KotId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeleteDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, true);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                if (!string.IsNullOrEmpty(categoryIdList.Trim()))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList);
                                }
                                else
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                }

                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateRestaurantBillForHoldUpNew(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> restaurentBillDetailBOList, List<GuestBillPaymentBO> paymentAdded, List<GuestBillPaymentBO> paymentUpdate, List<GuestBillPaymentBO> paymentDelete, List<RestaurantBillBO> categoryWisePercentageDiscountBOList)
        {
            int status = 0;
            bool retVal = false, isItemExist = false;

            KotBillDetailDA kotDetailsDa = new KotBillDetailDA();
            List<KotBillDetailBO> kotDetailList = new List<KotBillDetailBO>();
            kotDetailList = kotDetailsDa.GetKotBillDetailListByKotId(restaurentBillBO.KotId);

            if (kotDetailList.Count > 0)
            {
                isItemExist = true;
            }

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 0);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);

                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                            dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                            dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);

                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);


                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }

                        if (status > 0 && categoryWisePercentageDiscountBOList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (RestaurantBillBO percentageDiscountBO in categoryWisePercentageDiscountBOList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentAdded != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentUpdate != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentUpdate)
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, guestBillPaymentBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentDelete != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (GuestBillPaymentBO pd in paymentDelete)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "HotelGuestBillPayment");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.PaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }

                        if (status > 0 && isItemExist)
                        {
                            foreach (RestaurantBillDetailBO row in restaurentBillDetailBOList)
                            {
                                using (DbCommand commandUpdateReceipeCost = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotBillItemRecipeCost_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@KotId", DbType.Int32, row.KotId);
                                    dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandUpdateReceipeCost, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, true);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }


        public bool UpdateForRestauranBillReSettlement(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> BillDetail,
                                                               List<RestaurantBillDetailBO> BillDeletedDetail, List<GuestBillPaymentBO> paymentAdded,
                                                               List<GuestBillPaymentBO> paymentUpdate, List<GuestBillPaymentBO> previousLedgerPayment,
                                                               List<ItemClassificationBO> AddedClassificationList, List<ItemClassificationBO> DeletedClassificationList,
                                                               string categoryIdList, bool isBillWillSettle, bool isBillPayment, bool isGuestPayment
                                                               )
        {
            int status = 0;
            bool retVal = false;
            string paymentDeleteQuery = string.Empty, guestRoomPaymentDeleteQuery = string.Empty, billNPaymentDateUpdate = string.Empty;

            paymentDeleteQuery = string.Format("DELETE FROM HotelGuestBillPayment WHERE  ModuleName = '{0}' AND ServiceBillId = {1}", "Restaurant", restaurentBillBO.BillId);
            guestRoomPaymentDeleteQuery = string.Format("DELETE FROM HotelGuestExtraServiceBillApproved WHERE  ServiceBillId = {0} AND ServiceType = '{1}' AND PaymentMode = '{2}'", restaurentBillBO.BillId, "RestaurantService", "Other Room");

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        var isRoomWisePayment = paymentAdded.Where(p => p.PaymentMode == "Other Room").FirstOrDefault();
                        var isOtherPayment = paymentAdded.Where(p => p.PaymentMode != "Other Room").FirstOrDefault();

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBill_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@IsBillSettlement", DbType.Int32, 0);
                            dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);

                            if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                            else
                                dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, 0);

                            dbSmartAspects.AddInParameter(command, "@SalesAmount", DbType.Decimal, restaurentBillBO.SalesAmount);
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);

                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);

                            dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                            dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                            dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                            dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);

                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                            if (!string.IsNullOrEmpty(restaurentBillBO.TransactionType))
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, restaurentBillBO.TransactionType);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, DBNull.Value);

                            if (restaurentBillBO.TransactionId != null)
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, restaurentBillBO.TransactionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, DBNull.Value);

                            if (restaurentBillBO.PaymentInstructionId != null && restaurentBillBO.PaymentInstructionId != 0)
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, restaurentBillBO.PaymentInstructionId);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, DBNull.Value);
                            if (restaurentBillBO.ContactId != null && restaurentBillBO.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, restaurentBillBO.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                            else
                                dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);
                            if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                            else
                                dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, restaurentBillBO.BillPaidBySourceId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);

                            if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }

                        if (status > 0 && isBillPayment)
                        {
                            using (DbCommand commandPaymentDeletet = dbSmartAspects.GetSqlStringCommand(paymentDeleteQuery))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(commandPaymentDeletet, transction);
                            }
                        }

                        if (status > 0 && isGuestPayment)
                        {
                            using (DbCommand commandPaymentDeletet = dbSmartAspects.GetSqlStringCommand(guestRoomPaymentDeleteQuery))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(commandPaymentDeletet, transction);
                            }
                        }

                        if (status > 0 && previousLedgerPayment.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("PreviousLedgerBillPaymentProcessForResettlement_SP"))
                            {
                                foreach (GuestBillPaymentBO pd in previousLedgerPayment)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, pd.PaymentType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantClassificationDiscountByDiscountId_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in DeletedClassificationList)
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, percentageDiscountBO.DiscountId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && AddedClassificationList != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList.Where(d => d.DiscountId == 0))
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }

                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillClassificationDiscount_SP"))
                            {
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList.Where(d => d.DiscountId > 0))
                                {
                                    if (percentageDiscountBO.ClassificationId > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, percentageDiscountBO.DiscountId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ClassificationId", DbType.Int32, percentageDiscountBO.ClassificationId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@DiscountAmount", DbType.Int32, percentageDiscountBO.DiscountAmount);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentAdded.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded.Where(p => p.PaymentMode != "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        int companyId = 0;
                                        if (guestBillPaymentBO.CompanyId != null)
                                        {
                                            companyId = guestBillPaymentBO.CompanyId;
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        else
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, 0);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, guestBillPaymentBO.PaymentDescription);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                        //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && paymentUpdate.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("UpdateGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentUpdate.Where(p => p.PaymentMode != "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();

                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;
                                        guestBillPaymentBO.PaymentDate = DateTime.Now;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, guestBillPaymentBO.PaymentId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        else
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, 0);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, guestBillPaymentBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, guestBillPaymentBO.BankId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, guestBillPaymentBO.BranchName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, guestBillPaymentBO.ChecqueNumber);
                                        //dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, guestBillPaymentBO.ChecqueDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, guestBillPaymentBO.CardType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, guestBillPaymentBO.CardNumber);
                                        if (guestBillPaymentBO.PaymentMode == "Card")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, guestBillPaymentBO.ExpireDate);
                                        }

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        if (isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestExtraServiceBillInfo_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentAdded.Where(p => p.PaymentMode == "Other Room"))
                                {
                                    if (guestBillPaymentBO.PaidServiceId == 0)
                                    {
                                        commandGuestBillPayment.Parameters.Clear();
                                        guestBillPaymentBO.CreatedBy = restaurentBillBO.CreatedBy;

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, guestBillPaymentBO.PaymentAmount);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, guestBillPaymentBO.PaymentMode);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                        dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        //if (status > 0 && isRoomWisePayment != null || restaurentBillBO.RoomId > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillRegistrationIdInfoForOtherRoomPayment_SP"))
                        //    {
                        //        dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                        //        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        //    }
                        //}

                        if (status > 0 && isRoomWisePayment != null)
                        {
                            billNPaymentDateUpdate = string.Format("UPDATE RestaurantBill SET ApprovedDate = '{1}' WHERE BillId = {0} UPDATE HotelGuestExtraServiceBillApproved SET ServiceDate = '{1}' WHERE ServiceBillId = {0}", restaurentBillBO.BillId, restaurentBillBO.BillDate.ToString("yyyy-MM-dd"));

                            using (DbCommand command = dbSmartAspects.GetSqlStringCommand(billNPaymentDateUpdate))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0 && isOtherPayment != null)
                        {
                            billNPaymentDateUpdate = string.Format("UPDATE HotelGuestBillPayment SET PaymentDate = '{1}' WHERE ServiceBillId = {0}", restaurentBillBO.BillId, restaurentBillBO.BillDate.ToString("yyyy-MM-dd"));

                            using (DbCommand command = dbSmartAspects.GetSqlStringCommand(billNPaymentDateUpdate))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (BillDetail != null && status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    if (row.DetailId == 0)
                                    {
                                        row.BillId = restaurentBillBO.BillId;

                                        commandBillDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, row.BillId);
                                        dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                        if (restaurentBillBO.BearerId > 0)
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                        }
                                        else
                                        {
                                            dbSmartAspects.AddInParameter(commandBillDetails, "@BearerId", DbType.Int32, DBNull.Value);
                                        }

                                        dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && BillDetail != null)
                        {
                            using (DbCommand commandUpdateReceipeCost = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotBillItemRecipeCost_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    if (row.DetailId == 0)
                                    {
                                        commandUpdateReceipeCost.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@KotId", DbType.Int32, row.KotId);
                                        dbSmartAspects.AddInParameter(commandUpdateReceipeCost, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandUpdateReceipeCost, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && BillDeletedDetail != null)
                        {
                            using (DbCommand commandDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBillDetailsByDetailsNKotId_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDeletedDetail)
                                {
                                    commandDeleteDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeleteDetails, "@DetailId", DbType.Int32, row.DetailId);
                                    dbSmartAspects.AddInParameter(commandDeleteDetails, "@KotId", DbType.Int32, row.KotId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeleteDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, restaurentBillBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, false);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (isBillWillSettle)
                        {
                            if (status > 0)
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);

                                    if (!string.IsNullOrEmpty(categoryIdList.Trim()))
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList);
                                    }
                                    else
                                    {
                                        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                    }

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillItemRollBackAtReSettlementTime_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@CostcenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                    status = status >= 0 ? 1 : status;
                                }
                            }

                            if (status > 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("RestaurantBillSettlementInfoByBillId_SP"))
                                {
                                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }


                            if (status > 0)
                            {
                                using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("RestaurantBillReSettlementLog_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(commandPercentageDiscount, "@UserInfoId", DbType.Int32, restaurentBillBO.LastModifiedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                }
                            }

                            //    if (status > 0 && restaurentBillBO.SourceName == "RestaurantTable")
                            //    {
                            //        string tableIdLst = string.Empty;

                            //        foreach (RestaurantBillDetailBO dt in BillDetail)
                            //        {
                            //            if (tableIdLst != string.Empty)
                            //                tableIdLst += "," + dt.TableId.ToString();
                            //            else
                            //                tableIdLst = dt.TableId.ToString();
                            //        }

                            //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantCostCenterTableMappingInfo_SP"))
                            //        {
                            //            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, restaurentBillBO.CostCenterId);
                            //            dbSmartAspects.AddInParameter(command, "@TableIdList", DbType.String, tableIdLst);

                            //            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            //        }
                            //    }


                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public RestaurantBillDistribution GetRestaurantBillDistribution(string TransactionType, int CostCenterId, Int64? TransactionId,
                                                                    decimal? TransactionalAmount, string DiscountType, decimal Discount,
                                                                    bool IsVatEnable, bool IsServiceChargeEnable, bool IsSDChargeEnable,
                                                                    bool IsAdditionalChargeEnable, bool IsInclusiveOrExclusive)
        {
            RestaurantBillDistribution entityBOList = new RestaurantBillDistribution();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillDistribution"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, TransactionType);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, CostCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int64, TransactionId);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionalAmount", DbType.Decimal, TransactionalAmount);
                    dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, DiscountType);
                    dbSmartAspects.AddInParameter(cmd, "@Discount", DbType.Decimal, Discount);
                    dbSmartAspects.AddInParameter(cmd, "@IsVatEnable", DbType.Boolean, IsVatEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsServiceChargeEnable", DbType.Boolean, IsServiceChargeEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsSDChargeEnable", DbType.Boolean, IsSDChargeEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsAdditionalChargeEnable", DbType.Boolean, IsAdditionalChargeEnable);
                    dbSmartAspects.AddInParameter(cmd, "@IsInclusiveOrExclusive", DbType.Boolean, IsInclusiveOrExclusive);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    entityBOList = Table.AsEnumerable().Select(r => new RestaurantBillDistribution
                    {
                        TransactionId = r.Field<long?>("TransactionId"),
                        RackRate = r.Field<decimal?>("RackRate"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        SDCityCharge = r.Field<decimal?>("SDCityCharge"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        AdditionalCharge = r.Field<decimal?>("AdditionalCharge"),
                        CalculatedAmount = r.Field<decimal?>("CalculatedAmount")

                    }).FirstOrDefault();
                }
            }

            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillDistribution"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, TransactionType);
            //        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, CostCenterId);
            //        dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int64, TransactionId);
            //        dbSmartAspects.AddInParameter(cmd, "@TransactionalAmount", DbType.Decimal, TransactionalAmount);
            //        dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, DiscountType);
            //        dbSmartAspects.AddInParameter(cmd, "@Discount", DbType.Decimal, Discount);
            //        dbSmartAspects.AddInParameter(cmd, "@IsVatEnable", DbType.Boolean, IsVatEnable);
            //        dbSmartAspects.AddInParameter(cmd, "@IsServiceChargeEnable", DbType.Boolean, IsServiceChargeEnable);
            //        dbSmartAspects.AddInParameter(cmd, "@IsSDChargeEnable", DbType.Boolean, IsSDChargeEnable);
            //        dbSmartAspects.AddInParameter(cmd, "@IsAdditionalChargeEnable", DbType.Boolean, IsAdditionalChargeEnable);
            //        dbSmartAspects.AddInParameter(cmd, "@IsInclusiveOrExclusive", DbType.Boolean, IsInclusiveOrExclusive);



            //        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    RestaurantBillDistribution entityBO = new RestaurantBillDistribution();
            //                    entityBO.BillId = Convert.ToInt32(reader["BillId"]);
            //                    entityBO.BillDate = Convert.ToDateTime(reader["BillDate"]);
            //                    entityBO.BillNumber = reader["BillNumber"].ToString();
            //                    entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
            //                    entityBO.CostCenter = reader["CostCenter"].ToString();
            //                    entityBOList.Add(entityBO);
            //                }
            //            }
            //        }
            //    }
            //}

            return entityBOList;
        }





        public bool RestaurantBillReSettlementLog(int billId, int userInfoId)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("RestaurantBillReSettlementLog_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandPercentageDiscount, "@BillId", DbType.Int32, billId);
                            dbSmartAspects.AddInParameter(commandPercentageDiscount, "@UserInfoId", DbType.Int32, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public List<RestaurantBillReSettlementLogReportBO> GetRestaurantBillReSettlementLogReport(int costCenterId, string billNumber, DateTime dateFrom, DateTime dateTo)
        {
            List<RestaurantBillReSettlementLogReportBO> entityBOList = new List<RestaurantBillReSettlementLogReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RestaurantBillReSettlementLogReport_SP"))
                {
                    if (costCenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(billNumber))
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    entityBOList = Table.AsEnumerable().Select(r => new RestaurantBillReSettlementLogReportBO
                    {
                        ResettlementHistoryId = r.Field<Int64>("ResettlementHistoryId"),
                        BillId = r.Field<Int32>("BillId"),
                        BillNumber = r.Field<string>("BillNumber"),
                        KotId = r.Field<Int32>("BillId"),
                        ResettlementDate = r.Field<DateTime>("ResettlementDate"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        UserName = r.Field<string>("UserName"),
                        CalculatedDiscountAmount = r.Field<decimal>("CalculatedDiscountAmount"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitRate = r.Field<decimal>("UnitRate"),
                        Amount = r.Field<decimal>("Amount"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return entityBOList;
        }

        public List<RestaurantBillForReturnToStoreView> GetRestaurantBillInfoBySearchCriteriaForReturnToStock(DateTime? formDate, DateTime? toDate, string billNumber)
        {
            List<RestaurantBillForReturnToStoreView> entityBOList = new List<RestaurantBillForReturnToStoreView>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillsForReturnToStore_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(billNumber))
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, DBNull.Value);

                    if (formDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, formDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillForReturnToStoreView entityBO = new RestaurantBillForReturnToStoreView();

                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.CostCenter = reader["CostCenter"].ToString();
                                entityBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                entityBO.Location = reader["Name"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityBOList;
        }

        public List<RestaurantBillBO> GetRestaurantBillInfoByDateAfterDayClose(DateTime date, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RestaurantBillBO> entityBOList = new List<RestaurantBillBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillInfoByDateAfterDayClose_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@Date", DbType.DateTime, date);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBillBO entityBO = new RestaurantBillBO();
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.BillDate = Convert.ToDateTime(reader["BillDate"]);
                                //entityBO.IsBillSettlement = Convert.ToBoolean(reader["IsBillSettlement"]);
                                //entityBO.CustomerName = reader["CustomerName"].ToString();
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                //entityBO.BillStatus = reader["BillStatus"].ToString();
                                //entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                //entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                                entityBO.RoundedGrandTotal = Convert.ToDecimal(reader["RoundedGrandTotal"]);
                                entityBO.CostCenter = Convert.ToString(reader["CostCenter"]);
                                entityBO.IsSynced = Convert.ToBoolean(reader["IsSynced"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return entityBOList;
        }

        public bool UpdateVoucherSyncInformation(int billId, string billNumber, int userInfoId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBillSyncInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@BillId", DbType.Int32, billId);
                            dbSmartAspects.AddInParameter(commandMaster, "@UserInfoId", DbType.Int32, userInfoId);
                            dbSmartAspects.AddInParameter(commandMaster, "@BillNumber", DbType.String, billNumber);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }
    }
}
