using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class ApplicantResumeSearchDA : BaseService
    {
        public List<EmployeeBO> GetApplicantResumeInfo(string appName, string appId, string lookingFor, string availableFor, decimal preSalFrom, decimal preSalTo, decimal expSalFrom, decimal expSalTo, int currency, int jobCategory, int organizationType, int jobLocation, int expYrFrom, int expYrTo, int divisionId, int districtId, int thanaId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApplicantResumeInfo_SP"))
                {
                    if (!string.IsNullOrEmpty(appName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, appName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(appId))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.String, appId);
                    }
                    else {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.String, DBNull.Value);
                    }
                    if (lookingFor != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, lookingFor);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@JobLevel", DbType.String, DBNull.Value);
                    }
                    if (availableFor != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AvailableType", DbType.String, availableFor);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AvailableType", DbType.String, DBNull.Value);
                    }
                    if (preSalFrom != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PresentSalaryFrom", DbType.Decimal, preSalFrom);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PresentSalaryFrom", DbType.Decimal, DBNull.Value);
                    }
                    if (preSalTo != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PresentSalaryTo", DbType.Decimal, preSalTo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PresentSalaryTo", DbType.Decimal, DBNull.Value);
                    }
                    if (expSalFrom != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ExpectedSalaryFrom", DbType.Decimal, expSalFrom);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ExpectedSalaryFrom", DbType.Decimal, DBNull.Value);
                    }
                    if (expSalTo != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ExpectedSalaryTo", DbType.Decimal, expSalTo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ExpectedSalaryTo", DbType.Decimal, DBNull.Value);
                    }
                    if (currency != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Currency", DbType.Int32, currency);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Currency", DbType.Int32, DBNull.Value);
                    }
                    if (jobCategory != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PreferedJobType", DbType.Int32, jobCategory);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PreferedJobType", DbType.Int32, DBNull.Value);
                    }
                    if (organizationType != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PreferedOrganizationType", DbType.Int32, organizationType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PreferedOrganizationType", DbType.Int32, DBNull.Value);
                    }
                    if (jobLocation != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PreferedJobLocationId", DbType.Int32, jobLocation);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PreferedJobLocationId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@ExperienceFrom", DbType.Int32, expYrFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ExperienceTo", DbType.Int32, expYrTo);
                    if (divisionId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DivisionId", DbType.Int32, divisionId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DivisionId", DbType.Int32, DBNull.Value);
                    if (districtId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DistrictId", DbType.Int32, districtId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DistrictId", DbType.Int32, DBNull.Value);
                    if (thanaId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ThanaId", DbType.Int32, thanaId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@ThanaId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ApplicantResumeInfo");
                    DataTable Table = ds.Tables["ApplicantResumeInfo"];

                    boList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FirstName = r.Field<string>("FirstName"),
                        LastName = r.Field<string>("LastName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        IsApplicantRecruitment = r.Field<bool>("IsApplicantRecruitment")
                        //ReferenceBy = r.Field<string>("ReferenceBy")                        

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return boList;
        }

        public List<EmployeeBO> GetEmployeeResumeInfo(int empTypeId, int depId, int posId, int workStId, int fromAge, int toAge, int? fromJobLen, int? toJobLen, int? fromExp, int? toExp, int gradeId, string bloodGroup, DateTime? fromDOB, DateTime? toDOB, int divisionId, int districtId, int thanaId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeResumeInfo_SP"))
                {
                    if (empTypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpTypeId", DbType.Int32, empTypeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpTypeId", DbType.Int32, DBNull.Value);
                    }
                    if (depId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, depId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    }
                    if (posId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DesignationId", DbType.Int32, posId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DesignationId", DbType.Int32, DBNull.Value);
                    }
                    if (workStId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);
                    }
                    if (gradeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GradeId", DbType.Int32, gradeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GradeId", DbType.Int32, DBNull.Value);
                    }
                    if (fromAge != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AgeFrom", DbType.Int32, fromAge);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AgeFrom", DbType.Int32, DBNull.Value);
                    }
                    if (toAge != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AgeTo", DbType.Int32, toAge);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AgeTo", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@JobLenFrom", DbType.Int32, fromJobLen);
                    dbSmartAspects.AddInParameter(cmd, "@JobLenTo", DbType.Int32, toJobLen);
                    dbSmartAspects.AddInParameter(cmd, "@ExpFrom", DbType.Int32, fromExp);
                    dbSmartAspects.AddInParameter(cmd, "@ExpTo", DbType.Int32, toExp);
                    if (bloodGroup != "--- Please Select ---")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BloodGroup", DbType.String, bloodGroup);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@BloodGroup", DbType.String, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@DOBFrom", DbType.DateTime, fromDOB);
                    dbSmartAspects.AddInParameter(cmd, "@DOBTo", DbType.DateTime, toDOB);
                    if (divisionId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DivisionId", DbType.Int32, divisionId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DivisionId", DbType.Int32, DBNull.Value);
                    if (districtId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DistrictId", DbType.Int32, districtId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DistrictId", DbType.Int32, DBNull.Value);
                    if (thanaId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ThanaId", DbType.Int32, thanaId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@ThanaId", DbType.Int32, DBNull.Value);


                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeResumeInfo");
                    DataTable Table = ds.Tables["EmployeeResumeInfo"];

                    boList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FirstName = r.Field<string>("FirstName"),
                        LastName = r.Field<string>("LastName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmployeeName = r.Field<string>("EmployeeName")
                        //ReferenceBy = r.Field<string>("ReferenceBy")                        

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return boList;
        }
    }
}
