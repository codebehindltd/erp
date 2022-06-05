using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Data.Payroll
{
    public class EmpExperienceDA : BaseService
    {
        public List<EmpExperienceBO> GetEmpExperienceByEmpId(int empId)
        {
            List<EmpExperienceBO> boList = new List<EmpExperienceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpExperienceByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "EmpExperience");
                    DataTable Table = incrementDS.Tables["EmpExperience"];

                    boList = Table.AsEnumerable().Select(r => new EmpExperienceBO
                    {
                        ExperienceId = r.Field<int>("ExperienceId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyUrl = r.Field<string>("CompanyUrl"),
                        JoinDate = r.Field<DateTime>("JoinDate"),
                        JoinDesignation = r.Field<string>("JoinDesignation"),
                        LeaveDate = r.Field<DateTime?>("LeaveDate"),
                        LeaveDesignation = r.Field<string>("LeaveDesignation"),
                        Achievements = r.Field<string>("Achievements"),
                        ShowJoinDate = r.Field<string>("ShowJoinDate"),
                        ShowLeaveDate = r.Field<string>("ShowLeaveDate")
                    }).ToList();                      
                }
            }
            return boList;
        }
        public List<EmpExperienceBO> GetEmpExperienceByEmpIdForResume(int empId)
        {
            List<EmpExperienceBO> boList = new List<EmpExperienceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpExperienceByEmpIdForResume_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "EmpExperience");
                    DataTable Table = incrementDS.Tables["EmpExperience"];

                    boList = Table.AsEnumerable().Select(r => new EmpExperienceBO
                    {
                        ExperienceId = r.Field<int>("ExperienceId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyUrl = r.Field<string>("CompanyUrl"),
                        JoinDate = r.Field<DateTime>("JoinDate"),
                        JoinDepartment = r.Field<string>("JoinDepartment"),
                        JoinDesignation = r.Field<string>("JoinDesignation"),
                        LeaveDate = r.Field<DateTime?>("LeaveDate"),
                        LeaveDepartment = r.Field<string>("LeaveDepartment"),
                        LeaveDesignation = r.Field<string>("LeaveDesignation"),
                        LeaveCompany = r.Field<string>("LeaveCompany"),
                        Achievements = r.Field<string>("Achievements"),
                        ShowJoinDate = r.Field<string>("ShowJoinDate"),
                        ShowLeaveDate = r.Field<string>("ShowLeaveDate")
                    }).ToList();
                }
            }
            return boList;
        }
    }
}
