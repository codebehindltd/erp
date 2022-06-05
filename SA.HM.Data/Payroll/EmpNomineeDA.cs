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
    public class EmpNomineeDA : BaseService
    {
        public List<EmpNomineeBO> GetEmpNomineeByEmpId(int empId)
        {
            List<EmpNomineeBO> boList = new List<EmpNomineeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpNomineeByEmpId_SP"))
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
                                EmpNomineeBO bo = new EmpNomineeBO();

                                bo.NomineeId = Convert.ToInt32(reader["NomineeId"]);
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.NomineeName = reader["NomineeName"].ToString();
                                bo.Relationship = reader["Relationship"].ToString();
                                bo.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                                bo.Age = UtilityDA.AgeCalculation(Convert.ToDateTime(bo.DateOfBirth));   //reader["Age"].ToString();
                                bo.Percentage = Convert.ToDecimal(reader["Percentage"].ToString());
                                bo.ShowDateOfBirth = reader["ShowDateOfBirth"].ToString();

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
