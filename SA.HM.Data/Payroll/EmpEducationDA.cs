using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpEducationDA:BaseService
    {
        public List<EmpEducationBO> GetEmpEducationByEmpId(int empId)
        {
            List<EmpEducationBO> boList = new List<EmpEducationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpEducationByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpEducationBO bo = new EmpEducationBO();

                                bo.EducationId = Convert.ToInt32(reader["EducationId"]);
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.LevelId = Convert.ToInt32(reader["LevelId"]);
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.ExamName = reader["ExamName"].ToString();
                                bo.InstituteName = reader["InstituteName"].ToString();
                                bo.PassYear = reader["PassYear"].ToString();
                                bo.SubjectName = reader["SubjectName"].ToString();
                                bo.PassClass = reader["PassClass"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }

        public List<EmpEducationBO> GetEmpEducationEmpId(int empId)
        {
            List<EmpEducationBO> boList = new List<EmpEducationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeEducationForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpEducationBO bo = new EmpEducationBO();

                                bo.EducationId = Convert.ToInt32(reader["EducationId"]);
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.ExamName = reader["ExamName"].ToString();
                                bo.InstituteName = reader["InstituteName"].ToString();
                                bo.PassYear = reader["PassYear"].ToString();
                                bo.SubjectName = reader["SubjectName"].ToString();
                                bo.PassClass = reader["PassClass"].ToString();

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
