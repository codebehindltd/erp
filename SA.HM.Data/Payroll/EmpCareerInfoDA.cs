using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpCareerInfoDA:BaseService
    {
        public EmpCareerInfoBO GetEmpCareerInfoByEmpId(int empId)
        {
            EmpCareerInfoBO careerInfo = new EmpCareerInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpCareerInfoByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "CareerInfo");
                    DataTable Table = incrementDS.Tables["CareerInfo"];

                    careerInfo = Table.AsEnumerable().Select(r => new EmpCareerInfoBO
                    {
                        CareerInfoId = r.Field<int>("CareerInfoId"),
                        EmpId = r.Field<int?>("EmpId"),
                        Objective = r.Field<string>("Objective"),
                        PresentSalary = r.Field<decimal?>("PresentSalary"),
                        ExpectedSalary = r.Field<decimal?>("ExpectedSalary"),
                        Currency = r.Field<int>("Currency"),
                        JobLevel = r.Field<string>("JobLevel"),
                        AvailableType = r.Field<string>("AvailableType"),
                        PreferedJobType = r.Field<int?>("PreferedJobType"),
                        PreferedOrganizationType = r.Field<int?>("PreferedOrganizationType"),
                        CareerSummary = r.Field<string>("CareerSummary"),
                        PreferedJobLocationId = r.Field<int?>("PreferedJobLocationId"),                        
                        ExtraCurriculmActivities = r.Field<string>("ExtraCurriculmActivities")
                    }).FirstOrDefault();
                }
            }
            return careerInfo;
        }

        public List<EmpCareerInfoBO> GetEmpCareerInfoForReport(int empId)
        {
            List<EmpCareerInfoBO> careerInfoList = new List<EmpCareerInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpCareerInfoByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "CareerInfo");
                    DataTable Table = incrementDS.Tables["CareerInfo"];

                    careerInfoList = Table.AsEnumerable().Select(r => new EmpCareerInfoBO
                    {
                        CareerInfoId = r.Field<int>("CareerInfoId"),
                        EmpId = r.Field<int?>("EmpId"),
                        Objective = r.Field<string>("Objective"),
                        PresentSalary = r.Field<decimal?>("PresentSalary"),
                        ExpectedSalary = r.Field<decimal?>("ExpectedSalary"),
                        Currency = r.Field<int>("Currency"),
                        JobLevel = r.Field<string>("JobLevel"),
                        AvailableType = r.Field<string>("AvailableType"),
                        PreferedJobType = r.Field<int?>("PreferedJobType"),
                        PreferedOrganizationType = r.Field<int?>("PreferedOrganizationType"),
                        CareerSummary = r.Field<string>("CareerSummary"),
                        PreferedJobLocationId = r.Field<int?>("PreferedJobLocationId"),                        
                        ExtraCurriculmActivities = r.Field<string>("ExtraCurriculmActivities"),
                        PreferedJobLocation = r.Field<string>("PreferedJobLocation"),
                        PreferedJobCategoryText = r.Field<string>("PreferedJobCategoryText"),
                        PreferedOrganizationText = r.Field<string>("PreferedOrganizationText")
                    }).ToList();
                }
            }
            return careerInfoList;
        }
    }
}
