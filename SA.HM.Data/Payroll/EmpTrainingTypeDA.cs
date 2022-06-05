using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpTrainingTypeDA : BaseService
    {
        public Boolean SaveEmpTrainingTypeInfo(PayrollEmpTrainingTypeBO trainingTypeInfo, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTrainingType_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TrainingName", DbType.String, trainingTypeInfo.TrainingName);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, trainingTypeInfo.Remarks);                    
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, trainingTypeInfo.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TrainingTypeId", DbType.Int32, trainingTypeInfo.TrainingTypeId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpId = Convert.ToInt32(command.Parameters["@TrainingTypeId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateEmpTrainingTypeInfo(PayrollEmpTrainingTypeBO trainingTypeInfo)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTrainingType_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TrainingTypeId", DbType.Int32, trainingTypeInfo.TrainingTypeId);
                    dbSmartAspects.AddInParameter(command, "@TrainingName", DbType.String, trainingTypeInfo.TrainingName);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, trainingTypeInfo.Remarks);

                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, trainingTypeInfo.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public PayrollEmpTrainingTypeBO GetEmpTrainingTypeById(int trainingTypeId)
        {
            PayrollEmpTrainingTypeBO trainingTypeBO = new PayrollEmpTrainingTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrainingTypeById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TrainingTypeId", DbType.Int32, trainingTypeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTrainingType");
                    DataTable Table = ds.Tables["EmpTrainingType"];

                    trainingTypeBO = Table.AsEnumerable().Select(r => new PayrollEmpTrainingTypeBO
                    {                        
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Remarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }
            return trainingTypeBO;
        }
        public List<PayrollEmpTrainingTypeBO> GetEmpTrainingTypeInfoBySearchCriteriaForPagination(string trainingName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PayrollEmpTrainingTypeBO> trainingTypeList = new List<PayrollEmpTrainingTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTrainingTypeWithPagination_SP"))
                {                    
                    if (!string.IsNullOrEmpty(trainingName))
                        dbSmartAspects.AddInParameter(cmd, "@TrainingName", DbType.String, trainingName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TrainingName", DbType.String, DBNull.Value);                                                           

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTrainingType");
                    DataTable Table = ds.Tables["EmpTrainingType"];

                    trainingTypeList = Table.AsEnumerable().Select(r => new PayrollEmpTrainingTypeBO
                    {                        
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),                        
                        Remarks = r.Field<string>("Remarks")                        
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return trainingTypeList;
        }
        public Boolean DeleteEmpTrainingTypeInfo(int trainingTypeId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTrainingTypeById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TrainingTypeId", DbType.Int32, trainingTypeId);                    

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
