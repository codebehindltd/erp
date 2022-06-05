using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpCareerTrainingDA : BaseService
    {
        public List<EmpCareerTrainingBO> GetEmpCareerTrainingnByEmpId(int empId)
        {
            List<EmpCareerTrainingBO> boList = new List<EmpCareerTrainingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpCareerTrainingByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "CareerTraining");
                    DataTable Table = incrementDS.Tables["CareerTraining"];

                    boList = Table.AsEnumerable().Select(r => new EmpCareerTrainingBO
                    {
                        CareerTrainingId = r.Field<int>("CareerTrainingId"),
                        EmpId = r.Field<int?>("EmpId"),
                        TrainingTitle = r.Field<string>("TrainingTitle"),
                        Topic = r.Field<string>("Topic"),
                        Institute = r.Field<string>("Institute"),
                        Country = r.Field<int>("Country"),
                        Location = r.Field<string>("Location"),
                        TrainingYear = r.Field<string>("TrainingYear"),
                        Duration = r.Field<int?>("Duration"),
                        DurationType = r.Field<string>("DurationType")
                    }).ToList();                    
                }
            }
            return boList;
        }
        public List<EmpCareerTrainingBO> GetEmpCareerTrainingByEmpIdForResume(int empId)
        {
            List<EmpCareerTrainingBO> boList = new List<EmpCareerTrainingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpCareerTrainingByEmpIdForResume_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "CareerTraining");
                    DataTable Table = incrementDS.Tables["CareerTraining"];

                    boList = Table.AsEnumerable().Select(r => new EmpCareerTrainingBO
                    {
                        CareerTrainingId = r.Field<int>("CareerTrainingId"),
                        EmpId = r.Field<int?>("EmpId"),
                        TrainingTitle = r.Field<string>("TrainingTitle"),
                        Topic = r.Field<string>("Topic"),
                        Institute = r.Field<string>("Institute"),
                        Country = r.Field<int>("Country"),
                        Location = r.Field<string>("Location"),
                        TrainingYear = r.Field<string>("TrainingYear"),
                        Duration = r.Field<int?>("Duration"),
                        DurationType = r.Field<string>("DurationType"),
                        StartDate = r.Field<string>("StartDate")
                    }).ToList();
                }
            }
            return boList;
        }
    }
}
