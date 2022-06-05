using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class InterviewTypeDA: BaseService
    {
        public Boolean SaveInterviewTypeInfo(InterviewTypeBO bo, out int tmpInterviewTypeId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInterviewTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@InterviewName", DbType.String, bo.InterviewName);
                        dbSmartAspects.AddInParameter(command, "@Marks", DbType.Decimal, bo.Marks);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@InterviewTypeId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpInterviewTypeId = Convert.ToInt32(command.Parameters["@InterviewTypeId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateInterviewTypeInfo(InterviewTypeBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInterviewTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@InterviewTypeId", DbType.Int16, bo.InterviewTypeId);
                        dbSmartAspects.AddInParameter(command, "@InterviewName", DbType.String, bo.InterviewName);
                        dbSmartAspects.AddInParameter(command, "@Marks", DbType.Decimal, bo.Marks);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bo.LastModifiedBy);

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
        public List<InterviewTypeBO> GetInterviewTypeList()
        {
            List<InterviewTypeBO> boList = new List<InterviewTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInterviewTypeList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InterviewType");
                    DataTable Table = ds.Tables["InterviewType"];

                    boList = Table.AsEnumerable().Select(r => new InterviewTypeBO
                    {
                        InterviewTypeId = r.Field<Int16>("InterviewTypeId"),
                        InterviewName = r.Field<string>("InterviewName"),
                        Marks = r.Field<decimal>("Marks"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return boList;
        }
        public InterviewTypeBO GetDisciplinaryActionTypeById(int pkId)
        {
            InterviewTypeBO bo = new InterviewTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInterviewTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@InterviewTypeId", DbType.Int16, pkId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InterviewType");
                    DataTable Table = ds.Tables["InterviewType"];

                    bo = Table.AsEnumerable().Select(r => new InterviewTypeBO
                    {
                        InterviewTypeId = r.Field<Int16>("InterviewTypeId"),
                        InterviewName = r.Field<string>("InterviewName"),
                        Marks = r.Field<decimal>("Marks"),
                        Remarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }
    }
}
