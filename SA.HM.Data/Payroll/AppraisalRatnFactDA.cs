using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class AppraisalRatnFactDA : BaseService
    {
        public Boolean SaveAppraisalRtngFactInfo(AppraisalRatingFactorBO apprRtngFact)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveApprRtngFactInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@AppraisalIndicatorId", DbType.Int32, apprRtngFact.AppraisalIndicatorId);
                        dbSmartAspects.AddInParameter(command, "@RatingFactorName", DbType.String, apprRtngFact.RatingFactorName);
                        dbSmartAspects.AddInParameter(command, "@RatingWeight", DbType.Decimal, apprRtngFact.RatingWeight);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, apprRtngFact.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, apprRtngFact.CreatedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean UpdateAppraisalRtngFactInfo(AppraisalRatingFactorBO apprRtngFact)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateApprRtngFactInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RatingFactorId", DbType.Int32, apprRtngFact.RatingFactorId);
                        dbSmartAspects.AddInParameter(command, "@AppraisalIndicatorId", DbType.Int32, apprRtngFact.AppraisalIndicatorId);
                        dbSmartAspects.AddInParameter(command, "@RatingFactorName", DbType.String, apprRtngFact.RatingFactorName);
                        dbSmartAspects.AddInParameter(command, "@RatingWeight", DbType.Decimal, apprRtngFact.RatingWeight);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, apprRtngFact.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, apprRtngFact.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<AppraisalRatingFactorBO> GetAppraisalRtngFactInfoBySearchCriteriaForPagination(string marksIndId, string rtngFactName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<AppraisalRatingFactorBO> advtknList = new List<AppraisalRatingFactorBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprRtngFactInfoBySearchCriteriaForPagination_SP"))
                {
                    if (!string.IsNullOrEmpty(marksIndId))
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalIndicatorId", DbType.Int32, marksIndId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalIndicatorId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(rtngFactName))
                        dbSmartAspects.AddInParameter(cmd, "@RatingFactorName", DbType.String, rtngFactName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RatingFactorName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    //DataSet ds = new DataSet();
                    //dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalRatingFactor");
                    //DataTable Table = ds.Tables["PayrollAppraisalRatingFactor"];

                    //advtknList = Table.AsEnumerable().Select(r => new AppraisalRatingFactorBO
                    //{
                    //    RatingFactorId = r.Field<Int32>("RatingFactorId"),
                    //    RatingFactorName = r.Field<String>("RatingFactorName"),
                    //    AppraisalIndicatorName = r.Field<String>("AppraisalIndicatorName"),
                    //    RatingWeight = r.Field<Decimal>("RatingWeight"),
                    //    Remarks = r.Field<String>("Remarks"),

                    //}).ToList();
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalRatingFactorBO bO = new AppraisalRatingFactorBO();
                                bO.RatingFactorId = Convert.ToInt32(reader["RatingFactorId"]);
                                bO.RatingFactorName = Convert.ToString(reader["RatingFactorName"]);
                                bO.AppraisalIndicatorName = Convert.ToString(reader["AppraisalIndicatorName"]);
                                bO.RatingWeight = Convert.ToDecimal(reader["RatingWeight"]);
                                bO.Remarks = Convert.ToString(reader["Remarks"]);

                                advtknList.Add(bO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return advtknList;
        }

        public AppraisalRatingFactorBO GetAppraisalRtngFactInfoById(int rtngFactId)
        {
            AppraisalRatingFactorBO advanceTakenBO = new AppraisalRatingFactorBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalRtngFactInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RatingFactorId", DbType.Int32, rtngFactId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalRatingFactor");
                    DataTable Table = ds.Tables["PayrollAppraisalRatingFactor"];

                    advanceTakenBO = Table.AsEnumerable().Select(r => new AppraisalRatingFactorBO
                    {
                        RatingFactorId = r.Field<Int32>("RatingFactorId"),
                        AppraisalIndicatorId = r.Field<Int32>("AppraisalIndicatorId"),
                        RatingFactorName = r.Field<String>("RatingFactorName"),
                        RatingWeight = r.Field<Decimal>("RatingWeight"),
                        Remarks = r.Field<string>("Remarks"),

                    }).FirstOrDefault();
                }
            }
            return advanceTakenBO;
        }

        public Boolean DeleteRtngFactrById(int itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRtngFactInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RatingFactorId", DbType.Int32, itemId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<AppraisalRatingFactorBO> GetAllRatingFactorInfo()
        {
            List<AppraisalRatingFactorBO> rtngFactBOList = new List<AppraisalRatingFactorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllRtngFactor_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalRatingFactorBO rtngFactBO = new AppraisalRatingFactorBO();

                                rtngFactBO.AppraisalIndicatorId = Convert.ToInt32(reader["AppraisalIndicatorId"]);
                                rtngFactBO.RatingFactorId = Convert.ToInt32(reader["RatingFactorId"]);
                                rtngFactBO.RatingFactorName = reader["RatingFactorName"].ToString();
                                rtngFactBO.RatingWeight = Convert.ToDecimal(reader["RatingWeight"]);

                                rtngFactBOList.Add(rtngFactBO);
                            }
                        }
                    }
                }
            }
            return rtngFactBOList;
        }
        
        public List<AppraisalRatingFactorBO> GetRatingFactorBySetupCriteria(int appraisalEvalutionById)
        {
            List<AppraisalRatingFactorBO> rtngFactBOList = new List<AppraisalRatingFactorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRatingFactorBySetupCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalutionById);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalRatingFactorBO rtngFactBO = new AppraisalRatingFactorBO();

                                rtngFactBO.AppraisalIndicatorId = Convert.ToInt32(reader["AppraisalIndicatorId"]);
                                rtngFactBO.RatingFactorId = Convert.ToInt32(reader["RatingFactorId"]);
                                rtngFactBO.RatingFactorName = reader["RatingFactorName"].ToString();
                                rtngFactBO.RatingWeight = Convert.ToDecimal(reader["RatingWeight"]);
                                rtngFactBO.RatingFacotrDetailsId = Convert.ToInt32(reader["RatingFacotrDetailsId"]);
                                rtngFactBO.Remarks = reader["Remarks"].ToString();

                                rtngFactBOList.Add(rtngFactBO);
                            }
                        }
                    }
                }
            }
            return rtngFactBOList;
        }
    }
}
