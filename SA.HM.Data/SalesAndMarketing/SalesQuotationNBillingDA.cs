using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class SalesQuotationNBillingDA : BaseService
    {
        public List<ServiceTypeBO> GetServiceType()
        {
            List<ServiceTypeBO> serviceList = new List<ServiceTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceType_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ServiceTypeBO sv = new ServiceTypeBO();

                                sv.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);
                                sv.ServiceName = reader["ServiceName"].ToString();
                                sv.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();

                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }
        public List<ItemOrServiceDeliveryBO> GetItemOrServiceDelivery()
        {
            List<ItemOrServiceDeliveryBO> serviceList = new List<ItemOrServiceDeliveryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemOrServiceDelivery_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ItemOrServiceDeliveryBO sv = new ItemOrServiceDeliveryBO();

                                sv.ItemServiceDeliveryId = Convert.ToInt32(reader["ItemServiceDeliveryId"]);
                                sv.DeliveryTypeName = reader["DeliveryTypeName"].ToString();
                                sv.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();

                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }

        public List<SMBillingPeriodBO> GetBillingPeriod()
        {
            List<SMBillingPeriodBO> serviceList = new List<SMBillingPeriodBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillingPeriod_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMBillingPeriodBO sv = new SMBillingPeriodBO();

                                sv.BillingPeriodId = Convert.ToInt32(reader["BillingPeriodId"]);
                                sv.BillingPeriodName = reader["BillingPeriodName"].ToString();
                                sv.BillingPeriodValue = Convert.ToInt32(reader["BillingPeriodValue"]);
                                sv.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();

                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }

        public List<ContractPeriodBO> GetContractPeriod()
        {
            List<ContractPeriodBO> serviceList = new List<ContractPeriodBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContractPeriod_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ContractPeriodBO sv = new ContractPeriodBO();

                                sv.ContractPeriodId = Convert.ToInt32(reader["ContractPeriodId"]);
                                sv.ContractPeriodName = reader["ContractPeriodName"].ToString();
                                sv.ContractPeriodValue = Convert.ToInt32(reader["ContractPeriodValue"]);
                                sv.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();

                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }

        public List<SMCurrentVendorBO> GetCurrentVendor()
        {
            List<SMCurrentVendorBO> serviceList = new List<SMCurrentVendorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrentVendor_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMCurrentVendorBO sv = new SMCurrentVendorBO();

                                sv.CurrentVendorId = Convert.ToInt32(reader["CurrentVendorId"]);
                                sv.VendorName = reader["VendorName"].ToString();
                                sv.Address = reader["Address"].ToString();
                                sv.ContactNo = reader["ContactNo"].ToString();
                                sv.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                sv.ActiveStatus = reader["ActiveStatus"].ToString();

                                serviceList.Add(sv);
                            }
                        }
                    }
                }
            }
            return serviceList;
        }

        #region Quotation
        public bool UpdateApproval(int quotationId, int dealId, bool isReject, int lastModifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateApproval_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int64, dealId);
                        dbSmartAspects.AddInParameter(cmd, "@IsReject", DbType.Int64, isReject);
                        dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

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
        public bool UpdateApprovalFromSalesBilling(long quotationId, long glCompanyId, long glProjectId,  int lastModifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateQuotationApprovalFromBilling_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int64, glCompanyId);
                        dbSmartAspects.AddInParameter(cmd, "@GLProjectId", DbType.Int64, glProjectId);
                        
                        dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

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
        public Boolean SaveQuotation(SMQuotationBO quotation, List<SMQuotationDetailsBO> quotationDetails,
                        List<SMQuotationDetailsBO> quotationSrviceDetails, List<SMQuotationDetailsBO> QuotationDetail,
                                out int quotationId)
        {
            long quotationDetailsId = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveQuotation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, quotation.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, quotation.DealId);
                        dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, quotation.ContactId);
                        dbSmartAspects.AddInParameter(command, "@ProposalDate", DbType.Date, quotation.ProposalDate);
                        dbSmartAspects.AddInParameter(command, "@ServiceTypeId", DbType.Int32, quotation.ServiceTypeId);
                        dbSmartAspects.AddInParameter(command, "@TotalDeviceOrUser", DbType.Int32, quotation.TotalDeviceOrUser);
                        dbSmartAspects.AddInParameter(command, "@ContractPeriodId", DbType.Int32, quotation.ContractPeriodId);
                        dbSmartAspects.AddInParameter(command, "@BillingPeriodId", DbType.Int32, quotation.BillingPeriodId);
                        dbSmartAspects.AddInParameter(command, "@ItemServiceDeliveryId", DbType.Int32, quotation.ItemServiceDeliveryId);
                        dbSmartAspects.AddInParameter(command, "@CurrentVendorId", DbType.Int32, quotation.CurrentVendorId);
                        dbSmartAspects.AddInParameter(command, "@PriceValidity", DbType.String, quotation.PriceValidity);
                        dbSmartAspects.AddInParameter(command, "@DeployLocation", DbType.String, quotation.DeployLocation);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, quotation.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, quotation.CreatedBy);
                        //dbSmartAspects.AddInParameter(command, "@IsAccepted", DbType.Boolean, quotation.IsAccepted);
                        //dbSmartAspects.AddInParameter(command, "@IsRejected", DbType.Boolean, quotation.IsRejected);

                        dbSmartAspects.AddOutParameter(command, "@QuotationId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        quotationId = Convert.ToInt32(command.Parameters["@QuotationId"].Value);
                    }

                    if (status && quotationDetails.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationDetails)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);
                                dbSmartAspects.AddOutParameter(cmd, "@QuotationDetailsId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                                quotationDetailsId = Convert.ToInt64(cmd.Parameters["@QuotationDetailsId"].Value);
                            }
                        }
                    }

                    if (status && quotationSrviceDetails.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationSrviceDetails)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);
                                dbSmartAspects.AddOutParameter(cmd, "@QuotationDetailsId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                                quotationDetailsId = Convert.ToInt64(cmd.Parameters["@QuotationDetailsId"].Value);
                            }
                        }
                    }

                    if (status && QuotationDetail.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in QuotationDetail)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);

                                dbSmartAspects.AddOutParameter(cmd, "@QuotationDetailsId", DbType.Int64, sizeof(Int64));
                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                quotationDetailsId = Convert.ToInt64(cmd.Parameters["@QuotationDetailsId"].Value);

                                if (status && detailBO.QuotationDiscountDetails.Count > 0)
                                {
                                    using (DbCommand cmdDiscount = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateQuotationDiscountDetails_SP"))
                                    {
                                        foreach (SMQuotationDiscountDetails discountDetailBO in detailBO.QuotationDiscountDetails)
                                        {
                                            cmdDiscount.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Id", DbType.Int64, discountDetailBO.Id);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@SMQuotationDetailsId", DbType.Int64, quotationDetailsId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@OutLetId", DbType.Int64, discountDetailBO.OutLetId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Type", DbType.String, discountDetailBO.Type);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@TypeId", DbType.Int32, discountDetailBO.TypeId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountType", DbType.String, discountDetailBO.DiscountType);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmount", DbType.Decimal, discountDetailBO.DiscountAmount);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmountUSD", DbType.Decimal, discountDetailBO.DiscountAmountUSD);

                                            status = dbSmartAspects.ExecuteNonQuery(cmdDiscount, transaction) > 0 ? true : false;

                                        }
                                    }
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
        public Boolean UpdateQuotation(SMQuotationBO quotation, List<SMQuotationDetailsBO> quotationDetailsToAdded, List<SMQuotationDetailsBO> quotationSrviceDetailsToAdded,
                                       List<SMQuotationDetailsBO> quotationDetailsToEdit, List<SMQuotationDetailsBO> quotationSrviceDetailsToEdit,
                                       List<SMQuotationDetailsBO> quotationDetailsToDelete, List<SMQuotationDetailsBO> quotationSrviceDetailsToDelete,
                                       List<SMQuotationDetailsBO> QuotationDetail, string DeletedQuotationDetailIdList, string DeletedQuotationDiscountDetailIdList)
        {

            long quotationDetailsId = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateQuotation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@QuotationId", DbType.Int64, quotation.QuotationId);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, quotation.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@ProposalDate", DbType.Date, quotation.ProposalDate);
                        dbSmartAspects.AddInParameter(command, "@ServiceTypeId", DbType.Int32, quotation.ServiceTypeId);
                        dbSmartAspects.AddInParameter(command, "@TotalDeviceOrUser", DbType.Int32, quotation.TotalDeviceOrUser);
                        dbSmartAspects.AddInParameter(command, "@ContractPeriodId", DbType.Int32, quotation.ContractPeriodId);
                        dbSmartAspects.AddInParameter(command, "@BillingPeriodId", DbType.Int32, quotation.BillingPeriodId);
                        dbSmartAspects.AddInParameter(command, "@ItemServiceDeliveryId", DbType.Int32, quotation.ItemServiceDeliveryId);
                        dbSmartAspects.AddInParameter(command, "@CurrentVendorId", DbType.Int32, quotation.CurrentVendorId);
                        dbSmartAspects.AddInParameter(command, "@PriceValidity", DbType.String, quotation.PriceValidity);
                        dbSmartAspects.AddInParameter(command, "@DeployLocation", DbType.String, quotation.DeployLocation);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, quotation.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, quotation.LastModifiedBy);
                        //dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, quotation.DealId);
                        //dbSmartAspects.AddInParameter(command, "@IsAccepted", DbType.Boolean, quotation.IsAccepted);
                        //dbSmartAspects.AddInParameter(command, "@IsRejected", DbType.Boolean, quotation.IsRejected);

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                    }

                    if (status && quotationDetailsToAdded.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationDetailsToAdded)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotation.QuotationId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);
                                dbSmartAspects.AddOutParameter(cmd, "@QuotationDetailsId", DbType.Int64, sizeof(Int64));

                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                quotationDetailsId = Convert.ToInt64(cmd.Parameters["@QuotationDetailsId"].Value);


                            }
                        }
                    }

                    if (status && quotationDetailsToEdit.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationDetailsToEdit)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotation.QuotationId);
                                dbSmartAspects.AddInParameter(cmd, "@QuotationDetailsId", DbType.Int64, detailBO.QuotationDetailsId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status && quotationSrviceDetailsToAdded.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationSrviceDetailsToAdded)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotation.QuotationId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);
                                dbSmartAspects.AddOutParameter(cmd, "@QuotationDetailsId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                quotationDetailsId = Convert.ToInt64(cmd.Parameters["@QuotationDetailsId"].Value);

                            }
                        }
                    }

                    if (status && quotationSrviceDetailsToEdit.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationSrviceDetailsToEdit)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotation.QuotationId);
                                dbSmartAspects.AddInParameter(cmd, "@QuotationDetailsId", DbType.Int64, detailBO.QuotationDetailsId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status && quotationDetailsToDelete.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationDetailsToDelete)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "SMQuotationDetails");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "QuotationDetailsId");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKId", DbType.String, detailBO.QuotationDetailsId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false; ;
                            }
                        }
                    }

                    if (status && quotationSrviceDetailsToDelete.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in quotationSrviceDetailsToDelete)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "SMQuotationDetails");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "QuotationDetailsId");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKId", DbType.String, detailBO.QuotationDetailsId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false; ;
                            }
                        }
                    }

                    if (status && QuotationDetail.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in QuotationDetail.Where(i => i.QuotationDetailsId == 0))
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotation.QuotationId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);

                                dbSmartAspects.AddOutParameter(cmd, "@QuotationDetailsId", DbType.Int64, sizeof(Int64));
                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                quotationDetailsId = Convert.ToInt64(cmd.Parameters["@QuotationDetailsId"].Value);

                                if (status && detailBO.QuotationDiscountDetails.Count > 0)
                                {
                                    using (DbCommand cmdDiscount = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateQuotationDiscountDetails_SP"))
                                    {
                                        foreach (SMQuotationDiscountDetails discountDetailBO in detailBO.QuotationDiscountDetails)
                                        {
                                            cmdDiscount.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Id", DbType.Int64, discountDetailBO.Id);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@SMQuotationDetailsId", DbType.Int64, quotationDetailsId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@OutLetId", DbType.Int64, discountDetailBO.OutLetId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Type", DbType.String, discountDetailBO.Type);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@TypeId", DbType.Int32, discountDetailBO.TypeId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountType", DbType.String, discountDetailBO.DiscountType);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmount", DbType.Decimal, discountDetailBO.DiscountAmount);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmountUSD", DbType.Decimal, discountDetailBO.DiscountAmountUSD);

                                            status = dbSmartAspects.ExecuteNonQuery(cmdDiscount, transaction) > 0 ? true : false;

                                        }
                                    }
                                }
                            }
                        }

                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateQuotationDetails_SP"))
                        {
                            foreach (SMQuotationDetailsBO detailBO in QuotationDetail.Where(i => i.QuotationDetailsId > 0))
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, detailBO.QuotationId);
                                dbSmartAspects.AddInParameter(cmd, "@QuotationDetailsId", DbType.Int64, detailBO.QuotationDetailsId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                
                                if (status && detailBO.QuotationDiscountDetails.Count > 0)
                                {
                                    using (DbCommand cmdDiscount = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateQuotationDiscountDetails_SP"))
                                    {
                                        foreach (SMQuotationDiscountDetails discountDetailBO in detailBO.QuotationDiscountDetails)
                                        {
                                            cmdDiscount.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Id", DbType.Int64, discountDetailBO.Id);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@SMQuotationDetailsId", DbType.Int64, detailBO.QuotationDetailsId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@OutLetId", DbType.Int64, discountDetailBO.OutLetId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Type", DbType.String, discountDetailBO.Type);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@TypeId", DbType.Int32, discountDetailBO.TypeId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountType", DbType.String, discountDetailBO.DiscountType);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmount", DbType.Decimal, discountDetailBO.DiscountAmount);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmountUSD", DbType.Decimal, discountDetailBO.DiscountAmountUSD);

                                            status = dbSmartAspects.ExecuteNonQuery(cmdDiscount, transaction) > 0 ? true : false;

                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (status && !string.IsNullOrWhiteSpace(DeletedQuotationDetailIdList))
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataListDynamically_SP"))
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "SMQuotationDetails");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "QuotationDetailsId");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKIdList", DbType.String, DeletedQuotationDetailIdList);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

                        }
                    }

                    if (status && !string.IsNullOrWhiteSpace(DeletedQuotationDiscountDetailIdList))
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataListDynamically_SP"))
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "SMQuotationDiscountDetails");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "Id");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKIdList", DbType.String, DeletedQuotationDiscountDetailIdList);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

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

        public List<SMQuotationViewBO> GetQuotationForPagination(int companyId, int dealId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMQuotationViewBO> productList = new List<SMQuotationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationForPagination_SP"))
                {
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    if (dealId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int32, dealId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationViewBO productBO = new SMQuotationViewBO();

                                productBO.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                productBO.QuotationNo = reader["QuotationNo"].ToString();
                                productBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                productBO.ProposalDate = Convert.ToDateTime(reader["ProposalDate"]);
                                productBO.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);
                                productBO.TotalDeviceOrUser = Convert.ToInt32(reader["TotalDeviceOrUser"]);
                                productBO.ContractPeriodId = Int32.Parse(reader["ContractPeriodId"].ToString());
                                productBO.BillingPeriodId = Convert.ToInt32(reader["BillingPeriodId"]);
                                productBO.ItemServiceDeliveryId = Int32.Parse(reader["ItemServiceDeliveryId"].ToString());
                                productBO.CurrentVendorId = Convert.ToInt32(reader["CurrentVendorId"]);
                                productBO.Remarks = reader["Remarks"].ToString();
                                productBO.DealId = Convert.ToInt64(reader["DealId"]);
                                productBO.IsAccepted = Convert.ToBoolean(reader["IsAccepted"]);
                                productBO.CompanyName = reader["CompanyName"].ToString();
                                productBO.ServiceName = reader["ServiceName"].ToString();

                                productList.Add(productBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return productList;
        }

        public List<SMQuotationViewBO> GetQuotationForSalesBillOrSalesNote(string quotationNumber, int companyId, DateTime? fromDate, DateTime? toDate, string type, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMQuotationViewBO> productList = new List<SMQuotationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationForSalesBillOrSalesNote_SP"))
                {
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(quotationNumber))
                        dbSmartAspects.AddInParameter(cmd, "@QuotationNumber", DbType.String, quotationNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@QuotationNumber", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationViewBO productBO = new SMQuotationViewBO();

                                productBO.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                productBO.QuotationNo = reader["QuotationNo"].ToString();
                                productBO.ProposalDate = Convert.ToDateTime(reader["ProposalDate"]);
                                productBO.CompanyName = reader["CompanyName"].ToString();
                                productBO.DealName = reader["DealName"].ToString();
                                productBO.IsApprovedFromBilling = Convert.ToBoolean(reader["IsApprovedFromBilling"]);
                                productBO.IsSalesNoteFinal = Convert.ToBoolean(reader["IsSalesNoteFinal"]);
                                productBO.HasItem = Convert.ToBoolean(reader["HasItem"]);

                                productList.Add(productBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return productList;
        }

        public QuationWiseSalesListBO GetQuotationWiseSalesList_SP(int QuotationId)
        {
            QuationWiseSalesListBO salesList = new QuationWiseSalesListBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemAndServiceListWithSalesNote_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int32, QuotationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "QuotationWiseSales");

                    salesList.quotationWiseSales = ds.Tables[0].AsEnumerable().Select(r =>
                                new SMQuotationWiseSalesBO
                                {
                                    DealName = r.Field<string>("DealName"),
                                    DealAmount = r.Field<decimal>("DealAmount"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    CompanyAddress = r.Field<string>("CompanyAddress"),
                                    EmailAddress = r.Field<string>("EmailAddress"),
                                    WebAddress = r.Field<string>("WebAddress"),
                                    ContactPerson = r.Field<string>("ContactPerson"),
                                    ContactDesignation = r.Field<string>("ContactDesignation"),
                                    ContactNumber = r.Field<string>("ContactNumber"),
                                    QuotationNo = r.Field<string>("QuotationNo"),
                                    QuotationId = r.Field<Int64>("QuotationId")

                                }).ToList();

                    salesList.itemDetails = ds.Tables[1].AsEnumerable().Select(r =>
                                new SMQuotationDetailsBO
                                {
                                    Name = r.Field<string>("Name"),
                                    Quantity = r.Field<decimal>("Quantity"),
                                    UnitPrice = r.Field<decimal>("UnitPrice"),
                                    TotalPrice = r.Field<decimal>("TotalPrice"),
                                    SalesNote = r.Field<string>("SalesNote")

                                }).ToList();

                    salesList.seviceDetails = ds.Tables[2].AsEnumerable().Select(r =>
                               new SMQuotationDetailsBO
                               {
                                   Name = r.Field<string>("Name"),
                                   Quantity = r.Field<decimal>("Quantity"),
                                   UnitPrice = r.Field<decimal>("UnitPrice"),
                                   TotalPrice = r.Field<decimal>("TotalPrice"),
                                   SalesNote = r.Field<string>("SalesNote")

                               }).ToList();
                }
            }
            return salesList;
        }

        public List<SMQuotationViewBO> GetAllAcceptedQuotation()
        {
            List<SMQuotationViewBO> quotationList = new List<SMQuotationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAcceptedQuotation_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationViewBO quotation = new SMQuotationViewBO();

                                quotation.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                quotation.QuotationNo = reader["QuotationNo"].ToString();
                                quotation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                quotation.ProposalDate = Convert.ToDateTime(reader["ProposalDate"]);
                                quotation.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);
                                quotation.TotalDeviceOrUser = Convert.ToInt32(reader["TotalDeviceOrUser"]);
                                quotation.ContractPeriodId = Int32.Parse(reader["ContractPeriodId"].ToString());
                                quotation.BillingPeriodId = Convert.ToInt32(reader["BillingPeriodId"]);
                                quotation.ItemServiceDeliveryId = Int32.Parse(reader["ItemServiceDeliveryId"].ToString());
                                quotation.CurrentVendorId = Convert.ToInt32(reader["CurrentVendorId"]);
                                quotation.Remarks = reader["Remarks"].ToString();
                                quotation.IsSalesNoteFinal = Convert.ToBoolean(reader["IsSalesNoteFinal"]);

                                quotationList.Add(quotation);

                            }
                        }
                    }
                }
            }
            return quotationList;
        }


        public SMQuotationViewBO GetQuotationById(Int64 quotationId)
        {
            SMQuotationViewBO quotation = new SMQuotationViewBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                quotation.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                quotation.QuotationNo = reader["QuotationNo"].ToString();
                                quotation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                quotation.ContactId = Convert.ToInt64(reader["ContactId"]);
                                quotation.ProposalDate = Convert.ToDateTime(reader["ProposalDate"]);
                                quotation.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);
                                quotation.TotalDeviceOrUser = Convert.ToInt32(reader["TotalDeviceOrUser"]);
                                quotation.ContractPeriodId = Int32.Parse(reader["ContractPeriodId"].ToString());
                                quotation.BillingPeriodId = Convert.ToInt32(reader["BillingPeriodId"]);
                                quotation.ItemServiceDeliveryId = Int32.Parse(reader["ItemServiceDeliveryId"].ToString());
                                if (reader["CurrentVendorId"] != DBNull.Value)
                                {
                                    quotation.CurrentVendorId = Convert.ToInt32(reader["CurrentVendorId"]);
                                }
                                quotation.Remarks = reader["Remarks"].ToString();
                                quotation.PriceValidity = reader["PriceValidity"].ToString();
                                quotation.DeployLocation = reader["DeployLocation"].ToString();
                                quotation.IsSalesNoteFinal = Convert.ToBoolean(reader["IsSalesNoteFinal"]);
                                quotation.DealId = Convert.ToInt32(reader["DealId"]);
                            }
                        }
                    }
                }
            }
            return quotation;
        }

        public List<SMQuotationDetailsBO> GetQuotationItemDetailsById(Int64 quotationId, string itemType)
        {
            List<SMQuotationDetailsBO> quotation = new List<SMQuotationDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationItemDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationDetailsBO q = new SMQuotationDetailsBO();

                                q.QuotationDetailsId = Convert.ToInt64(reader["QuotationDetailsId"]);
                                q.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                q.ItemType = reader["ItemType"].ToString();
                                q.CategoryId = Convert.ToInt32(reader["CategoryId"]);

                                q.ServiceBandWidthId = Convert.ToInt32(reader["ServiceBandWidthId"]);
                                q.ServicePackageId = Convert.ToInt32(reader["ServicePackageId"]);
                                q.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);

                                q.ItemId = Convert.ToInt32(reader["ItemId"]);
                                q.StockBy = Convert.ToInt32(reader["StockBy"]);
                                q.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                q.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                q.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);

                                q.ItemName = reader["ItemName"].ToString();
                                q.HeadName = reader["HeadName"].ToString();
                                q.SalesNote = reader["SalesNote"].ToString();
                                //q.CostCenterList= reader["CostCenterList"].ToString();
                                quotation.Add(q);
                            }
                        }
                    }
                }
            }
            return quotation;
        }

        public List<SMQuotationDetailsBO> GetQuotationServiceDetailsById(Int64 quotationId, string itemType)
        {
            List<SMQuotationDetailsBO> quotation = new List<SMQuotationDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationServiceDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationDetailsBO q = new SMQuotationDetailsBO();

                                q.QuotationDetailsId = Convert.ToInt64(reader["QuotationDetailsId"]);
                                q.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                q.ItemType = reader["ItemType"].ToString();
                                q.CategoryId = Convert.ToInt32(reader["CategoryId"]);

                                q.ServiceBandWidthId = Convert.ToInt32(reader["ServiceBandWidthId"]);
                                q.ServicePackageId = Convert.ToInt32(reader["ServicePackageId"]);
                                q.ServiceTypeId = Convert.ToInt32(reader["ServiceTypeId"]);

                                q.ItemId = Convert.ToInt32(reader["ItemId"]);
                                q.StockBy = Convert.ToInt32(reader["StockBy"]);
                                q.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                q.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                q.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);

                                q.ItemName = reader["ItemName"].ToString();
                                q.HeadName = reader["HeadName"].ToString();
                                q.PackageName = reader["PackageName"].ToString();
                                //q.BandWidthName = reader["BandWidthName"].ToString();
                                q.Uplink = Convert.ToInt32(reader["Uplink"]);
                                q.Downlink = Convert.ToInt32(reader["Downlink"]);
                                q.ServiceType = reader["ServiceType"].ToString();
                                //q.UpLink = Convert.ToInt32(reader["UpLink"].ToString());
                                q.SalesNote = reader["SalesNote"].ToString();
                                quotation.Add(q);
                            }
                        }
                    }
                }
            }
            return quotation;
        }

        public List<SMQuotationDetailsBO> GetQuotationDetailsById(Int64 quotationId)
        {
            List<SMQuotationDetailsBO> quotationDetails = new List<SMQuotationDetailsBO>();
            List<SMQuotationDiscountDetails> quotationDiscountDetails = new List<SMQuotationDiscountDetails>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Quotationdetails");

                    quotationDetails = ds.Tables[0].AsEnumerable().Select(r => UtilityDA.ConvertDataRowToObjet<SMQuotationDetailsBO>(r)).ToList();

                    quotationDiscountDetails = ds.Tables[1].AsEnumerable().Select(r => new SMQuotationDiscountDetails {
                        Id = r.Field<long>("Id"),
                        SMQuotationDetailsId = r.Field<long>("SMQuotationDetailsId"),
                        OutLetId = r.Field<long>("OutLetId"),
                        Type = r.Field<string>("Type"),
                        TypeId = r.Field<int>("TypeId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        DiscountAmountUSD = r.Field<decimal>("DiscountAmountUSD"),
                        OutLetName = r.Field<string>("OutLetName"),
                        TypeName = r.Field<string>("TypeName")

                    }).ToList();
                    foreach (var detail in quotationDetails)
                    {
                        detail.QuotationDiscountDetails = quotationDiscountDetails.Where(i => i.SMQuotationDetailsId == detail.QuotationDetailsId).ToList();
                    }
                }
            }
            return quotationDetails;
        }

        public List<SMQuotationBO> GetAllSoldOutQuotation()
        {
            List<SMQuotationBO> QuotationList = new List<SMQuotationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllSoldOutQuotation_SP"))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMQuotationBO Quotation = new SMQuotationBO();
                                    Quotation.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                    Quotation.QuotationNo = reader["QuotationNo"].ToString();
                                    QuotationList.Add(Quotation);
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return QuotationList;
        }
        public SMQuotationReportViewBO GetQuotationDetailsByIdForReport(long quotationId)
        {
            SMQuotationReportViewBO Quotation = new SMQuotationReportViewBO();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationDetailsByIdForReport_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");

                        Quotation.Quotation = ds.Tables[0].AsEnumerable().Select(r => new SMQuotationViewBO()
                        {
                            QuotationId = r.Field<long>("QuotationId"),
                            QuotationNo = r.Field<string>("QuotationNo"),
                            CompanyId = r.Field<int>("CompanyId"),
                            TotalDeviceOrUser = r.Field<short>("TotalDeviceOrUser"),
                            ProposalDate = r.Field<DateTime>("ProposalDate"),
                            ProposalDateToDisplay = r.Field<string>("ProposalDateToDisplay"),
                            Remarks = r.Field<string>("Remarks"),
                            ServiceName = r.Field<string>("ServiceName"),
                            ContractPeriodName = r.Field<string>("ContractPeriodName"),
                            BillingPeriodName = r.Field<string>("BillingPeriodName"),
                            ItemServiceDeliveryName = r.Field<string>("ItemServiceDeliveryName"),
                            CurrentVendorName = r.Field<string>("CurrentVendorName")

                        }).ToList();

                        Quotation.Company = ds.Tables[1].AsEnumerable().Select(r => new GuestCompanyBO()
                        {
                            CompanyId = r.Field<int>("CompanyId"),
                            CompanyName = r.Field<string>("CompanyName"),
                            CompanyAddress = r.Field<string>("CompanyAddress"),
                            ContactPerson = r.Field<string>("ContactPerson"),
                            ContactDesignation = r.Field<string>("ContactDesignation"),
                            EmailAddress = r.Field<string>("EmailAddress"),
                            ContactNumber = r.Field<string>("ContactNumber")
                        }).ToList();

                        Quotation.QuotationItemDetails = ds.Tables[2].AsEnumerable().Select(r => new SMQuotationDetailsBO()
                        {
                            ItemId = r.Field<int>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            HeadName = r.Field<string>("HeadName"),
                            Quantity = r.Field<decimal>("Quantity"),
                            UnitPrice = r.Field<decimal>("UnitPrice"),
                            TotalPrice = r.Field<decimal>("TotalPrice"),
                            Category = r.Field<string>("Category"),
                            SalesNote = r.Field<string>("SalesNote")
                        }).ToList();

                        Quotation.QuotationServiceDetails = ds.Tables[3].AsEnumerable().Select(r => new SMQuotationDetailsBO()
                        {
                            ItemId = r.Field<int>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            HeadName = r.Field<string>("HeadName"),
                            Quantity = r.Field<decimal>("Quantity"),
                            UnitPrice = r.Field<decimal>("UnitPrice"),
                            TotalPrice = r.Field<decimal>("TotalPrice"),
                            PackageName = r.Field<string>("PackageName"),
                            Uplink = r.Field<int>("Uplink"),
                            ServiceType = r.Field<string>("ServiceType"),
                            //UpLink = r.Field<int>("UpLink"),
                            SalesNote = r.Field<string>("SalesNote")
                        }).ToList();
                        Quotation.QuotationRestaurantDetails = ds.Tables[4].AsEnumerable().Select(r => new SMQuotationDiscountDetails()
                        {
                            Id = r.Field<long>("Id"),
                            SMQuotationDetailsId = r.Field<long>("SMQuotationDetailsId"),
                            OutLetId = r.Field<long>("OutLetId"),
                            Type = r.Field<string>("Type"),
                            TypeId = r.Field<int>("TypeId"),
                            DiscountType = r.Field<string>("DiscountType"),
                            DiscountAmount = r.Field<decimal>("DiscountAmount"),
                            DiscountAmountUSD = r.Field<decimal>("DiscountAmountUSD"),
                            OutLetName = r.Field<string>("OutLetName"),
                            TypeName = r.Field<string>("TypeName")
                        }).ToList();
                        Quotation.QuotationBanquetDetails = ds.Tables[5].AsEnumerable().Select(r => new SMQuotationDiscountDetails()
                        {
                            Id = r.Field<long>("Id"),
                            SMQuotationDetailsId = r.Field<long>("SMQuotationDetailsId"),
                            OutLetId = r.Field<long>("OutLetId"),
                            Type = r.Field<string>("Type"),
                            TypeId = r.Field<int>("TypeId"),
                            DiscountType = r.Field<string>("DiscountType"),
                            DiscountAmount = r.Field<decimal>("DiscountAmount"),
                            DiscountAmountUSD = r.Field<decimal>("DiscountAmountUSD"),
                            OutLetName = r.Field<string>("OutLetName"),
                            TypeName = r.Field<string>("TypeName")
                        }).ToList();
                        Quotation.QuotationGuestRoomDetails = ds.Tables[6].AsEnumerable().Select(r => new SMQuotationDiscountDetails()
                        {
                            Id = r.Field<long>("Id"),
                            SMQuotationDetailsId = r.Field<long>("SMQuotationDetailsId"),
                            OutLetId = r.Field<long>("OutLetId"),
                            Type = r.Field<string>("Type"),
                            TypeId = r.Field<int>("TypeId"),
                            DiscountType = r.Field<string>("DiscountType"),
                            DiscountAmount = r.Field<decimal>("DiscountAmount"),
                            DiscountAmountUSD = r.Field<decimal>("DiscountAmountUSD"),
                            OutLetName = r.Field<string>("OutLetName"),
                            TypeName = r.Field<string>("TypeName")
                        }).ToList();

                        Quotation.QuotationServiceOutletDetails = ds.Tables[7].AsEnumerable().Select(r => new SMQuotationDiscountDetails()
                        {
                            Id = r.Field<long>("Id"),
                            SMQuotationDetailsId = r.Field<long>("SMQuotationDetailsId"),
                            OutLetId = r.Field<long>("OutLetId"),
                            Type = r.Field<string>("Type"),
                            TypeId = r.Field<int>("TypeId"),
                            DiscountType = r.Field<string>("DiscountType"),
                            DiscountAmount = r.Field<decimal>("DiscountAmount"),
                            DiscountAmountUSD = r.Field<decimal>("DiscountAmountUSD"),
                            OutLetName = r.Field<string>("OutLetName"),
                            TypeName = r.Field<string>("TypeName")
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Quotation;
        }
        public SMQuotationViewBO GetQuotationDetailsViewById(long quotationId)
        {
            SMQuotationViewBO Quotation = new SMQuotationViewBO();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationDetailsViewById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, quotationId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");

                        DataTable dataTable = ds.Tables["SalesCall"];

                        Quotation = dataTable.AsEnumerable().Select(r => new SMQuotationViewBO()
                        {
                            QuotationId = r.Field<long>("QuotationId"),
                            QuotationNo = r.Field<string>("QuotationNo"),
                            CompanyId = r.Field<int>("CompanyId"),
                            ContactId = r.Field<long>("ContactId"),
                            DealId = r.Field<long>("DealId"),
                            TotalDeviceOrUser = r.Field<short>("TotalDeviceOrUser"),
                            ProposalDate = r.Field<DateTime>("ProposalDate"),
                            ProposalDateToDisplay = r.Field<string>("ProposalDateToDisplay"),
                            Remarks = r.Field<string>("Remarks"),
                            ServiceName = r.Field<string>("ServiceName"),
                            ContractPeriodName = r.Field<string>("ContractPeriodName"),
                            BillingPeriodName = r.Field<string>("BillingPeriodName"),
                            ItemServiceDeliveryName = r.Field<string>("ItemServiceDeliveryName"),
                            CurrentVendorName = r.Field<string>("CurrentVendorName")

                        }).FirstOrDefault();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Quotation;
        }

        public List<InvItemAutoSearchBO> GetQuotationDetailsFromSiteServeyFeedback(int siteServeyFeedBackId)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemDetailsFromSiteServeyFeedback_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FeedbackId", DbType.Int32, siteServeyFeedBackId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    itemInfo = Table.AsEnumerable().Select(r => new InvItemAutoSearchBO
                    {
                        ItemId = r.Field<int>("ItemId"),
                        Name = r.Field<string>("Name") + " (" + r.Field<string>("CategoryName") + ")",
                        Code = r.Field<string>("Code"),
                        CategoryId = r.Field<int>("CategoryId"),
                        IsRecipe = r.Field<bool>("IsRecipe"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        UnitHead = r.Field<string>("HeadName"),
                        Quantity = r.Field<decimal>("Quantity")

                    }).ToList();
                }
            }

            return itemInfo;
        }

        #endregion
        #region Sales Note
        public Boolean UpdateQuotationDetailsSalesNote(List<SMQuotationDetailsBO> quotationDetails)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateQuotationDetailsSalesNote_SP"))
                    {
                        foreach (SMQuotationDetailsBO detailBO in quotationDetails)
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@QuotationDetailsId", DbType.Int64, detailBO.QuotationDetailsId);
                            dbSmartAspects.AddInParameter(cmd, "@SalesNote", DbType.String, detailBO.SalesNote);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
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
        public bool FinalizeQuotationSalesNote(long id)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateQuotationIsSalesNoteFinal_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@QuotationId", DbType.Int64, id);

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
        #endregion

        #region Bandwidth 
        public List<ServiceBandwidthBO> GetServiceBandwidthGriding(string name, bool status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ServiceBandwidthBO> bOs = new List<ServiceBandwidthBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBandwidthGriding_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ServiceBandwidthBO bandwidthBO = new ServiceBandwidthBO();

                                    bandwidthBO.ServiceBandWidthId = Convert.ToInt32(reader["ServiceBandWidthId"]);
                                    bandwidthBO.BandWidthName = reader["BandWidthName"].ToString();
                                    bandwidthBO.UplinkFrequency = reader["UplinkFrequency"].ToString();
                                    bandwidthBO.DownlinkFrequency = reader["DownlinkFrequency"].ToString();

                                    if (reader["ActiveStat"] != DBNull.Value)
                                    {
                                        bandwidthBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);

                                    }
                                    bOs.Add(bandwidthBO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bOs;
        }
        public ServiceBandwidthBO GetServiceBandwidthById(long Id)
        {
            ServiceBandwidthBO bandwidthBO = new ServiceBandwidthBO();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceBandwidthById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, Id);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "InvServiceBandwidth");

                        bandwidthBO = ds.Tables[0].AsEnumerable().Select(r => new ServiceBandwidthBO
                        {
                            ServiceBandWidthId = r.Field<Int32>("ServiceBandWidthId"),
                            BandWidthName = r.Field<string>("BandWidthName"),
                            Uplink = r.Field<Int32>("Uplink"),
                            UplinkFrequency = r.Field<string>("UplinkFrequency"),
                            Downlink = r.Field<Int32?>("Downlink"),
                            DownlinkFrequency = r.Field<string>("DownlinkFrequency"),
                            ActiveStat = r.Field<bool?>("ActiveStat"),
                            Description = r.Field<string>("Description")

                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bandwidthBO;
        }
        public bool SaveOrUpdateBandwidth(ServiceBandwidthBO bandwidthBO, out long outId)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateBandwidth_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ServiceBandWidthId", DbType.Int32, bandwidthBO.ServiceBandWidthId);
                            dbSmartAspects.AddInParameter(command, "@BandWidthName", DbType.String, bandwidthBO.BandWidthName);
                            dbSmartAspects.AddInParameter(command, "@Uplink", DbType.Int32, bandwidthBO.Uplink);
                            dbSmartAspects.AddInParameter(command, "@UplinkFrequency", DbType.String, bandwidthBO.UplinkFrequency);
                            dbSmartAspects.AddInParameter(command, "@Downlink", DbType.Int32, bandwidthBO.Downlink);
                            dbSmartAspects.AddInParameter(command, "@DownlinkFrequency", DbType.String, bandwidthBO.DownlinkFrequency);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bandwidthBO.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, bandwidthBO.Description);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, bandwidthBO.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            status = (dbSmartAspects.ExecuteNonQuery(command, transction) > 0);

                            outId = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        }

                        if (status)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                            status = false;
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
        #endregion
    }
}
