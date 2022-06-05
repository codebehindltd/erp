using HotelManagement.Entity.FixedAsset;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.FixedAsset
{
    public class DepreciationDA : BaseService
    {
        public FADepreciationViewBO GetFADepreciationDetailsByNCompanyIdNProjectIdNFiscalYearId(int companyId, int projectcId, int fiscalYearId)
        {
            FADepreciationViewBO DepreciationView = new FADepreciationViewBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFADepreciationDetailsByCompanyIdNProjectIdNFiscalYearId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectcId", DbType.Int64, projectcId);
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DepreciationDetails");
                    if (ds.Tables[0] != null)
                        DepreciationView.DepreciationBO = ds.Tables[0].AsEnumerable().Select(r => new FADepreciationBO
                        {
                            Id = r.Field<Int64>("Id"),
                            CompanyId = r.Field<int>("CompanyId"),
                            ProjectId = r.Field<int>("ProjectId"),
                            FiscalYearId = r.Field<int>("FiscalYearId"),
                            AccountHeadId = r.Field<int>("AccountHeadId")

                        }).FirstOrDefault();

                    if (ds.Tables[1] != null)
                        DepreciationView.DepreciationDetailsBOList = ds.Tables[1].AsEnumerable().Select(r => new FADepreciationDetailsBO
                        {
                            Id = r.Field<Int64>("Id"),
                            DepreciationId = r.Field<Int64>("DepreciationId"),
                            TransactionNodeId = r.Field<Int64>("TransactionNodeId"),
                            DepreciationPercentage = r.Field<decimal?>("DepreciationPercentage")

                        }).ToList();

                }
            }
            return DepreciationView;
        }
        public bool SaveOrUpdateDepreciation(FADepreciationBO Depreciation, List<FADepreciationDetailsBO> DepreciationDetailsList, out long DepreciationId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateDepreciation_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, Depreciation.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, Depreciation.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, Depreciation.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, Depreciation.FiscalYearId);
                            dbSmartAspects.AddInParameter(cmd, "@AccountHeadId", DbType.Int32, Depreciation.AccountHeadId);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, Depreciation.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, Depreciation.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@DepreciationId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            DepreciationId = Convert.ToInt64(cmd.Parameters["@DepreciationId"].Value);

                        }

                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateDepreciationDetails_SP"))
                            {
                                foreach (var detail in DepreciationDetailsList)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detail.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@DepreciationId", DbType.Int64, DepreciationId);
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionNodeId", DbType.Int64, detail.TransactionNodeId);
                                    dbSmartAspects.AddInParameter(cmd, "@DepreciationPercentage", DbType.Decimal, detail.DepreciationPercentage);
                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

    }
}
