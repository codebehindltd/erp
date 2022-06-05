using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HMCommon
{
    public class DiscountDA : BaseService
    {
        public bool SaveDiscount(DiscountMasterBO discount, out long discountId)
        {
            bool status = false;
            int executeValue = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDiscount_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@DiscountName", DbType.String, discount.DiscountName);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, discount.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, discount.FromDate);
                            dbSmartAspects.AddInParameter(command, "@Todate", DbType.DateTime, discount.Todate);
                            dbSmartAspects.AddInParameter(command, "@DiscountFor", DbType.String, discount.DiscountFor);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, discount.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, discount.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));

                            executeValue = dbSmartAspects.ExecuteNonQuery(command, transction);

                            discountId = Convert.ToInt64(command.Parameters["@Id"].Value);
                        }
                        if (executeValue > 0 && discount.DiscountDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDiscountDetail_SP"))
                            {
                                foreach (var detail in discount.DiscountDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@DiscountMasterId", DbType.Int64, discountId);
                                    dbSmartAspects.AddInParameter(command, "@DiscountForId", DbType.Int64, detail.DiscountForId);
                                    dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, detail.DiscountType);
                                    dbSmartAspects.AddInParameter(command, "@Discount", DbType.Decimal, detail.Discount);

                                    executeValue = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        if (executeValue > 0)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }
            return status;
        }
        public bool UpdateDiscount(DiscountMasterBO discount, string deletedItemIds)
        {
            bool status = false;
            int executeValue = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDiscount_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, discount.Id);
                            dbSmartAspects.AddInParameter(command, "@DiscountName", DbType.String, discount.DiscountName);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, discount.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, discount.FromDate);
                            dbSmartAspects.AddInParameter(command, "@Todate", DbType.DateTime, discount.Todate);
                            dbSmartAspects.AddInParameter(command, "@DiscountFor", DbType.String, discount.DiscountFor);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, discount.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, discount.CreatedBy);


                            executeValue = dbSmartAspects.ExecuteNonQuery(command, transction);

                        }
                        if (executeValue > 0 && discount.DiscountDetails.Count > 0)
                        {
                            foreach (var detail in discount.DiscountDetails)
                            {
                                if (detail.Id > 0)
                                {
                                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDiscountDetail_SP"))
                                    {
                                        command.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, detail.Id);
                                        dbSmartAspects.AddInParameter(command, "@DiscountForId", DbType.Int64, detail.DiscountForId);
                                        dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, detail.DiscountType);
                                        dbSmartAspects.AddInParameter(command, "@Discount", DbType.Decimal, detail.Discount);

                                        executeValue = dbSmartAspects.ExecuteNonQuery(command, transction);
                                    }
                                }
                                else
                                {
                                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDiscountDetail_SP"))
                                    {
                                        command.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(command, "@DiscountMasterId", DbType.Int64, discount.Id);
                                        dbSmartAspects.AddInParameter(command, "@DiscountForId", DbType.Int64, detail.DiscountForId);
                                        dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, detail.DiscountType);
                                        dbSmartAspects.AddInParameter(command, "@Discount", DbType.Decimal, detail.Discount);

                                        executeValue = dbSmartAspects.ExecuteNonQuery(command, transction);
                                    }
                                }


                            }

                        }

                        if (executeValue > 0 && !string.IsNullOrEmpty(deletedItemIds))
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDiscountDetail_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@DiscountDetailsIdList", DbType.String, deletedItemIds);

                                executeValue = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }

                        }
                        if (executeValue > 0)
                        {
                            transction.Commit();
                            status = true;
                        }
                        else
                        {
                            transction.Rollback();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        status = false;
                        throw ex;
                    }
                }
            }

            return status;
        }
        public bool DeleteDiscount(long id)
        {
            bool status = false;
            int executeValue = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                try
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDiscount_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, id);

                        executeValue = dbSmartAspects.ExecuteNonQuery(command);
                    }
                    status = (executeValue > 0);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return status;
        }
        public List<DiscountMasterBO> GetAllDiscount(DateTime fromDate, DateTime toDate, int costCenterId, string Name)
        {
            List<DiscountMasterBO> discountMasterBOs = new List<DiscountMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetAllDiscountBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, toDate);

                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, DBNull.Value);
                    if (!string.IsNullOrEmpty(Name))
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, Name);
                    else
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "Discount");
                    DataTable Table = ds.Tables["Discount"];

                    discountMasterBOs = Table.AsEnumerable().Select(r => new DiscountMasterBO
                    {
                        Id = r.Field<long>("Id"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        Todate = r.Field<DateTime>("Todate"),
                        DiscountFor = r.Field<string>("DiscountFor"),
                        Remarks = r.Field<string>("Remarks"),
                        DiscountName = r.Field<string>("DiscountName"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")
                    }).ToList();

                }
            }

            return discountMasterBOs;
        }

        public DiscountMasterBO GetDiscountById(long Id)
        {
            DiscountMasterBO discount = new DiscountMasterBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetDiscountById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, Id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "Discount");

                    discount = ds.Tables[0].AsEnumerable().Select(r => new DiscountMasterBO
                    {
                        Id = r.Field<long>("Id"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        Todate = r.Field<DateTime>("Todate"),
                        DiscountFor = r.Field<string>("DiscountFor"),
                        Remarks = r.Field<string>("Remarks"),
                        DiscountName = r.Field<string>("DiscountName"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")
                    }).FirstOrDefault();

                    discount.DiscountDetails = ds.Tables[1].AsEnumerable().Select(r => new DiscountDetailBO
                    {
                        Id = r.Field<long>("Id"),
                        DiscountMasterId = r.Field<long>("DiscountMasterId"),
                        DiscountForId = r.Field<long>("DiscountForId"),
                        DiscountForName = r.Field<string>("DiscountForName"),
                        DiscountType = r.Field<string>("DiscountType"),
                        Discount = r.Field<decimal>("Discount")
                    }).ToList();

                }
            }

            return discount;
        }

        public List<GetDiscountDetailsBO> GetAllDiscountByCostcenterId(long Id)
        {
            List<GetDiscountDetailsBO> discountBOs = new List<GetDiscountDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetAllDiscount_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DiscountFor", DbType.String, "Item");
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, Id);
                    
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "Discount");
                    DataTable Table = ds.Tables["Discount"];

                    discountBOs = Table.AsEnumerable().Select(r => new GetDiscountDetailsBO
                    {
                        DiscountFor = r.Field<string>("DiscountFor"),
                        DiscountName = r.Field<string>("DiscountName"),
                        DiscountForId = r.Field<Int64>("DiscountForId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        Discount = r.Field<decimal>("Discount")
                    }).ToList();

                }

                List<GetDiscountDetailsBO> discountBOCategory = new List<GetDiscountDetailsBO>();

                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetAllDiscount_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DiscountFor", DbType.String, "Category");
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, Id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "Discount");
                    DataTable Table = ds.Tables["Discount"];

                    discountBOCategory = Table.AsEnumerable().Select(r => new GetDiscountDetailsBO
                    {
                        DiscountFor = r.Field<string>("DiscountFor"),
                        DiscountName = r.Field<string>("DiscountName"),
                        DiscountForId = r.Field<Int64>("DiscountForId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        Discount = r.Field<decimal>("Discount")
                    }).ToList();

                }
                discountBOs.AddRange(discountBOCategory);

            }

            return discountBOs;
        }

    }
}
