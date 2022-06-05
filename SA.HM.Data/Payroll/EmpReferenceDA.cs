using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpReferenceDA : BaseService
    {
        public List<EmpReferenceBO> GetEmpReferenceByEmpId(int empId)
        {
            List<EmpReferenceBO> boList = new List<EmpReferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpReferenceByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Reference");
                    DataTable Table = incrementDS.Tables["Reference"];

                    boList = Table.AsEnumerable().Select(r => new EmpReferenceBO
                    {
                        ReferenceId = r.Field<int>("ReferenceId"),
                        EmpId = r.Field<int?>("EmpId"),
                        Name = r.Field<string>("Name"),
                        Organization = r.Field<string>("Organization"),
                        Designation = r.Field<string>("Designation"),
                        Address = r.Field<string>("Address"),
                        Email = r.Field<string>("Email"),
                        Mobile = r.Field<string>("Mobile"),
                        Relation = r.Field<string>("Relation"),
                        Relationship = r.Field<string>("Relation"),
                        Description = r.Field<string>("Description"),
                        RowRank = r.Field<Int64>("RowRank")
                    }).ToList();
                }
            }
            return boList;
        }
    }
}
