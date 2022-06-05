using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpLanguageDA : BaseService
    {
        public List<EmpLanguageBO> GetEmpLanguageByEmpId(int empId)
        {
            List<EmpLanguageBO> boList = new List<EmpLanguageBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLanguageByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "LanguageInfo");
                    DataTable Table = incrementDS.Tables["LanguageInfo"];

                    boList = Table.AsEnumerable().Select(r => new EmpLanguageBO
                    {
                        LanguageId = r.Field<int>("LanguageId"),
                        EmpId = r.Field<int>("EmpId"),
                        Language = r.Field<string>("Language"),
                        Reading = r.Field<string>("Reading"),
                        Writing = r.Field<string>("Writing"),
                        Speaking = r.Field<string>("Speaking"),
                        ReadingLevel = r.Field<string>("ReadingLevel"),
                        WritingLevel = r.Field<string>("WritingLevel"),
                        SpeakingLevel = r.Field<string>("SpeakingLevel") 
                    }).ToList();
                }
            }
            return boList;
        }
    }
}
