using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.RetailPOS;
using HotelManagement.Entity.Membership;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurentPosDA : BaseService
    {
        public int SaveRestaurantBillForPos(string formName, KotBillMasterBO billmaster, List<KotBillDetailBO> billDetails,
                                            RestaurantBillBO restaurentBillBO, List<GuestBillPaymentBO> paymentDetail,
                                            List<RestaurantSalesReturnItemBO> salesReturnItem,
                                            string categoryIdList, bool isAmountWillDistribution, bool isBillWillSettle, out int billID, int memberId)
        {
            int status = 0;
            int kotId = 0;
            billID = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillMasterForRetailPos_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, billmaster.BearerId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, billmaster.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, billmaster.SourceName);
                            dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, billmaster.SourceId);
                            dbSmartAspects.AddInParameter(command, "@TokenNumber", DbType.String, billmaster.TokenNumber);
                            dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, billmaster.TotalAmount);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, billmaster.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.Boolean, billmaster.IsBillHoldup);
                            dbSmartAspects.AddInParameter(command, "@KotStatus", DbType.String, billmaster.KotStatus);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billmaster.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@KotId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            kotId = Convert.ToInt32(command.Parameters["@KotId"].Value);
                        }

                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForRetailPOSReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, billmaster.ReferenceKotId);
                                dbSmartAspects.AddInParameter(command, "@IsKotReturn", DbType.Boolean, billmaster.IsKotReturn);
                                dbSmartAspects.AddInParameter(command, "@ReferenceKotId", DbType.Int32, kotId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfo_SP"))
                            {
                                foreach (KotBillDetailBO kbd in billDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);

                                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, kbd.ColorId);
                                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, kbd.SizeId);
                                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, kbd.StyleId);

                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billmaster.CreatedBy);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        if (status > 0 && salesReturnItem.Count > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailsForRetailPOSReturn_SP"))
                            {
                                foreach (RestaurantSalesReturnItemBO kbd in salesReturnItem)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kbd.KotDetailId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@IsItemReturn", DbType.Boolean, true);
                                    dbSmartAspects.AddInParameter(command, "@ReturnQuantity", DbType.Decimal, kbd.ItemUnit);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantSalesReturnItemForRetailPOS_SP"))
                            {
                                foreach (RestaurantSalesReturnItemBO kbd in salesReturnItem)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);
                                    dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, kbd.ItemName);
                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurentBillForRetailPOS_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                                dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                                //dbSmartAspects.AddInParameter(command, "@BillPaymentDate", DbType.DateTime, restaurentBillBO.BillPaymentDate);
                                dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                                dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                                dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                dbSmartAspects.AddInParameter(command, "@DeliveredBy", DbType.Int32, restaurentBillBO.DeliveredBy);
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
                                dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                                dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                                dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                                dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                                dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                                dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                                dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                                dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                                dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                                dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);
                                dbSmartAspects.AddInParameter(command, "@RefundId", DbType.Int32, restaurentBillBO.RefundId);
                                dbSmartAspects.AddInParameter(command, "@RefundRemarks", DbType.String, restaurentBillBO.RefundRemarks);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);
                                dbSmartAspects.AddInParameter(command, "@BillDeclaration", DbType.String, restaurentBillBO.BillDeclaration);

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


                                dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, kotId);
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);

                                if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                                else
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);

                                if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                    dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                                else
                                    dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                                if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                    dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                                else
                                    dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                                if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                    dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                                else
                                    dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);

                                dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);

                                billID = Convert.ToInt32(command.Parameters["@BillId"].Value);

                            }
                        }

                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBillForRetailPOSReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@ExchangeItemVatAmount", DbType.Decimal, restaurentBillBO.ExchangeItemVatAmount);
                                dbSmartAspects.AddInParameter(command, "@ExchangeItemTotal", DbType.Decimal, restaurentBillBO.ExchangeItemTotal);
                                dbSmartAspects.AddInParameter(command, "@ReferenceBillId", DbType.Int32, billID);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        //Save Kot Information ------------------------------------
                        if (status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                                dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, kotId);

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




                        if (status > 0 && paymentDetail != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentDetail)
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
                                if (formName == "Billing")
                                {
                                    using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcessForBilling_SP"))
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
                                else
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
                        }

                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcessForRetailPosSalesReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, "Percentage");
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, 0.00);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
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
        public int SaveRestaurantBillForBilling(string formName, KotBillMasterBO billmaster, List<KotBillDetailBO> billDetails,
                                            List<PMProductOutSerialInfoBO> AddedSerialzableProduct, List<PMProductOutSerialInfoBO> DeletedSerialzableProduct,
                                            RestaurantBillBO restaurentBillBO, List<GuestBillPaymentBO> paymentDetail,
                                            List<RestaurantSalesReturnItemBO> salesReturnItem,
                                            string categoryIdList, bool isAmountWillDistribution, bool isBillWillSettle, out int billID, int memberId)
        {
            int status = 0;
            int kotId = 0;
            billID = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillMasterForRetailPos_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, billmaster.BearerId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, billmaster.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, billmaster.SourceName);
                            dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, billmaster.SourceId);
                            dbSmartAspects.AddInParameter(command, "@TokenNumber", DbType.String, billmaster.TokenNumber);
                            dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, billmaster.TotalAmount);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, billmaster.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.Boolean, billmaster.IsBillHoldup);
                            dbSmartAspects.AddInParameter(command, "@KotStatus", DbType.String, billmaster.KotStatus);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billmaster.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@KotId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            kotId = Convert.ToInt32(command.Parameters["@KotId"].Value);
                        }

                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForRetailPOSReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, billmaster.ReferenceKotId);
                                dbSmartAspects.AddInParameter(command, "@IsKotReturn", DbType.Boolean, billmaster.IsKotReturn);
                                dbSmartAspects.AddInParameter(command, "@ReferenceKotId", DbType.Int32, kotId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfoForBilling_SP"))
                            {
                                foreach (KotBillDetailBO kbd in billDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);

                                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, kbd.ColorId);
                                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, kbd.SizeId);
                                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, kbd.StyleId);

                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billmaster.CreatedBy);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0 && salesReturnItem.Count > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailsForRetailPOSReturn_SP"))
                            {
                                foreach (RestaurantSalesReturnItemBO kbd in salesReturnItem)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kbd.KotDetailId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@IsItemReturn", DbType.Boolean, true);
                                    dbSmartAspects.AddInParameter(command, "@ReturnQuantity", DbType.Decimal, kbd.ItemUnit);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantSalesReturnItemForRetailPOS_SP"))
                            {
                                foreach (RestaurantSalesReturnItemBO kbd in salesReturnItem)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);
                                    dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, kbd.ItemName);
                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurentBillForRetailPOS_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, restaurentBillBO.TableId);
                                dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);
                                dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, restaurentBillBO.BillDate);
                                dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, restaurentBillBO.PaxQuantity);
                                dbSmartAspects.AddInParameter(command, "@CustomerName", DbType.String, restaurentBillBO.CustomerName);
                                dbSmartAspects.AddInParameter(command, "@CustomerMobile", DbType.String, restaurentBillBO.CustomerMobile);
                                dbSmartAspects.AddInParameter(command, "@CustomerAddress", DbType.String, restaurentBillBO.CustomerAddress);
                                dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, restaurentBillBO.BearerId);
                                dbSmartAspects.AddInParameter(command, "@DeliveredBy", DbType.Int32, restaurentBillBO.DeliveredBy);
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
                                dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                                dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                                dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

                                dbSmartAspects.AddInParameter(command, "@IsInvoiceServiceChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceServiceChargeEnable);
                                dbSmartAspects.AddInParameter(command, "@IsInvoiceVatAmountEnable", DbType.Boolean, restaurentBillBO.IsInvoiceVatAmountEnable);
                                dbSmartAspects.AddInParameter(command, "@IsInvoiceCitySDChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceCitySDChargeEnable);
                                dbSmartAspects.AddInParameter(command, "@IsInvoiceAdditionalChargeEnable", DbType.Boolean, restaurentBillBO.IsInvoiceAdditionalChargeEnable);
                                dbSmartAspects.AddInParameter(command, "@CitySDCharge", DbType.Decimal, restaurentBillBO.CitySDCharge);
                                dbSmartAspects.AddInParameter(command, "@AdditionalChargeType", DbType.String, restaurentBillBO.AdditionalChargeType);
                                dbSmartAspects.AddInParameter(command, "@AdditionalCharge", DbType.Decimal, restaurentBillBO.AdditionalCharge);
                                dbSmartAspects.AddInParameter(command, "@RefundId", DbType.Int32, restaurentBillBO.RefundId);
                                dbSmartAspects.AddInParameter(command, "@RefundRemarks", DbType.String, restaurentBillBO.RefundRemarks);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);
                                dbSmartAspects.AddInParameter(command, "@BillDeclaration", DbType.String, restaurentBillBO.BillDeclaration);

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


                                dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, kotId);
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);

                                if (!string.IsNullOrEmpty(restaurentBillBO.Remarks))
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, restaurentBillBO.Remarks);
                                else
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, restaurentBillBO.ProjectId);

                                if (!string.IsNullOrEmpty(restaurentBillBO.Subject))
                                    dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, restaurentBillBO.Subject);
                                else
                                    dbSmartAspects.AddInParameter(command, "@Subject", DbType.String, DBNull.Value);

                                if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                    dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                                else
                                    dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                                if (!string.IsNullOrEmpty(restaurentBillBO.BillType))
                                    dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, restaurentBillBO.BillType);
                                else
                                    dbSmartAspects.AddInParameter(command, "@BillType", DbType.String, DBNull.Value);

                                if (!string.IsNullOrEmpty(restaurentBillBO.BillingType))
                                    dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, restaurentBillBO.BillingType);
                                else
                                    dbSmartAspects.AddInParameter(command, "@BillingType", DbType.String, DBNull.Value);

                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);

                                dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);

                                billID = Convert.ToInt32(command.Parameters["@BillId"].Value);

                            }
                        }

                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurentBillForRetailPOSReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@ExchangeItemVatAmount", DbType.Decimal, restaurentBillBO.ExchangeItemVatAmount);
                                dbSmartAspects.AddInParameter(command, "@ExchangeItemTotal", DbType.Decimal, restaurentBillBO.ExchangeItemTotal);
                                dbSmartAspects.AddInParameter(command, "@ReferenceBillId", DbType.Int32, billID);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        //Save Kot Information ------------------------------------
                        if (status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                                dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, kotId);

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

                        if (status > 0 && AddedSerialzableProduct.Count > 0)
                        {

                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillItemSerialInfo_SP"))
                            {
                                foreach (PMProductOutSerialInfoBO rd in AddedSerialzableProduct)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@BillId", DbType.Int64, billID);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, billmaster.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && paymentDetail != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentDetail)
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

                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, guestBillPaymentBO.CardReference);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, guestBillPaymentBO.CardHolderName);
                                        if(guestBillPaymentBO.PaymentDescription == "")
                                        {
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, "Bill Detail: " + restaurentBillBO.Remarks);                                            
                                        }
                                        else
                                        {
                                            string PaymentDescription = guestBillPaymentBO.PaymentDescription + "[Bill Detail: " + restaurentBillBO.Remarks + "]";
                                            dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, PaymentDescription);
                                        }

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
                                if (formName == "Billing")
                                {
                                    using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcessForBilling_SP"))
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
                                else
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
                        }

                        if (status > 0 && restaurentBillBO.IsBillReSettlement)
                        {
                            using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcessForRetailPosSalesReturn_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, "Percentage");
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, 0.00);
                                dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
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
        public void BillingAccountsVoucherPostingProcess(int billID)
        {
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("BillingAccountsVoucherPostingProcess_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                        dbSmartAspects.AddOutParameter(commandBillDetails, "@mErr", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
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
        }

        public void SaveMembershipPointDetails(RestaurantBillBO restaurentBillBO, int memberId, int billID)
        {
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    //save Point Details Information
                    if (restaurentBillBO.PointAmount > 0 && memberId > 0)
                    {

                        using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveMembershipPointDetails_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandBillDetails, "@MemberId", DbType.Int32, memberId);
                            dbSmartAspects.AddInParameter(commandBillDetails, "@PaymentAmount", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                            dbSmartAspects.AddInParameter(commandBillDetails, "@PointWiseAmount", DbType.Decimal, restaurentBillBO.PointAmount);
                            dbSmartAspects.AddInParameter(commandBillDetails, "@PointType", DbType.String, "Redeemed");
                            dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                            status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                        }

                    }

                    if (memberId > 0)
                    {

                        using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveMembershipPointDetails_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandBillDetails, "@MemberId", DbType.Int32, memberId);
                            dbSmartAspects.AddInParameter(commandBillDetails, "@PaymentAmount", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                            dbSmartAspects.AddInParameter(commandBillDetails, "@PointWiseAmount", DbType.Decimal, restaurentBillBO.PointAmount);
                            dbSmartAspects.AddInParameter(commandBillDetails, "@PointType", DbType.String, "Awarded");
                            dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                            status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
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

        }

        public int UpdateBillForFullRefund(int memberId, RestaurantBillBO restaurentBillBO, GuestBillPaymentBO BillPayment)
        {
            //
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBillForFullRefund_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            dbSmartAspects.AddInParameter(command, "@memberId", DbType.Int32, memberId);
                            dbSmartAspects.AddInParameter(command, "@PointAmount", DbType.Decimal, restaurentBillBO.PointAmount);
                            dbSmartAspects.AddInParameter(command, "@RefundRemarks", DbType.String, restaurentBillBO.RefundRemarks);
                            dbSmartAspects.AddInParameter(command, "@RefundId", DbType.Int32, restaurentBillBO.RefundId);
                            dbSmartAspects.AddInParameter(command, "@ModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }
                        using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        {
                            if (BillPayment.PaidServiceId == 0)
                            {
                                commandGuestBillPayment.Parameters.Clear();

                                BillPayment.CreatedBy = restaurentBillBO.CreatedBy;
                                BillPayment.PaymentDate = DateTime.Now;

                                int companyId = 0;
                                if (BillPayment.CompanyId != 0)
                                {
                                    companyId = BillPayment.CompanyId;
                                    if (BillPayment.PaymentMode == "Company")
                                    {
                                        BillPayment.PaymentType = "Company";
                                    }
                                }

                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, BillPayment.PaymentDate);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, BillPayment.PaymentType);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, BillPayment.RegistrationId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@FieldId", DbType.Int32, BillPayment.FieldId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, BillPayment.CurrencyAmount);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentAmount", DbType.Decimal, BillPayment.PaymentAmount);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentMode", DbType.String, BillPayment.PaymentMode);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountsPostingHeadId", DbType.Int32, BillPayment.AccountsPostingHeadId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CompanyId", DbType.Int32, companyId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BankId", DbType.Int32, BillPayment.BankId);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BranchName", DbType.String, BillPayment.BranchName);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueNumber", DbType.String, BillPayment.ChecqueNumber);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ChecqueDate", DbType.DateTime, BillPayment.ChecqueDate);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardType", DbType.String, BillPayment.CardType);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardNumber", DbType.String, BillPayment.CardNumber);
                                if (BillPayment.PaymentMode == "Card")
                                {
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ExpireDate", DbType.DateTime, BillPayment.ExpireDate);
                                }

                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardReference", DbType.String, BillPayment.CardReference);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CardHolderName", DbType.String, BillPayment.CardHolderName);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDescription", DbType.String, BillPayment.PaymentDescription);
                                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                            }

                        }
                        transction.Commit();
                    }

                    catch (Exception e)
                    {

                    }
                }

            }

            return status;

        }

        public MemMemberBasicsBO GetPointsByCustomerCode(string customerCode)
        {
            decimal point = 0;
            MemMemberBasicsBO memBasicInfo = new MemMemberBasicsBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPointsByCustomerCode"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomerCode", DbType.String, customerCode);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                memBasicInfo.AchievePoint = decimal.Parse(reader["AchievePoint"].ToString());
                                memBasicInfo.MemberId = int.Parse(reader["MemberId"].ToString());
                                memBasicInfo.MemberName = reader["FullName"].ToString();
                                memBasicInfo.PointWiseAmount = decimal.Parse(reader["PointWiseAmount"].ToString());

                            }
                        }
                    }

                }
            }
            return memBasicInfo;
        }

        public int UpdateRestaurantBillForPos(string formName, int kotId, KotBillMasterBO billmaster, List<KotBillDetailBO> billDetails, List<KotBillDetailBO> editeDetails, List<KotBillDetailBO> deletedDetails, RestaurantBillBO restaurentBillBO, List<GuestBillPaymentBO> paymentDetail,
                                              string categoryIdList, bool isAmountWillDistribution, bool isBillWillSettle, out int billID)
        {
            int status = 0;
            billID = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForSettlement_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                            dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, billmaster.IsBillHoldup);
                            dbSmartAspects.AddInParameter(command, "@IsBillProcessed", DbType.String, billmaster.IsBillProcessed);
                            dbSmartAspects.AddInParameter(command, "@KotStatus", DbType.String, billmaster.KotStatus);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0 && billDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfo_SP"))
                            {
                                foreach (KotBillDetailBO kbd in billDetails)
                                {
                                    command.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);

                                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, 0);

                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);

                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        else if (editeDetails.Count > 0 || deletedDetails.Count > 0) { status = 1; }

                        if (status > 0 && editeDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetailsForRetailPOS_SP"))
                            {
                                foreach (KotBillDetailBO kbd in editeDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kbd.KotDetailId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);
                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        else if (deletedDetails.Count > 0) { status = 1; }

                        if (status > 0 && deletedDetails.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (KotBillDetailBO pd in deletedDetails)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "RestaurantKotBillDetail");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "KotDetailId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.KotDetailId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }

                        if (status > 0)
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
                                dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                                dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                                dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

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

                                dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, kotId);
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);

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
                        }

                        //Save Kot Information ------------------------------------
                        if (status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                                dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, kotId);

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

                        if (status > 0 && paymentDetail != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in paymentDetail)
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
                                if (formName == "Billing")
                                {
                                    using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcessForBilling_SP"))
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
                                else
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

                                //using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                //{
                                //    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, billID);

                                //    if (!string.IsNullOrEmpty(categoryIdList))
                                //    {
                                //        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, categoryIdList.Trim());
                                //    }
                                //    else
                                //    {
                                //        dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);
                                //    }

                                //    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                //    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                //    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                //    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                //}
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
                                    dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, string.Empty);

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

        public int SaveRestaurantBillHoldUpForPos(KotBillMasterBO billmaster, List<KotBillDetailBO> billDetails, out int kotId)
        {
            int status = 0;
            kotId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillMasterInfoForAll_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, billmaster.BearerId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, billmaster.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, billmaster.SourceName);
                            dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, billmaster.SourceId);
                            dbSmartAspects.AddInParameter(command, "@TokenNumber", DbType.String, billmaster.TokenNumber);
                            dbSmartAspects.AddInParameter(command, "@TotalAmount", DbType.Decimal, billmaster.TotalAmount);
                            dbSmartAspects.AddInParameter(command, "@PaxQuantity", DbType.Int32, billmaster.PaxQuantity);
                            dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.Boolean, billmaster.IsBillHoldup);
                            dbSmartAspects.AddInParameter(command, "@KotStatus", DbType.String, billmaster.KotStatus);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billmaster.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@KotId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command);

                            kotId = Convert.ToInt32(command.Parameters["@KotId"].Value);
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfo_SP"))
                            {
                                foreach (KotBillDetailBO kbd in billDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);

                                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, 0);

                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, kbd.CreatedBy);

                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command);
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
                        throw ex;
                    }
                }
            }

            return kotId;
        }

        public int UpdateRestaurantBillHoldUpForPos(int kotId, List<KotBillDetailBO> billDetails, List<KotBillDetailBO> editeDetails, List<KotBillDetailBO> deletedDetails)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (billDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfo_SP"))
                            {
                                foreach (KotBillDetailBO kbd in billDetails)
                                {
                                    command.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);

                                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, 0);

                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, kbd.CreatedBy);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        else if (editeDetails.Count > 0 || deletedDetails.Count > 0) { status = 1; }

                        if (status > 0 && editeDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetails_SP"))
                            {
                                foreach (KotBillDetailBO kbd in editeDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kbd.KotDetailId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);
                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);

                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, kbd.CreatedBy);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);


                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        else if (deletedDetails.Count > 0) { status = 1; }

                        if (status > 0 && deletedDetails.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (KotBillDetailBO pd in deletedDetails)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "RestaurantKotBillDetail");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "KotDetailId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.KotDetailId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
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
                        throw ex;
                    }
                }
            }

            return kotId;
        }

        public bool UpdateForRestauranBillReSettlement(string formName, int kotId, RestaurantBillBO restaurentBillBO, List<KotBillDetailBO> billDetails, List<KotBillDetailBO> editeDetails,
                                                       List<KotBillDetailBO> deletedDetails, List<GuestBillPaymentBO> paymentAdded)
        {
            int status = 0;
            bool retVal = false;
            string paymentDeleteQuery = string.Empty, billNPaymentDateUpdate = string.Empty, companyPaymentDeleteQuery = string.Empty;

            paymentDeleteQuery = string.Format("DELETE FROM HotelGuestBillPayment WHERE  ModuleName = '{0}' AND ServiceBillId = {1}", "Restaurant", restaurentBillBO.BillId);
            companyPaymentDeleteQuery = string.Format("DELETE FROM HotelCompanyPaymentLedger WHERE  ModuleName = '{0}' AND BillId = {1}", "Restaurant", restaurentBillBO.BillId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        var isOtherPayment = paymentAdded.Where(p => p.PaymentMode != "Other Room").FirstOrDefault();

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
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.Boolean, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@IsNonChargeable", DbType.Boolean, restaurentBillBO.IsNonChargeable);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
                            dbSmartAspects.AddInParameter(command, "@RoundedAmount", DbType.Decimal, restaurentBillBO.RoundedAmount);
                            dbSmartAspects.AddInParameter(command, "@RoundedGrandTotal", DbType.Decimal, restaurentBillBO.RoundedGrandTotal);
                            dbSmartAspects.AddInParameter(command, "@InvoiceServiceRate", DbType.Decimal, restaurentBillBO.InvoiceServiceRate);

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

                            dbSmartAspects.AddInParameter(command, "@BillPaidBySourceId", DbType.Int32, kotId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);

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

                        if (billDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveKotBillDetailInfo_SP"))
                            {
                                foreach (KotBillDetailBO kbd in billDetails)
                                {
                                    command.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);

                                    dbSmartAspects.AddInParameter(command, "@ColorId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@SizeId", DbType.Int32, 0);
                                    dbSmartAspects.AddInParameter(command, "@StyleId", DbType.Int32, 0);

                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        else if (editeDetails.Count > 0 || deletedDetails.Count > 0) { status = 1; }

                        if (status > 0 && editeDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillDetails_SP"))
                            {
                                foreach (KotBillDetailBO kbd in editeDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, kbd.KotDetailId);
                                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kbd.KotId);
                                    dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, kbd.ItemType);
                                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, kbd.ItemId);
                                    dbSmartAspects.AddInParameter(command, "@ItemUnit", DbType.Decimal, kbd.ItemUnit);
                                    dbSmartAspects.AddInParameter(command, "@UnitRate", DbType.Decimal, kbd.UnitRate);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, kbd.Amount);
                                    dbSmartAspects.AddInParameter(command, "@InvoiceDiscount", DbType.Decimal, kbd.InvoiceDiscount);
                                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, restaurentBillBO.LastModifiedBy);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, kbd.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        else if (deletedDetails.Count > 0) { status = 1; }

                        if (status > 0 && deletedDetails.Count > 0)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (KotBillDetailBO pd in deletedDetails)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "RestaurantKotBillDetail");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "KotDetailId");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, pd.KotDetailId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }
                        }
                        status = 1;

                        if (status > 0)
                        {
                            using (DbCommand commandPaymentDeletet = dbSmartAspects.GetSqlStringCommand(paymentDeleteQuery))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(commandPaymentDeletet, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandPaymentDeletet = dbSmartAspects.GetSqlStringCommand(companyPaymentDeleteQuery))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(commandPaymentDeletet, transction);
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
                                            if (guestBillPaymentBO.PaymentMode == "Company")
                                            {
                                                guestBillPaymentBO.PaymentType = "Company";
                                            }
                                        }

                                        guestBillPaymentBO.PaymentDate = restaurentBillBO.BillDate;
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ModuleName", DbType.String, "Restaurant");
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@RegistrationId", DbType.Int32, restaurentBillBO.RegistrationId);
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
                                        status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    }
                                }
                            }
                        }

                        //if (status > 0 && isOtherPayment != null)
                        //{
                        //    billNPaymentDateUpdate = string.Format("UPDATE RestaurantBill SET BillDate = '{1}' WHERE BillId = {0} UPDATE HotelGuestBillPayment SET PaymentDate = '{1}' WHERE ServiceBillId = {0}", restaurentBillBO.BillId, restaurentBillBO.BillDate);

                        //    using (DbCommand command = dbSmartAspects.GetSqlStringCommand(billNPaymentDateUpdate))
                        //    {
                        //        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        //    }
                        //}

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateKotBillMasterForHoldUp_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, kotId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                dbSmartAspects.AddInParameter(command, "@IsBillHoldup", DbType.String, false);
                                dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, restaurentBillBO.SourceName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            if (formName == "Billing")
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcessForBilling_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }
                            else
                            {
                                using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);

                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                                    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                                }
                            }

                            //using (DbCommand cmdVSCSR = dbSmartAspects.GetStoredProcCommand("ServiceRateNVatNServiceChargeProcess_SP"))
                            //{
                            //    dbSmartAspects.AddInParameter(cmdVSCSR, "@BillId", DbType.Int32, restaurentBillBO.BillId);
                            //    dbSmartAspects.AddInParameter(cmdVSCSR, "@CategoryIdList", DbType.String, DBNull.Value);

                            //    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            //    dbSmartAspects.AddInParameter(cmdVSCSR, "@DiscountPercent", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            //    dbSmartAspects.AddInParameter(cmdVSCSR, "@CostCenterId", DbType.Int32, restaurentBillBO.CostCenterId);

                            //    status = dbSmartAspects.ExecuteNonQuery(cmdVSCSR, transction);
                            //}
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

        public List<RestaurantTokenBO> GetHoldUpPosInfo()
        {
            List<RestaurantTokenBO> token = new List<RestaurantTokenBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHoldupPosDetails_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantTokenInfo");
                    DataTable Table = ds.Tables["RestaurantTokenInfo"];

                    token = Table.AsEnumerable().Select(r => new RestaurantTokenBO
                    {
                        KotId = r.Field<Int32>("KotId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        SourceId = r.Field<Int32>("KotId"),
                        TokenNumber = r.Field<string>("TokenNumber"),
                        IsBillHoldup = r.Field<bool>("IsBillHoldup")

                    }).ToList();
                }
            }

            return token;
        }

        public List<InvItemAutoSearchBO> GetOrderedItemByKotId(int kotId, int costCenterId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOrderedItemByKotId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        KotDetailId = r.Field<int>("KotDetailId"),
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead"),
                        Quantity = r.Field<decimal>("Quantity"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        InvoiceDiscount = r.Field<decimal>("InvoiceDiscount")

                    }).ToList();
                }
            }

            return itemInfo;
        }


        public GuestExtraServiceBillApprovedBO GetRoomWiseRestaurantBillPaymentByBillIdServiceTypePaymentMode(int serviceBillId)
        {
            GuestExtraServiceBillApprovedBO roomWisePayment = new GuestExtraServiceBillApprovedBO();
            string query = string.Format("SELECT * FROM HotelGuestExtraServiceBillApproved WHERE ServiceBillId = {0} AND ServiceType = '{1}' AND PaymentMode = '{2}'", serviceBillId, "RestaurantService", "Other Room");

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillInfo");
                    DataTable Table = ds.Tables["BillInfo"];

                    roomWisePayment = Table.AsEnumerable().Select(r => new GuestExtraServiceBillApprovedBO
                    {
                        ApprovedId = r.Field<int>("ApprovedId"),
                        CostCenterId = r.Field<int?>("CostCenterId"),
                        RegistrationId = r.Field<int?>("RegistrationId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        ApprovedDate = r.Field<DateTime?>("ApprovedDate"),
                        ServiceBillId = r.Field<int>("ServiceBillId"),
                        ServiceDate = r.Field<DateTime?>("ServiceDate"),
                        ServiceType = r.Field<string>("ServiceType"),
                        ServiceId = r.Field<int?>("ServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        ServiceQuantity = r.Field<decimal?>("ServiceQuantity"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        VatAmount = r.Field<decimal?>("VatAmount"),
                        ServiceCharge = r.Field<decimal?>("ServiceCharge"),
                        InvoiceServiceRate = r.Field<decimal?>("InvoiceServiceRate"),
                        IsServiceChargeEnable = r.Field<bool?>("IsServiceChargeEnable"),
                        InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                        IsVatAmountEnable = r.Field<bool?>("IsVatAmountEnable"),
                        InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                        CalculatedTotalAmount = r.Field<decimal?>("CalculatedTotalAmount"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        IsPaidService = r.Field<bool?>("IsPaidService"),
                        IsPaidServiceAchieved = r.Field<bool?>("IsPaidServiceAchieved"),
                        IsDayClosed = r.Field<bool?>("IsDayClosed")

                    }).FirstOrDefault();
                }
            }

            return roomWisePayment;
        }


        //----------------------------------------------
        public List<RestaurantBillDetailBO> GetRestaurantBillDetailsByBillId(int billId)
        {
            List<RestaurantBillDetailBO> entityBOList = new List<RestaurantBillDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillDetailsByBillId_SP"))
                {
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
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
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

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            //dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, restaurentBillBO.DealId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            billID = Convert.ToInt32(command.Parameters["@BillId"].Value);

                        }

                        //Save Kot Information ------------------------------------
                        if (BillDetail != null && status > 0)
                        {
                            foreach (RestaurantBillDetailBO row in BillDetail)
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

                                    status = dbSmartAspects.ExecuteNonQuery(command);
                                }
                            }

                            if (status > 0 && restaurentBillBO.SourceName == "RestaurantToken")
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
        string categoryIdList, bool isBillWillSettle)
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
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
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
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList)
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
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
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

                        if (BillDetail != null && status > 0)
                        {
                            using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetail_SP"))
                            {
                                foreach (RestaurantBillDetailBO row in BillDetail)
                                {
                                    if (row.BillId == 0)
                                    {
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

                                    status = dbSmartAspects.ExecuteNonQuery(command);
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
        public int SaveRestaurantBillForNewHoldUp(RestaurantBillBO restaurentBillBO, List<RestaurantBillDetailBO> BillDetail, List<GuestBillPaymentBO> guestPaymentDetailList,
                                                  List<ItemClassificationBO> AddedClassificationList, out int billID)
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
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
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

                            if (!string.IsNullOrEmpty(restaurentBillBO.PaymentRemarks))
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, restaurentBillBO.PaymentRemarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentRemarks", DbType.String, DBNull.Value);

                            //dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, restaurentBillBO.DealId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, restaurentBillBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@BillId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            billID = Convert.ToInt32(command.Parameters["@BillId"].Value);

                        }

                        //Save Kot Information ------------------------------------
                        if (BillDetail != null && status > 0)
                        {
                            foreach (RestaurantBillDetailBO row in BillDetail)
                            {
                                row.BillId = billID;
                                using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBillDetailForHoldUp_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int32, billID);
                                    dbSmartAspects.AddInParameter(commandBillDetails, "@KotId", DbType.Int32, row.KotId);

                                    dbSmartAspects.AddOutParameter(commandBillDetails, "@DetailId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
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
                                        dbSmartAspects.AddOutParameter(commandPercentageDiscount, "@DiscountId", DbType.Int32, sizeof(Int32));

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
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

                        //if (status > 0 && guestPaymentDetailList != null)
                        //{
                        //    using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveGuestBillPaymentInfoModarate_SP"))
                        //    {
                        //        foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
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
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, billID);
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
                        //                dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                        //                dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@PaymentId", DbType.Int32, sizeof(Int32));
                        //                //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //                //tmpPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);

                        //                status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                        //            }
                        //        }
                        //    }
                        //}

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
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
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
                                foreach (ItemClassificationBO percentageDiscountBO in AddedClassificationList)
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
                                    if (row.BillId == 0)
                                    {
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
                            dbSmartAspects.AddInParameter(command, "@IsComplementary", DbType.String, restaurentBillBO.IsComplementary);
                            dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, restaurentBillBO.DiscountType);
                            dbSmartAspects.AddInParameter(command, "@DiscountTransactionId", DbType.Int32, restaurentBillBO.DiscountTransactionId);
                            dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, restaurentBillBO.DiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, restaurentBillBO.CalculatedDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, restaurentBillBO.ServiceCharge);
                            dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, restaurentBillBO.VatAmount);
                            dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, restaurentBillBO.GrandTotal);
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
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, restaurentBillBO.BillNumber);
                                        dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ServiceBillId", DbType.Int32, restaurentBillBO.BillId);
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


        public MembershipPointDetailsBO GetMembershipPointDetails(int billId)
        {
            MembershipPointDetailsBO membershipPointDetails = new MembershipPointDetailsBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMembershipPointDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "membershipPointDetails");
                    DataTable Table = ds.Tables["membershipPointDetails"];

                    membershipPointDetails = Table.AsEnumerable().Select(r => new MembershipPointDetailsBO
                    {
                        MemberID = r.Field<Int32>("MemberID"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        PointWiseAmount = r.Field<decimal>("PointWiseAmount"),
                        RedeemedAmount = r.Field<decimal>("RedeemedAmount"),
                        TotalPoint = r.Field<decimal>("AchievePoint"),
                        MemberCode = r.Field<string>("MembershipNumber")
                    }).FirstOrDefault();
                }
            }
            return membershipPointDetails;
        }



        public RestaurantBillBO GetRetailsPosBillByKotId(int kotId, string sourceName)
        {
            RestaurantBillBO kotBill = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillByKotIdForRetailPOS_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "KotBill");
                    DataTable Table = ds.Tables["KotBill"];

                    kotBill = Table.AsEnumerable().Select(r => new RestaurantBillBO
                    {
                        BillId = r.Field<Int32>("BillId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        IsBillSettlement = r.Field<bool>("IsBillSettlement"),
                        IsBillReSettlement = r.Field<bool>("IsBillReSettlement"),
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
                        Remarks = r.Field<string>("Remarks"),
                        IsBillPreviewButtonEnable = r.Field<bool>("IsBillPreviewButtonEnable"),

                        TransactionType = r.Field<string>("TransactionType"),
                        TransactionId = r.Field<Int64?>("TransactionId")

                    }).FirstOrDefault();
                }
            }
            return kotBill;
        }

        public RetailPosBillReturnBO GetRetailPosBillWithSalesReturn(int billId)
        {
            RetailPosBillReturnBO salesList = new RetailPosBillReturnBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RetailPosBillWithSalesReturn_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PosSales");

                    salesList.PosBillWithSalesReturn = ds.Tables[0].AsEnumerable().Select(r =>
                                new RetailPosBillWithSalesReturnBO
                                {
                                    BillId = r.Field<int>("BillId"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    ReturnBillId = r.Field<int?>("ReturnBillId"),
                                    BillDate = r.Field<DateTime?>("BillDate"),
                                    CostCenter = r.Field<string>("CostCenter"),
                                    CashierName = r.Field<string>("CashierName"),
                                    CustomerName = r.Field<string>("CustomerName"),
                                    SalesAmount = r.Field<decimal?>("SalesAmount"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    CalculatedDiscountAmount = r.Field<decimal?>("CalculatedDiscountAmount"),
                                    AmountAfterDiscount = r.Field<decimal?>("AmountAfterDiscount"),
                                    InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                                    InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                                    InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                                    GrandTotal = r.Field<decimal?>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                                    IsInvoiceVatAmountEnable = r.Field<bool?>("IsInvoiceVatAmountEnable"),
                                    IsInvoiceServiceChargeEnable = r.Field<bool?>("IsInvoiceServiceChargeEnable"),
                                    InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                                    KotId = r.Field<int>("KotId"),
                                    BearerId = r.Field<int?>("BearerId"),
                                    ReferenceKotId = r.Field<int?>("ReferenceKotId"),
                                    ItemId = r.Field<int?>("ItemId"),
                                    ItemName = r.Field<string>("ItemName"),
                                    ItemUnit = r.Field<decimal?>("ItemUnit"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    ItemTotalAmount = r.Field<decimal?>("ItemTotalAmount"),
                                    PointsAwarded = r.Field<decimal?>("PointsAwarded"),
                                    PointsRedeemed = r.Field<decimal?>("PointsRedeemed"),
                                    PointsRedeemedAmount = r.Field<decimal?>("PointsRedeemedAmount"),
                                    BalancePoints = r.Field<decimal?>("BalancePoints")

                                }).ToList();

                    salesList.PosSalesReturnItem = ds.Tables[1].AsEnumerable().Select(r =>
                                new RetailPosSalesReturnItemBO
                                {
                                    BillId = r.Field<int?>("BillId"),
                                    ReturnBillId = r.Field<int?>("ReturnBillId"),
                                    ReturnBillNumber = r.Field<string>("ReturnBillNumber"),
                                    ReturnBillDate = r.Field<DateTime?>("ReturnBillDate"),
                                    ExchangeItemVatAmount = r.Field<decimal?>("ExchangeItemVatAmount"),
                                    ExchangeItemTotal = r.Field<decimal?>("ExchangeItemTotal"),
                                    KotId = r.Field<int>("KotId"),
                                    BearerId = r.Field<int?>("BearerId"),
                                    ReferenceKotId = r.Field<int?>("ReferenceKotId"),
                                    ItemId = r.Field<int>("ItemId"),
                                    ItemName = r.Field<string>("ItemName"),
                                    ItemUnit = r.Field<decimal?>("ItemUnit"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    ItemTotalAmount = r.Field<decimal?>("ItemTotalAmount"),
                                    PointsRedeemed = r.Field<decimal?>("PointsRedeemed"),
                                    PreviousDIscount = r.Field<decimal?>("PreviousDIscount")

                                }).ToList();

                    salesList.PosSalesReturnPayment = ds.Tables[2].AsEnumerable().Select(r =>
                               new RetailPosSalesPaymentBO
                               {
                                   PaymentId = r.Field<int>("PaymentId"),
                                   BillId = r.Field<int?>("BillId"),
                                   PayMode = r.Field<string>("PayMode"),
                                   PaymentAmount = r.Field<decimal?>("PaymentAmount")

                               }).ToList();
                }
            }
            return salesList;
        }


        public List<RestaurantSalesReturnItemViewBO> GetReturnItemDetailsByBillId(int billId)
        {

            List<RestaurantSalesReturnItemViewBO> itemList = new List<RestaurantSalesReturnItemViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReturnItemDetailsByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReturnItems");
                    DataTable Table = ds.Tables["ReturnItems"];

                    itemList = Table.AsEnumerable().Select(r => new RestaurantSalesReturnItemViewBO
                    {

                        ReturnId = r.Field<long>("ReturnId"),
                        BillId = r.Field<int>("BillId"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<string>("StockBy"),
                        RemainingQuantity = r.Field<decimal>("RemainingQuantity")

                    }).ToList();
                }
            }

            return itemList;
        }

        public bool UpdateStockInformationAfterItemReturn(List<RestaurantSalesReturnItemViewBO> returnItemsList, int userInfoId)
        {
            bool status = false;
            int returnValue = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateStockInformationAfterItemReturn_SP"))
                    {
                        foreach (var item in returnItemsList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@ReturnId", DbType.Int64, item.ReturnId);
                            dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, item.BillId);
                            dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, item.ItemId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, item.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, item.LocationId);
                            dbSmartAspects.AddInParameter(command, "@ReturnUnit", DbType.Decimal, item.ReturnedUnit);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);

                            returnValue = dbSmartAspects.ExecuteNonQuery(command);

                            if (returnValue < 1)
                            {
                                status = false;
                                break;
                            }

                        }
                        if (returnValue > 0)
                        {
                            status = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    throw ex;
                }
            }

            return status;
        }

        public RestaurantBillBO GetBillInfoForRetailPosByBillId(int billID)
        {
            RestaurantBillBO billBO = new RestaurantBillBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillInfoForRetailPosByBillId_SP"))
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
                                billBO.IsBillReSettlement = Convert.ToBoolean(reader["IsBillReSettlement"].ToString());
                                billBO.CustomerName = reader["CustomerName"].ToString();
                                billBO.CardNumber = reader["CardNumber"].ToString();
                                billBO.BillDate = Convert.ToDateTime(reader["BillDate"].ToString());
                                billBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                billBO.CreatedDate = reader["CreatedDate"].ToString();

                                billBO.InvoiceServiceRate = Convert.ToDecimal(reader["InvoiceServiceRate"].ToString());
                                billBO.IsInvoiceServiceChargeEnable = Convert.ToBoolean(reader["IsInvoiceServiceChargeEnable"].ToString());
                                //billBO.InvoiceServiceCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                billBO.IsInvoiceCitySDChargeEnable = Convert.ToBoolean(reader["IsInvoiceCitySDChargeEnable"].ToString());
                                //billBO.InvoiceCitySDCharge = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                billBO.IsInvoiceVatAmountEnable = Convert.ToBoolean(reader["IsInvoiceVatAmountEnable"].ToString());
                                //billBO.InvoiceVatAmount = Convert.ToDecimal(reader["CostCenterId"].ToString());
                                billBO.IsInvoiceAdditionalChargeEnable = Convert.ToBoolean(reader["IsInvoiceAdditionalChargeEnable"].ToString());
                                //billBO.InvoiceAdditionalCharge = Convert.ToString(reader["CostCenterId"].ToString());

                                billBO.ReferenceBillId = Convert.ToInt32(reader["ReferenceBillId"].ToString());
                                billBO.ReturnBillId = Convert.ToInt32(reader["ReturnBillId"].ToString());
                                billBO.ReturnDate = Convert.ToDateTime(reader["ReturnDate"].ToString());

                            }
                        }
                    }
                }
            }
            return billBO;
        }

        public RetailPosBillReturnBO RetailPosBill(int billId)
        {
            RetailPosBillReturnBO salesList = new RetailPosBillReturnBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("RetailPosBill_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PosSales");

                    salesList.PosBillWithSalesReturn = ds.Tables[0].AsEnumerable().Select(r =>
                                new RetailPosBillWithSalesReturnBO
                                {
                                    TransactionType = r.Field<string>("TransactionType"),                                    
                                    BillId = r.Field<int>("BillId"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    ReturnBillId = r.Field<int?>("ReturnBillId"),
                                    BillDate = r.Field<DateTime?>("BillDate"),
                                    CostCenter = r.Field<string>("CostCenter"),
                                    CashierName = r.Field<string>("CashierName"),
                                    CustomerName = r.Field<string>("CustomerName"),
                                    SalesAmount = r.Field<decimal?>("SalesAmount"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    CalculatedDiscountAmount = r.Field<decimal?>("CalculatedDiscountAmount"),
                                    AmountAfterDiscount = r.Field<decimal?>("AmountAfterDiscount"),
                                    InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                                    InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                                    InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                                    GrandTotal = r.Field<decimal?>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                                    IsInvoiceVatAmountEnable = r.Field<bool?>("IsInvoiceVatAmountEnable"),
                                    IsInvoiceServiceChargeEnable = r.Field<bool?>("IsInvoiceServiceChargeEnable"),
                                    InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                                    KotId = r.Field<int>("KotId"),
                                    BearerId = r.Field<int?>("BearerId"),
                                    ReferenceKotId = r.Field<int?>("ReferenceKotId"),
                                    ItemId = r.Field<int?>("ItemId"),
                                    ItemName = r.Field<string>("ItemName"),
                                    ItemUnit = r.Field<decimal?>("ItemUnit"),
                                    UnitHead = r.Field<string>("UnitHead"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    ItemTotalAmount = r.Field<decimal?>("ItemTotalAmount"),
                                    PointsAwarded = r.Field<decimal?>("PointsAwarded"),
                                    PointsRedeemed = r.Field<decimal?>("PointsRedeemed"),
                                    PointsRedeemedAmount = r.Field<decimal?>("PointsRedeemedAmount"),
                                    BalancePoints = r.Field<decimal?>("BalancePoints"),
                                    Attention = r.Field<string>("Attention"),
                                    PaymentInstruction = r.Field<string>("PaymentInstruction"),
                                    BillSubject = r.Field<string>("BillSubject"),
                                    BagWaight = r.Field<int?>("BagWaight")
                                }).ToList();

                    salesList.PosSalesReturnPayment = ds.Tables[1].AsEnumerable().Select(r =>
                               new RetailPosSalesPaymentBO
                               {
                                   PaymentId = r.Field<int>("PaymentId"),
                                   BillId = r.Field<int?>("BillId"),
                                   PayMode = r.Field<string>("PayMode"),
                                   PaymentAmount = r.Field<decimal?>("PaymentAmount")

                               }).ToList();
                }
            }
            return salesList;
        }
        public RetailPosBillReturnBO STPosBill(int billId)
        {
            RetailPosBillReturnBO salesList = new RetailPosBillReturnBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("STPosBill_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PosSales");

                    salesList.PosBillWithSalesReturn = ds.Tables[0].AsEnumerable().Select(r =>
                                new RetailPosBillWithSalesReturnBO
                                {
                                    //BillId = r.Field<int>("BillId"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    ReturnBillId = r.Field<int?>("ReturnBillId"),
                                    BillDate = r.Field<DateTime?>("BillDate"),
                                    BillDateDisplay = r.Field<string>("BillDateDisplay"),
                                    CostCenter = r.Field<string>("CostCenter"),
                                    CashierName = r.Field<string>("CashierName"),
                                    CustomerName = r.Field<string>("CustomerName"),
                                    SalesAmount = r.Field<decimal?>("SalesAmount"),
                                    DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                                    CalculatedDiscountAmount = r.Field<decimal?>("CalculatedDiscountAmount"),
                                    AmountAfterDiscount = r.Field<decimal?>("AmountAfterDiscount"),
                                    InvoiceVatAmount = r.Field<decimal?>("InvoiceVatAmount"),
                                    InvoiceServiceCharge = r.Field<decimal?>("InvoiceServiceCharge"),
                                    InvoiceAdditionalCharge = r.Field<decimal?>("InvoiceAdditionalCharge"),
                                    GrandTotal = r.Field<decimal?>("GrandTotal"),
                                    RoundedAmount = r.Field<decimal?>("RoundedAmount"),
                                    RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                                    IsInvoiceVatAmountEnable = r.Field<bool?>("IsInvoiceVatAmountEnable"),
                                    IsInvoiceServiceChargeEnable = r.Field<bool?>("IsInvoiceServiceChargeEnable"),
                                    InvoiceCitySDCharge = r.Field<decimal?>("InvoiceCitySDCharge"),
                                    KotId = r.Field<int>("KotId"),
                                    BearerId = r.Field<int?>("BearerId"),
                                    ReferenceKotId = r.Field<int?>("ReferenceKotId"),
                                    ItemId = r.Field<int?>("ItemId"),
                                    ItemName = r.Field<string>("ItemName"),
                                    ItemUnit = r.Field<decimal?>("ItemUnit"),
                                    UnitHead = r.Field<string>("UnitHead"),
                                    UnitRate = r.Field<decimal?>("UnitRate"),
                                    ItemTotalAmount = r.Field<decimal?>("ItemTotalAmount"),
                                    PointsAwarded = r.Field<decimal?>("PointsAwarded"),
                                    PointsRedeemed = r.Field<decimal?>("PointsRedeemed"),
                                    PointsRedeemedAmount = r.Field<decimal?>("PointsRedeemedAmount"),
                                    BalancePoints = r.Field<decimal?>("BalancePoints"),
                                    Attention = r.Field<string>("Attention"),
                                    PaymentInstruction = r.Field<string>("PaymentInstruction"),
                                    BillSubject = r.Field<string>("BillSubject")
                                }).ToList();

                    salesList.PosSalesReturnPayment = ds.Tables[1].AsEnumerable().Select(r =>
                               new RetailPosSalesPaymentBO
                               {
                                   PaymentId = r.Field<int>("PaymentId"),
                                   BillId = r.Field<int?>("BillId"),
                                   PayMode = r.Field<string>("PayMode"),
                                   PaymentAmount = r.Field<decimal?>("PaymentAmount")

                               }).ToList();
                }
            }
            return salesList;
        }

    }
}
