using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SalesManagment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class CostAnalysisDA : BaseService
    {
        public Boolean SaveCostAnalysis(SMCostAnalysis costAnalysis, List<SMCostAnalysisDetail> costAnalysisItemDetails, List<SMCostAnalysisDetail> costAnalysisServiceDetails, out int costAnalysisId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCostAnalysis_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, costAnalysis.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, costAnalysis.Name);
                        dbSmartAspects.AddInParameter(command, "@TotalCost", DbType.Decimal, costAnalysis.TotalCost);
                        dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, costAnalysis.GrandTotal);
                        dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, costAnalysis.DiscountType);
                        dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, costAnalysis.DiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, costAnalysis.CalculatedDiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@DiscountedAmount", DbType.Decimal, costAnalysis.DiscountedAmount);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, costAnalysis.Remarks);
                        dbSmartAspects.AddInParameter(command, "@UserinfoId", DbType.Int32, costAnalysis.CreatedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;

                        costAnalysisId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }

                    if (status && costAnalysisItemDetails.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCostAnalysisDetail_SP"))
                        {
                            foreach (SMCostAnalysisDetail detailBO in costAnalysisItemDetails)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detailBO.Id);
                                dbSmartAspects.AddInParameter(cmd, "@SMCostAnalysisId", DbType.Int64, costAnalysisId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalOfferedPrice", DbType.Decimal, detailBO.TotalOfferedPrice);
                                dbSmartAspects.AddInParameter(cmd, "@AverageCost", DbType.Decimal, detailBO.AverageCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalCost", DbType.Decimal, detailBO.TotalCost);
                                dbSmartAspects.AddInParameter(cmd, "@AdditionalCost", DbType.Decimal, detailBO.AdditionalCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalProjetcedCost", DbType.Decimal, detailBO.TotalProjetcedCost);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status && costAnalysisServiceDetails.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCostAnalysisDetail_SP"))
                        {
                            foreach (SMCostAnalysisDetail detailBO in costAnalysisServiceDetails)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detailBO.Id);
                                dbSmartAspects.AddInParameter(cmd, "@SMCostAnalysisId", DbType.Int64, costAnalysisId);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalOfferedPrice", DbType.Decimal, detailBO.TotalOfferedPrice);
                                dbSmartAspects.AddInParameter(cmd, "@AverageCost", DbType.Decimal, detailBO.AverageCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalCost", DbType.Decimal, detailBO.TotalCost);
                                dbSmartAspects.AddInParameter(cmd, "@AdditionalCost", DbType.Decimal, detailBO.AdditionalCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalProjetcedCost", DbType.Decimal, detailBO.TotalProjetcedCost);

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

        public Boolean UpdateCostAnalysis(SMCostAnalysis costAnalysis, List<SMCostAnalysisDetail> costAnalysisDetailsToAdded, List<SMCostAnalysisDetail> costAnalysisServiceDetailsToAdded,
                                       List<SMCostAnalysisDetail> costAnalysisDetailsToDelete, List<SMCostAnalysisDetail> costAnalysisServiceDetailsToDelete)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCostAnalysis_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, costAnalysis.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, costAnalysis.Name);
                        dbSmartAspects.AddInParameter(command, "@TotalCost", DbType.Decimal, costAnalysis.TotalCost);
                        dbSmartAspects.AddInParameter(command, "@GrandTotal", DbType.Decimal, costAnalysis.GrandTotal);
                        dbSmartAspects.AddInParameter(command, "@DiscountType", DbType.String, costAnalysis.DiscountType);
                        dbSmartAspects.AddInParameter(command, "@DiscountAmount", DbType.Decimal, costAnalysis.DiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@CalculatedDiscountAmount", DbType.Decimal, costAnalysis.CalculatedDiscountAmount);
                        dbSmartAspects.AddInParameter(command, "@DiscountedAmount", DbType.Decimal, costAnalysis.DiscountedAmount);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, costAnalysis.Remarks);
                        dbSmartAspects.AddInParameter(command, "@UserinfoId", DbType.Int32, costAnalysis.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                    }

                    if (status && costAnalysisDetailsToAdded.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCostAnalysisDetail_SP"))
                        {
                            foreach (SMCostAnalysisDetail detailBO in costAnalysisDetailsToAdded)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detailBO.Id);
                                dbSmartAspects.AddInParameter(cmd, "@SMCostAnalysisId", DbType.Int64, costAnalysis.Id);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalOfferedPrice", DbType.Decimal, detailBO.TotalOfferedPrice);
                                dbSmartAspects.AddInParameter(cmd, "@AverageCost", DbType.Decimal, detailBO.AverageCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalCost", DbType.Decimal, detailBO.TotalCost);
                                dbSmartAspects.AddInParameter(cmd, "@AdditionalCost", DbType.Decimal, detailBO.AdditionalCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalProjetcedCost", DbType.Decimal, detailBO.TotalProjetcedCost);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status && costAnalysisServiceDetailsToAdded.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCostAnalysisDetail_SP"))
                        {
                            foreach (SMCostAnalysisDetail detailBO in costAnalysisServiceDetailsToAdded)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detailBO.Id);
                                dbSmartAspects.AddInParameter(cmd, "@SMCostAnalysisId", DbType.Int64, costAnalysis.Id);
                                dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, detailBO.ItemType);
                                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, detailBO.CategoryId);
                                dbSmartAspects.AddInParameter(cmd, "@ServicePackageId", DbType.Int32, detailBO.ServicePackageId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceBandWidthId", DbType.Int32, detailBO.ServiceBandWidthId);
                                dbSmartAspects.AddInParameter(cmd, "@ServiceTypeId", DbType.Int32, detailBO.ServiceTypeId);
                                //dbSmartAspects.AddInParameter(cmd, "@UpLink", DbType.Decimal, detailBO.UpLink);
                                dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, detailBO.ItemId);
                                dbSmartAspects.AddInParameter(cmd, "@StockBy", DbType.Int32, detailBO.StockBy);
                                dbSmartAspects.AddInParameter(cmd, "@Quantity", DbType.Decimal, detailBO.Quantity);
                                dbSmartAspects.AddInParameter(cmd, "@UnitPrice", DbType.Decimal, detailBO.UnitPrice);
                                dbSmartAspects.AddInParameter(cmd, "@TotalOfferedPrice", DbType.Decimal, detailBO.TotalOfferedPrice);
                                dbSmartAspects.AddInParameter(cmd, "@AverageCost", DbType.Decimal, detailBO.AverageCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalCost", DbType.Decimal, detailBO.TotalCost);
                                dbSmartAspects.AddInParameter(cmd, "@AdditionalCost", DbType.Decimal, detailBO.AdditionalCost);
                                dbSmartAspects.AddInParameter(cmd, "@TotalProjetcedCost", DbType.Decimal, detailBO.TotalProjetcedCost);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status && costAnalysisDetailsToDelete.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            foreach (SMCostAnalysisDetail detailBO in costAnalysisDetailsToDelete)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "SMCostAnalysisDetail");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "Id");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKId", DbType.String, detailBO.Id);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false; ;
                            }
                        }
                    }

                    if (status && costAnalysisServiceDetailsToDelete.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            foreach (SMCostAnalysisDetail detailBO in costAnalysisServiceDetailsToDelete)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@TableName", DbType.String, "SMCostAnalysisDetail");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKField", DbType.String, "Id");
                                dbSmartAspects.AddInParameter(cmd, "@TablePKId", DbType.String, detailBO.Id);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false; ;
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

        public List<SMCostAnalysis> GetCostAnalysisWithPagination(string name, DateTime fromDate, DateTime toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMCostAnalysis> costAnalysisList = new List<SMCostAnalysis>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostAnalysisWithPagination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");

                    costAnalysisList = ds.Tables[0].AsEnumerable().Select(r => new SMCostAnalysis
                    {
                        Id = r.Field<long>("Id"),
                        Name = r.Field<string>("Name"),
                        Remarks = r.Field<string>("Remarks"),
                        DiscountType = r.Field<string>("DiscountType"),
                        TotalCost = r.Field<decimal>("TotalCost"),
                        GrandTotal = r.Field<decimal>("GrandTotal"),
                        DiscountAmount = r.Field<decimal>("DiscountAmount"),
                        CalculatedDiscountAmount = r.Field<decimal>("CalculatedDiscountAmount"),
                        DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CreatedDate = r.Field<DateTime?>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return costAnalysisList;
        }

        public SMCostAnalysisView GetCostAnalysisViewById(long id)
        {
            SMCostAnalysisView costAnalysis = new SMCostAnalysisView();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostAnalysisViewById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CostAnalysis");

                    if (ds.Tables[0] != null)
                    {
                        costAnalysis.CostAnalysis = ds.Tables[0].AsEnumerable().Select(r => new SMCostAnalysis
                        {
                            Id = r.Field<long>("Id"),
                            Name = r.Field<string>("Name"),
                            Remarks = r.Field<string>("Remarks"),
                            DiscountType = r.Field<string>("DiscountType"),
                            TotalCost = r.Field<decimal>("TotalCost"),
                            GrandTotal = r.Field<decimal>("GrandTotal"),
                            DiscountAmount = r.Field<decimal>("DiscountAmount"),
                            CalculatedDiscountAmount = r.Field<decimal>("CalculatedDiscountAmount"),
                            DiscountedAmount = r.Field<decimal>("DiscountedAmount"),
                            CreatedBy = r.Field<int?>("CreatedBy"),
                            CreatedDate = r.Field<DateTime?>("CreatedDate"),
                            LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                            LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")
                        }).FirstOrDefault();
                    }

                    if (ds.Tables[1] != null)
                    {
                        costAnalysis.AllItems = ds.Tables[1].AsEnumerable().Select(r => new SMCostAnalysisDetail
                        {
                            Id = r.Field<long>("Id"),
                            SMCostAnalysisId = r.Field<long>("SMCostAnalysisId"),
                            ItemType = r.Field<string>("ItemType"),
                            CategoryId = r.Field<int?>("CategoryId"),
                            ServiceTypeId = r.Field<int?>("ServiceTypeId"),
                            ServicePackageId = r.Field<int?>("ServicePackageId"),
                            ServiceBandWidthId = r.Field<int?>("ServiceBandWidthId"),
                            UpLink = r.Field<int>("UpLink"),
                            ItemId = r.Field<int>("ItemId"),
                            ItemName = r.Field<string>("ItemName"),
                            StockBy = r.Field<int>("StockBy"),
                            StockByName = r.Field<string>("StockByName"),
                            Quantity = r.Field<decimal>("Quantity"),
                            UnitPrice = r.Field<decimal>("UnitPrice"),
                            TotalOfferedPrice = r.Field<decimal>("TotalOfferedPrice"),
                            AverageCost = r.Field<decimal>("AverageCost"),
                            TotalCost = r.Field<decimal>("TotalCost"),
                            AdditionalCost = r.Field<decimal>("AdditionalCost"),
                            TotalProjetcedCost = r.Field<decimal>("TotalProjetcedCost")
                        }).ToList();
                    }

                }
            }
            return costAnalysis;
        }
        
    }
}
