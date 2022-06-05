using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class ApplicantResultDA : BaseService
    {
        public Boolean SaveApplicantResultInfo(List<PayrollApplicantResultBO> boList, int createdBy)
        {
            Boolean status = false;
            int count = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveApplicantResultInfo_SP"))
                    {
                        foreach (PayrollApplicantResultBO bo in boList)
                        {
                            command.Parameters.Clear();
                            dbSmartAspects.AddInParameter(command, "@JobCircularId", DbType.Int64, bo.JobCircularId);
                            dbSmartAspects.AddInParameter(command, "@ApplicantId", DbType.Int64, bo.ApplicantId);
                            dbSmartAspects.AddInParameter(command, "@InterviewTypeId", DbType.Int16, bo.InterviewTypeId);
                            dbSmartAspects.AddInParameter(command, "@MarksObtain", DbType.Decimal, bo.MarksObtain);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);                            

                            count += dbSmartAspects.ExecuteNonQuery(command);                            
                        }


                        if (count == boList.Count)
                        {
                            status = true;
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
        public Boolean UpdateApplicantResultInfo(List<PayrollApplicantResultBO> addtList, List<PayrollApplicantResultBO> editList, List<PayrollApplicantResultBO> deleteList, int modifiedBy)
        {
            Boolean status = false;
            int count = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateApplicantResultInfo_SP"))
                        {

                            foreach (PayrollApplicantResultBO bo in editList)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@ApplicantResultId", DbType.Int64, bo.ApplicantResultId);
                                dbSmartAspects.AddInParameter(command, "@JobCircularId", DbType.Int64, bo.JobCircularId);
                                dbSmartAspects.AddInParameter(command, "@ApplicantId", DbType.Int64, bo.ApplicantId);
                                dbSmartAspects.AddInParameter(command, "@InterviewTypeId", DbType.Int16, bo.InterviewTypeId);
                                dbSmartAspects.AddInParameter(command, "@MarksObtain", DbType.Decimal, bo.MarksObtain);
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, modifiedBy);

                                count += dbSmartAspects.ExecuteNonQuery(command);
                            }


                            if (count == editList.Count)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            if (addtList != null)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveApplicantResultInfo_SP"))
                                {
                                    foreach (PayrollApplicantResultBO bo in addtList)
                                    {
                                        command.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(command, "@JobCircularId", DbType.Int64, bo.JobCircularId);
                                        dbSmartAspects.AddInParameter(command, "@ApplicantId", DbType.Int64, bo.ApplicantId);
                                        dbSmartAspects.AddInParameter(command, "@InterviewTypeId", DbType.Int16, bo.InterviewTypeId);
                                        dbSmartAspects.AddInParameter(command, "@MarksObtain", DbType.Decimal, bo.MarksObtain);
                                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, modifiedBy);

                                        count += dbSmartAspects.ExecuteNonQuery(command);
                                    }


                                    if (count == addtList.Count)
                                    {
                                        status = true;
                                    }
                                }
                            }
                            if (deleteList != null)
                            {
                                foreach (PayrollApplicantResultBO bo in deleteList)
                                {
                                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PayrollApplicantResult");
                                        dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "ApplicantResultId");
                                        dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, bo.ApplicantResultId);

                                        count = dbSmartAspects.ExecuteNonQuery(command);
                                    }
                                    if (count == deleteList.Count)
                                    {
                                        status = true;
                                    }
                                }
                            }
                        }
                        if (status)
                        {
                            transction.Commit();
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
        public List<PayrollApplicantResultBO> GetInterviewResultBySearchCriteriaForPaging(long jobCircularId, int recordPerPage, int pageIndex, out int totalRecords, DateTime? FormDate, DateTime? ToDate, String jobTitle)
        {
            List<PayrollApplicantResultBO> boList = new List<PayrollApplicantResultBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInterviewResultBySearchCriteriaForPaging_SP"))
                {
                    if (jobCircularId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    if (FormDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@formDate", DbType.DateTime, FormDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@formDate", DbType.Int32, DBNull.Value);

                    if (ToDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@toDate", DbType.DateTime, ToDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@toDate", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@jobTitle", DbType.String, jobTitle);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InterviewResult");
                    DataTable Table = ds.Tables["InterviewResult"];

                    boList = Table.AsEnumerable().Select(r => new PayrollApplicantResultBO
                    {
                        ApplicantId = r.Field<Int64>("ApplicantId"),
                        JobCircularId = r.Field<Int64>("JobCircularId"),
                        JobTitle = r.Field<string>("JobTitle"),
                        ApplicantName = r.Field<string>("DisplayName")

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return boList;
        }

        public PayrollApplicantResultBO GetInterviewResultByApplicantId(long applicantId, long jobCircularId)
        {
            PayrollApplicantResultBO bo = new PayrollApplicantResultBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInterviewResultByApplicantId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ApplicantId", DbType.Int64, applicantId);
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InterviewResult");
                    DataTable Table = ds.Tables["InterviewResult"];

                    bo = Table.AsEnumerable().Select(r => new PayrollApplicantResultBO
                    {
                        ApplicantResultId = r.Field<Int64>("ApplicantResultId"),
                        JobCircularId = r.Field<Int64>("JobCircularId"),
                        ApplicantId = r.Field<Int64>("ApplicantId"),
                        InterviewTypeId = r.Field<Int16>("InterviewTypeId"),
                        MarksObtain = r.Field<decimal>("MarksObtain"),
                        Remarks = r.Field<string>("Remarks"),
                        InterviewName = r.Field<string>("InterviewName"),
                        TotalMarks = r.Field<decimal>("TotalMarks")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }

        public List<PayrollApplicantResultBO> GetInterviewListByApplicantId(long applicantId, long jobCircularId)
        {
            List<PayrollApplicantResultBO> boList = new List<PayrollApplicantResultBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInterviewResultByApplicantId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ApplicantId", DbType.Int64, applicantId);
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InterviewResult");
                    DataTable Table = ds.Tables["InterviewResult"];

                    boList = Table.AsEnumerable().Select(r => new PayrollApplicantResultBO
                    {
                        ApplicantResultId = r.Field<Int64>("ApplicantResultId"),
                        JobCircularId = r.Field<Int64>("JobCircularId"),
                        ApplicantId = r.Field<Int64>("ApplicantId"),
                        InterviewTypeId = r.Field<Int16>("InterviewTypeId"),
                        MarksObtain = r.Field<decimal>("MarksObtain"),
                        Remarks = r.Field<string>("Remarks"),
                        InterviewName = r.Field<string>("InterviewName"),
                        TotalMarks = r.Field<decimal>("TotalMarks")

                    }).ToList();
                }
            }
            return boList;
        }
    }
}
