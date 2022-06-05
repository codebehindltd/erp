using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class AppraisalMarksIndDA : BaseService
    {
        public Boolean SaveAppraisalMarksIndInfo(AppraisalMarksIndicatorBO apprMarksInd)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveApprMarksIndInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@AppraisalIndicatorName", DbType.String, apprMarksInd.AppraisalIndicatorName);
                        dbSmartAspects.AddInParameter(command, "@AppraisalWeight", DbType.Decimal, apprMarksInd.AppraisalWeight);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, apprMarksInd.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, apprMarksInd.CreatedBy);

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

        public Boolean UpdateAppraisalMarksIndInfo(AppraisalMarksIndicatorBO apprMarksInd)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateApprMarksIndInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@MarksIndicatorId", DbType.Int32, apprMarksInd.MarksIndicatorId);
                        dbSmartAspects.AddInParameter(command, "@AppraisalIndicatorName", DbType.String, apprMarksInd.AppraisalIndicatorName);
                        dbSmartAspects.AddInParameter(command, "@AppraisalWeight", DbType.Decimal, apprMarksInd.AppraisalWeight);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, apprMarksInd.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, apprMarksInd.LastModifiedBy);

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

        public List<AppraisalMarksIndicatorBO> GetAllAppraisalMarksIndInfo()
        {
            List<AppraisalMarksIndicatorBO> apprIndBOList = new List<AppraisalMarksIndicatorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAppraisalMarksIndInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalMarksIndicatorBO apprIndBO = new AppraisalMarksIndicatorBO();

                                apprIndBO.MarksIndicatorId = Convert.ToInt32(reader["MarksIndicatorId"]);
                                apprIndBO.AppraisalIndicatorName = reader["AppraisalIndicatorName"].ToString();
                                apprIndBO.AppraisalWeight = Convert.ToDecimal(reader["AppraisalWeight"].ToString());
                                apprIndBO.Remarks = reader["Remarks"].ToString();

                                apprIndBOList.Add(apprIndBO);
                            }
                        }
                    }
                }
            }
            return apprIndBOList.OrderBy(o => o.MarksIndicatorId).ToList();
        }

        public List<AppraisalMarksIndicatorBO> GetAppraisalMarksIndicatorBySetupCriteria(int appraisalEvalutionById)
        {
            List<AppraisalMarksIndicatorBO> apprIndBOList = new List<AppraisalMarksIndicatorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalMarksIndicatorBySetupCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalutionById);
                    
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalMarksIndicatorBO apprIndBO = new AppraisalMarksIndicatorBO();

                                apprIndBO.MarksIndicatorId = Convert.ToInt32(reader["MarksIndicatorId"]);
                                apprIndBO.AppraisalIndicatorName = reader["AppraisalIndicatorName"].ToString();
                                apprIndBO.AppraisalWeight = Convert.ToDecimal(reader["AppraisalWeight"].ToString());
                                apprIndBO.Remarks = reader["Remarks"].ToString();

                                apprIndBOList.Add(apprIndBO);
                            }
                        }
                    }
                }
            }
            return apprIndBOList.OrderBy(o => o.MarksIndicatorId).ToList();
        }

        public List<AppraisalMarksIndicatorBO> GetAppraisalMarksIndInfoBySearchCriteriaForPagination(string indctrName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<AppraisalMarksIndicatorBO> advtknList = new List<AppraisalMarksIndicatorBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprMarksIndInfoBySearchCriteriaForPagination_SP"))
                {
                    if (!string.IsNullOrEmpty(indctrName))
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalIndicatorName", DbType.String, indctrName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalIndicatorName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalMarksIndicator");
                    DataTable Table = ds.Tables["PayrollAppraisalMarksIndicator"];

                    advtknList = Table.AsEnumerable().Select(r => new AppraisalMarksIndicatorBO
                    {
                        MarksIndicatorId = r.Field<Int32>("MarksIndicatorId"),
                        AppraisalIndicatorName = r.Field<String>("AppraisalIndicatorName"),
                        AppraisalWeight = r.Field<Decimal>("AppraisalWeight"),
                        Remarks = r.Field<String>("Remarks"),

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return advtknList;
        }

        public AppraisalMarksIndicatorBO GetAppraisalMarksIndInfoById(int marksIndId)
        {
            AppraisalMarksIndicatorBO advanceTakenBO = new AppraisalMarksIndicatorBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalMarksIndInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, marksIndId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalMarksIndicator");
                    DataTable Table = ds.Tables["PayrollAppraisalMarksIndicator"];

                    advanceTakenBO = Table.AsEnumerable().Select(r => new AppraisalMarksIndicatorBO
                    {
                        MarksIndicatorId = r.Field<Int32>("MarksIndicatorId"),
                        AppraisalIndicatorName = r.Field<String>("AppraisalIndicatorName"),
                        AppraisalWeight = r.Field<Decimal>("AppraisalWeight"),
                        Remarks = r.Field<string>("Remarks"),

                    }).FirstOrDefault();
                }
            }
            return advanceTakenBO;
        }

        public Boolean DeleteMarksIndById(int itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteMarksIndInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MarksIndicatorId", DbType.Int32, itemId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
