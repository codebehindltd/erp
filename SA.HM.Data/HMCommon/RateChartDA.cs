using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.HMCommon
{
    public class RateChartDA : BaseService
    {
        public bool SaveOrUpdateRateChart(RateChartMaster RateChartMaster, List<RateChartDetail> RateChartDetail, string DeletedRateChartDetailIdList, string DeletedRateChartDiscountDetailIdList, out long rateMasterId)
        {
            long RateChartDetailId = 0;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRateChartMaster_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, RateChartMaster.Id);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, RateChartMaster.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@PromotionName", DbType.String, RateChartMaster.PromotionName);
                        dbSmartAspects.AddInParameter(command, "@EffectFrom", DbType.DateTime, RateChartMaster.EffectFrom);
                        dbSmartAspects.AddInParameter(command, "@EffectTo", DbType.DateTime, RateChartMaster.EffectTo);
                        dbSmartAspects.AddInParameter(command, "@RateChartFor", DbType.String, RateChartMaster.RateChartFor);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, RateChartMaster.CreatedBy);
                        dbSmartAspects.AddInParameter(command, "@TotalPrice", DbType.Decimal, RateChartMaster.TotalPrice);
                        dbSmartAspects.AddOutParameter(command, "@RateMasterId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;

                        rateMasterId = Convert.ToInt64(command.Parameters["@RateMasterId"].Value);
                    }

                    if (status && RateChartDetail.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRateChartDetail_SP"))
                        {
                            foreach (RateChartDetail detailBO in RateChartDetail)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detailBO.Id);
                                dbSmartAspects.AddInParameter(cmd, "@RateChartMasterId", DbType.Int64, rateMasterId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, detailBO.ServiceType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, detailBO.TotalPrice);
                                dbSmartAspects.AddInParameter(cmd, "@IsDiscountForAll", DbType.Boolean, detailBO.IsDiscountForAll);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, detailBO.DiscountType);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmount", DbType.Decimal, detailBO.DiscountAmount);
                                dbSmartAspects.AddInParameter(cmd, "@DiscountAmountUSD", DbType.Decimal, detailBO.DiscountAmountUSD);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);

                                dbSmartAspects.AddOutParameter(cmd, "@RateChartDetailId", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                RateChartDetailId = Convert.ToInt64(cmd.Parameters["@RateChartDetailId"].Value);

                                if (status && detailBO.RateChartDiscountDetails.Count > 0)
                                {
                                    using (DbCommand cmdDiscount = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRateChartDiscountDetails_SP"))
                                    {
                                        foreach (RateChartDiscountDetail discountDetailBO in detailBO.RateChartDiscountDetails)
                                        {
                                            cmdDiscount.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Id", DbType.Int64, discountDetailBO.Id);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@RateChartDetailId", DbType.Int64, RateChartDetailId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@OutLetId", DbType.Int64, discountDetailBO.OutLetId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@Type", DbType.String, discountDetailBO.Type);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@TypeId", DbType.Int32, discountDetailBO.TypeId);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountType", DbType.String, discountDetailBO.DiscountType);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmount", DbType.Decimal, discountDetailBO.DiscountAmount);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@DiscountAmountUSD", DbType.Decimal, discountDetailBO.DiscountAmountUSD);
                                            dbSmartAspects.AddInParameter(cmdDiscount, "@OfferredPrice", DbType.Decimal, discountDetailBO.OfferredPrice);

                                            status = dbSmartAspects.ExecuteNonQuery(cmdDiscount, transaction) > 0 ? true : false;

                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (status && !string.IsNullOrWhiteSpace(DeletedRateChartDetailIdList))
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataListDynamically_SP"))
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "RateChartDetail");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "Id");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKIdList", DbType.String, DeletedRateChartDetailIdList);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

                        }
                    }

                    if (status && !string.IsNullOrWhiteSpace(DeletedRateChartDiscountDetailIdList))
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataListDynamically_SP"))
                        {
                            cmd.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "RateChartDiscountDetail");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "Id");
                            dbSmartAspects.AddInParameter(cmd, "@TablePKIdList", DbType.String, DeletedRateChartDiscountDetailIdList);

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

        public List<RateChartDetail> GetRateChartDetailByRateChartMasterIdForEdit(long rateChartMasterId)
        {
            List<RateChartDetail> RateChartDetail = new List<RateChartDetail>();
            List<RateChartDiscountDetail> RateChartDiscountDetail = new List<RateChartDiscountDetail>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRateChartDetailByRateChartMasterIdForEdit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RateChartMasterId", DbType.Int64, rateChartMasterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Quotationdetails");

                    RateChartDetail = ds.Tables[0].AsEnumerable().Select(r => new RateChartDetail
                    {
                        Id = r.Field<long>("Id"),
                        RateChartMasterId = r.Field<long>("RateChartMasterId"),
                        ServiceType = r.Field<string>("ServiceType"),
                        CategoryId = r.Field<int>("CategoryId"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<int>("StockBy"),
                        UnitHead = r.Field<string>("UnitHead"),
                        Quantity = r.Field<decimal>("Quantity"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        TotalPrice = r.Field<decimal>("TotalPrice"),
                        Uplink = r.Field<int>("Uplink"),
                        Downlink = r.Field<int>("Downlink"),
                        ServicePackageId = r.Field<int?>("ServicePackageId"),
                        PackageName = r.Field<string>("PackageName"),
                        IsDiscountForAll = r.Field<bool>("IsDiscountForAll"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        DiscountAmountUSD = r.Field<decimal?>("DiscountAmountUSD"),
                        ServiceTypeId = r.Field<int?>("ServiceTypeId")
                    }).ToList();

                    RateChartDiscountDetail = ds.Tables[1].AsEnumerable().Select(r => new RateChartDiscountDetail
                    {
                        Id = r.Field<long>("Id"),
                        RateChartDetailId = r.Field<long>("RateChartDetailId"),
                        OutLetId = r.Field<long>("OutLetId"),
                        Type = r.Field<string>("Type"),
                        TypeId = r.Field<int>("TypeId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        DiscountAmount = r.Field<decimal?>("DiscountAmount"),
                        DiscountAmountUSD = r.Field<decimal?>("DiscountAmountUSD"),
                        OfferredPrice = r.Field<decimal?>("OfferredPrice"),                           
                        OutLetName = r.Field<string>("OutLetName"),
                        TypeName = r.Field<string>("TypeName")

                    }).ToList();
                    foreach (var detail in RateChartDetail)
                    {
                        detail.RateChartDiscountDetails = RateChartDiscountDetail.Where(i => i.RateChartDetailId == detail.Id).ToList();
                    }
                }
            }
            return RateChartDetail;
        }

        public List<RateChartMaster> GetRateChartBySearchCriteriaWithPagination(string promotionName, int companyId, DateTime effectFrom, DateTime effectTo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RateChartMaster> productList = new List<RateChartMaster>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRateChartBySearchCriteriaWithPagination_SP"))
                {
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    if (!string.IsNullOrEmpty(promotionName))
                        dbSmartAspects.AddInParameter(cmd, "@PromotionName", DbType.String, promotionName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PromotionName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@EffectFrom", DbType.DateTime, effectFrom);
                    dbSmartAspects.AddInParameter(cmd, "@EffectTo", DbType.DateTime, effectTo);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RateChart");

                    DataTable dataTable = ds.Tables["RateChart"];

                    productList = dataTable.AsEnumerable().Select(r => new RateChartMaster
                    {
                        Id = r.Field<long>("Id"),
                        PromotionName = r.Field<string>("PromotionName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        EffectFrom = r.Field<DateTime>("EffectFrom"),
                        EffectTo = r.Field<DateTime>("EffectTo")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return productList;
        }

        public bool DeleteRateChartById(long rateChartId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteRateChartById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RateChartMasterId", DbType.Int64, rateChartId);

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
    }
}
