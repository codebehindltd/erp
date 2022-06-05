using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.Payroll
{
    public class EmpDependentDA : BaseService
    {
        public List<EmpDependentBO> GetEmpDependentByEmpId(int empId)
        {
            List<EmpDependentBO> boList = new List<EmpDependentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDependentByEmpId_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpDependentBO bo = new EmpDependentBO();

                                bo.DependentId = Convert.ToInt32(reader["DependentId"]);
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.DependentName = reader["DependentName"].ToString();
                                bo.BloodGroup = reader["BloodGroup"].ToString();
                                bo.BloodGroupId = Convert.ToInt32(reader["BloodGroupId"]);
                                bo.Relationship = reader["Relationship"].ToString();
                                bo.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                                bo.Age = UtilityDA.AgeCalculation(Convert.ToDateTime(bo.DateOfBirth)); //reader["Age"].ToString();
                                bo.ShowDateOfBirth = reader["ShowDateOfBirth"].ToString();
                                bo.BloodGroup = reader["BloodGroup"].ToString();
                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
    }
}
