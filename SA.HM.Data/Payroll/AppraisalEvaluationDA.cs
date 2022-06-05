using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class AppraisalEvaluationDA : BaseService
    {
        public Boolean SaveAppraisalEvaluation(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> appraisalEvalutionRatingList, out int evaluationId)
        {
            try
            {
                int evaluatorId = Convert.ToInt32(appraisalEvalution.EvalutiorId);
                Boolean status = false;
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAppraisalEvaluationByInfo_SP"))
                        {
                            //dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, appraisalEvalution.EmpId);
                            dbSmartAspects.AddInParameter(command, "@EvalutiorId", DbType.Int32, evaluatorId);
                            dbSmartAspects.AddInParameter(command, "@EvalutionFromDate", DbType.DateTime, appraisalEvalution.EvaluationFromDate);
                            dbSmartAspects.AddInParameter(command, "@EvalutionToDate", DbType.DateTime, appraisalEvalution.EvaluationToDate);
                            dbSmartAspects.AddInParameter(command, "@AppraisalType", DbType.String, appraisalEvalution.AppraisalType);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, appraisalEvalution.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@EvaluationId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            evaluationId = Convert.ToInt32(command.Parameters["@EvaluationId"].Value);
                        }
                        if (status)
                        {
                            status = false;

                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveAppraisalEvalutionRatingFactorDetails_SP"))
                            {
                                foreach (AppraisalEvalutionRatingFactorDetailsBO appraisalEvalutionRating in appraisalEvalutionRatingList)
                                {
                                    status = false;
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, evaluationId);
                                    dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, appraisalEvalutionRating.MarksIndicatorId);
                                    dbSmartAspects.AddInParameter(cmd, "@AppraisalRatingFactorId", DbType.Int32, appraisalEvalutionRating.AppraisalRatingFactorId);
                                    dbSmartAspects.AddInParameter(cmd, "@Marks", DbType.Decimal, appraisalEvalutionRating.Marks);
                                    dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, appraisalEvalutionRating.Remarks);

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
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }

        public List<AppraisalEvalutionRatingFactorDetailsBO> GetAppraisalEvalutionRatingFactorDetailsList(int id)
        {
            List<AppraisalEvalutionRatingFactorDetailsBO> AppraisalEvalutionRatingFactorDetailsBOList = new List<AppraisalEvalutionRatingFactorDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvalutionRatingFactorDetailsList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalEvalutionRatingFactorDetailsBO BO = new AppraisalEvalutionRatingFactorDetailsBO();
                                BO.RatingFacotrDetailsId = Convert.ToInt32(reader["RatingFacotrDetailsId"]);
                                BO.AppraisalEvalutionById = Convert.ToInt32(reader["AppraisalEvalutionById"]);
                                BO.MarksIndicatorId = Convert.ToInt32(reader["MarksIndicatorId"]);
                                BO.AppraisalRatingFactorId = Convert.ToInt32(reader["AppraisalRatingFactorId"]);
                                BO.AppraisalWeight = Convert.ToDecimal(reader["AppraisalWeight"]);
                                BO.RatingWeight = Convert.ToDecimal(reader["RatingWeight"]);
                                BO.RatingValue = Convert.ToDecimal(reader["RatingValue"]);
                                BO.Marks = Convert.ToDecimal(reader["Marks"]);
                                BO.RatingDropDownValue = reader["RatingDropDownValue"].ToString();
                                BO.Remarks = reader["Remarks"].ToString();
                                AppraisalEvalutionRatingFactorDetailsBOList.Add(BO);
                            }
                        }
                    }
                }
            }
            return AppraisalEvalutionRatingFactorDetailsBOList;
        }

        public List<EmployeeForEvalutionBO> GetEmployeeForEvalutionForGridPaging(int evalutiorId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmployeeForEvalutionBO> EmployeeForEvalutionBOList = new List<EmployeeForEvalutionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByEvalutorIdForGridPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EvalutiorId", DbType.Int32, evalutiorId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    EmployeeForEvalutionBOList = Table.AsEnumerable().Select(r => new EmployeeForEvalutionBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmployeeName = r.Field<string>("EmpCode") + " - " + r.Field<String>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        AppraisalType = r.Field<string>("AppraisalType"),
                        EvaluationFromDate = r.Field<DateTime>("EvaluationFromDate"),

                        SerialNumber = r.Field<Int64>("SerialNumber"),
                        EvaluationToDate = r.Field<DateTime>("EvaluationToDate")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return EmployeeForEvalutionBOList;
        }


        public Boolean UpdateAppraisalEvaluation(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> newAppraisalEvalutionRatingList, List<AppraisalEvalutionRatingFactorDetailsBO> editAppraisalEvalutionRatingList, List<AppraisalEvalutionRatingFactorDetailsBO> deleteAppraisalEvalutionRatingList)
        {
            int evaluatorId = Convert.ToInt32(appraisalEvalution.EvalutiorId);
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAppraisalEvaluationByInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalution.AppraisalEvalutionById);
                            dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, appraisalEvalution.ApprovalStatus);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, appraisalEvalution.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                        {
                            if (newAppraisalEvalutionRatingList.Count > 0)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveAppraisalEvalutionRatingFactorDetails_SP"))
                                {
                                    foreach (AppraisalEvalutionRatingFactorDetailsBO addEvalutionRating in newAppraisalEvalutionRatingList)
                                    {
                                        status = false;
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalution.AppraisalEvalutionById);
                                        dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, addEvalutionRating.MarksIndicatorId);
                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalRatingFactorId", DbType.Int32, addEvalutionRating.AppraisalRatingFactorId);

                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalWeight", DbType.Decimal, addEvalutionRating.AppraisalWeight);
                                        dbSmartAspects.AddInParameter(cmd, "@RatingWeight", DbType.Decimal, addEvalutionRating.RatingWeight);

                                        dbSmartAspects.AddInParameter(cmd, "@RatingValue", DbType.Decimal, addEvalutionRating.RatingValue);
                                        dbSmartAspects.AddInParameter(cmd, "@Marks", DbType.Decimal, addEvalutionRating.Marks);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                    }
                                }
                            }

                            if (editAppraisalEvalutionRatingList.Count > 0)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateAppraisalEvalutionRatingFactorDetails_SP"))
                                {
                                    foreach (AppraisalEvalutionRatingFactorDetailsBO editEvalutionRating in editAppraisalEvalutionRatingList)
                                    {
                                        status = false;
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@RatingFacotrDetailsId", DbType.Int32, editEvalutionRating.RatingFacotrDetailsId);
                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalution.AppraisalEvalutionById);
                                        dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, editEvalutionRating.MarksIndicatorId);
                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalRatingFactorId", DbType.Int32, editEvalutionRating.AppraisalRatingFactorId);
                                        dbSmartAspects.AddInParameter(cmd, "@RatingValue", DbType.Decimal, editEvalutionRating.RatingValue);
                                        dbSmartAspects.AddInParameter(cmd, "@Marks", DbType.Decimal, editEvalutionRating.Marks);
                                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, editEvalutionRating.Remarks);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                    }
                                }
                            }

                            if (deleteAppraisalEvalutionRatingList.Count > 0)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteAppraisalEvalutionRatingFactorDetailsById_SP"))
                                {
                                    foreach (AppraisalEvalutionRatingFactorDetailsBO deleteEvalutionRating in deleteAppraisalEvalutionRatingList)
                                    {
                                        status = false;
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@RatingFacotrDetailsId", DbType.Int32, deleteEvalutionRating.RatingFacotrDetailsId);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                        status = true;
                                    }
                                }
                            }
                        }

                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

                return status;
            }
        }
        public Boolean UpdateAppraisalEvaluationByEvaloator(AppraisalEvalutionByBO appraisalEvalution, List<AppraisalEvalutionRatingFactorDetailsBO> appraisalEvalutionRatingList)
        {
            int evaluatorId = Convert.ToInt32(appraisalEvalution.EvalutiorId);
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAppraisalEvaluationByInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalution.AppraisalEvalutionById);
                            dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, appraisalEvalution.ApprovalStatus);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, appraisalEvalution.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                        {
                            if (appraisalEvalutionRatingList.Count > 0)
                            {
                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateAppraisalEvalutionRatingFactorDetails_SP"))
                                {
                                    foreach (AppraisalEvalutionRatingFactorDetailsBO editEvalutionRating in appraisalEvalutionRatingList)
                                    {
                                        status = false;
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@RatingFacotrDetailsId", DbType.Int32, editEvalutionRating.RatingFacotrDetailsId);
                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalution.AppraisalEvalutionById);
                                        dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, editEvalutionRating.MarksIndicatorId);
                                        dbSmartAspects.AddInParameter(cmd, "@AppraisalRatingFactorId", DbType.Int32, editEvalutionRating.AppraisalRatingFactorId);
                                        dbSmartAspects.AddInParameter(cmd, "@RatingValue", DbType.Decimal, editEvalutionRating.RatingValue);
                                        dbSmartAspects.AddInParameter(cmd, "@Marks", DbType.Decimal, editEvalutionRating.Marks);
                                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, editEvalutionRating.Remarks);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                    }
                                }
                            }
                        }

                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

                return status;
            }
        }
        public Boolean UpdateAppraisalEvaluationApproval(AppraisalEvalutionByBO appraisalEvalution)
        {
            int evaluatorId = Convert.ToInt32(appraisalEvalution.EvalutiorId);
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAppraisalEvaluationByInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalution.AppraisalEvalutionById);
                            dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, appraisalEvalution.ApprovalStatus);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, appraisalEvalution.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }

                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

                return status;
            }
        }

        public List<AppraisalEvaluationViewBO> GetApprEvaluationInfoWithPagination(string empId, string appraisalType, DateTime? fromDate, DateTime? toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<AppraisalEvaluationViewBO> evaluationList = new List<AppraisalEvaluationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvaluationInfoWithPagination_SP"))
                {
                    if (!string.IsNullOrEmpty(empId))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    }

                    if (appraisalType != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, appraisalType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, DBNull.Value);
                    }
                    
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalEvalutionBy");
                    DataTable Table = ds.Tables["PayrollAppraisalEvalutionBy"];

                    evaluationList = Table.AsEnumerable().Select(r => new AppraisalEvaluationViewBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EmpId = r.Field<Int32?>("EmpId"),
                        EmployeeName = r.Field<String>("EmployeeName"),
                        TotalMarks = r.Field<Decimal?>("TotalMarks"),
                        MarksOutOf = r.Field<string>("MarksOutOf"),
                        EvalutiorName = r.Field<String>("EvalutiorName"),
                        AppraisalDuration = r.Field<String>("AppraisalDuration"),
                        EvalutionCompletionByString = r.Field<String>("EvalutionCompletionByString")
                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return evaluationList;
        }

        public AppraisalEvalutionByBO GetAppraisalEvaluationInfoById(int apprEvaId)
        {
            AppraisalEvalutionByBO apprEvaBO = new AppraisalEvalutionByBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvaluationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, apprEvaId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalEvalutionBy");
                    DataTable Table = ds.Tables["PayrollAppraisalEvalutionBy"];

                    apprEvaBO = Table.AsEnumerable().Select(r => new AppraisalEvalutionByBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EvaluationFromDate = r.Field<DateTime>("EvaluationFromDate"),
                        EvaluationToDate = r.Field<DateTime>("EvaluationToDate"),
                        AppraisalType = r.Field<string>("AppraisalType"),
                        EmpId = r.Field<int>("EmpId"),
                        EvalutiorId = r.Field<int>("EvalutiorId"),
                        AppraisalConfigType = r.Field<string>("AppraisalConfigType"),
                        EvalutionCompletionBy = r.Field<DateTime>("EvalutionCompletionBy"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        EmployeeCode = r.Field<string>("ForAppraisalEmployeeCode")

                    }).FirstOrDefault();
                }
            }
            return apprEvaBO;
        }

        public List<AppraisalEvalutionRatingFactorDetailsBO> GetAppraisalEvaluationDetailsInfoByApprIdId(int appraisalEvalutionById, int empId, int evalutiorId)
        {
            List<AppraisalEvalutionRatingFactorDetailsBO> apprEvaBOList = new List<AppraisalEvalutionRatingFactorDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvaluationDetailsInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalutionById);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@EvalutiorId", DbType.Int32, evalutiorId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalEvalutionRatingFactorDetails");
                    DataTable Table = ds.Tables["PayrollAppraisalEvalutionRatingFactorDetails"];

                    apprEvaBOList = Table.AsEnumerable().Select(r => new AppraisalEvalutionRatingFactorDetailsBO
                    {
                        RatingFacotrDetailsId = r.Field<Int32>("RatingFacotrDetailsId"),
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        MarksIndicatorId = r.Field<Int32>("MarksIndicatorId"),
                        AppraisalRatingFactorId = r.Field<Int32>("AppraisalRatingFactorId"),
                        RatingValue = r.Field<Decimal>("RatingValue"),
                        Marks = r.Field<Decimal>("Marks"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return apprEvaBOList.OrderBy(o => o.AppraisalRatingFactorId).ToList();
        }

        public Boolean DeleteAppraisalEvaluationById(int evaluationId)
        {
            try
            {
                Boolean status = false;
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteApprEvaluationDetails_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, evaluationId);

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                        }

                        if (status)
                        {
                            status = false;
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteApprEvaluationinfo_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@AppraisalEvalutionById", DbType.Int32, evaluationId);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
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
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }

        public Boolean SaveAppraisalEvaluationCriteria(AppraisalEvalutionByBO AppraisalEvalutionBy, List<string> EmpLst, List<AppraisalEvalutionRatingFactorDetailsBO> AppraisalEvalutionCriteria, out int evaluationId)
        {
            int evaluatorId = Convert.ToInt32(AppraisalEvalutionBy.EvalutiorId);
            Boolean status = false;
            evaluationId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAppraisalEvaluationByInfo_SP"))
                        {
                            foreach (string emp in EmpLst)
                            {
                                status = false;
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@AppraisalConfigType", DbType.String, AppraisalEvalutionBy.AppraisalConfigType);
                                dbSmartAspects.AddInParameter(command, "@EvalutiorId", DbType.Int32, AppraisalEvalutionBy.EvalutiorId);
                                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, Convert.ToInt32(emp));
                                dbSmartAspects.AddInParameter(command, "@EvalutionCompletionBy", DbType.DateTime, AppraisalEvalutionBy.EvalutionCompletionBy);
                                dbSmartAspects.AddInParameter(command, "@EvalutionFromDate", DbType.DateTime, AppraisalEvalutionBy.EvaluationFromDate);
                                dbSmartAspects.AddInParameter(command, "@EvalutionToDate", DbType.DateTime, AppraisalEvalutionBy.EvaluationToDate);
                                dbSmartAspects.AddInParameter(command, "@AppraisalType", DbType.String, AppraisalEvalutionBy.AppraisalType);
                                dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, AppraisalEvalutionBy.ApprovalStatus);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, AppraisalEvalutionBy.CreatedBy);

                                dbSmartAspects.AddOutParameter(command, "@EvaluationId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                evaluationId = Convert.ToInt32(command.Parameters["@EvaluationId"].Value);

                                if (status)
                                {
                                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveAppraisalEvalutionRatingFactorDetails_SP"))
                                    {
                                        foreach (AppraisalEvalutionRatingFactorDetailsBO appraisalEvalutionRating in AppraisalEvalutionCriteria)
                                        {
                                            status = false;
                                            cmd.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, evaluationId);
                                            dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, appraisalEvalutionRating.MarksIndicatorId);
                                            dbSmartAspects.AddInParameter(cmd, "@AppraisalRatingFactorId", DbType.Int32, appraisalEvalutionRating.AppraisalRatingFactorId);

                                            dbSmartAspects.AddInParameter(cmd, "@AppraisalWeight", DbType.Decimal, appraisalEvalutionRating.AppraisalWeight);
                                            dbSmartAspects.AddInParameter(cmd, "@RatingWeight", DbType.Decimal, appraisalEvalutionRating.RatingWeight);

                                            dbSmartAspects.AddInParameter(cmd, "@RatingValue", DbType.Decimal, appraisalEvalutionRating.RatingValue);
                                            dbSmartAspects.AddInParameter(cmd, "@Marks", DbType.Decimal, appraisalEvalutionRating.Marks);

                                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                        }
                                    }
                                }
                            }
                        }

                        //if (status)
                        //{
                        //    status = false;

                        //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveAppraisalEvalutionRatingFactorDetails_SP"))
                        //    {
                        //        foreach (string emp in EmpLst)
                        //        {
                        //            foreach (AppraisalEvalutionRatingFactorDetailsBO appraisalEvalutionRating in AppraisalEvalutionCriteria)
                        //            {
                        //                status = false;
                        //                cmd.Parameters.Clear();

                        //                dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, evaluationId);

                        //                dbSmartAspects.AddInParameter(cmd, "@MarksIndicatorId", DbType.Int32, appraisalEvalutionRating.MarksIndicatorId);
                        //                dbSmartAspects.AddInParameter(cmd, "@AppraisalRatingFactorId", DbType.Int32, appraisalEvalutionRating.AppraisalRatingFactorId);

                        //                dbSmartAspects.AddInParameter(cmd, "@AppraisalWeight", DbType.Decimal, appraisalEvalutionRating.AppraisalWeight);
                        //                dbSmartAspects.AddInParameter(cmd, "@RatingWeight", DbType.Decimal, appraisalEvalutionRating.RatingWeight);

                        //                dbSmartAspects.AddInParameter(cmd, "@RatingValue", DbType.Decimal, appraisalEvalutionRating.RatingValue);
                        //                dbSmartAspects.AddInParameter(cmd, "@Marks", DbType.Decimal, appraisalEvalutionRating.Marks);

                        //                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                        //            }
                        //        }
                        //    }
                        //}

                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }

        public List<EmployeeForEvalutionBO> GetEmployeeByEvalutorId(int evalutiorId)
        {
            List<EmployeeForEvalutionBO> evaluationList = new List<EmployeeForEvalutionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByEvalutorId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EvalutiorId", DbType.Int32, evalutiorId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalEvalutionBy");
                    DataTable Table = ds.Tables["PayrollAppraisalEvalutionBy"];

                    evaluationList = Table.AsEnumerable().Select(r => new EmployeeForEvalutionBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmployeeName = r.Field<string>("EmpCode") + " - " + r.Field<String>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        AppraisalType = r.Field<string>("AppraisalType"),
                        EvaluationFromDate = r.Field<DateTime>("EvaluationFromDate"),
                        EvaluationToDate = r.Field<DateTime>("EvaluationToDate")

                    }).ToList();
                }
            }
            return evaluationList;
        }

        public EmployeeForEvalutionBO GetEmployeeByEvalutorNEmpId(int appraisalEvalutionById)
        {
            EmployeeForEvalutionBO evaluationList = new EmployeeForEvalutionBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByEvalutorNEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AppraisalEvalutionById", DbType.Int32, appraisalEvalutionById);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollAppraisalEvalutionBy");
                    DataTable Table = ds.Tables["PayrollAppraisalEvalutionBy"];

                    evaluationList = Table.AsEnumerable().Select(r => new EmployeeForEvalutionBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmployeeName = r.Field<String>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        AppraisalType = r.Field<string>("AppraisalType"),
                        EvaluationFromDate = r.Field<DateTime>("EvaluationFromDate"),
                        EvaluationToDate = r.Field<DateTime>("EvaluationToDate"),
                        EvalutionCompletionBy = r.Field<DateTime>("EvalutionCompletionBy")
                    }).FirstOrDefault();
                }
            }
            return evaluationList;
        }

        //Appraisal Evaluation Reports
        public List<AppraisalEvaluationReportViewBO> GetAppraisalEvaluationForReport(string departmentId, string appraisalType, string empId, string dateFrom, string dateTo)
        {
            List<AppraisalEvaluationReportViewBO> viewlist = new List<AppraisalEvaluationReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvaluationForReport_SP"))
                {

                    if (!string.IsNullOrEmpty(departmentId))
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, Convert.ToInt32(departmentId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(appraisalType))
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, appraisalType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(empId))
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, Convert.ToInt32(empId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(dateFrom))
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, Convert.ToDateTime(dateFrom));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(dateTo))
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, Convert.ToDateTime(dateTo));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AppraisalEvalutionReport");
                    DataTable Table = ds.Tables["AppraisalEvalutionReport"];

                    viewlist = Table.AsEnumerable().Select(r => new AppraisalEvaluationReportViewBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EmpId = r.Field<Int32>("EmpId"),
                        DisplayName = r.Field<String>("DisplayName"),
                        EmployeeType = r.Field<String>("EmployeeType"),
                        EvaluationFromDate = r.Field<DateTime>("EvaluationFromDate"),
                        EvaluationFromDateForReport = r.Field<String>("EvaluationFromDateForReport"),
                        EvaluationToDate = r.Field<DateTime>("EvaluationToDate"),
                        EvaluationToDateForReport = r.Field<String>("EvaluationToDateForReport"),
                        Designation = r.Field<String>("Designation"),
                        Department = r.Field<String>("Department"),
                        JoinDate = r.Field<DateTime>("JoinDate"),
                        Joblength = r.Field<int>("Joblength"),
                        AppraisalType = r.Field<String>("AppraisalType"),
                        Marks = r.Field<Decimal>("Marks")

                    }).ToList();
                }
            }
            return viewlist;

        }

        public List<AppraisalEvaluationDetailsReportViewBO> GetAppraisalEvaluationDetailsForReport(string departmentId, string appraisalType, string empId, DateTime? dateFrom, DateTime? dateTo)
        {
            List<AppraisalEvaluationDetailsReportViewBO> viewlist = new List<AppraisalEvaluationDetailsReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvaluationDetailsForReport_SP"))
                {
                    if (!string.IsNullOrEmpty(departmentId))
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, Convert.ToInt32(departmentId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(appraisalType))
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, appraisalType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(empId))
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, Convert.ToInt32(empId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, Convert.ToDateTime(dateFrom));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, Convert.ToDateTime(dateTo));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AppraisalEvalutionDetailsReport");
                    DataTable Table = ds.Tables["AppraisalEvalutionDetailsReport"];

                    viewlist = Table.AsEnumerable().Select(r => new AppraisalEvaluationDetailsReportViewBO
                    {
                        AppraisalEvalutionById = r.Field<int>("AppraisalEvalutionById"),
                        EmpId = r.Field<int>("EmpId"),
                        AppraisalType = r.Field<String>("AppraisalType"),
                        DisplayName = r.Field<String>("DisplayName"),
                        JoinDate = r.Field<DateTime>("JoinDate"),
                        Joblength = r.Field<int>("Joblength"),
                        Department = r.Field<String>("Department"),
                        Designation = r.Field<String>("Designation"),
                        EmployeeType = r.Field<String>("EmployeeType"),
                        Marks = r.Field<decimal>("Marks"),
                        AppraisalIndicatorName = r.Field<String>("AppraisalIndicatorName"),
                        RatingWeight = r.Field<decimal>("RatingWeight"),
                        RatingFactorName = r.Field<String>("RatingFactorName"),
                        RatingValue = r.Field<decimal>("RatingValue"),
                        IndividualMarks = r.Field<decimal>("IndividualMarks")

                    }).ToList();
                }
            }
            return viewlist;

        }

        // -------- Employee Nominee ------------------

        public Boolean SaveBestEmployeeNomination(BestEmployeeNominationBO BestEmployeeNomination, List<BestEmployeeNominationDetailsBO> BestEmployeeNominationDetails, out int bestEmpNomineeId)
        {
            try
            {
                bestEmpNomineeId = 0;
                Boolean status = false;

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBestEmployeeNomination_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, BestEmployeeNomination.DepartmentId);
                            dbSmartAspects.AddInParameter(command, "@Years", DbType.Int16, BestEmployeeNomination.Years);
                            dbSmartAspects.AddInParameter(command, "@Months", DbType.Byte, BestEmployeeNomination.Months);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, BestEmployeeNomination.ApprovedStatus);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, "Open");

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, BestEmployeeNomination.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@BestEmpNomineeId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            bestEmpNomineeId = Convert.ToInt32(command.Parameters["@BestEmpNomineeId"].Value);
                        }

                        if (status)
                        {
                            status = false;

                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveBestEmployeeNominationDetails_SP"))
                            {
                                foreach (BestEmployeeNominationDetailsBO bestEmp in BestEmployeeNominationDetails)
                                {
                                    status = false;
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@BestEmpNomineeId", DbType.Int64, bestEmpNomineeId);
                                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, bestEmp.EmpId);
                                    //dbSmartAspects.AddInParameter(cmd, "@IsSelectedAsBestEmployee", DbType.Boolean, bestEmp.IsSelectedAsMonthlyBestEmployee);

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
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }
        public Boolean UpdateBestEmployeeSelection(BestEmployeeNominationBO BestEmployeeNomination, List<BestEmployeeNominationDetailsBO> BestEmployeeNominationDetails, string processType)
        {
            try
            {
                Boolean status = false;

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateBestEmployeeSelection_SP"))
                        {
                            foreach (BestEmployeeNominationDetailsBO bestEmp in BestEmployeeNominationDetails)
                            {
                                status = false;
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@BestEmpNomineeId", DbType.Int64, bestEmp.BestEmpNomineeId);
                                dbSmartAspects.AddInParameter(cmd, "@BestEmpNomineeDetailsId", DbType.Int64, bestEmp.BestEmpNomineeDetailsId);
                                //dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, bestEmp.EmpId);
                                //dbSmartAspects.AddInParameter(cmd, "@IsSelectedAsMonthlyBestEmployee", DbType.Boolean, bestEmp.IsSelectedAsMonthlyBestEmployee);
                                //dbSmartAspects.AddInParameter(cmd, "@IsSelectedAsYearlyBestEmployee", DbType.Boolean, bestEmp.IsSelectedAsYearlyBestEmployee);
                                dbSmartAspects.AddInParameter(cmd, "@ProcessType", DbType.String, processType);

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
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }
        public bool UpdateMonthlyBestEmpForYearlyBestEmp(List<BestEmployeeNominationDetailsBO> BestEmployeeNominationDetails)
        {
            try
            {
                Boolean status = false;

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateMonthlyBestEmployeeForYearlySelection_SP"))
                        {
                            foreach (BestEmployeeNominationDetailsBO bestEmp in BestEmployeeNominationDetails)
                            {
                                status = false;
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@BestEmpNomineeId", DbType.Int64, bestEmp.BestEmpNomineeId);
                                dbSmartAspects.AddInParameter(cmd, "@BestEmpNomineeDetailsId", DbType.Int64, bestEmp.BestEmpNomineeDetailsId);
                                //dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, bestEmp.EmpId);
                                //dbSmartAspects.AddInParameter(cmd, "@IsSelectedAsMonthlyBestEmployee", DbType.Boolean, bestEmp.IsSelectedAsMonthlyBestEmployee);
                                //dbSmartAspects.AddInParameter(cmd, "@IsSelectedAsYearlyBestEmployee", DbType.Boolean, bestEmp.IsSelectedAsYearlyBestEmployee);
                                //dbSmartAspects.AddInParameter(cmd, "@ProcessType", DbType.String, processType);

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
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }

        public List<BestEmployeeNominationViewBO> GetMonthlyBestEmployeeNomination(int departmentId, short years, byte months, string approvedStatus, string status)
        {
            List<BestEmployeeNominationViewBO> apprEvaBO = new List<BestEmployeeNominationViewBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBestEmployeeNomination_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                        dbSmartAspects.AddInParameter(cmd, "@Years", DbType.Int16, years);
                        dbSmartAspects.AddInParameter(cmd, "@Months", DbType.Byte, months);
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approvedStatus);
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "BestEmployeeNominationViewBO");
                        DataTable Table = ds.Tables["BestEmployeeNominationViewBO"];

                        apprEvaBO = Table.AsEnumerable().Select(r => new BestEmployeeNominationViewBO
                        {
                            BestEmpNomineeId = r.Field<Int64>("BestEmpNomineeId"),
                            BestEmpNomineeDetailsId = r.Field<Int64>("BestEmpNomineeDetailsId"),
                            DepartmentId = r.Field<Int32>("DepartmentId"),
                            EmpId = r.Field<Int32>("EmpId"),
                            Years = r.Field<Int16>("Years"),
                            Months = r.Field<Byte>("Months"),
                            ApprovedStatus = r.Field<String>("ApprovedStatus"),
                            IsSelectedAsMonthlyBestEmployee = r.Field<Boolean?>("IsSelectedAsMonthlyBestEmployee"),
                            EmpCode = r.Field<String>("EmpCode"),
                            EmployeeName = r.Field<String>("EmployeeName"),
                            Designation = r.Field<String>("Designation")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return apprEvaBO;
        }
        public List<BestEmployeeNominationViewBO> GetYearlyBestEmployeeNomination(int departmentId, short years, string approvedStatus, string status)
        {
            List<BestEmployeeNominationViewBO> apprEvaBO = new List<BestEmployeeNominationViewBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetYearlyBestEmployeeNomination_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                        dbSmartAspects.AddInParameter(cmd, "@Years", DbType.Int16, years);
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approvedStatus);
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "BestEmployeeNominationViewBO");
                        DataTable Table = ds.Tables["BestEmployeeNominationViewBO"];

                        apprEvaBO = Table.AsEnumerable().Select(r => new BestEmployeeNominationViewBO
                        {
                            BestEmpNomineeId = r.Field<Int64>("BestEmpNomineeId"),
                            BestEmpNomineeDetailsId = r.Field<Int64>("BestEmpNomineeDetailsId"),
                            DepartmentId = r.Field<Int32>("DepartmentId"),
                            EmpId = r.Field<Int32>("EmpId"),
                            Years = r.Field<Int16>("Years"),
                            Months = r.Field<Byte>("Months"),
                            ApprovedStatus = r.Field<String>("ApprovedStatus"),
                            IsSelectedAsMonthlyBestEmployee = r.Field<Boolean?>("IsSelectedAsMonthlyBestEmployee"),
                            EmpCode = r.Field<String>("EmpCode"),
                            EmployeeName = r.Field<String>("EmployeeName"),
                            Designation = r.Field<String>("Designation")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return apprEvaBO;
        }
        public List<BestEmployeeNominationViewBO> GetMonthlySelectedBestEmployee(int departmentId, short years)
        {
            List<BestEmployeeNominationViewBO> apprEvaBO = new List<BestEmployeeNominationViewBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthlySelectedBestEmployee_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                        dbSmartAspects.AddInParameter(cmd, "@Years", DbType.Int16, years);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "BestEmployeeNominationViewBO");
                        DataTable Table = ds.Tables["BestEmployeeNominationViewBO"];

                        apprEvaBO = Table.AsEnumerable().Select(r => new BestEmployeeNominationViewBO
                        {
                            BestEmpNomineeId = r.Field<Int64>("BestEmpNomineeId"),
                            BestEmpNomineeDetailsId = r.Field<Int64>("BestEmpNomineeDetailsId"),
                            DepartmentId = r.Field<Int32>("DepartmentId"),
                            EmpId = r.Field<Int32>("EmpId"),
                            Years = r.Field<Int16>("Years"),
                            Months = r.Field<Byte>("Months"),
                            ApprovedStatus = r.Field<String>("ApprovedStatus"),
                            IsSelectedAsMonthlyBestEmployee = r.Field<Boolean?>("IsSelectedAsMonthlyBestEmployee"),
                            EmpCode = r.Field<String>("EmpCode"),
                            EmployeeName = r.Field<String>("EmployeeName"),
                            Designation = r.Field<String>("Designation")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return apprEvaBO;
        }
        public List<BestEmployeeNominationViewBO> GetEmployeeOfTheYearMonthNomination(int departmentId, short years, byte months, string processType)
        {
            List<BestEmployeeNominationViewBO> apprEvaBO = new List<BestEmployeeNominationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeOfTheYearMonthNomination_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    dbSmartAspects.AddInParameter(cmd, "@Years", DbType.Int16, years);
                    dbSmartAspects.AddInParameter(cmd, "@Months", DbType.Byte, months);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessType", DbType.String, processType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BestEmployeeNominationViewBO");
                    DataTable Table = ds.Tables["BestEmployeeNominationViewBO"];

                    apprEvaBO = Table.AsEnumerable().Select(r => new BestEmployeeNominationViewBO
                    {
                        BestEmpNomineeId = r.Field<Int64>("BestEmpNomineeId"),
                        BestEmpNomineeDetailsId = r.Field<Int64>("BestEmpNomineeDetailsId"),
                        DepartmentId = r.Field<Int32>("DepartmentId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        Years = r.Field<Int16>("Years"),
                        Months = r.Field<Byte>("Months"),
                        ApprovedStatus = r.Field<String>("ApprovedStatus"),
                        Status = r.Field<String>("Status"),
                        IsSelectedAsMonthlyBestEmployee = r.Field<Boolean?>("IsSelectedAsMonthlyBestEmployee"),
                        EmpCode = r.Field<String>("EmpCode"),
                        EmployeeName = r.Field<String>("EmployeeName"),
                        Designation = r.Field<String>("Designation")

                    }).ToList();
                }
            }
            return apprEvaBO;
        }
        public List<BestEmployeeNominationViewBO> GetEmployeeOfTheYearMonth(int departmentId, short? years, byte? months, string processType)
        {
            List<BestEmployeeNominationViewBO> apprEvaBO = new List<BestEmployeeNominationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeOfTheYearMonth_SP"))
                {
                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (years != null)
                        dbSmartAspects.AddInParameter(cmd, "@Years", DbType.Int16, years);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Years", DbType.Int16, DBNull.Value);

                    if (months != null)
                        dbSmartAspects.AddInParameter(cmd, "@Months", DbType.Byte, months);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Months", DbType.Byte, DBNull.Value);

                    if (!string.IsNullOrEmpty(processType))
                        dbSmartAspects.AddInParameter(cmd, "@ProcessType", DbType.String, processType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProcessType", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BestEmployeeNominationViewBO");
                    DataTable Table = ds.Tables["BestEmployeeNominationViewBO"];

                    apprEvaBO = Table.AsEnumerable().Select(r => new BestEmployeeNominationViewBO
                    {
                        BestEmpNomineeId = r.Field<Int64>("BestEmpNomineeId"),
                        BestEmpNomineeDetailsId = r.Field<Int64>("BestEmpNomineeDetailsId"),
                        DepartmentId = r.Field<Int32>("DepartmentId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        Years = r.Field<Int16>("Years"),
                        Months = r.Field<Byte>("Months"),
                        ApprovedStatus = r.Field<String>("ApprovedStatus"),
                        Status = r.Field<String>("Status"),
                        IsSelectedAsMonthlyBestEmployee = r.Field<Boolean?>("IsSelectedAsMonthlyBestEmployee"),
                        IsSelectedAsYearlyBestEmployee = r.Field<Boolean?>("IsSelectedAsYearlyBestEmployee"),
                        EmpCode = r.Field<String>("EmpCode"),
                        EmployeeName = r.Field<String>("EmployeeName"),
                        Designation = r.Field<String>("Designation"),
                        DepartmentName = r.Field<String>("DepartmentName")

                    }).ToList();
                }
            }
            return apprEvaBO;
        }

        public List<BestEmployeeNominationViewBO> GetEmployeeOfTheYearByEmpId(int empId)
        {
            List<BestEmployeeNominationViewBO> apprEvaBO = new List<BestEmployeeNominationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeOfTheYearByEmpId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@empId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BestEmployeeNominationViewBO");
                    DataTable Table = ds.Tables["BestEmployeeNominationViewBO"];

                    apprEvaBO = Table.AsEnumerable().Select(r => new BestEmployeeNominationViewBO
                    {
                        BestEmpNomineeId = r.Field<Int64>("BestEmpNomineeId"),
                        BestEmpNomineeDetailsId = r.Field<Int64>("BestEmpNomineeDetailsId"),
                        DepartmentId = r.Field<Int32>("DepartmentId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        Years = r.Field<Int16>("Years"),
                        Months = r.Field<Byte>("Months"),
                        ApprovedStatus = r.Field<String>("ApprovedStatus"),
                        Status = r.Field<String>("Status"),
                        IsSelectedAsMonthlyBestEmployee = r.Field<Boolean?>("IsSelectedAsMonthlyBestEmployee"),
                        IsSelectedAsYearlyBestEmployee = r.Field<Boolean?>("IsSelectedAsYearlyBestEmployee")

                    }).ToList();
                }
            }
            return apprEvaBO;
        }

        public List<AppraisalEvaluationReportViewBO> GetAppraisalEvaluationForReport(string departmentId, string appraisalType, string empId, DateTime? dateFrom, DateTime? dateTo)
        {
            List<AppraisalEvaluationReportViewBO> viewlist = new List<AppraisalEvaluationReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAppraisalEvaluationForReport_SP"))
                {

                    if (!string.IsNullOrEmpty(departmentId))
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, Convert.ToInt32(departmentId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(appraisalType))
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, appraisalType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AppraisalType", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(empId))
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, Convert.ToInt32(empId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, Convert.ToDateTime(dateFrom));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);
                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, Convert.ToDateTime(dateTo));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AppraisalEvalutionReport");
                    DataTable Table = ds.Tables["AppraisalEvalutionReport"];

                    viewlist = Table.AsEnumerable().Select(r => new AppraisalEvaluationReportViewBO
                    {
                        AppraisalEvalutionById = r.Field<Int32>("AppraisalEvalutionById"),
                        EmpId = r.Field<Int32>("EmpId"),
                        DisplayName = r.Field<String>("DisplayName"),
                        EmployeeType = r.Field<String>("EmployeeType"),
                        EvaluationFromDate = r.Field<DateTime>("EvaluationFromDate"),
                        EvaluationFromDateForReport = r.Field<String>("EvaluationFromDateForReport"),
                        EvaluationToDate = r.Field<DateTime>("EvaluationToDate"),
                        EvaluationToDateForReport = r.Field<String>("EvaluationToDateForReport"),
                        Designation = r.Field<String>("Designation"),
                        Department = r.Field<String>("Department"),
                        JoinDate = r.Field<DateTime>("JoinDate"),
                        Joblength = r.Field<int>("Joblength"),
                        AppraisalType = r.Field<String>("AppraisalType"),
                        AllocatedMarks = r.Field<Decimal>("AllocatedMarks"),
                        GainMarksRatingValue = r.Field<Decimal>("GainMarksRatingValue"),
                        Marks = r.Field<Decimal>("Marks"),
                        EvaloatorName = r.Field<String>("EvaloatorName")

                    }).ToList();
                }
            }
            return viewlist;

        }

    }
}
