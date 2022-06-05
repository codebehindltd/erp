using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class JobCircularNRecruitmentDA : BaseService
    {
        public Boolean SaveJobCircularInfo(PayrollJobCircularBO jobCircular, out int tmpJobCircularId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollJobCircular_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@StaffRequisitionDetailsId", DbType.Int32, jobCircular.StaffRequisitionDetailsId);
                        dbSmartAspects.AddInParameter(command, "@JobTitle", DbType.String, jobCircular.JobTitle);
                        dbSmartAspects.AddInParameter(command, "@CircularDate", DbType.DateTime, jobCircular.CircularDate);
                        dbSmartAspects.AddInParameter(command, "@JobType", DbType.String, jobCircular.JobType);
                        dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, jobCircular.JobLevel);
                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, jobCircular.DepartmentId);
                        dbSmartAspects.AddInParameter(command, "@NoOfVancancie", DbType.Int16, jobCircular.NoOfVancancie);
                        dbSmartAspects.AddInParameter(command, "@DemandedTime", DbType.DateTime, jobCircular.DemandedTime);
                        dbSmartAspects.AddInParameter(command, "@AgeRangeFrom", DbType.Byte, jobCircular.AgeRangeFrom);
                        dbSmartAspects.AddInParameter(command, "@AgeRangeTo", DbType.Byte, jobCircular.AgeRangeTo);
                        dbSmartAspects.AddInParameter(command, "@Gender", DbType.String, jobCircular.Gender);
                        dbSmartAspects.AddInParameter(command, "@YearOfExperiance", DbType.Byte, jobCircular.YearOfExperiance);
                        dbSmartAspects.AddInParameter(command, "@JobDescription", DbType.String, jobCircular.JobDescription);
                        dbSmartAspects.AddInParameter(command, "@EducationalQualification", DbType.String, jobCircular.EducationalQualification);
                        dbSmartAspects.AddInParameter(command, "@AdditionalJobRequirement", DbType.String, jobCircular.AdditionalJobRequirement);
                        dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, jobCircular.ApprovedStatus);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, jobCircular.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@JobCircularId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpJobCircularId = Convert.ToInt32(command.Parameters["@JobCircularId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateJobCircularInfo(PayrollJobCircularBO jobCircular)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePayrollJobCircular_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@JobCircularId", DbType.String, jobCircular.JobCircularId);
                        dbSmartAspects.AddInParameter(command, "@StaffRequisitionDetailsId", DbType.Int32, jobCircular.StaffRequisitionDetailsId);
                        dbSmartAspects.AddInParameter(command, "@JobTitle", DbType.String, jobCircular.JobTitle);
                        dbSmartAspects.AddInParameter(command, "@CircularDate", DbType.DateTime, jobCircular.CircularDate);
                        dbSmartAspects.AddInParameter(command, "@JobType", DbType.String, jobCircular.JobType);
                        dbSmartAspects.AddInParameter(command, "@JobLevel", DbType.String, jobCircular.JobLevel);
                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, jobCircular.DepartmentId);
                        dbSmartAspects.AddInParameter(command, "@NoOfVancancie", DbType.Int16, jobCircular.NoOfVancancie);
                        dbSmartAspects.AddInParameter(command, "@DemandedTime", DbType.DateTime, jobCircular.DemandedTime);
                        dbSmartAspects.AddInParameter(command, "@AgeRangeFrom", DbType.Byte, jobCircular.AgeRangeFrom);
                        dbSmartAspects.AddInParameter(command, "@AgeRangeTo", DbType.Byte, jobCircular.AgeRangeTo);
                        dbSmartAspects.AddInParameter(command, "@Gender", DbType.String, jobCircular.Gender);
                        dbSmartAspects.AddInParameter(command, "@YearOfExperiance", DbType.Byte, jobCircular.YearOfExperiance);
                        dbSmartAspects.AddInParameter(command, "@JobDescription", DbType.String, jobCircular.JobDescription);
                        dbSmartAspects.AddInParameter(command, "@EducationalQualification", DbType.String, jobCircular.EducationalQualification);
                        dbSmartAspects.AddInParameter(command, "@AdditionalJobRequirement", DbType.String, jobCircular.AdditionalJobRequirement);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, jobCircular.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean UpdateJobCircularApprovedStatus(PayrollJobCircularBO jobCircular)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateJobCircularApprovedStatus_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@JobCircularId", DbType.String, jobCircular.JobCircularId);
                        dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, jobCircular.ApprovedStatus);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, jobCircular.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public PayrollJobCircularBO GetJobCircularById(Int64 JobCircularId)
        {
            PayrollJobCircularBO jobCircularlst = new PayrollJobCircularBO();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollJobCircularById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, JobCircularId);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularBO
                                   {
                                       JobCircularId = r.Field<Int64>("JobCircularId"),
                                       StaffRequisitionDetailsId = r.Field<Int32>("StaffRequisitionDetailsId"),
                                       JobTitle = r.Field<string>("JobTitle"),
                                       CircularDate = r.Field<DateTime>("CircularDate"),
                                       JobType = r.Field<int>("JobType"),
                                       JobLevel = r.Field<string>("JobLevel"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       NoOfVancancie = r.Field<Int16>("NoOfVancancie"),
                                       DemandedTime = r.Field<DateTime>("DemandedTime"),
                                       AgeRangeFrom = r.Field<byte>("AgeRangeFrom"),
                                       AgeRangeTo = r.Field<byte>("AgeRangeTo"),
                                       Gender = r.Field<string>("Gender"),
                                       YearOfExperiance = r.Field<byte>("YearOfExperiance"),
                                       JobDescription = r.Field<string>("JobDescription"),
                                       EducationalQualification = r.Field<string>("EducationalQualification"),
                                       AdditionalJobRequirement = r.Field<string>("AdditionalJobRequirement"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       DepartmentName = r.Field<string>("DepartmentName"),
                                       JobTypeName = r.Field<string>("JobTypeName")

                                   }).FirstOrDefault();
                }
            }

            return jobCircularlst;
        }

        public List<PayrollJobCircularBO> GetJobCircularForReport(Int64 JobCircularId)
        {
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollJobCircularById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, JobCircularId);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularBO
                                   {
                                       JobCircularId = r.Field<Int64>("JobCircularId"),
                                       StaffRequisitionDetailsId = r.Field<Int32>("StaffRequisitionDetailsId"),
                                       JobTitle = r.Field<string>("JobTitle"),
                                       CircularDate = r.Field<DateTime>("CircularDate"),
                                       JobType = r.Field<int>("JobType"),
                                       JobLevel = r.Field<string>("JobLevel"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       NoOfVancancie = r.Field<Int16>("NoOfVancancie"),
                                       DemandedTime = r.Field<DateTime>("DemandedTime"),
                                       AgeRangeFrom = r.Field<byte>("AgeRangeFrom"),
                                       AgeRangeTo = r.Field<byte>("AgeRangeTo"),
                                       Gender = r.Field<string>("Gender"),
                                       YearOfExperiance = r.Field<byte>("YearOfExperiance"),
                                       JobDescription = r.Field<string>("JobDescription"),
                                       EducationalQualification = r.Field<string>("EducationalQualification"),
                                       AdditionalJobRequirement = r.Field<string>("AdditionalJobRequirement"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       DepartmentName = r.Field<string>("DepartmentName"),
                                       JobTypeName = r.Field<string>("JobTypeName")

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }

        public List<PayrollJobCircularBO> GetJobCircularByDate(DateTime? currentDate)
        {
            currentDate = null;

            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollJobCircularByDate_SP"))
                {
                    if (currentDate == null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CurrentDate", DbType.DateTime, currentDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CurrentDate", DbType.DateTime, DBNull.Value);
                    }

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularBO
                                   {
                                       JobCircularId = r.Field<Int64>("JobCircularId"),
                                       JobTitle = r.Field<string>("JobTitle"),
                                       CircularDate = r.Field<DateTime>("CircularDate"),
                                       JobType = r.Field<int>("JobType"),
                                       JobLevel = r.Field<string>("JobLevel"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       NoOfVancancie = r.Field<Int16>("NoOfVancancie"),
                                       DemandedTime = r.Field<DateTime>("DemandedTime"),
                                       AgeRangeFrom = r.Field<byte>("AgeRangeFrom"),
                                       AgeRangeTo = r.Field<byte>("AgeRangeTo"),
                                       Gender = r.Field<string>("Gender"),
                                       YearOfExperiance = r.Field<byte>("YearOfExperiance"),
                                       JobDescription = r.Field<string>("JobDescription"),
                                       EducationalQualification = r.Field<string>("EducationalQualification"),
                                       AdditionalJobRequirement = r.Field<string>("AdditionalJobRequirement"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       OpenFrom = r.Field<DateTime?>("OpenFrom"),
                                       OpenTo = r.Field<DateTime?>("OpenTo"),
                                       DepartmentName = r.Field<string>("DepartmentName")

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }

        public List<PayrollJobCircularApplicantMappingBO> GetApplicantByJobCircular(Int64 jobCircularId)
        {
            List<PayrollJobCircularApplicantMappingBO> jobCircularlst = new List<PayrollJobCircularApplicantMappingBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApplicantByJobCircular_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularApplicantMappingBO
                                   {
                                       ApplicantId = r.Field<Int64>("ApplicantId"),
                                       EmployeeName = r.Field<string>("EmployeeName")

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }

        public List<PayrollJobCircularBO> GetJobCircular(int DepartmentId, DateTime? DemandedTimeFrom, DateTime? DemandedTimeTo, string JobTitle)
        {
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollJobCircular_SP"))
                {
                    if (DepartmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DepartmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (DemandedTimeFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeFrom", DbType.DateTime, DemandedTimeFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeFrom", DbType.Int32, DBNull.Value);

                    if (DemandedTimeTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeTo", DbType.DateTime, DemandedTimeTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeTo", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(JobTitle))
                        dbSmartAspects.AddInParameter(cmd, "@JobTitle", DbType.String, JobTitle);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@JobTitle", DbType.Int32, DBNull.Value);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularBO
                                   {
                                       JobCircularId = r.Field<Int64>("JobCircularId"),
                                       JobTitle = r.Field<string>("JobTitle"),
                                       CircularDate = r.Field<DateTime>("CircularDate"),
                                       JobType = r.Field<int>("JobType"),
                                       JobLevel = r.Field<string>("JobLevel"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       NoOfVancancie = r.Field<Int16>("NoOfVancancie"),
                                       DemandedTime = r.Field<DateTime>("DemandedTime"),
                                       AgeRangeFrom = r.Field<byte>("AgeRangeFrom"),
                                       AgeRangeTo = r.Field<byte>("AgeRangeTo"),
                                       Gender = r.Field<string>("Gender"),
                                       YearOfExperiance = r.Field<byte>("YearOfExperiance"),
                                       JobDescription = r.Field<string>("JobDescription"),
                                       EducationalQualification = r.Field<string>("EducationalQualification"),
                                       AdditionalJobRequirement = r.Field<string>("AdditionalJobRequirement"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       DepartmentName = r.Field<string>("DepartmentName"),
                                       JobTypeName = r.Field<string>("JobTypeName"),

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }

        public List<PayrollJobCircularBO> GetApprovedJobCircular(int DepartmentId, DateTime? DemandedTimeFrom, DateTime? DemandedTimeTo, string JobTitle)
        {
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollApprovedJobCircular_SP"))
                {
                    if (DepartmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DepartmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (DemandedTimeFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeFrom", DbType.DateTime, DemandedTimeFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeFrom", DbType.Int32, DBNull.Value);

                    if (DemandedTimeTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeTo", DbType.DateTime, DemandedTimeTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DemandedTimeTo", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(JobTitle))
                        dbSmartAspects.AddInParameter(cmd, "@JobTitle", DbType.String, JobTitle);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@JobTitle", DbType.Int32, DBNull.Value);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularBO
                                   {
                                       JobCircularId = r.Field<Int64>("JobCircularId"),
                                       JobTitle = r.Field<string>("JobTitle"),
                                       CircularDate = r.Field<DateTime>("CircularDate"),
                                       JobType = r.Field<int>("JobType"),
                                       JobLevel = r.Field<string>("JobLevel"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       NoOfVancancie = r.Field<Int16>("NoOfVancancie"),
                                       DemandedTime = r.Field<DateTime>("DemandedTime"),
                                       AgeRangeFrom = r.Field<byte>("AgeRangeFrom"),
                                       AgeRangeTo = r.Field<byte>("AgeRangeTo"),
                                       Gender = r.Field<string>("Gender"),
                                       YearOfExperiance = r.Field<byte>("YearOfExperiance"),
                                       JobDescription = r.Field<string>("JobDescription"),
                                       EducationalQualification = r.Field<string>("EducationalQualification"),
                                       AdditionalJobRequirement = r.Field<string>("AdditionalJobRequirement"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       DepartmentName = r.Field<string>("DepartmentName"),
                                       JobTypeName = r.Field<string>("JobTypeName"),

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }

        public List<PayrollJobCircularBO> GetApplicantNResult(int departmentId, int jobCircularId)
        {
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApplicantInterviewResult_SP"))
                {
                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int64, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int64, DBNull.Value);

                    if (jobCircularId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new PayrollJobCircularBO
                                   {
                                       JobCircularId = r.Field<Int64>("JobCircularId"),
                                       JobTitle = r.Field<string>("JobTitle"),
                                       CircularDate = r.Field<DateTime>("CircularDate"),
                                       JobType = r.Field<int>("JobType"),
                                       JobLevel = r.Field<string>("JobLevel"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       NoOfVancancie = r.Field<Int16>("NoOfVancancie"),
                                       DemandedTime = r.Field<DateTime>("DemandedTime"),
                                       AgeRangeFrom = r.Field<byte>("AgeRangeFrom"),
                                       AgeRangeTo = r.Field<byte>("AgeRangeTo"),
                                       Gender = r.Field<string>("Gender"),
                                       YearOfExperiance = r.Field<byte>("YearOfExperiance"),
                                       JobDescription = r.Field<string>("JobDescription"),
                                       EducationalQualification = r.Field<string>("EducationalQualification"),
                                       AdditionalJobRequirement = r.Field<string>("AdditionalJobRequirement"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       DepartmentName = r.Field<string>("DepartmentName"),
                                       JobTypeName = r.Field<string>("JobTypeName"),
                                       MarksObtain = r.Field<decimal>("MarksObtain"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       ApplicantId = r.Field<Int64>("ApplicantId"),
                                       PresentPhone = r.Field<string>("PresentPhone")

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }

        public bool SavePayrollJobCircularApplicantMapping(string applicantType, List<string> emp, List<string> job)
        {

            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (emp.Count > 0)
                        {
                            foreach (string jb in job)
                            {
                                foreach (string em in emp)
                                {
                                    using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SavePayrollJobCircularApplicantMapping_SP"))
                                    {
                                        cmdOutDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@JobCircularId", DbType.Int64, Convert.ToInt64(jb));
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ApplicantId", DbType.Int64, Convert.ToInt64(em));
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ApplicantType", DbType.String, applicantType);

                                        status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public List<ApplicantNInterviewDetailsBO> GetApplicantNInterviewDetails(Int64 jobCircularId, string applicantId)
        {
            List<ApplicantNInterviewDetailsBO> jobCircularlst = new List<ApplicantNInterviewDetailsBO>();

            DataSet bonusDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApplicantAppointmentLetter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ApplicantId", DbType.String, applicantId);
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);

                    dbSmartAspects.LoadDataSet(cmd, bonusDS, "CircularList");
                    DataTable table = bonusDS.Tables["CircularList"];

                    jobCircularlst = table.AsEnumerable().Select(r =>
                                   new ApplicantNInterviewDetailsBO
                                   {
                                       JobTitle = r.Field<string>("JobTitle"),
                                       DepartmentName = r.Field<string>("DepartmentName"),
                                       PresentAddress = r.Field<string>("PresentAddress"),
                                       ApplicantName = r.Field<string>("ApplicantName")

                                   }).ToList();
                }
            }

            return jobCircularlst;
        }
    }
}
