using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;
using System.Collections;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Data.SqlTypes;

namespace HotelManagement.Data.Payroll
{
    public class EmployeeDA : BaseService
    {
        public List<EmployeeBO> SearchEmployeeInfoByCategory(string EmpName, string EmpCode, string Department, string Designation)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SearchEmployeeInfoByCategory_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(Department))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Department", DbType.String, Department);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Department", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(Designation))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Designation", DbType.String, Designation);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Designation", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(EmpCode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, EmpCode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(EmpName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, EmpName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetEmployeeInfo()
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmpDisplayNameWithCode = bo.EmpCode + "-" + bo.DisplayName;
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.ShowProbableGratuityEligibilityDate = reader["ShowProbableGratuityEligibilityDate"].ToString();
                                bo.ShowProbablePFEligibilityDate = reader["ShowProbablePFEligibilityDate"].ToString();
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);
                                bo.EmpIdNNodeId = reader["EmpId"].ToString() + "~" + reader["NodeId"].ToString();
                                if (!string.IsNullOrEmpty(reader["ProvisionPeriod"].ToString()))
                                {
                                    bo.ProvisionPeriod = Convert.ToDateTime(reader["ProvisionPeriod"]);
                                }
                                else
                                {
                                    bo.ProvisionPeriod = null;
                                }
                                bo.ShowProvisionPeriod = reader["ShowProvisionPeriod"].ToString();

                                bo.RepotingTo = Convert.ToInt32(reader["RepotingTo"]);
                                bo.RepotingTo2 = Convert.ToInt32(reader["RepotingTo2"]);
                                
                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetActiveEmployeeInfo()
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveEmployeeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmpDisplayNameWithCode = bo.EmpCode + "-" + bo.DisplayName;
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.ShowProbableGratuityEligibilityDate = reader["ShowProbableGratuityEligibilityDate"].ToString();
                                bo.ShowProbablePFEligibilityDate = reader["ShowProbablePFEligibilityDate"].ToString();
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);
                                bo.EmpIdNNodeId = reader["EmpId"].ToString() + "~" + reader["NodeId"].ToString();
                                if (!string.IsNullOrEmpty(reader["ProvisionPeriod"].ToString()))
                                {
                                    bo.ProvisionPeriod = Convert.ToDateTime(reader["ProvisionPeriod"]);
                                }
                                else
                                {
                                    bo.ProvisionPeriod = null;
                                }
                                bo.ShowProvisionPeriod = reader["ShowProvisionPeriod"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetEmployeeInfoForWaiter()
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoForWaiter_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.ShowProbableGratuityEligibilityDate = reader["ShowProbableGratuityEligibilityDate"].ToString();
                                bo.ShowProbablePFEligibilityDate = reader["ShowProbablePFEligibilityDate"].ToString();
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);
                                bo.EmpIdNNodeId = reader["EmpId"].ToString() + "~" + reader["NodeId"].ToString();
                                if (!string.IsNullOrEmpty(reader["ProvisionPeriod"].ToString()))
                                {
                                    bo.ProvisionPeriod = Convert.ToDateTime(reader["ProvisionPeriod"]);
                                }
                                else
                                {
                                    bo.ProvisionPeriod = null;
                                }
                                bo.ShowProvisionPeriod = reader["ShowProvisionPeriod"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetEmployeeInfoByStatus(string activeStat)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoByActiveStat_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.String, activeStat);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmpInfo");
                    DataTable table = employeeDS.Tables["EmpInfo"];

                    boList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<Int32>("EmpId"),
                                       DisplayName = r.Field<string>("DisplayName")

                                   }).ToList();

                }
            }
            return boList;
        }
        public List<EmployeeBO> GetEmployeeByDepartment(int departmentId)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByDepartment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();

                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["EmpCode"].ToString() + " - " + reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.BasicAmount = Convert.ToDecimal(reader["BasicAmount"].ToString());
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                bo.PresentPhone = reader["PresentPhone"].ToString();

                                if (reader["BasicAmount"].ToString() != "")
                                    bo.BasicAmount = Convert.ToDecimal(reader["BasicAmount"].ToString());
                                else
                                    bo.BasicAmount = 0M;

                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);
                                bo.CurrencyName = Convert.ToString(reader["CurrencyName"]);
                                bo.EmployeeStatus = Convert.ToString(reader["EmployeeStatus"]);

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetTaskAssignedEmployeeByDepartment(int departmentId)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskAssignedEmployeeByDepartment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.PresentPhone = reader["PresentPhone"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetApplicantInfo()
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApplicantInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public Boolean SaveEmployeeInfo(EmployeeBO employeeBO, out int tmpEmployeeId, out string empCode, List<EmpEducationBO> educationBO, List<EmpExperienceBO> experienceBO, List<EmpDependentBO> dependentBO, EmpBankInfoBO bankInfo, List<EmpNomineeBO> nomineeBo, EmpCareerInfoBO careerInfo, List<EmpCareerTrainingBO> trainingList, List<EmpLanguageBO> languageList, List<EmpReferenceBO> referenceList, List<PayrollEmpBenefitBO> benefitList)
        {
            bool retVal = false;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveEmployeeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpCode", DbType.String, employeeBO.EmpCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, employeeBO.Title);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, employeeBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, employeeBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DisplayName", DbType.String, employeeBO.DisplayName);
                        dbSmartAspects.AddInParameter(commandMaster, "@JoinDate", DbType.DateTime, employeeBO.JoinDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartmentId", DbType.Int32, employeeBO.DepartmentId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpTypeId", DbType.Int32, employeeBO.EmpTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DesignationId", DbType.Int32, employeeBO.DesignationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GradeId", DbType.Int32, employeeBO.GradeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@OfficialEmail", DbType.String, employeeBO.OfficialEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceBy", DbType.String, employeeBO.ReferenceBy);

                        if (employeeBO.ResignationDate != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.DateTime, employeeBO.ResignationDate);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.DateTime, DBNull.Value);

                        if (employeeBO.InitialContractEndDate != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@InitialContractEndDate", DbType.DateTime, employeeBO.InitialContractEndDate);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@InitialContractEndDate", DbType.DateTime, DBNull.Value);

                        if (employeeBO.ProvisionPeriod != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@ProvisionPeriod", DbType.DateTime, employeeBO.ProvisionPeriod);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@ProvisionPeriod", DbType.DateTime, DBNull.Value);


                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, employeeBO.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, employeeBO.CreatedBy);

                        //New Fields
                        dbSmartAspects.AddInParameter(commandMaster, "@FathersName", DbType.String, employeeBO.FathersName);
                        dbSmartAspects.AddInParameter(commandMaster, "@MothersName", DbType.String, employeeBO.MothersName);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpDateOfBirth", DbType.DateTime, employeeBO.EmpDateOfBirth);
                        if (employeeBO.EmpDateOfMarriage != DateTime.MinValue)
                            dbSmartAspects.AddInParameter(commandMaster, "@MarriageDate", DbType.DateTime, employeeBO.EmpDateOfMarriage);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@MarriageDate", DbType.DateTime, DBNull.Value);
                        dbSmartAspects.AddInParameter(commandMaster, "@Gender", DbType.String, employeeBO.Gender);
                        dbSmartAspects.AddInParameter(commandMaster, "@BloodGroup", DbType.String, employeeBO.BloodGroup);

                        dbSmartAspects.AddInParameter(commandMaster, "@Religion", DbType.String, employeeBO.Religion);
                        dbSmartAspects.AddInParameter(commandMaster, "@Height", DbType.String, employeeBO.Height);

                        dbSmartAspects.AddInParameter(commandMaster, "@MaritalStatus", DbType.String, employeeBO.MaritalStatus);


                        dbSmartAspects.AddInParameter(commandMaster, "@CountryId", DbType.Int32, employeeBO.CountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Nationality", DbType.String, employeeBO.Nationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, employeeBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DivisionId", DbType.Int32, employeeBO.DivisionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DistrictId", DbType.Int32, employeeBO.DistrictId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ThanaId", DbType.Int32, employeeBO.ThanaId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, employeeBO.CostCenterId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, employeeBO.PassportNumber);

                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, employeeBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, employeeBO.PIssueDate);

                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, employeeBO.PExpireDate);
                        //dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocationId", DbType.Int32, employeeBO.CurrentLocationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentAddress", DbType.String, employeeBO.PresentAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCity", DbType.String, employeeBO.PresentCity);

                        dbSmartAspects.AddInParameter(commandMaster, "@PresentZipCode", DbType.String, employeeBO.PresentZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCountry", DbType.String, employeeBO.PresentCountry);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCountryId", DbType.Int32, employeeBO.PresentCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentPhone", DbType.String, employeeBO.PresentPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentAddress", DbType.String, employeeBO.PermanentAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCity", DbType.String, employeeBO.PermanentCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentZipCode", DbType.String, employeeBO.PermanentZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCountry", DbType.String, employeeBO.PermanentCountry);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCountryId", DbType.Int32, employeeBO.PermanentCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentPhone", DbType.String, employeeBO.PermanentPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@PersonalEmail", DbType.String, employeeBO.PersonalEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@AlternativeEmail", DbType.String, employeeBO.AlternativeEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsApplicant", DbType.Boolean, employeeBO.IsApplicant);

                        if (employeeBO.WorkStationId != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkStationId", DbType.Int32, employeeBO.WorkStationId);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkStationId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactName", DbType.String, employeeBO.EmergencyContactName);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactRelationship", DbType.String, employeeBO.EmergencyContactRelationship);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactNumber", DbType.String, employeeBO.EmergencyContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactNumberHome", DbType.String, employeeBO.EmergencyContactNumberHome);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactEmail", DbType.String, employeeBO.EmergencyContactEmail);

                        if (employeeBO.DonorId != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, employeeBO.DonorId);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@ActivityCode", DbType.String, employeeBO.ActivityCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@RepotingTo", DbType.Int32, employeeBO.RepotingTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@RepotingTo2", DbType.Int32, employeeBO.RepotingTo2);
                        dbSmartAspects.AddInParameter(commandMaster, "@GlCompanyId", DbType.Int32, employeeBO.GlCompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GlProjectId", DbType.Int32, employeeBO.GlProjectId);

                        dbSmartAspects.AddInParameter(commandMaster, "@EmployeeStatusId", DbType.Int32, employeeBO.EmployeeStatusId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayrollCurrencyId", DbType.Int32, employeeBO.PayrollCurrencyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@NotEffectOnHead", DbType.Int32, employeeBO.NotEffectOnHead);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsProvidentFundDeduct", DbType.Boolean, employeeBO.IsProvidentFundDeduct);
                        
                        dbSmartAspects.AddInParameter(commandMaster, "@TinNumber", DbType.String, employeeBO.TinNumber);


                        dbSmartAspects.AddInParameter(commandMaster, "@AppoinmentLetter", DbType.String, employeeBO.AppoinmentLetter);
                        dbSmartAspects.AddInParameter(commandMaster, "@JoiningAgreement", DbType.String, employeeBO.JoiningAgreement);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceBond", DbType.String, employeeBO.ServiceBond);
                        dbSmartAspects.AddInParameter(commandMaster, "@DSOAC", DbType.String, employeeBO.DSOAC);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationLetter", DbType.String, employeeBO.ConfirmationLetter);

                        dbSmartAspects.AddInParameter(commandMaster, "@EmpCompanyId", DbType.Int32, employeeBO.EmpCompanyId);

                        dbSmartAspects.AddOutParameter(commandMaster, "@EmpId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@AppCode", DbType.String, sizeof(decimal));
                        //dbSmartAspects.AddOutParameter();
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpEmployeeId = Convert.ToInt32(commandMaster.Parameters["@EmpId"].Value);
                        empCode = Convert.ToString(commandMaster.Parameters["@AppCode"].Value);

                    }
                    if (status > 0)
                    {
                        int countEducation = 0;
                        int countExperience = 0;
                        int countDependent = 0;
                        int countBank = 0;
                        int countNominee = 0;
                        int countCareer = 0;
                        int countTraining = 0;
                        int countLanguage = 0;
                        int countReference = 0;

                        //---------Emp Education------------------
                        if (educationBO != null)
                        {
                            using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("SaveEmpEducationInfo_SP"))
                            {
                                foreach (EmpEducationBO bo in educationBO)
                                {
                                    commandEducation.Parameters.Clear();


                                    dbSmartAspects.AddInParameter(commandEducation, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandEducation, "@LevelId", DbType.Int32, bo.LevelId);
                                    dbSmartAspects.AddInParameter(commandEducation, "@ExamName", DbType.String, bo.ExamName);
                                    dbSmartAspects.AddInParameter(commandEducation, "@InstituteName", DbType.String, bo.InstituteName);
                                    dbSmartAspects.AddInParameter(commandEducation, "@PassYear", DbType.String, bo.PassYear);
                                    dbSmartAspects.AddInParameter(commandEducation, "@SubjectName", DbType.String, bo.SubjectName);
                                    dbSmartAspects.AddInParameter(commandEducation, "@PassClass", DbType.String, bo.PassClass);

                                    countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transction);
                                }
                            }
                        }
                        //---------Emp Experience------------------
                        if (experienceBO != null)
                        {
                            using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("SaveEmpExperienceInfo_SP"))
                            {
                                foreach (EmpExperienceBO bo in experienceBO)
                                {
                                    commandExperience.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandExperience, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandExperience, "@CompanyName", DbType.String, bo.CompanyName);
                                    dbSmartAspects.AddInParameter(commandExperience, "@CompanyUrl", DbType.String, bo.CompanyUrl);
                                    dbSmartAspects.AddInParameter(commandExperience, "@JoinDate", DbType.DateTime, bo.JoinDate);
                                    dbSmartAspects.AddInParameter(commandExperience, "@JoinDesignation", DbType.String, bo.JoinDesignation);
                                    dbSmartAspects.AddInParameter(commandExperience, "@LeaveDate", DbType.DateTime, bo.LeaveDate);
                                    dbSmartAspects.AddInParameter(commandExperience, "@LeaveDesignation", DbType.String, bo.LeaveDesignation);
                                    dbSmartAspects.AddInParameter(commandExperience, "@Achievements", DbType.String, bo.Achievements);

                                    countExperience += dbSmartAspects.ExecuteNonQuery(commandExperience, transction);
                                }
                            }
                        }
                        //---------Emp Dependent------------------
                        if (dependentBO != null)
                        {
                            using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("SaveEmpDependentInfo_SP"))
                            {
                                foreach (EmpDependentBO bo in dependentBO)
                                {
                                    commandDependent.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDependent, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandDependent, "@BloodGroupId", DbType.Int32, bo.BloodGroupId);
                                    dbSmartAspects.AddInParameter(commandDependent, "@DependentName", DbType.String, bo.DependentName);
                                    dbSmartAspects.AddInParameter(commandDependent, "@Relationship", DbType.String, bo.Relationship);
                                    dbSmartAspects.AddInParameter(commandDependent, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                    dbSmartAspects.AddInParameter(commandDependent, "@Age", DbType.String, bo.Age);
                                    countDependent += dbSmartAspects.ExecuteNonQuery(commandDependent, transction);
                                }
                            }
                        }

                        //---------Emp Bank Info------------------
                        if (bankInfo != null)
                        {
                            if (bankInfo.BankId > 0)
                            {
                                using (DbCommand commandBankInfo = dbSmartAspects.GetStoredProcCommand("SaveEmpBankInfo_SP"))
                                {
                                    commandBankInfo.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandBankInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@BankId", DbType.String, bankInfo.BankId);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@BranchName", DbType.String, bankInfo.BranchName);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@AccountName", DbType.String, bankInfo.AccountName);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@AccountNumber", DbType.String, bankInfo.AccountNumber);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@AccountType", DbType.String, bankInfo.AccountType);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@CardNumber", DbType.String, bankInfo.CardNumber);
                                    dbSmartAspects.AddInParameter(commandBankInfo, "@Remarks", DbType.String, bankInfo.BankRemarks);

                                    countBank += dbSmartAspects.ExecuteNonQuery(commandBankInfo, transction);
                                }
                            }
                        }

                        //---------Emp Nominee------------------
                        if (nomineeBo != null)
                        {
                            using (DbCommand commandNominee = dbSmartAspects.GetStoredProcCommand("SaveEmpNomineeInfo_SP"))
                            {
                                foreach (EmpNomineeBO bo in nomineeBo)
                                {
                                    commandNominee.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandNominee, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandNominee, "@NomineeName", DbType.String, bo.NomineeName);
                                    dbSmartAspects.AddInParameter(commandNominee, "@Relationship", DbType.String, bo.Relationship);
                                    dbSmartAspects.AddInParameter(commandNominee, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                    dbSmartAspects.AddInParameter(commandNominee, "@Age", DbType.String, bo.Age);
                                    dbSmartAspects.AddInParameter(commandNominee, "@Percentage", DbType.String, bo.Percentage);
                                    countNominee += dbSmartAspects.ExecuteNonQuery(commandNominee, transction);
                                }
                            }
                        }

                        //---------Emp Career Info------------------
                        if (careerInfo != null)
                        {
                            using (DbCommand commandCareerInfo = dbSmartAspects.GetStoredProcCommand("SaveEmpCareerInfo_SP"))
                            {
                                commandCareerInfo.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandCareerInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@Objective", DbType.String, careerInfo.Objective);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@PresentSalary", DbType.Decimal, careerInfo.PresentSalary);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@ExpectedSalary", DbType.Decimal, careerInfo.ExpectedSalary);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@Currency", DbType.Int32, Convert.ToInt32(careerInfo.Currency));
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@JobLevel", DbType.String, careerInfo.JobLevel);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@AvailableType", DbType.String, careerInfo.AvailableType);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedJobType", DbType.Int32, careerInfo.PreferedJobType);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedOrganizationType", DbType.Int32, careerInfo.PreferedOrganizationType);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@CareerSummary", DbType.String, careerInfo.CareerSummary);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedJobLocationId", DbType.Int32, careerInfo.PreferedJobLocationId);
                                dbSmartAspects.AddInParameter(commandCareerInfo, "@ExtraCurriculmActivities", DbType.String, careerInfo.ExtraCurriculmActivities);

                                countCareer += dbSmartAspects.ExecuteNonQuery(commandCareerInfo, transction);
                            }
                        }

                        //---------Emp Training------------------
                        if (trainingList != null)
                        {
                            using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("SaveEmpCareerTrainingInfo_SP"))
                            {
                                foreach (EmpCareerTrainingBO bo in trainingList)
                                {
                                    commandTraining.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandTraining, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandTraining, "@TrainingTitle", DbType.String, bo.TrainingTitle);
                                    dbSmartAspects.AddInParameter(commandTraining, "@Topic", DbType.String, bo.Topic);
                                    dbSmartAspects.AddInParameter(commandTraining, "@Institute", DbType.String, bo.Institute);
                                    dbSmartAspects.AddInParameter(commandTraining, "@Country", DbType.Int32, bo.Country);
                                    dbSmartAspects.AddInParameter(commandTraining, "@Location", DbType.String, bo.Location);
                                    dbSmartAspects.AddInParameter(commandTraining, "@TrainingYear", DbType.String, bo.TrainingYear);
                                    dbSmartAspects.AddInParameter(commandTraining, "@Duration", DbType.Int32, bo.Duration);
                                    dbSmartAspects.AddInParameter(commandTraining, "@DurationType", DbType.String, bo.DurationType);

                                    countTraining += dbSmartAspects.ExecuteNonQuery(commandTraining, transction);
                                }
                            }
                        }

                        //---------Emp Language------------------
                        if (languageList != null)
                        {
                            using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("SaveEmpLanguageInfo_SP"))
                            {
                                foreach (EmpLanguageBO bo in languageList)
                                {
                                    commandLanguage.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandLanguage, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandLanguage, "@Language", DbType.String, bo.Language);
                                    dbSmartAspects.AddInParameter(commandLanguage, "@Reading", DbType.String, bo.Reading);
                                    dbSmartAspects.AddInParameter(commandLanguage, "@Writing", DbType.String, bo.Writing);
                                    dbSmartAspects.AddInParameter(commandLanguage, "@Speaking", DbType.String, bo.Speaking);

                                    countLanguage += dbSmartAspects.ExecuteNonQuery(commandLanguage, transction);
                                }
                            }
                        }

                        //---------Emp Reference------------------
                        if (referenceList != null)
                        {
                            using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("SaveEmpReferenceInfo_SP"))
                            {
                                foreach (EmpReferenceBO bo in referenceList)
                                {
                                    commandReference.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandReference, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandReference, "@Name", DbType.String, bo.Name);
                                    dbSmartAspects.AddInParameter(commandReference, "@Organization", DbType.String, bo.Organization);
                                    dbSmartAspects.AddInParameter(commandReference, "@Designation", DbType.String, bo.Designation);
                                    dbSmartAspects.AddInParameter(commandReference, "@Address", DbType.String, bo.Address);
                                    dbSmartAspects.AddInParameter(commandReference, "@Mobile", DbType.String, bo.Mobile);
                                    dbSmartAspects.AddInParameter(commandReference, "@Email", DbType.String, bo.Email);
                                    dbSmartAspects.AddInParameter(commandReference, "@Relation", DbType.String, bo.Relationship);

                                    dbSmartAspects.AddInParameter(commandReference, "@Description", DbType.String, bo.Description);

                                    countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transction);
                                }
                            }
                        }

                        //Emp Benefit
                        if (benefitList != null)
                        {
                            using (DbCommand commandBenefit = dbSmartAspects.GetStoredProcCommand("SaveEmpBenefitInfo_SP"))
                            {
                                foreach (PayrollEmpBenefitBO bo in benefitList)
                                {
                                    commandBenefit.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandBenefit, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandBenefit, "@BenefitHeadId", DbType.Int64, bo.BenefitHeadId);
                                    dbSmartAspects.AddInParameter(commandBenefit, "@EffectiveDate", DbType.DateTime, bo.EffectiveDate);

                                    status = dbSmartAspects.ExecuteNonQuery(commandBenefit, transction);
                                }
                            }
                        }

                        using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                        {
                            commandDocument.Parameters.Clear();
                            dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, tmpEmployeeId);
                            dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, employeeBO.RandomEmpId);
                            status = dbSmartAspects.ExecuteNonQuery(commandDocument, transction);
                        }
                        //if (count == educationBO.Count)
                        //{
                        transction.Commit();
                        retVal = true;
                        //}
                        //else
                        //{
                        //    retVal = false;
                        //}
                    }
                    else
                    {
                        retVal = false;
                    }
                }

            }
            // SaveEmployeeImageAndSignature(tmpEmployeeId, employeeBO);
            return retVal;
        }
        public void SaveEmployeeImageAndSignature(int empId, EmployeeBO employeeBO)
        {
            List<DocumentsBO> list = new List<DocumentsBO>();
            employeeBO.Signature.OwnerId = empId;
            employeeBO.Image.OwnerId = empId;

            if (!string.IsNullOrEmpty(employeeBO.Signature.Name))
            {
                DocumentsBO docSignature = employeeBO.Signature;
                list.Add(docSignature);
            }
            if (!string.IsNullOrEmpty(employeeBO.Image.Name))
            {
                DocumentsBO docImage = employeeBO.Image;
                list.Add(docImage);
            }

            if (list.Count > 0)
            {
                DocumentsDA docDA = new DocumentsDA();
                Boolean status = docDA.SaveDocumentsInfo(list);
            }
        }
        public Boolean UpdateEmployeeInfo(EmployeeBO employeeBO, List<EmpEducationBO> educationBO, ArrayList arrayEducationDelete, List<EmpExperienceBO> experienceBO, ArrayList arrayExperienceDelete, List<EmpDependentBO> dependentBO, ArrayList arrayDependentDelete, EmpBankInfoBO bankInfo, List<EmpNomineeBO> nomineeBo, ArrayList arrayNomineeDelete, EmpCareerInfoBO careerInfo, List<EmpCareerTrainingBO> trainingList, ArrayList arrayTrainingDelete, List<EmpLanguageBO> languageList, ArrayList arrayLanguageDelete, List<EmpReferenceBO> referenceList, ArrayList arrayReferenceDelete, List<PayrollEmpBenefitBO> savedBenefitList, List<PayrollEmpBenefitBO> deletedbenefitList)
        {
            bool retVal = false;
            int status = 0;
            int tmpEmployeeId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, employeeBO.EmpId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, employeeBO.Title);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpCode", DbType.String, employeeBO.EmpCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, employeeBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, employeeBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DisplayName", DbType.String, employeeBO.DisplayName);
                        dbSmartAspects.AddInParameter(commandMaster, "@JoinDate", DbType.DateTime, employeeBO.JoinDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartmentId", DbType.Int32, employeeBO.DepartmentId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpTypeId", DbType.Int32, employeeBO.EmpTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DesignationId", DbType.Int32, employeeBO.DesignationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GradeId", DbType.Int32, employeeBO.GradeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@OfficialEmail", DbType.String, employeeBO.OfficialEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceBy", DbType.String, employeeBO.ReferenceBy);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.String, employeeBO.ResignationDate);

                        if (employeeBO.ResignationDate != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.DateTime, employeeBO.ResignationDate);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.DateTime, DBNull.Value);

                        if (employeeBO.InitialContractEndDate != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@InitialContractEndDate", DbType.DateTime, employeeBO.InitialContractEndDate);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@InitialContractEndDate", DbType.DateTime, DBNull.Value);

                        if (employeeBO.ProvisionPeriod != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@ProvisionPeriod", DbType.DateTime, employeeBO.ProvisionPeriod);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@ProvisionPeriod", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, employeeBO.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, employeeBO.LastModifiedBy);
                        dbSmartAspects.AddInParameter(commandMaster, "@FathersName", DbType.String, employeeBO.FathersName);
                        dbSmartAspects.AddInParameter(commandMaster, "@MothersName", DbType.String, employeeBO.MothersName);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpDateOfBirth", DbType.DateTime, employeeBO.EmpDateOfBirth);
                        dbSmartAspects.AddInParameter(commandMaster, "@MarriageDate", DbType.DateTime, employeeBO.EmpDateOfMarriage);

                        dbSmartAspects.AddInParameter(commandMaster, "@Gender", DbType.String, employeeBO.Gender);
                        dbSmartAspects.AddInParameter(commandMaster, "@BloodGroup", DbType.String, employeeBO.BloodGroup);

                        dbSmartAspects.AddInParameter(commandMaster, "@Religion", DbType.String, employeeBO.Religion);
                        dbSmartAspects.AddInParameter(commandMaster, "@Height", DbType.String, employeeBO.Height);

                        dbSmartAspects.AddInParameter(commandMaster, "@MaritalStatus", DbType.String, employeeBO.MaritalStatus);


                        dbSmartAspects.AddInParameter(commandMaster, "@CountryId", DbType.Int32, employeeBO.CountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Nationality", DbType.String, employeeBO.Nationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, employeeBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DivisionId", DbType.Int32, employeeBO.DivisionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DistrictId", DbType.Int32, employeeBO.DistrictId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ThanaId", DbType.Int32, employeeBO.ThanaId);
                        dbSmartAspects.AddInParameter(commandMaster, "@CostCenterId", DbType.Int32, employeeBO.CostCenterId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, employeeBO.PassportNumber);

                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, employeeBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, employeeBO.PIssueDate);

                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, employeeBO.PExpireDate);
                        //dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocationId", DbType.Int32, employeeBO.CurrentLocationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentAddress", DbType.String, employeeBO.PresentAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCity", DbType.String, employeeBO.PresentCity);

                        dbSmartAspects.AddInParameter(commandMaster, "@PresentZipCode", DbType.String, employeeBO.PresentZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCountry", DbType.String, employeeBO.PresentCountry);

                        dbSmartAspects.AddInParameter(commandMaster, "@PresentPhone", DbType.String, employeeBO.PresentPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentAddress", DbType.String, employeeBO.PermanentAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCity", DbType.String, employeeBO.PermanentCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentZipCode", DbType.String, employeeBO.PermanentZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCountry", DbType.String, employeeBO.PermanentCountry);

                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentPhone", DbType.String, employeeBO.PermanentPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@PersonalEmail", DbType.String, employeeBO.PersonalEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@AlternativeEmail", DbType.String, employeeBO.AlternativeEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsApplicant", DbType.Boolean, employeeBO.IsApplicant);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsApplicantRecruitment", DbType.Boolean, employeeBO.IsApplicantRecruitment);
                        
                        if (employeeBO.WorkStationId != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkStationId", DbType.Int32, employeeBO.WorkStationId);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkStationId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactName", DbType.String, employeeBO.EmergencyContactName);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactRelationship", DbType.String, employeeBO.EmergencyContactRelationship);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactNumber", DbType.String, employeeBO.EmergencyContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactNumberHome", DbType.String, employeeBO.EmergencyContactNumberHome);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactEmail", DbType.String, employeeBO.EmergencyContactEmail);

                        dbSmartAspects.AddInParameter(commandMaster, "@IsProvidentFundDeduct", DbType.Boolean, employeeBO.IsProvidentFundDeduct);

                        dbSmartAspects.AddInParameter(commandMaster, "@TinNumber", DbType.String, employeeBO.TinNumber);

                        dbSmartAspects.AddInParameter(commandMaster, "@AppoinmentLetter", DbType.String, employeeBO.AppoinmentLetter);
                        dbSmartAspects.AddInParameter(commandMaster, "@JoiningAgreement", DbType.String, employeeBO.JoiningAgreement);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceBond", DbType.String, employeeBO.ServiceBond);
                        dbSmartAspects.AddInParameter(commandMaster, "@DSOAC", DbType.String, employeeBO.DSOAC);
                        dbSmartAspects.AddInParameter(commandMaster, "@ConfirmationLetter", DbType.String, employeeBO.ConfirmationLetter);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpCompanyId", DbType.Int32, employeeBO.EmpCompanyId);

                        if (employeeBO.DonorId != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, employeeBO.DonorId);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@ActivityCode", DbType.String, employeeBO.ActivityCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@RepotingTo", DbType.Int32, employeeBO.RepotingTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@RepotingTo2", DbType.Int32, employeeBO.RepotingTo2);
                        dbSmartAspects.AddInParameter(commandMaster, "@GlCompanyId", DbType.Int32, employeeBO.GlCompanyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GlProjectId", DbType.Int32, employeeBO.GlProjectId);

                        dbSmartAspects.AddInParameter(commandMaster, "@EmployeeStatusId", DbType.Int32, employeeBO.EmployeeStatusId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayrollCurrencyId", DbType.Int32, employeeBO.PayrollCurrencyId);
                        dbSmartAspects.AddInParameter(commandMaster, "@NotEffectOnHead", DbType.Int32, employeeBO.NotEffectOnHead);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpEmployeeId = employeeBO.EmpId;

                        if (status > 0)
                        {
                            int countEducation = 0;
                            int countExperience = 0;
                            int countDependent = 0;
                            int countBank = 0;
                            int countNominee = 0;
                            int countTraining = 0;
                            int countLanguage = 0;
                            int countReference = 0;

                            //---Emp Education-----------------------------------------------------------------------------------------------------Start
                            if (educationBO != null)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("SaveEmpEducationInfo_SP"))
                                {
                                    foreach (EmpEducationBO bo in educationBO)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandEducation.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandEducation, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@LevelId", DbType.Int32, bo.LevelId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@ExamName", DbType.String, bo.ExamName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@InstituteName", DbType.String, bo.InstituteName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassYear", DbType.String, bo.PassYear);
                                            dbSmartAspects.AddInParameter(commandEducation, "@SubjectName", DbType.String, bo.SubjectName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassClass", DbType.String, bo.PassClass);

                                            countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transction);
                                        }
                                    }
                                }
                            }

                            if (educationBO != null)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("UpdateEmpEducationInfo_SP"))
                                {
                                    foreach (EmpEducationBO bo in educationBO)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandEducation.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandEducation, "@EducationId", DbType.Int32, bo.EducationId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@LevelId", DbType.Int32, bo.LevelId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@ExamName", DbType.String, bo.ExamName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@InstituteName", DbType.String, bo.InstituteName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassYear", DbType.String, bo.PassYear);
                                            dbSmartAspects.AddInParameter(commandEducation, "@SubjectName", DbType.String, bo.SubjectName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassClass", DbType.String, bo.PassClass);

                                            countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transction);
                                        }
                                    }
                                }
                            }

                            if (educationBO != null)
                            {
                                if (countEducation == educationBO.Count)
                                {
                                    if (arrayEducationDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayEducationDelete)
                                        {
                                            using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "PayrollEmpEducation");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "EducationId");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                            }
                                        }
                                    }

                                    //transction.Commit();
                                    //retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Education-----------------------------------------------------------------------------------------------------End

                            //---Emp Experience-----------------------------------------------------------------------------------------------------Start
                            if (experienceBO != null)
                            {
                                using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("SaveEmpExperienceInfo_SP"))
                                {
                                    foreach (EmpExperienceBO bo in experienceBO)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandExperience.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandExperience, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyName", DbType.String, bo.CompanyName);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyUrl", DbType.String, bo.CompanyUrl);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDate", DbType.DateTime, bo.JoinDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDesignation", DbType.String, bo.JoinDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDate", DbType.DateTime, bo.LeaveDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDesignation", DbType.String, bo.LeaveDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@Achievements", DbType.String, bo.Achievements);

                                            countExperience += dbSmartAspects.ExecuteNonQuery(commandExperience, transction);
                                        }
                                    }
                                }
                            }

                            if (experienceBO != null)
                            {
                                using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("UpdateEmpExperienceInfo_SP"))
                                {
                                    foreach (EmpExperienceBO bo in experienceBO)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandExperience.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandExperience, "@ExperienceId", DbType.Int32, bo.ExperienceId);
                                            dbSmartAspects.AddInParameter(commandExperience, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyName", DbType.String, bo.CompanyName);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyUrl", DbType.String, bo.CompanyUrl);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDate", DbType.DateTime, bo.JoinDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDesignation", DbType.String, bo.JoinDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDate", DbType.DateTime, bo.LeaveDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDesignation", DbType.String, bo.LeaveDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@Achievements", DbType.String, bo.Achievements);

                                            countExperience += dbSmartAspects.ExecuteNonQuery(commandExperience, transction);
                                        }
                                    }
                                }
                            }
                            if (experienceBO != null)
                            {
                                if (countExperience == experienceBO.Count)
                                {
                                    if (arrayExperienceDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayExperienceDelete)
                                        {
                                            using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandExperience, "@TableName", DbType.String, "PayrollEmpExperience");
                                                dbSmartAspects.AddInParameter(commandExperience, "@TablePKField", DbType.String, "ExperienceId");
                                                dbSmartAspects.AddInParameter(commandExperience, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandExperience);
                                            }
                                        }
                                    }

                                    //transction.Commit();
                                    //retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Experience-----------------------------------------------------------------------------------------------------End

                            //---Emp Dependent-----------------------------------------------------------------------------------------------------Start
                            if (dependentBO != null)
                            {
                                using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("SaveEmpDependentInfo_SP"))
                                {
                                    foreach (EmpDependentBO bo in dependentBO)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandDependent.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandDependent, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DependentName", DbType.String, bo.DependentName);
                                            dbSmartAspects.AddInParameter(commandDependent, "@BloodGroupId", DbType.Int32, bo.BloodGroupId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Age", DbType.String, bo.Age);

                                            countDependent += dbSmartAspects.ExecuteNonQuery(commandDependent, transction);
                                        }
                                    }
                                }
                            }

                            if (dependentBO != null)
                            {
                                using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("UpdateEmpDependentInfo_SP"))
                                {
                                    foreach (EmpDependentBO bo in dependentBO)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandDependent.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandDependent, "@DependentId", DbType.Int32, bo.DependentId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@BloodGroupId", DbType.Int32, bo.BloodGroupId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DependentName", DbType.String, bo.DependentName);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Age", DbType.String, bo.Age);
                                            countDependent += dbSmartAspects.ExecuteNonQuery(commandDependent, transction);
                                        }
                                    }
                                }
                            }

                            if (dependentBO != null)
                            {
                                if (countDependent == dependentBO.Count)
                                {
                                    if (arrayDependentDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayDependentDelete)
                                        {
                                            using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandDependent, "@TableName", DbType.String, "PayrollEmpDependent");
                                                dbSmartAspects.AddInParameter(commandDependent, "@TablePKField", DbType.String, "DependentId");
                                                dbSmartAspects.AddInParameter(commandDependent, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandDependent);
                                            }
                                        }
                                    }
                                    //transction.Commit();
                                    //retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Dependent-----------------------------------------------------------------------------------------------------End

                            //---Emp Bank Info-----------------------------------------------------------------------------------------------------Start

                            if (bankInfo != null)
                            {
                                if (bankInfo.BankInfoId == 0)
                                {
                                    if (bankInfo.BankId > 0)
                                    {
                                        using (DbCommand commandBankInfo = dbSmartAspects.GetStoredProcCommand("SaveEmpBankInfo_SP"))
                                        {
                                            commandBankInfo.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandBankInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@BankId", DbType.String, bankInfo.BankId);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@BranchName", DbType.String, bankInfo.BranchName);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@AccountName", DbType.String, bankInfo.AccountName);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@AccountNumber", DbType.String, bankInfo.AccountNumber);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@AccountType", DbType.String, bankInfo.AccountType);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@CardNumber", DbType.String, bankInfo.CardNumber);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@Remarks", DbType.String, bankInfo.BankRemarks);

                                            countBank += dbSmartAspects.ExecuteNonQuery(commandBankInfo, transction);
                                        }
                                    }
                                }
                                else
                                {
                                    using (DbCommand commandBankInfo = dbSmartAspects.GetStoredProcCommand("UpdateEmpBankInfo_SP"))
                                    {
                                        commandBankInfo.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBankInfo, "@BankInfoId", DbType.Int32, bankInfo.BankInfoId);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@BankId", DbType.String, bankInfo.BankId);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@BranchName", DbType.String, bankInfo.BranchName);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@AccountName", DbType.String, bankInfo.AccountName);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@AccountNumber", DbType.String, bankInfo.AccountNumber);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@AccountType", DbType.String, bankInfo.AccountType);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@CardNumber", DbType.String, bankInfo.CardNumber);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@Remarks", DbType.String, bankInfo.BankRemarks);

                                        countBank += dbSmartAspects.ExecuteNonQuery(commandBankInfo, transction);

                                    }
                                }
                            }
                            //---Emp Bank Info-----------------------------------------------------------------------------------------------------End

                            //---Emp Nominee-----------------------------------------------------------------------------------------------------Start
                            if (nomineeBo != null)
                            {
                                using (DbCommand commandNominee = dbSmartAspects.GetStoredProcCommand("SaveEmpNomineeInfo_SP"))
                                {
                                    foreach (EmpNomineeBO bo in nomineeBo)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandNominee.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandNominee, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandNominee, "@NomineeName", DbType.String, bo.NomineeName);
                                            dbSmartAspects.AddInParameter(commandNominee, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandNominee, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandNominee, "@Age", DbType.String, bo.Age);
                                            dbSmartAspects.AddInParameter(commandNominee, "@Percentage", DbType.Decimal, bo.Percentage);

                                            countNominee += dbSmartAspects.ExecuteNonQuery(commandNominee, transction);
                                        }
                                    }
                                }
                            }

                            if (nomineeBo != null)
                            {
                                using (DbCommand commandNomineeUpdate = dbSmartAspects.GetStoredProcCommand("UpdateEmpNomineeInfo_SP"))
                                {
                                    foreach (EmpNomineeBO bo in nomineeBo)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandNomineeUpdate.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@NomineeId", DbType.Int32, bo.NomineeId);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@NomineeName", DbType.String, bo.NomineeName);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@Age", DbType.String, bo.Age);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@Percentage", DbType.Decimal, bo.Percentage);

                                            countNominee += dbSmartAspects.ExecuteNonQuery(commandNomineeUpdate, transction);
                                        }
                                    }
                                }
                            }

                            if (nomineeBo != null)
                            {
                                if (countNominee == nomineeBo.Count)
                                {
                                    if (arrayNomineeDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayNomineeDelete)
                                        {
                                            using (DbCommand commandDeleteNominee = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandDeleteNominee, "@TableName", DbType.String, "PayrollEmpNomineeInfo");
                                                dbSmartAspects.AddInParameter(commandDeleteNominee, "@TablePKField", DbType.String, "NomineeId");
                                                dbSmartAspects.AddInParameter(commandDeleteNominee, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandDeleteNominee);
                                            }
                                        }

                                        //transction.Commit();
                                        //retVal = true;
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Nominee-----------------------------------------------------------------------------------------------------End

                            //---Emp Career Info-------------------------------------------------------------------------------------------------Start
                            if (careerInfo != null)
                            {
                                using (DbCommand commandCareerInfo = dbSmartAspects.GetStoredProcCommand("UpdateEmpCareerInfo_SP"))
                                {
                                    commandCareerInfo.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@CareerInfoId", DbType.Int32, careerInfo.CareerInfoId);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@Objective", DbType.String, careerInfo.Objective);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PresentSalary", DbType.Decimal, careerInfo.PresentSalary);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@ExpectedSalary", DbType.Decimal, careerInfo.ExpectedSalary);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@Currency", DbType.Int32, Convert.ToInt32(careerInfo.Currency));
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@JobLevel", DbType.String, careerInfo.JobLevel);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@AvailableType", DbType.String, careerInfo.AvailableType);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedJobType", DbType.Int32, careerInfo.PreferedJobType);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedOrganizationType", DbType.Int32, careerInfo.PreferedOrganizationType);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@CareerSummary", DbType.String, careerInfo.CareerSummary);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedJobLocationId", DbType.Int32, careerInfo.PreferedJobLocationId);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@ExtraCurriculmActivities", DbType.String, careerInfo.ExtraCurriculmActivities);

                                    countDependent += dbSmartAspects.ExecuteNonQuery(commandCareerInfo, transction);

                                }
                            }
                            //---Emp Career Info-------------------------------------------------------------------------------------------------End

                            //---Emp Career Training---------------------------------------------------------------------------------------------Start
                            if (trainingList != null)
                            {
                                using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("SaveEmpCareerTrainingInfo_SP"))
                                {
                                    foreach (EmpCareerTrainingBO bo in trainingList)
                                    {
                                        if (bo.EmpId == null)
                                        {
                                            commandTraining.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandTraining, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingTitle", DbType.String, bo.TrainingTitle);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Topic", DbType.String, bo.Topic);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Institute", DbType.String, bo.Institute);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Country", DbType.Int32, bo.Country);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Location", DbType.String, bo.Location);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingYear", DbType.String, bo.TrainingYear);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Duration", DbType.Int32, bo.Duration);
                                            dbSmartAspects.AddInParameter(commandTraining, "@DurationType", DbType.String, bo.DurationType);

                                            countTraining += dbSmartAspects.ExecuteNonQuery(commandTraining, transction);
                                        }
                                    }
                                }
                            }

                            if (trainingList != null)
                            {
                                using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("UpdateEmpCareerTrainingInfo_SP"))
                                {
                                    foreach (EmpCareerTrainingBO bo in trainingList)
                                    {
                                        if (bo.EmpId != null)
                                        {
                                            commandTraining.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandTraining, "@CareerTrainingId", DbType.Int32, bo.CareerTrainingId);
                                            dbSmartAspects.AddInParameter(commandTraining, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingTitle", DbType.String, bo.TrainingTitle);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Topic", DbType.String, bo.Topic);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Institute", DbType.String, bo.Institute);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Country", DbType.Int32, bo.Country);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Location", DbType.String, bo.Location);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingYear", DbType.String, bo.TrainingYear);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Duration", DbType.Int32, bo.Duration);
                                            dbSmartAspects.AddInParameter(commandTraining, "@DurationType", DbType.String, bo.DurationType);

                                            countTraining += dbSmartAspects.ExecuteNonQuery(commandTraining, transction);
                                        }
                                    }
                                }
                            }

                            if (trainingList != null)
                            {
                                if (countTraining == trainingList.Count)
                                {
                                    if (arrayTrainingDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayTrainingDelete)
                                        {
                                            using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandTraining, "@TableName", DbType.String, "PayrollEmpCareerTraining");
                                                dbSmartAspects.AddInParameter(commandTraining, "@TablePKField", DbType.String, "CareerTrainingId");
                                                dbSmartAspects.AddInParameter(commandTraining, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandTraining);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Career Training-----------------------------------------------------------------------------------------------End

                            //---Emp Language Training---------------------------------------------------------------------------------------------Start
                            if (languageList != null)
                            {
                                using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("SaveEmpLanguageInfo_SP"))
                                {
                                    foreach (EmpLanguageBO bo in languageList)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandLanguage.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandLanguage, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Language", DbType.String, bo.Language);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Reading", DbType.String, bo.Reading);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Writing", DbType.String, bo.Writing);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Speaking", DbType.String, bo.Speaking);

                                            countLanguage += dbSmartAspects.ExecuteNonQuery(commandLanguage, transction);
                                        }
                                    }
                                }
                            }
                            if (languageList != null)
                            {
                                using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("UpdateEmpLanguageInfo_SP"))
                                {
                                    foreach (EmpLanguageBO bo in languageList)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandLanguage.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandLanguage, "@LanguageId", DbType.Int32, bo.LanguageId);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Language", DbType.String, bo.Language);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Reading", DbType.String, bo.Reading);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Writing", DbType.String, bo.Writing);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Speaking", DbType.String, bo.Speaking);

                                            countLanguage += dbSmartAspects.ExecuteNonQuery(commandLanguage, transction);
                                        }
                                    }
                                }
                            }

                            if (languageList != null)
                            {
                                if (countLanguage == languageList.Count)
                                {
                                    if (arrayLanguageDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayLanguageDelete)
                                        {
                                            using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandLanguage, "@TableName", DbType.String, "PayrollEmpLanguage");
                                                dbSmartAspects.AddInParameter(commandLanguage, "@TablePKField", DbType.String, "LanguageId");
                                                dbSmartAspects.AddInParameter(commandLanguage, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandLanguage);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Language Training-----------------------------------------------------------------------------------------------End

                            //---Emp Reference-----------------------------------------------------------------------------------------------------Start
                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("SaveEmpReferenceInfo_SP"))
                                {
                                    foreach (EmpReferenceBO bo in referenceList)
                                    {
                                        if (bo.EmpId == null)
                                        {
                                            commandReference.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandReference, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandReference, "@Name", DbType.String, bo.Name);
                                            dbSmartAspects.AddInParameter(commandReference, "@Organization", DbType.String, bo.Organization);
                                            dbSmartAspects.AddInParameter(commandReference, "@Designation", DbType.String, bo.Designation);
                                            dbSmartAspects.AddInParameter(commandReference, "@Address", DbType.String, bo.Address);
                                            dbSmartAspects.AddInParameter(commandReference, "@Mobile", DbType.String, bo.Mobile);
                                            dbSmartAspects.AddInParameter(commandReference, "@Email", DbType.String, bo.Email);
                                            dbSmartAspects.AddInParameter(commandReference, "@Relation", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandReference, "@Description", DbType.String, bo.Description);

                                            countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transction);
                                        }
                                    }
                                }
                            }

                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("UpdateEmpReferenceInfo_SP"))
                                {
                                    foreach (EmpReferenceBO bo in referenceList)
                                    {
                                        if (bo.EmpId != null)
                                        {
                                            commandReference.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandReference, "@ReferenceId", DbType.Int32, bo.ReferenceId);
                                            dbSmartAspects.AddInParameter(commandReference, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandReference, "@Name", DbType.String, bo.Name);
                                            dbSmartAspects.AddInParameter(commandReference, "@Organization", DbType.String, bo.Organization);
                                            dbSmartAspects.AddInParameter(commandReference, "@Designation", DbType.String, bo.Designation);
                                            dbSmartAspects.AddInParameter(commandReference, "@Address", DbType.String, bo.Address);
                                            dbSmartAspects.AddInParameter(commandReference, "@Mobile", DbType.String, bo.Mobile);
                                            dbSmartAspects.AddInParameter(commandReference, "@Email", DbType.String, bo.Email);
                                            dbSmartAspects.AddInParameter(commandReference, "@Relation", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandReference, "@Description", DbType.String, bo.Description);

                                            countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transction);
                                        }
                                    }
                                }
                            }

                            if (referenceList != null)
                            {
                                if (countReference == referenceList.Count)
                                {
                                    if (arrayReferenceDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayReferenceDelete)
                                        {
                                            using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandReference, "@TableName", DbType.String, "PayrollEmpReference");
                                                dbSmartAspects.AddInParameter(commandReference, "@TablePKField", DbType.String, "ReferenceId");
                                                dbSmartAspects.AddInParameter(commandReference, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandReference);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }
                            //---Emp Reference-----------------------------------------------------------------------------------------------------End

                            //---Emp Benefit-----------------------------------------------------------------------------------------------------Start
                            if (savedBenefitList != null)
                            {
                                using (DbCommand commandBenefit = dbSmartAspects.GetStoredProcCommand("SaveEmpBenefitInfo_SP"))
                                {
                                    foreach (PayrollEmpBenefitBO bo in savedBenefitList)
                                    {
                                        commandBenefit.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBenefit, "@EmpId", DbType.Int32, tmpEmployeeId);
                                        dbSmartAspects.AddInParameter(commandBenefit, "@BenefitHeadId", DbType.Int64, bo.BenefitHeadId);
                                        dbSmartAspects.AddInParameter(commandBenefit, "@EffectiveDate", DbType.DateTime, bo.EffectiveDate);

                                        status = dbSmartAspects.ExecuteNonQuery(commandBenefit);
                                    }
                                }
                            }
                            if (deletedbenefitList != null)
                            {
                                foreach (PayrollEmpBenefitBO bo in deletedbenefitList)
                                {
                                    using (DbCommand commandBenefit = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandBenefit, "@TableName", DbType.String, "PayrollEmpBenefit");
                                        dbSmartAspects.AddInParameter(commandBenefit, "@TablePKField", DbType.String, "EmpBenefitMappingId");
                                        dbSmartAspects.AddInParameter(commandBenefit, "@TablePKId", DbType.String, bo.EmpBenefitMappingId.ToString());

                                        status = dbSmartAspects.ExecuteNonQuery(commandBenefit);
                                    }
                                }
                            }
                            //---Emp Benefit-----------------------------------------------------------------------------------------------------End

                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            //     UpdateEmployeeDocuments(employeeBO.EmpId, employeeBO.RandomEmpId);
            return retVal;
        }
        public Boolean UpdateApplicantInfo(EmployeeBO employeeBO, List<EmpEducationBO> educationBO, ArrayList arrayEducationDelete, List<EmpExperienceBO> experienceBO, ArrayList arrayExperienceDelete, List<EmpDependentBO> dependentBO, ArrayList arrayDependentDelete, EmpBankInfoBO bankInfo, List<EmpNomineeBO> nomineeBo, ArrayList arrayNomineeDelete, EmpCareerInfoBO careerInfo, List<EmpCareerTrainingBO> trainingList, ArrayList arrayTrainingDelete, List<EmpLanguageBO> languageList, ArrayList arrayLanguageDelete, List<EmpReferenceBO> referenceList, ArrayList arrayReferenceDelete)
        {
            bool retVal = false;
            int status = 0;
            int tmpEmployeeId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateApplicantInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, employeeBO.EmpId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpCode", DbType.String, employeeBO.EmpCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, employeeBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, employeeBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@DisplayName", DbType.String, employeeBO.DisplayName);
                        dbSmartAspects.AddInParameter(commandMaster, "@JoinDate", DbType.DateTime, employeeBO.JoinDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@DepartmentId", DbType.Int32, employeeBO.DepartmentId);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpTypeId", DbType.Int32, employeeBO.EmpTypeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DesignationId", DbType.Int32, employeeBO.DesignationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GradeId", DbType.Int32, employeeBO.GradeId);
                        dbSmartAspects.AddInParameter(commandMaster, "@OfficialEmail", DbType.String, employeeBO.OfficialEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@ReferenceBy", DbType.String, employeeBO.ReferenceBy);
                        //dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.String, employeeBO.ResignationDate);

                        if (employeeBO.ResignationDate != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.DateTime, employeeBO.ResignationDate);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@ResignationDate", DbType.DateTime, DBNull.Value);

                        if (employeeBO.ProvisionPeriod != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@ProvisionPeriod", DbType.DateTime, employeeBO.ProvisionPeriod);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@ProvisionPeriod", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, employeeBO.Remarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, employeeBO.LastModifiedBy);
                        dbSmartAspects.AddInParameter(commandMaster, "@FathersName", DbType.String, employeeBO.FathersName);
                        dbSmartAspects.AddInParameter(commandMaster, "@MothersName", DbType.String, employeeBO.MothersName);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmpDateOfBirth", DbType.DateTime, employeeBO.EmpDateOfBirth);

                        dbSmartAspects.AddInParameter(commandMaster, "@Gender", DbType.String, employeeBO.Gender);
                        dbSmartAspects.AddInParameter(commandMaster, "@BloodGroup", DbType.String, employeeBO.BloodGroup);

                        dbSmartAspects.AddInParameter(commandMaster, "@Religion", DbType.String, employeeBO.Religion);
                        dbSmartAspects.AddInParameter(commandMaster, "@Height", DbType.String, employeeBO.Height);

                        dbSmartAspects.AddInParameter(commandMaster, "@MaritalStatus", DbType.String, employeeBO.MaritalStatus);


                        dbSmartAspects.AddInParameter(commandMaster, "@CountryId", DbType.Int32, employeeBO.CountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Nationality", DbType.String, employeeBO.Nationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, employeeBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DivisionId", DbType.Int32, employeeBO.DivisionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@DistrictId", DbType.Int32, employeeBO.DistrictId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ThanaId", DbType.Int32, employeeBO.ThanaId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, employeeBO.PassportNumber);

                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, employeeBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, employeeBO.PIssueDate);

                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, employeeBO.PExpireDate);
                        //dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocationId", DbType.Int32, employeeBO.CurrentLocationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentAddress", DbType.String, employeeBO.PresentAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCity", DbType.String, employeeBO.PresentCity);

                        dbSmartAspects.AddInParameter(commandMaster, "@PresentZipCode", DbType.String, employeeBO.PresentZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCountry", DbType.String, employeeBO.PresentCountry);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentCountryId", DbType.Int32, employeeBO.PresentCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PresentPhone", DbType.String, employeeBO.PresentPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentAddress", DbType.String, employeeBO.PermanentAddress);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCity", DbType.String, employeeBO.PermanentCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentZipCode", DbType.String, employeeBO.PermanentZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCountry", DbType.String, employeeBO.PermanentCountry);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentCountryId", DbType.String, employeeBO.PermanentCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PermanentPhone", DbType.String, employeeBO.PermanentPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@PersonalEmail", DbType.String, employeeBO.PersonalEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@AlternativeEmail", DbType.String, employeeBO.AlternativeEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsApplicant", DbType.Boolean, employeeBO.IsApplicant);

                        if (employeeBO.WorkStationId != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkStationId", DbType.Int32, employeeBO.WorkStationId);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@WorkStationId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactName", DbType.String, employeeBO.EmergencyContactName);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactRelationship", DbType.String, employeeBO.EmergencyContactRelationship);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactNumber", DbType.String, employeeBO.EmergencyContactNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactNumberHome", DbType.String, employeeBO.EmergencyContactNumberHome);
                        dbSmartAspects.AddInParameter(commandMaster, "@EmergencyContactEmail", DbType.String, employeeBO.EmergencyContactEmail);

                        if (employeeBO.DonorId != null)
                            dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, employeeBO.DonorId);
                        else
                            dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(commandMaster, "@ActivityCode", DbType.String, employeeBO.ActivityCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@RepotingTo", DbType.Int32, employeeBO.RepotingTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@RepotingTo2", DbType.Int32, employeeBO.RepotingTo2);
                        dbSmartAspects.AddInParameter(commandMaster, "@GlCompanyId", DbType.Int32, employeeBO.GlCompanyId);
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpEmployeeId = employeeBO.EmpId;

                        if (status > 0)
                        {
                            int countEducation = 0;
                            int countExperience = 0;
                            int countDependent = 0;
                            int countBank = 0;
                            int countNominee = 0;
                            int countTraining = 0;
                            int countLanguage = 0;
                            int countReference = 0;

                            //---Emp Education-----------------------------------------------------------------------------------------------------Start
                            if (educationBO != null)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("SaveEmpEducationInfo_SP"))
                                {
                                    foreach (EmpEducationBO bo in educationBO)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandEducation.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandEducation, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@LevelId", DbType.Int32, bo.LevelId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@ExamName", DbType.String, bo.ExamName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@InstituteName", DbType.String, bo.InstituteName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassYear", DbType.String, bo.PassYear);
                                            dbSmartAspects.AddInParameter(commandEducation, "@SubjectName", DbType.String, bo.SubjectName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassClass", DbType.String, bo.PassClass);

                                            countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transction);
                                        }
                                    }
                                }
                            }

                            if (educationBO != null)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("UpdateEmpEducationInfo_SP"))
                                {
                                    foreach (EmpEducationBO bo in educationBO)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandEducation.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandEducation, "@EducationId", DbType.Int32, bo.EducationId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@LevelId", DbType.Int32, bo.LevelId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@ExamName", DbType.String, bo.ExamName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@InstituteName", DbType.String, bo.InstituteName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassYear", DbType.String, bo.PassYear);
                                            dbSmartAspects.AddInParameter(commandEducation, "@SubjectName", DbType.String, bo.SubjectName);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassClass", DbType.String, bo.PassClass);

                                            countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transction);
                                        }
                                    }
                                }
                            }

                            if (educationBO != null)
                            {
                                if (countEducation == educationBO.Count)
                                {
                                    if (arrayEducationDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayEducationDelete)
                                        {
                                            using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "PayrollEmpEducation");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "EducationId");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                            }
                                        }
                                    }

                                    //transction.Commit();
                                    //retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Education-----------------------------------------------------------------------------------------------------End

                            //---Emp Experience-----------------------------------------------------------------------------------------------------Start
                            if (experienceBO != null)
                            {
                                using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("SaveEmpExperienceInfo_SP"))
                                {
                                    foreach (EmpExperienceBO bo in experienceBO)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandExperience.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandExperience, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyName", DbType.String, bo.CompanyName);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyUrl", DbType.String, bo.CompanyUrl);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDate", DbType.DateTime, bo.JoinDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDesignation", DbType.String, bo.JoinDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDate", DbType.DateTime, bo.LeaveDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDesignation", DbType.String, bo.LeaveDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@Achievements", DbType.String, bo.Achievements);

                                            countExperience += dbSmartAspects.ExecuteNonQuery(commandExperience, transction);
                                        }
                                    }
                                }
                            }

                            if (experienceBO != null)
                            {
                                using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("UpdateEmpExperienceInfo_SP"))
                                {
                                    foreach (EmpExperienceBO bo in experienceBO)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandExperience.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandExperience, "@ExperienceId", DbType.Int32, bo.ExperienceId);
                                            dbSmartAspects.AddInParameter(commandExperience, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyName", DbType.String, bo.CompanyName);
                                            dbSmartAspects.AddInParameter(commandExperience, "@CompanyUrl", DbType.String, bo.CompanyUrl);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDate", DbType.DateTime, bo.JoinDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@JoinDesignation", DbType.String, bo.JoinDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDate", DbType.DateTime, bo.LeaveDate);
                                            dbSmartAspects.AddInParameter(commandExperience, "@LeaveDesignation", DbType.String, bo.LeaveDesignation);
                                            dbSmartAspects.AddInParameter(commandExperience, "@Achievements", DbType.String, bo.Achievements);

                                            countExperience += dbSmartAspects.ExecuteNonQuery(commandExperience, transction);
                                        }
                                    }
                                }
                            }
                            if (experienceBO != null)
                            {
                                if (countExperience == experienceBO.Count)
                                {
                                    if (arrayExperienceDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayExperienceDelete)
                                        {
                                            using (DbCommand commandExperience = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandExperience, "@TableName", DbType.String, "PayrollEmpExperience");
                                                dbSmartAspects.AddInParameter(commandExperience, "@TablePKField", DbType.String, "ExperienceId");
                                                dbSmartAspects.AddInParameter(commandExperience, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandExperience);
                                            }
                                        }
                                    }

                                    //transction.Commit();
                                    //retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Experience-----------------------------------------------------------------------------------------------------End

                            //---Emp Dependent-----------------------------------------------------------------------------------------------------Start
                            if (dependentBO != null)
                            {
                                using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("SaveEmpDependentInfo_SP"))
                                {
                                    foreach (EmpDependentBO bo in dependentBO)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandDependent.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandDependent, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DependentName", DbType.String, bo.DependentName);
                                            dbSmartAspects.AddInParameter(commandDependent, "@BloodGroupId", DbType.Int32, bo.BloodGroupId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Age", DbType.String, bo.Age);

                                            countDependent += dbSmartAspects.ExecuteNonQuery(commandDependent, transction);
                                        }
                                    }
                                }
                            }

                            if (dependentBO != null)
                            {
                                using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("UpdateEmpDependentInfo_SP"))
                                {
                                    foreach (EmpDependentBO bo in dependentBO)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandDependent.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandDependent, "@DependentId", DbType.Int32, bo.DependentId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DependentName", DbType.String, bo.DependentName);
                                            dbSmartAspects.AddInParameter(commandDependent, "@BloodGroupId", DbType.Int32, bo.BloodGroupId);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandDependent, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandDependent, "@Age", DbType.String, bo.Age);
                                            countDependent += dbSmartAspects.ExecuteNonQuery(commandDependent, transction);
                                        }
                                    }
                                }
                            }

                            if (dependentBO != null)
                            {
                                if (countDependent == dependentBO.Count)
                                {
                                    if (arrayDependentDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayDependentDelete)
                                        {
                                            using (DbCommand commandDependent = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandDependent, "@TableName", DbType.String, "PayrollEmpDependent");
                                                dbSmartAspects.AddInParameter(commandDependent, "@TablePKField", DbType.String, "DependentId");
                                                dbSmartAspects.AddInParameter(commandDependent, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandDependent);
                                            }
                                        }
                                    }
                                    //transction.Commit();
                                    //retVal = true;
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Dependent-----------------------------------------------------------------------------------------------------End

                            //---Emp Bank Info-----------------------------------------------------------------------------------------------------Start

                            if (bankInfo != null)
                            {
                                if (bankInfo.BankInfoId == 0)
                                {
                                    if (bankInfo.BankId > 0)
                                    {
                                        using (DbCommand commandBankInfo = dbSmartAspects.GetStoredProcCommand("SaveEmpBankInfo_SP"))
                                        {
                                            commandBankInfo.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandBankInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@BankId", DbType.String, bankInfo.BankId);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@BranchName", DbType.String, bankInfo.BranchName);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@AccountName", DbType.String, bankInfo.AccountName);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@AccountNumber", DbType.String, bankInfo.AccountNumber);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@AccountType", DbType.String, bankInfo.AccountType);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@CardNumber", DbType.String, bankInfo.CardNumber);
                                            dbSmartAspects.AddInParameter(commandBankInfo, "@Remarks", DbType.String, bankInfo.BankRemarks);

                                            countBank += dbSmartAspects.ExecuteNonQuery(commandBankInfo, transction);
                                        }
                                    }
                                }
                            }

                            if (bankInfo != null)
                            {
                                if (bankInfo.BankInfoId > 0)
                                {
                                    using (DbCommand commandBankInfo = dbSmartAspects.GetStoredProcCommand("UpdateEmpBankInfo_SP"))
                                    {
                                        commandBankInfo.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandBankInfo, "@BankInfoId", DbType.Int32, bankInfo.BankInfoId);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@BankId", DbType.String, bankInfo.BankId);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@BranchName", DbType.String, bankInfo.BranchName);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@AccountName", DbType.String, bankInfo.AccountName);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@AccountNumber", DbType.String, bankInfo.AccountNumber);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@AccountType", DbType.String, bankInfo.AccountType);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@CardNumber", DbType.String, bankInfo.CardNumber);
                                        dbSmartAspects.AddInParameter(commandBankInfo, "@Remarks", DbType.String, bankInfo.BankRemarks);

                                        countBank += dbSmartAspects.ExecuteNonQuery(commandBankInfo, transction);

                                    }
                                }
                            }
                            //---Emp Bank Info-----------------------------------------------------------------------------------------------------End

                            //---Emp Nominee-----------------------------------------------------------------------------------------------------Start
                            if (nomineeBo != null)
                            {
                                using (DbCommand commandNominee = dbSmartAspects.GetStoredProcCommand("SaveEmpNomineeInfo_SP"))
                                {
                                    foreach (EmpNomineeBO bo in nomineeBo)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandNominee.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandNominee, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandNominee, "@NomineeName", DbType.String, bo.NomineeName);
                                            dbSmartAspects.AddInParameter(commandNominee, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandNominee, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandNominee, "@Age", DbType.String, bo.Age);
                                            dbSmartAspects.AddInParameter(commandNominee, "@Percentage", DbType.Decimal, bo.Percentage);

                                            countNominee += dbSmartAspects.ExecuteNonQuery(commandNominee, transction);
                                        }
                                    }
                                }
                            }

                            if (nomineeBo != null)
                            {
                                using (DbCommand commandNomineeUpdate = dbSmartAspects.GetStoredProcCommand("UpdateEmpNomineeInfo_SP"))
                                {
                                    foreach (EmpNomineeBO bo in nomineeBo)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandNomineeUpdate.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@NomineeId", DbType.Int32, bo.NomineeId);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@NomineeName", DbType.String, bo.NomineeName);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@DateOfBirth", DbType.DateTime, bo.DateOfBirth);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@Age", DbType.String, bo.Age);
                                            dbSmartAspects.AddInParameter(commandNomineeUpdate, "@Percentage", DbType.Decimal, bo.Percentage);

                                            countNominee += dbSmartAspects.ExecuteNonQuery(commandNomineeUpdate, transction);
                                        }
                                    }
                                }
                            }

                            if (nomineeBo != null)
                            {
                                if (countNominee == nomineeBo.Count)
                                {
                                    if (arrayNomineeDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayNomineeDelete)
                                        {
                                            using (DbCommand commandDeleteNominee = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandDeleteNominee, "@TableName", DbType.String, "PayrollEmpNomineeInfo");
                                                dbSmartAspects.AddInParameter(commandDeleteNominee, "@TablePKField", DbType.String, "NomineeId");
                                                dbSmartAspects.AddInParameter(commandDeleteNominee, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandDeleteNominee);
                                            }
                                        }

                                        //transction.Commit();
                                        //retVal = true;
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Nominee-----------------------------------------------------------------------------------------------------End

                            //---Emp Career Info-------------------------------------------------------------------------------------------------Start
                            if (careerInfo != null)
                            {
                                using (DbCommand commandCareerInfo = dbSmartAspects.GetStoredProcCommand("UpdateEmpCareerInfo_SP"))
                                {
                                    commandCareerInfo.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@CareerInfoId", DbType.Int32, careerInfo.CareerInfoId);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@EmpId", DbType.Int32, tmpEmployeeId);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@Objective", DbType.String, careerInfo.Objective);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PresentSalary", DbType.Decimal, careerInfo.PresentSalary);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@ExpectedSalary", DbType.Decimal, careerInfo.ExpectedSalary);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@Currency", DbType.Int32, Convert.ToInt32(careerInfo.Currency));
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@JobLevel", DbType.String, careerInfo.JobLevel);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@AvailableType", DbType.String, careerInfo.AvailableType);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedJobType", DbType.Int32, careerInfo.PreferedJobType);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedOrganizationType", DbType.Int32, careerInfo.PreferedOrganizationType);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@CareerSummary", DbType.String, careerInfo.CareerSummary);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@PreferedJobLocationId", DbType.Int32, careerInfo.PreferedJobLocationId);
                                    dbSmartAspects.AddInParameter(commandCareerInfo, "@ExtraCurriculmActivities", DbType.String, careerInfo.ExtraCurriculmActivities);

                                    countDependent += dbSmartAspects.ExecuteNonQuery(commandCareerInfo, transction);

                                }
                            }
                            //---Emp Career Info-------------------------------------------------------------------------------------------------End

                            //---Emp Career Training---------------------------------------------------------------------------------------------Start
                            if (trainingList != null)
                            {
                                using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("SaveEmpCareerTrainingInfo_SP"))
                                {
                                    foreach (EmpCareerTrainingBO bo in trainingList)
                                    {
                                        if (bo.EmpId == null)
                                        {
                                            commandTraining.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandTraining, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingTitle", DbType.String, bo.TrainingTitle);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Topic", DbType.String, bo.Topic);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Institute", DbType.String, bo.Institute);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Country", DbType.Int32, bo.Country);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Location", DbType.String, bo.Location);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingYear", DbType.String, bo.TrainingYear);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Duration", DbType.Int32, bo.Duration);
                                            dbSmartAspects.AddInParameter(commandTraining, "@DurationType", DbType.String, bo.DurationType);

                                            countTraining += dbSmartAspects.ExecuteNonQuery(commandTraining, transction);
                                        }
                                    }
                                }
                            }

                            if (trainingList != null)
                            {
                                using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("UpdateEmpCareerTrainingInfo_SP"))
                                {
                                    foreach (EmpCareerTrainingBO bo in trainingList)
                                    {
                                        if (bo.EmpId != null)
                                        {
                                            commandTraining.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandTraining, "@CareerTrainingId", DbType.Int32, bo.CareerTrainingId);
                                            dbSmartAspects.AddInParameter(commandTraining, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingTitle", DbType.String, bo.TrainingTitle);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Topic", DbType.String, bo.Topic);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Institute", DbType.String, bo.Institute);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Country", DbType.Int32, bo.Country);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Location", DbType.String, bo.Location);
                                            dbSmartAspects.AddInParameter(commandTraining, "@TrainingYear", DbType.String, bo.TrainingYear);
                                            dbSmartAspects.AddInParameter(commandTraining, "@Duration", DbType.Int32, bo.Duration);
                                            dbSmartAspects.AddInParameter(commandTraining, "@DurationType", DbType.String, bo.DurationType);

                                            countTraining += dbSmartAspects.ExecuteNonQuery(commandTraining, transction);
                                        }
                                    }
                                }
                            }

                            if (trainingList != null)
                            {
                                if (countTraining == trainingList.Count)
                                {
                                    if (arrayTrainingDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayTrainingDelete)
                                        {
                                            using (DbCommand commandTraining = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandTraining, "@TableName", DbType.String, "PayrollEmpCareerTraining");
                                                dbSmartAspects.AddInParameter(commandTraining, "@TablePKField", DbType.String, "CareerTrainingId");
                                                dbSmartAspects.AddInParameter(commandTraining, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandTraining);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Career Training-----------------------------------------------------------------------------------------------End

                            //---Emp Language Training---------------------------------------------------------------------------------------------Start
                            if (languageList != null)
                            {
                                using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("SaveEmpLanguageInfo_SP"))
                                {
                                    foreach (EmpLanguageBO bo in languageList)
                                    {
                                        if (bo.EmpId == 0)
                                        {
                                            commandLanguage.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandLanguage, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Language", DbType.String, bo.Language);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Reading", DbType.String, bo.Reading);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Writing", DbType.String, bo.Writing);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Speaking", DbType.String, bo.Speaking);

                                            countLanguage += dbSmartAspects.ExecuteNonQuery(commandLanguage, transction);
                                        }
                                    }
                                }
                            }
                            if (languageList != null)
                            {
                                using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("UpdateEmpLanguageInfo_SP"))
                                {
                                    foreach (EmpLanguageBO bo in languageList)
                                    {
                                        if (bo.EmpId != 0)
                                        {
                                            commandLanguage.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandLanguage, "@LanguageId", DbType.Int32, bo.LanguageId);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Language", DbType.String, bo.Language);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Reading", DbType.String, bo.Reading);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Writing", DbType.String, bo.Writing);
                                            dbSmartAspects.AddInParameter(commandLanguage, "@Speaking", DbType.String, bo.Speaking);

                                            countLanguage += dbSmartAspects.ExecuteNonQuery(commandLanguage, transction);
                                        }
                                    }
                                }
                            }

                            if (languageList != null)
                            {
                                if (countLanguage == languageList.Count)
                                {
                                    if (arrayLanguageDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayLanguageDelete)
                                        {
                                            using (DbCommand commandLanguage = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandLanguage, "@TableName", DbType.String, "PayrollEmpLanguage");
                                                dbSmartAspects.AddInParameter(commandLanguage, "@TablePKField", DbType.String, "LanguageId");
                                                dbSmartAspects.AddInParameter(commandLanguage, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandLanguage);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Language Training-----------------------------------------------------------------------------------------------End

                            //---Emp Reference-----------------------------------------------------------------------------------------------------Start
                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("SaveEmpReferenceInfo_SP"))
                                {
                                    foreach (EmpReferenceBO bo in referenceList)
                                    {
                                        if (bo.EmpId == null)
                                        {
                                            commandReference.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandReference, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandReference, "@Name", DbType.String, bo.Name);
                                            dbSmartAspects.AddInParameter(commandReference, "@Organization", DbType.String, bo.Organization);
                                            dbSmartAspects.AddInParameter(commandReference, "@Designation", DbType.String, bo.Designation);
                                            dbSmartAspects.AddInParameter(commandReference, "@Address", DbType.String, bo.Address);
                                            dbSmartAspects.AddInParameter(commandReference, "@Mobile", DbType.String, bo.Mobile);
                                            dbSmartAspects.AddInParameter(commandReference, "@Email", DbType.String, bo.Email);
                                            dbSmartAspects.AddInParameter(commandReference, "@Relation", DbType.String, bo.Relationship);

                                            dbSmartAspects.AddInParameter(commandReference, "@Description", DbType.String, bo.Description);

                                            countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transction);
                                        }
                                    }
                                }
                            }

                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("UpdateEmpReferenceInfo_SP"))
                                {
                                    foreach (EmpReferenceBO bo in referenceList)
                                    {
                                        if (bo.EmpId != null)
                                        {
                                            commandReference.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandReference, "@ReferenceId", DbType.Int32, bo.ReferenceId);
                                            dbSmartAspects.AddInParameter(commandReference, "@EmpId", DbType.Int32, tmpEmployeeId);
                                            dbSmartAspects.AddInParameter(commandReference, "@Name", DbType.String, bo.Name);
                                            dbSmartAspects.AddInParameter(commandReference, "@Organization", DbType.String, bo.Organization);
                                            dbSmartAspects.AddInParameter(commandReference, "@Designation", DbType.String, bo.Designation);
                                            dbSmartAspects.AddInParameter(commandReference, "@Address", DbType.String, bo.Address);
                                            dbSmartAspects.AddInParameter(commandReference, "@Mobile", DbType.String, bo.Mobile);
                                            dbSmartAspects.AddInParameter(commandReference, "@Email", DbType.String, bo.Email);
                                            dbSmartAspects.AddInParameter(commandReference, "@Relation", DbType.String, bo.Relationship);

                                            dbSmartAspects.AddInParameter(commandReference, "@Description", DbType.String, bo.Description);

                                            countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transction);
                                        }
                                    }
                                }
                            }

                            if (referenceList != null)
                            {
                                if (countReference == referenceList.Count)
                                {
                                    if (arrayReferenceDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayReferenceDelete)
                                        {
                                            using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandReference, "@TableName", DbType.String, "PayrollEmpReference");
                                                dbSmartAspects.AddInParameter(commandReference, "@TablePKField", DbType.String, "ReferenceId");
                                                dbSmartAspects.AddInParameter(commandReference, "@TablePKId", DbType.String, delId);

                                                status = dbSmartAspects.ExecuteNonQuery(commandReference);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retVal = false;
                                }
                            }

                            //---Emp Reference-----------------------------------------------------------------------------------------------------End

                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            //     UpdateEmployeeDocuments(employeeBO.EmpId, employeeBO.RandomEmpId);
            return retVal;
        }
        private void UpdateEmployeeImageAndSignatureInfo(EmployeeBO employeeBO)
        {
            List<DocumentsBO> list = new List<DocumentsBO>();
            DocumentsBO docSignature = employeeBO.Signature;
            DocumentsBO docImage = employeeBO.Image;
            list.Add(docSignature);
            list.Add(docImage);
            DocumentsDA docDA = new DocumentsDA();
            Boolean status = docDA.UpdateDocumentsInfo(list);
        }
        public List<EmployeeBO> GetEmployeeInfoBySearchCriteria(string searchText)
        {
            List<EmployeeBO> companyList = new List<EmployeeBO>();
            string query = "SELECT EmpId, DisplayName = EmpCode + ' - ' + DisplayName, EmpCode FROM PayrollEmployee WHERE (EmpCode + ' - ' + DisplayName) LIKE '%" + searchText + "%'";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "Supplier");
                    DataTable table = SupplierDS.Tables["Supplier"];

                    companyList = table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode")
                    }).ToList();
                }
            }
            return companyList;
        }
        public List<EmployeePaymentLedgerReportVwBo> GetEmployeeLedger(int companyId, int projectId, int empId, DateTime dateFrom, DateTime dateTo, string paymentStatus, string reportType)
        {
            List<EmployeePaymentLedgerReportVwBo> supplierInfo = new List<EmployeePaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePaymentLedgerReport_SP"))
                {
                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (paymentStatus != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, paymentStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerReportVwBo
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        Narration = r.Field<string>("Narration"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        BalanceCommulative = r.Field<decimal?>("BalanceCommulative"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public EmployeeBO GetEmployeeInfoById(int pkId)
        {
            EmployeeBO bo = new EmployeeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, pkId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpInfo");
                    DataTable Table = ds.Tables["EmpInfo"];

                    bo = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Title = r.Field<string>("Title"),
                        FirstName = r.Field<string>("FirstName"),
                        LastName = r.Field<string>("LastName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department"),
                        EmpTypeId = r.Field<int>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        TypeCategory = r.Field<string>("TypeCategory"),
                        DesignationId = r.Field<int>("DesignationId"),
                        GradeId = r.Field<int>("GradeId"),
                        GradeName = r.Field<string>("Grade"),
                        GratuityEligibilityDate = r.Field<DateTime?>("GratuityEligibilityDate"),
                        ProvisionPeriod = r.Field<DateTime?>("ProvisionPeriod"),
                        InitialContractEndDate = r.Field<DateTime?>("InitialContractEndDate"),
                        RepotingTo = r.Field<int>("RepotingTo"),
                        RepotingTo2 = r.Field<int>("RepotingTo2"),
                        EmpCompanyId = r.Field<int>("EmpCompanyId"),
                        GlCompanyId = r.Field<int>("GlCompanyId"),
                        GlProjectId = r.Field<int>("GlProjectId"),
                        Designation = r.Field<string>("Designation"),
                        OfficialEmail = r.Field<string>("OfficialEmail"),
                        ReferenceBy = r.Field<string>("ReferenceBy"),
                        Remarks = r.Field<string>("Remarks"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        EmpDateOfBirth = r.Field<DateTime?>("EmpDateOfBirth"),
                        EmpDateOfMarriage = r.Field<DateTime?>("MarriageDate"),
                        Gender = r.Field<string>("Gender"),
                        Religion = r.Field<string>("Religion"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        Height = r.Field<string>("Height"),
                        CountryId = r.Field<int>("CountryId"),
                        Nationality = r.Field<string>("Nationality"),
                        NationalId = r.Field<string>("NationalId"),
                        DivisionId = r.Field<int>("DivisionId"),
                        DistrictId = r.Field<int>("DistrictId"),
                        ThanaId = r.Field<int>("ThanaId"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        PIssuePlace = r.Field<string>("PIssuePlace"),
                        PIssueDate = r.Field<DateTime?>("PIssueDate"),
                        PExpireDate = r.Field<DateTime?>("PExpireDate"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PresentCountryId = r.Field<Int32>("PresentCountryId"),
                        PresentZipCode = r.Field<string>("PresentZipCode"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentZipCode = r.Field<string>("PermanentZipCode"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        PermanentCountryId = r.Field<Int32>("PermanentCountryId"),
                        PermanentPhone = r.Field<string>("PermanentPhone"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        AlternativeEmail = r.Field<string>("AlternativeEmail"),
                        WorkStationId = r.Field<int>("WorkStationId"),
                        DonorId = r.Field<int>("DonorId"),
                        ResignationDate = r.Field<DateTime?>("ResignationDate"),
                        EmergencyContactName = r.Field<string>("EmergencyContactName"),
                        EmergencyContactRelationship = r.Field<string>("EmergencyContactRelationship"),
                        EmergencyContactNumber = r.Field<string>("EmergencyContactNumber"),
                        EmergencyContactNumberHome = r.Field<string>("EmergencyContactNumberHome"),
                        EmergencyContactEmail = r.Field<string>("EmergencyContactEmail"),
                        ActivityCode = r.Field<string>("ActivityCode"),
                        IsApplicant = r.Field<Boolean>("IsApplicant"),
                        PFEligibilityDate = r.Field<DateTime?>("PFEligibilityDate"),
                        PFTerminateDate = r.Field<DateTime?>("PFTerminateDate"),
                        EmployeeStatusId = r.Field<int>("EmployeeStatusId"),
                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        NotEffectOnHead = r.Field<int>("NotEffectOnHead"),
                        Balance = r.Field<decimal>("Balance"),
                        IsContractualType = r.Field<Boolean>("IsContractualType"),
                        IsProvidentFundDeduct = r.Field<Boolean>("IsProvidentFundDeduct"),
                        TinNumber = r.Field<string>("TinNumber"),
                        AppoinmentLetter = r.Field<string>("AppoinmentLetter"),
                        JoiningAgreement = r.Field<string>("JoiningAgreement"),
                        ServiceBond = r.Field<string>("ServiceBond"),
                        DSOAC = r.Field<string>("DSOAC"),
                        ConfirmationLetter = r.Field<string>("ConfirmationLetter")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public List<EmployeeBO> GetEmployeeInfoForMonthlySalaryProcess(DateTime fromProcessDate, DateTime toProcessDate)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoForMonthlySalaryProcess_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromProcessDate", DbType.DateTime, fromProcessDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToProcessDate", DbType.DateTime, toProcessDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public EmployeeBO GetEmployeeInfoByCode(string pEmpCode)
        {
            EmployeeBO bo = new EmployeeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoByCode_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, pEmpCode);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = (Convert.ToDateTime(reader["JoinDate"]));
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                bo.GradeName = reader["GradeName"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.FathersName = reader["FathersName"].ToString();
                                bo.MothersName = reader["MothersName"].ToString();
                                if (!string.IsNullOrEmpty(reader["EmpDateOfBirth"].ToString()))
                                {
                                    bo.EmpDateOfBirth = Convert.ToDateTime(reader["EmpDateOfBirth"]);
                                }
                                bo.Gender = reader["Gender"].ToString();
                                bo.Religion = reader["Religion"].ToString();
                                bo.MaritalStatus = reader["MaritalStatus"].ToString();
                                bo.BloodGroup = reader["BloodGroup"].ToString();
                                bo.Height = reader["Height"].ToString();
                                bo.CountryId = Int32.Parse(reader["CountryId"].ToString());
                                bo.NationalId = reader["NationalId"].ToString();
                                bo.PassportNumber = reader["PassportNumber"].ToString();
                                bo.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    bo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"]);
                                }
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    bo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"]);
                                }
                                bo.PresentAddress = reader["PresentAddress"].ToString();
                                bo.PresentCity = reader["PresentCity"].ToString();
                                bo.PresentZipCode = reader["PresentZipCode"].ToString();
                                bo.PresentCountry = reader["PresentCountry"].ToString();
                                bo.PresentPhone = reader["PresentPhone"].ToString();
                                bo.PermanentAddress = reader["PermanentAddress"].ToString();
                                bo.PermanentCity = reader["PermanentCity"].ToString();
                                bo.PermanentZipCode = reader["PermanentZipCode"].ToString();
                                bo.PermanentCountry = reader["PermanentCountry"].ToString();
                                bo.PermanentPhone = reader["PermanentPhone"].ToString();
                                bo.PersonalEmail = reader["PersonalEmail"].ToString();
                                if (!string.IsNullOrEmpty(reader["WorkStationName"].ToString()))
                                {
                                    bo.WorkStationName = reader["WorkStationName"].ToString();
                                }

                                bo.EmergencyContactName = reader["EmergencyContactName"].ToString();
                                bo.EmergencyContactRelationship = reader["EmergencyContactRelationship"].ToString();
                                bo.EmergencyContactNumber = reader["EmergencyContactNumber"].ToString();
                                bo.EmergencyContactNumberHome = reader["EmergencyContactNumberHome"].ToString();
                                bo.EmergencyContactEmail = reader["EmergencyContactEmail"].ToString();
                                bo.CurrencyName = reader["CurrencyName"].ToString();
                                bo.EmployeeStatus = reader["EmployeeStatus"].ToString();
                                bo.RepotingTo = Convert.ToInt32(reader["ReportingToId"]);
                                bo.RepotingTo2 = Convert.ToInt32(reader["ReportingTo2Id"]);
                                bo.RepotingToOne = reader["RepotingToOne"].ToString();
                                bo.RepotingToTwo = reader["RepotingToTwo"].ToString();
                                bo.GlCompanyId = Convert.ToInt32(reader["GlCompanyId"]);
                                bo.GlProjectId = Convert.ToInt32(reader["GlProjectId"]);
                                bo.GLCompanyName = reader["GLCompanyName"].ToString();

                            }
                        }
                    }
                }
            }
            return bo;
        }
        public Boolean UpdateEmployeeDocuments(int EmpId, int RandomId)
        {
            Boolean status = false;
            int success = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteEmployeeDocument_SP"))
                {
                    dbSmartAspects.AddInParameter(commandEducation, "@EmpId", DbType.Int32, EmpId);
                    success = dbSmartAspects.ExecuteNonQuery(commandEducation);
                }

                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, EmpId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, RandomId);
                    success = dbSmartAspects.ExecuteNonQuery(commandDocument);
                }
            }
            return status;
        }
        public List<EmployeeBO> GetEmployeeInfoForReport(int empId)
        {
            List<EmployeeBO> employeeList = new List<EmployeeBO>();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInformationForReport_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    employeeList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       //JoinDate = r.Field<DateTime>("JoinDate"),
                                       JoinDateForReport = r.Field<string>("JoinDateForReport"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       EmpTypeId = r.Field<int>("EmpTypeId"),
                                       EmpType = r.Field<string>("EmpType"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       OfficialEmail = r.Field<string>("OfficialEmail"),
                                       BasicAmount = r.Field<decimal>("BasicAmount"),
                                       ReferenceBy = r.Field<string>("ReferenceBy"),
                                       //ResignationDate = r.Field<string>("ResignationDate"),
                                       Remarks = r.Field<string>("Remarks"),
                                       CreatedBy = r.Field<int>("CreatedBy"),
                                       //LastModifiedBy = r.Field<int>("LastModifiedBy"),
                                       FathersName = r.Field<string>("FathersName"),
                                       MothersName = r.Field<string>("MothersName"),
                                       //EmpDateOfBirth = r.Field<DateTime?>("EmpDateOfBirth"),
                                       DateOfBirthForReport = r.Field<string>("DateOfBirthForReport"),
                                       Gender = r.Field<string>("Gender"),
                                       BloodGroup = r.Field<string>("BloodGroup"),
                                       Religion = r.Field<string>("Religion"),
                                       Height = r.Field<string>("Height"),
                                       MaritalStatus = r.Field<string>("MaritalStatus"),
                                       CountryId = r.Field<int>("CountryId"),
                                       Nationality = r.Field<string>("Nationality"),
                                       NationalId = r.Field<string>("NationalId"),
                                       PassportNumber = r.Field<string>("PassportNumber"),
                                       PIssuePlace = r.Field<string>("PIssuePlace"),
                                       PIssueDate = r.Field<DateTime?>("PIssueDate"),
                                       ShowPIssueDate = r.Field<string>("ShowPIssueDate"),
                                       PExpireDate = r.Field<DateTime?>("PExpireDate"),
                                       ShowPExpireDate = r.Field<string>("ShowPExpireDate"),
                                       //CurrentLocationId = r.Field<int?>("CurrentLocationId"),
                                       CurrentLocation = r.Field<string>("CurrentLocation"),
                                       PresentAddress = r.Field<string>("PresentAddress"),
                                       PresentCity = r.Field<string>("PresentCity"),
                                       PresentZipCode = r.Field<string>("PresentZipCode"),
                                       PresentCountry = r.Field<string>("PresentCountry"),
                                       PresentPhone = r.Field<string>("PresentPhone"),
                                       PermanentAddress = r.Field<string>("PermanentAddress"),
                                       PermanentCity = r.Field<string>("PermanentCity"),
                                       PermanentZipCode = r.Field<string>("PermanentZipCode"),
                                       PermanentCountry = r.Field<string>("PermanentCountry"),
                                       PermanentPhone = r.Field<string>("PermanentPhone"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       AlternativeEmail = r.Field<string>("AlternativeEmail"),
                                       BankId = r.Field<int?>("BankId"),
                                       BankName = r.Field<string>("BankName"),
                                       BranchName = r.Field<string>("BranchName"),
                                       AccountType = r.Field<string>("AccountType"),
                                       AccountName = r.Field<string>("AccountName"),
                                       AccountNumber = r.Field<string>("AccountNumber"),
                                       BankRemarks = r.Field<string>("BankRemarks"),

                                       WorkStationId = r.Field<int?>("WorkStationId"),
                                       WorkStationName = r.Field<string>("WorkStationName"),
                                       EmergencyContactName = r.Field<string>("EmergencyContactName"),
                                       EmergencyContactRelationship = r.Field<string>("EmergencyContactRelationship"),
                                       EmergencyContactNumber = r.Field<string>("EmergencyContactNumber"),
                                       EmergencyContactNumberHome = r.Field<string>("EmergencyContactNumberHome"),
                                       EmergencyContactEmail = r.Field<string>("EmergencyContactEmail"),
                                       DonorId = r.Field<int?>("DonorId"),
                                       DonorName = r.Field<string>("DonorName"),
                                       ActivityCode = r.Field<string>("ActivityCode"),
                                       Grade = r.Field<string>("Grade"),
                                       ShowProvisionPeriod = r.Field<string>("ShowProvisionPeriod"),
                                       ShowContractEndDate = r.Field<string>("ShowContractEndDate"),
                                       ReportingTo = r.Field<string>("ReportingTo"),
                                       TinNumber = r.Field<string>("TinNumber"),
                                       ThanaName = r.Field<string>("ThanaName"),
                                       DivisionName = r.Field<string>("ThanaName"),
                                       DistrictName = r.Field<string>("DistrictName")

                                   }).ToList();
                }
            }

            return employeeList;
        }
        public EmployeeBO GetEmpInformationByEmpCodeNPwd(string empCode, string empPassword)
        {
            EmployeeBO bo = new EmployeeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpInformationByEmpCodeNPwd_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Empcode", DbType.String, empCode);
                    dbSmartAspects.AddInParameter(cmd, "@EmpPassword", DbType.String, empPassword);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.EmpPassword = reader["EmpPassword"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();

                            }
                        }
                    }
                }
            }
            return bo;
        }
        public Boolean ChangeEmpPassword(EmployeeBO employeeBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ChangeEmpPassword_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, employeeBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@EmpPassword", DbType.String, employeeBO.EmpPassword);


                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<EmpListForReportViewBO> GetEmployeeInfoListForReport(int employeeStatusId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoListForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeStatusId", DbType.Int32, employeeStatusId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        DisplayName = r.Field<string>("FullName"),
                        FullName = r.Field<string>("FullName"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        Gender = r.Field<string>("Gender"),
                        Height = r.Field<string>("Height"),
                        EmpDateOfBirth = r.Field<string>("EmpDateOfBirth"),
                        CountryId = r.Field<int>("CountryId"),
                        NationalId = r.Field<string>("NationalId"),
                        Religion = r.Field<string>("Religion"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        JoinDateString = r.Field<string>("JoinDateString"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpListForReport(int reportType, int empId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInformationListForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.Int32, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        DisplayName = r.Field<string>("FullName"),
                        FullName = r.Field<string>("FullName"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        Gender = r.Field<string>("Gender"),
                        Height = r.Field<string>("Height"),
                        EmpDateOfBirth = r.Field<string>("EmpDateOfBirth"),
                        CountryId = r.Field<int>("CountryId"),
                        NationalId = r.Field<string>("NationalId"),
                        Religion = r.Field<string>("Religion"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        JoinDateString = r.Field<string>("JoinDateString"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpPromotionLetterForReport(int promotionId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpPromotionLetterForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PromotionId", DbType.Int32, promotionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        PromotionDateDisplay = r.Field<string>("PromotionDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        PreviousDesignation = r.Field<string>("PreviousDesignation"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        PreviousGrade = r.Field<string>("PreviousGrade"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        Department = r.Field<string>("Department"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        ApprovalStatus = r.Field<string>("ApprovalStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpIncrementLetterForReport(int IncrementId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpIncrementLetterForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IncrementId", DbType.Int32, IncrementId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        IncrementDateDisplay = r.Field<string>("IncrementDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        Department = r.Field<string>("Department"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        BasicSalary = r.Field<decimal>("BasicSalary"),
                        IncrementMode = r.Field<string>("IncrementMode"),
                        EffectiveDateDisplay = r.Field<string>("EffectiveDateDisplay"),
                        TransactionAmount = r.Field<decimal>("TransactionAmount"),
                        IncrementAmount = r.Field<decimal>("IncrementAmount"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpAppoinmentLetterForReport(int empId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAppoinmentLetterForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        PromotionDateDisplay = r.Field<string>("PromotionDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        Department = r.Field<string>("Department"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        SalaryProcessType = r.Field<string>("SalaryProcessType"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        LetterBody = r.Field<string>("LetterBody"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpConfirmationLetterForReport(int empId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpConfirmationLetterForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        PromotionDateDisplay = r.Field<string>("PromotionDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        Department = r.Field<string>("Department"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        SalaryProcessType = r.Field<string>("SalaryProcessType"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        LetterBody = r.Field<string>("LetterBody"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpJoiningAgreementForReport(int empId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpJoiningAgreementForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        PromotionDateDisplay = r.Field<string>("PromotionDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        Department = r.Field<string>("Department"),
                        CompanyName = r.Field<string>("CompanyName"),
                        LetterBody = r.Field<string>("LetterBody"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpServiceBondLetterForReport(int empId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpServiceBondLetterForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        PromotionDateDisplay = r.Field<string>("PromotionDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FathersName = r.Field<string>("FathersName"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        Department = r.Field<string>("Department"),
                        CompanyName = r.Field<string>("CompanyName"),
                        LetterBody = r.Field<string>("LetterBody"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmpDSOACLetterForReport(int empId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDSOACLetterForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        PromotionDateDisplay = r.Field<string>("PromotionDateDisplay"),
                        ReferenceName = r.Field<string>("ReferenceName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        GLCompanyId = r.Field<Int32>("GLCompanyId"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        Department = r.Field<string>("Department"),
                        CompanyName = r.Field<string>("CompanyName"),
                        LetterBody = r.Field<string>("LetterBody"),
                        BestWishes = r.Field<string>("BestWishes"),
                        BestRegards = r.Field<string>("BestRegards"),
                        LetterNote = r.Field<string>("LetterNote"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        CreatedBy = r.Field<string>("CreatedBy"),
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmpListForReportViewBO> GetEmployeeList(int employeeStatusId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeStatusId", DbType.Int32, employeeStatusId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        NationalIdOrOtherCertificate = r.Field<string>("NationalIdOrOtherCertificate")

                    }).ToList();
                }
            }

            return empList;
        }

        //public List<EmpPayslipReportViewBO> GetEmpPayslipForReport(int empId, DateTime processDateFrom, DateTime processDateTo, int departmentId, int gradeId)
        //{
        //    List<EmpPayslipReportViewBO> empPayslip = new List<EmpPayslipReportViewBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePayslip_SP"))
        //        {
        //            if (empId != 0)
        //                dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
        //            else
        //                dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

        //            if (departmentId != 0)
        //                dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
        //            else
        //                dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

        //            if (gradeId != 0)
        //                dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
        //            else
        //                dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

        //            dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
        //            dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);

        //            DataSet ds = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, ds, "EmployeePayslip");
        //            DataTable Table = ds.Tables["EmployeePayslip"];

        //            empPayslip = Table.AsEnumerable().Select(r => new EmpPayslipReportViewBO
        //            {
        //                ProcessDate = r.Field<string>("ProcessDate"),
        //                EmpId = r.Field<int>("EmpId"),
        //                EmpCode = r.Field<string>("EmpCode"),
        //                DisplayName = r.Field<string>("DisplayName"),
        //                JoinDate = r.Field<string>("JoinDate"),
        //                EmpTypeId = r.Field<int?>("EmpTypeId"),
        //                EmpType = r.Field<string>("EmpType"),
        //                Designation = r.Field<string>("Designation"),
        //                Department = r.Field<string>("Department"),
        //                WorkStation = r.Field<string>("WorkStation"),
        //                TransactionType = r.Field<string>("TransactionType"),
        //                SalaryType = r.Field<string>("SalaryType"),
        //                SalaryHeadId = r.Field<int?>("SalaryHeadId"),
        //                SalaryHeadNote = r.Field<string>("SalaryHeadNote"),
        //                SalaryCategory = r.Field<string>("SalaryCategory"),
        //                SalaryHead = r.Field<string>("SalaryHead"),
        //                SalaryHeadAmount = r.Field<decimal?>("SalaryHeadAmount"),
        //                IsBonusPaid = r.Field<bool?>("IsBonusPaid"),
        //                BasicSalary = r.Field<decimal?>("BasicSalary"),
        //                GrossSalary = r.Field<decimal?>("GrossSalary"),
        //                DateFrom = r.Field<string>("DateFrom"),
        //                DateTo = r.Field<string>("DateTo"),
        //                PaidDays = r.Field<int?>("PaidDays"),
        //                Project = r.Field<string>("Project"),
        //                ResignationDate = r.Field<string>("ResignationDate")

        //            }).ToList();
        //        }
        //    }

        //    return empPayslip;
        //}

        public List<EmployeePayslipBO> GetEmployeePayslip(int glCompanyId, int empId, DateTime processDateFrom, DateTime processDateTo, short processYear, int departmentId, int gradeId, int workStationId, string currencyType)
        {
            List<EmployeePayslipBO> empPayslip = new List<EmployeePayslipBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeePayslip_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (workStationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeePayslip");
                    DataTable Table = ds.Tables["EmployeePayslip"];

                    empPayslip = Table.AsEnumerable().Select(r => new EmployeePayslipBO
                    {

                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),
                        ContractEndDate = r.Field<DateTime?>("ContractEndDate"),
                        ShowContractEndDate = r.Field<string>("ShowContractEndDate"),
                        EmpTypeId = r.Field<int?>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        WorkStation = r.Field<string>("WorkStation"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime?>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime?>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        //DonorId = r.Field<int?>("DonorId"),
                        //Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal?>("BasicAmount"),
                        SalaryHeadId = r.Field<int?>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        SalaryAmount = r.Field<decimal?>("SalaryAmount"),
                        TotalAllowance = r.Field<decimal?>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal?>("TotalDeduction"),
                        GrossAmount = r.Field<decimal?>("GrossAmount"),
                        HomeTakenAmount = r.Field<decimal?>("HomeTakenAmount"),
                        EmployeeRank = r.Field<int>("EmployeeRank"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),

                        BankId = r.Field<int?>("BankId"),
                        BankName = r.Field<string>("BankName"),
                        AccountName = r.Field<string>("AccountName"),
                        AccountNumber = r.Field<string>("AccountNumber"),
                        AccountType = r.Field<string>("AccountType"),
                        BranchName = r.Field<string>("BranchName"),

                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        BasicAmountInEmployeeCurrency = r.Field<decimal>("BasicAmountInEmployeeCurrency")

                    }).ToList();
                }
            }

            return empPayslip;
        }

        public List<EmployeePayslipBO> GetEmployeePayslipForRedcross(int glCompanyId, int empId, DateTime processDateFrom, DateTime processDateTo, short processYear, int departmentId, int gradeId, int workStationId)
        {
            List<EmployeePayslipBO> empPayslip = new List<EmployeePayslipBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeePayslipForRedcross_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (workStationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeePayslip");
                    DataTable Table = ds.Tables["EmployeePayslip"];

                    empPayslip = Table.AsEnumerable().Select(r => new EmployeePayslipBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),
                        ContractEndDate = r.Field<DateTime?>("ContractEndDate"),
                        ShowContractEndDate = r.Field<string>("ShowContractEndDate"),
                        EmpTypeId = r.Field<int?>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        WorkStation = r.Field<string>("WorkStation"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime?>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime?>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        //DonorId = r.Field<int?>("DonorId"),
                        //Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal?>("BasicAmount"),
                        SalaryHeadId = r.Field<int?>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryHeadNote = r.Field<string>("SalaryHeadNote"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        SalaryAmount = r.Field<decimal?>("SalaryAmount"),
                        TotalAllowance = r.Field<decimal?>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal?>("TotalDeduction"),
                        GrossAmount = r.Field<decimal?>("GrossAmount"),
                        HomeTakenAmount = r.Field<decimal?>("HomeTakenAmount"),
                        MedicalAllowance = r.Field<decimal?>("MedicalAllowance"),
                        EmployeeRank = r.Field<int>("EmployeeRank"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),

                        BankId = r.Field<int?>("BankId"),
                        BankName = r.Field<string>("BankName"),
                        AccountNumber = r.Field<string>("AccountNumber")

                    }).ToList();
                }
            }

            return empPayslip;
        }
        public List<EmployeePayslipBO> GetEmployeePayslipForSouthSudan(int glCompanyId, int empId, DateTime processDateFrom, DateTime processDateTo, short processYear, int departmentId, int gradeId, int workStationId)
        {
            List<EmployeePayslipBO> empPayslip = new List<EmployeePayslipBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeePayslipForRedcross_SP"))
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeePayslipForSouthSudan_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (workStationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeePayslip");
                    DataTable Table = ds.Tables["EmployeePayslip"];

                    empPayslip = Table.AsEnumerable().Select(r => new EmployeePayslipBO
                    {
                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),
                        ContractEndDate = r.Field<DateTime?>("ContractEndDate"),
                        ShowContractEndDate = r.Field<string>("ShowContractEndDate"),
                        EmpTypeId = r.Field<int?>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        WorkStation = r.Field<string>("WorkStation"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime?>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime?>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        //DonorId = r.Field<int?>("DonorId"),
                        //Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal?>("BasicAmount"),
                        SalaryHeadId = r.Field<int?>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryHeadNote = r.Field<string>("SalaryHeadNote"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        SalaryAmount = r.Field<decimal?>("SalaryAmount"),
                        TotalAllowance = r.Field<decimal?>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal?>("TotalDeduction"),
                        GrossAmount = r.Field<decimal?>("GrossAmount"),
                        HomeTakenAmount = r.Field<decimal?>("HomeTakenAmount"),
                        MedicalAllowance = r.Field<decimal?>("MedicalAllowance"),
                        NSSFEmployeeContribution = r.Field<decimal?>("NSSFEmployeeContribution"),
                        NSSFCompanyContribution = r.Field<decimal?>("NSSFCompanyContribution"),                        
                        CurrencyExchangeRate = r.Field<decimal?>("CurrencyExchangeRate"),
                        EmployeeRank = r.Field<int>("EmployeeRank"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),
                        CompanyContributionLabelTitle = r.Field<string>("CompanyContributionLabelTitle"),
                        BankId = r.Field<int?>("BankId"),
                        BankName = r.Field<string>("BankName"),
                        AccountNumber = r.Field<string>("AccountNumber")

                    }).ToList();
                }
            }

            return empPayslip;
        }

        public List<EmployeePayslipBO> GetEmployeePayslipForIPTech(int glCompanyId, int empId, DateTime processDateFrom, DateTime processDateTo, short processYear, int departmentId, int gradeId, int workStationId, string currencyType)
        {
            List<EmployeePayslipBO> empPayslip = new List<EmployeePayslipBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeePayslipForIPTech_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (gradeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, gradeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Grade", DbType.Int32, DBNull.Value);

                    if (workStationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeePayslip");
                    DataTable Table = ds.Tables["EmployeePayslip"];

                    empPayslip = Table.AsEnumerable().Select(r => new EmployeePayslipBO
                    {

                        ProcessId = r.Field<int>("ProcessId"),
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ShowJoiningDate = r.Field<string>("ShowJoiningDate"),
                        ContractEndDate = r.Field<DateTime?>("ContractEndDate"),
                        //EmpTypeId = r.Field<int?>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        Location = r.Field<string>("Location"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        SalaryDateFrom = r.Field<DateTime?>("SalaryDateFrom"),
                        SalaryDateTo = r.Field<DateTime?>("SalaryDateTo"),
                        PaidDays = r.Field<int?>("PaidDays"),
                        //DonorId = r.Field<int?>("DonorId"),
                        //Project = r.Field<string>("Project"),
                        BasicAmount = r.Field<decimal?>("BasicAmount"),
                        SalaryHeadId = r.Field<int?>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        SalaryEffectiveness = r.Field<string>("SalaryEffectiveness"),
                        SalaryAmount = r.Field<decimal?>("SalaryAmount"),
                        TotalAllowance = r.Field<decimal?>("TotalAllowance"),
                        TotalDeduction = r.Field<decimal?>("TotalDeduction"),
                        GrossAmount = r.Field<decimal?>("GrossAmount"),
                        HomeTakenAmount = r.Field<decimal?>("HomeTakenAmount"),
                        //EmployeeRank = r.Field<int>("EmployeeRank"),
                        SalaryTypeRank = r.Field<int>("SalaryTypeRank"),

                        //BankId = r.Field<int?>("BankId"),
                        //BankName = r.Field<string>("BankName"),
                        //AccountNumber = r.Field<string>("AccountNumber"),

                        PayrollCurrencyId = r.Field<int>("PayrollCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        BasicAmountInEmployeeCurrency = r.Field<decimal>("BasicAmountInEmployeeCurrency")

                    }).ToList();
                }
            }

            return empPayslip;
        }

        public EmpBankInfoBO GetEmployeeBankInfo(int empId)
        {
            EmpBankInfoBO bankInfo = new EmpBankInfoBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBankInfo_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BankInfo");
                    DataTable Table = ds.Tables["BankInfo"];

                    bankInfo = Table.AsEnumerable().Select(r => new EmpBankInfoBO
                    {
                        BankInfoId = r.Field<int>("BankInfoId"),
                        EmpId = r.Field<int>("EmpId"),
                        BankId = r.Field<int>("BankId"),
                        BranchName = r.Field<string>("BranchName"),
                        AccountName = r.Field<string>("AccountName"),
                        AccountNumber = r.Field<string>("AccountNumber"),
                        AccountType = r.Field<string>("AccountType"),
                        CardNumber = r.Field<string>("CardNumber"),
                        BankRemarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }

            return bankInfo;
        }
        public List<EmpWorkStationBO> GetEmployWorkStation()
        {
            List<EmpWorkStationBO> warkStation = new List<EmpWorkStationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeWorkStation_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "WorkStation");
                    DataTable Table = ds.Tables["WorkStation"];

                    warkStation = Table.AsEnumerable().Select(r => new EmpWorkStationBO
                    {
                        WorkStationId = r.Field<int>("WorkStationId"),
                        WorkStationName = r.Field<string>("WorkStationName")

                    }).ToList();
                }
            }

            return warkStation;
        }
        public List<PayrollDonorBO> GetDonor()
        {
            List<PayrollDonorBO> donor = new List<PayrollDonorBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollDonor_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Donor");
                    DataTable Table = ds.Tables["Donor"];

                    donor = Table.AsEnumerable().Select(r => new PayrollDonorBO
                    {
                        DonorId = r.Field<int>("DonorId"),
                        DonorCode = r.Field<string>("DonorCode"),
                        DonorName = r.Field<string>("DonorName")

                    }).ToList();
                }
            }

            return donor;
        }
        public List<EmpEducationLevelBO> GetEducationLevel()
        {
            List<EmpEducationLevelBO> boList = new List<EmpEducationLevelBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEducationlevel_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EducationLevel");
                    DataTable Table = ds.Tables["EducationLevel"];

                    boList = Table.AsEnumerable().Select(r => new EmpEducationLevelBO
                    {
                        LevelId = r.Field<int>("LevelId"),
                        LevelName = r.Field<string>("LevelName")

                    }).ToList();
                }
            }

            return boList;
        }
        public List<EmployeeLastSalaryPayBO> GetLastMonthSalaryEmployee(int salaryYear, DateTime salaryDateFrom, DateTime salaryDateTo)
        {
            List<EmployeeLastSalaryPayBO> employeeList = new List<EmployeeLastSalaryPayBO>();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLastMonthSalaryEmployee_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, salaryDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int16, salaryYear);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    employeeList = table.AsEnumerable().Select(r =>
                                   new EmployeeLastSalaryPayBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       EmpType = r.Field<string>("EmpType"),
                                       Location = r.Field<string>("Location"),
                                       Position = r.Field<string>("Position"),
                                       Project = r.Field<string>("Project")

                                   }).ToList();
                }
            }

            return employeeList;
        }
        public PayrollEmpLastMonthBenifitsPaymentBO GetLastMonthSalaryEmployeeBenifits(int empId, int salaryYear, DateTime salaryDateFrom, DateTime salaryDateTo)
        {
            PayrollEmpLastMonthBenifitsPaymentBO empBenefits = new PayrollEmpLastMonthBenifitsPaymentBO();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLastMonthSalaryEmployeeBenifits_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int16, salaryYear);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateFrom", DbType.DateTime, salaryDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryDateTo", DbType.DateTime, salaryDateTo);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empBenefits = table.AsEnumerable().Select(r =>
                                   new PayrollEmpLastMonthBenifitsPaymentBO
                                   {
                                       BenifitId = r.Field<Int32>("BenifitId"),
                                       AfterServiceBenefit = r.Field<decimal>("AfterServiceBenefit"),
                                       EmployeePFContribution = r.Field<decimal>("EmployeePFContribution"),
                                       CompanyPFContribution = r.Field<decimal>("CompanyPFContribution"),
                                       LeaveBalanceDays = r.Field<decimal>("LeaveBalanceDays"),
                                       LeaveBalanceAmount = r.Field<decimal>("LeaveBalanceAmount")

                                   }).FirstOrDefault();
                }
            }

            return empBenefits;
        }
        public LastMonthSalaryEmployeeBenifitsBO GetLeaveBalanceAmount(int empId, DateTime processDateFrom, DateTime processDateTo, int leaveDay)
        {
            LastMonthSalaryEmployeeBenifitsBO empBenefits = new LastMonthSalaryEmployeeBenifitsBO();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveBalanceAmount_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, processDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@LeaveDay", DbType.Int32, leaveDay);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empBenefits = table.AsEnumerable().Select(r =>
                                   new LastMonthSalaryEmployeeBenifitsBO
                                   {
                                       LeaveBalanceAmount = r.Field<decimal>("LeaveBalanceAmount")

                                   }).FirstOrDefault();
                }
            }

            return empBenefits;
        }
        public bool SavePayrollEmpLastMonthBenifitsPayment(PayrollEmpLastMonthBenifitsPaymentBO benefitBo, out int tmpEmpBenefitId)
        {
            int status = 0;

            tmpEmpBenefitId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveEmpLastMonthBenifitsPayment_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, benefitBo.EmpId);
                    dbSmartAspects.AddInParameter(commandMaster, "@AfterServiceBenefit", DbType.Decimal, benefitBo.AfterServiceBenefit);
                    dbSmartAspects.AddInParameter(commandMaster, "@EmployeePFContribution", DbType.Decimal, benefitBo.EmployeePFContribution);
                    dbSmartAspects.AddInParameter(commandMaster, "@CompanyPFContribution", DbType.Decimal, benefitBo.CompanyPFContribution);
                    dbSmartAspects.AddInParameter(commandMaster, "@LeaveBalanceDays", DbType.Decimal, benefitBo.LeaveBalanceDays);
                    dbSmartAspects.AddInParameter(commandMaster, "@LeaveBalanceAmount", DbType.Decimal, benefitBo.LeaveBalanceAmount);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessYear", DbType.Int16, benefitBo.ProcessYear);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessDateFrom", DbType.DateTime, benefitBo.ProcessDateFrom);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessDateTo", DbType.DateTime, benefitBo.ProcessDateTo);
                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, benefitBo.CreatedBy);

                    dbSmartAspects.AddOutParameter(commandMaster, "@BenifitId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                    tmpEmpBenefitId = Convert.ToInt32(commandMaster.Parameters["@BenifitId"].Value);
                }
            }

            return (status == 1 ? true : false);
        }
        public bool UpdatePayrollEmpLastMonthBenifitsPayment(PayrollEmpLastMonthBenifitsPaymentBO benefitBo)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateEmpLastMonthBenifitsPayment_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@BenifitId", DbType.Int32, benefitBo.BenifitId);
                    dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, benefitBo.EmpId);
                    dbSmartAspects.AddInParameter(commandMaster, "@AfterServiceBenefit", DbType.Decimal, benefitBo.AfterServiceBenefit);
                    dbSmartAspects.AddInParameter(commandMaster, "@EmployeePFContribution", DbType.Decimal, benefitBo.EmployeePFContribution);
                    dbSmartAspects.AddInParameter(commandMaster, "@CompanyPFContribution", DbType.Decimal, benefitBo.CompanyPFContribution);
                    dbSmartAspects.AddInParameter(commandMaster, "@LeaveBalanceDays", DbType.Decimal, benefitBo.LeaveBalanceDays);
                    dbSmartAspects.AddInParameter(commandMaster, "@LeaveBalanceAmount", DbType.Decimal, benefitBo.LeaveBalanceAmount);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessYear", DbType.Int16, benefitBo.ProcessYear);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessDateFrom", DbType.DateTime, benefitBo.ProcessDateFrom);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProcessDateTo", DbType.DateTime, benefitBo.ProcessDateTo);
                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, benefitBo.CreatedBy);

                    status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                }
            }

            return (status == 1 ? true : false);
        }
        public EmployeeBO GetEmployeeInfoForResignationInfoById(int empId)
        {
            EmployeeBO bo = new EmployeeBO();
            bo = null;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoForResignationInfoByCode_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                            }
                        }
                    }
                }
            }
            if (bo != null)
                return bo;
            else
                return null;
        }
        public Boolean UpdateEmpResignInfo(int empId, DateTime date)
        {
            Boolean status = false;
            int success = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpResignInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(command, "@ResignDate", DbType.DateTime, date);
                    success = dbSmartAspects.ExecuteNonQuery(command);
                }
            }
            if (success > 0)
            {
                status = true;
            }
            return status;
        }
        public List<EmployeeBO> GetEmpInformationBySearchCriteriaForPaging(int companyId, int projectId, string empName, string code, string department, string designation, bool isApplicant, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpInfoBySearchCriteriaForPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        if (companyId > 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                        }
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(empName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, empName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, code);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(department))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Department", DbType.String, department);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Department", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(designation))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Designation", DbType.String, designation);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Designation", DbType.String, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@IsApplicant", DbType.Boolean, isApplicant);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       JoinDate = r.Field<DateTime>("JoinDate"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       EmpTypeId = r.Field<int>("EmpTypeId"),
                                       EmpType = r.Field<string>("EmpType"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       OfficialEmail = r.Field<string>("OfficialEmail"),
                                       ReferenceBy = r.Field<string>("ReferenceBy"),
                                       SerialNumber = r.Field<Int64>("SerialNumber"),
                                       Remarks = r.Field<string>("Remarks"),
                                       AppoinmentLetter = r.Field<string>("AppoinmentLetter"),
                                       JoiningAgreement = r.Field<string>("JoiningAgreement"),
                                       ServiceBond = r.Field<string>("ServiceBond"),
                                       DSOAC = r.Field<string>("DSOAC"),
                                       ConfirmationLetter = r.Field<string>("ConfirmationLetter")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            if (empList.Count > 0)
            {
                empList[0].TotalEmployeeNumber = totalRecords;
            }
            return empList;
        }
        public List<EmployeeBO> GetEmpInformationBySearchCriteria(string text, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpInfoBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Keyword", DbType.String, text);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Keyword", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       Name = r.Field<string>("Name"),
                                       Path = r.Field<string>("Path"),
                                       Extention = r.Field<string>("Extention"),

                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       JoinDate = r.Field<DateTime>("JoinDate"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       EmpTypeId = r.Field<int>("EmpTypeId"),
                                       EmpType = r.Field<string>("EmpType"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       OfficialEmail = r.Field<string>("OfficialEmail"),
                                       ReferenceBy = r.Field<string>("ReferenceBy"),
                                       PermanentPhone = r.Field<string>("PermanentPhone"),
                                       PresentPhone = r.Field<string>("PresentPhone"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       PresentAddress = r.Field<string>("PresentAddress"),
                                       EmergencyContactNumber = r.Field<string>("EmergencyContactNumber"),
                                       EmergencyContactNumberHome = r.Field<string>("EmergencyContactNumberHome"),
                                       EmployeeStatus = r.Field<string>("EmployeeStatus"),
                                       Remarks = r.Field<string>("Remarks")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            if (empList.Count > 0)
            {
                empList[0].TotalEmployeeNumber = totalRecords;
            }
            return empList;
        }

        public List<EmployeeBO> GetUpcomingEmployeeBirthday()
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUpcomingEmployeeBirthday"))
                {

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       JoinDate = r.Field<DateTime>("JoinDate"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       EmpDateOfBirth = r.Field<DateTime>("EmpDateOfBirth")
                                   }).ToList();

                }
            }
            return empList;
        }
        public List<EmployeeBO> GetUpcomingEmployeeWorkAnniversary()
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUpcomingEmployeeWorkAnniversary_SP"))
                {

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       JoinDate = r.Field<DateTime>("JoinDate"),
                                       JoinDateDisplay = r.Field<string>("JoinDateDisplay"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       WorkAnniversary = r.Field<int>("WorkAnniversary")
                                   }).ToList();

                }
            }
            return empList;
        }
        public List<EmployeeBO> GetUpcomingEmployeeProvisionPeriod()
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUpcomingEmployeeProvisionPeriod_SP"))
                {

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       JoinDate = r.Field<DateTime?>("JoinDate"),
                                       ProvisionPeriod = r.Field<DateTime?>("ProvisionPeriod"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       EmpDateOfBirth = r.Field<DateTime?>("EmpDateOfBirth")
                                   }).ToList();

                }
            }
            return empList;
        }
        public List<EmployeeBO> GetCompanyEmployeeCount()
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyEmployeeCount_SP"))
                {

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       CompanyId = r.Field<int>("CompanyId"),
                                       EmployeeCount = r.Field<int>("EmployeeCount"),
                                       CompanyName = r.Field<string>("CompanyName")
                                   }).ToList();

                }
            }
            return empList;
        }

        public List<EmployeeBO> GetHolidayInformation(DateTime fromDate, DateTime toDate)
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHolidayInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "HolidayInformation");
                    DataTable table = employeeDS.Tables["HolidayInformation"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       HolidayName = r.Field<string>("HolidayName"),
                                       StartDate = r.Field<DateTime>("StartDate"),
                                       EndDate = r.Field<DateTime>("EndDate"),
                                       Description = r.Field<string>("Description")
                                   }).ToList();

                }
            }
            return empList;
        }

        // Employee Transfer
        public bool SaveEmpTransfer(EmpTransferBO transferbo, out int transferId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveEmpTransfer_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@TransferDate", DbType.DateTime, transferbo.TransferDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int64, transferbo.EmpId);
                    dbSmartAspects.AddInParameter(commandMaster, "@PreviousDepartmentId", DbType.Int32, transferbo.PreviousDepartmentId);
                    dbSmartAspects.AddInParameter(commandMaster, "@CurrentDepartmentId", DbType.Int32, transferbo.CurrentDepartmentId);
                    dbSmartAspects.AddInParameter(commandMaster, "@PreviousDesignationId", DbType.Int32, transferbo.PreviousDesignationId);
                    dbSmartAspects.AddInParameter(commandMaster, "@CurrentDesignationId", DbType.Int32, transferbo.CurrentDesignationId);

                    if (transferbo.PreviousLocation != null)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousLocation", DbType.Int32, transferbo.PreviousLocation);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousLocation", DbType.Int32, DBNull.Value);

                    if (transferbo.CurrentLocation != null)
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocation", DbType.Int32, transferbo.CurrentLocation);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocation", DbType.Int32, DBNull.Value);
                    if (transferbo.PreviousCompanyId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousCompanyId", DbType.Int32, transferbo.PreviousCompanyId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousCompanyId", DbType.Int32, DBNull.Value);
                    if (transferbo.PreviousReportingToId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingToId", DbType.Int32, transferbo.PreviousReportingToId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingToId", DbType.Int32, DBNull.Value);
                    if (transferbo.PreviousReportingTo2Id != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingTo2Id", DbType.Int32, transferbo.PreviousReportingTo2Id);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingTo2Id", DbType.Int32, DBNull.Value);
                    if (transferbo.CurrentCompanyId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentCompanyId", DbType.Int32, transferbo.CurrentCompanyId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentCompanyId", DbType.Int32, DBNull.Value);
                    if (transferbo.PreviousProjectId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousProjectId", DbType.Int32, transferbo.CurrentProjectId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousProjectId", DbType.Int32, DBNull.Value);
                    if (transferbo.CurrentProjectId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentProjectId", DbType.Int32, transferbo.CurrentProjectId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentProjectId", DbType.Int32, DBNull.Value);
                    if (!string.IsNullOrEmpty(transferbo.Description))
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, transferbo.Description);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(commandMaster, "@ReportingDate", DbType.DateTime, transferbo.ReportingDate);
                    //dbSmartAspects.AddInParameter(commandMaster, "@JoinedDate", DbType.DateTime, transferbo.JoinedDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@ReportingToId", DbType.Int32, transferbo.ReportingToId);
                    dbSmartAspects.AddInParameter(commandMaster, "@ReportingTo2Id", DbType.Int64, transferbo.ReportingTo2Id);
                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, transferbo.CreatedBy);
                    dbSmartAspects.AddOutParameter(commandMaster, "@TransferId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(commandMaster) > 0 ? true : false;

                    transferId = Convert.ToInt32(commandMaster.Parameters["@TransferId"].Value);
                }
            }

            return status;
        }
        public bool UpdateEmpTransfer(EmpTransferBO transferbo)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateEmpTransfer_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@TransferId", DbType.Int32, transferbo.TransferId);
                    dbSmartAspects.AddInParameter(commandMaster, "@TransferDate", DbType.DateTime, transferbo.TransferDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int64, transferbo.EmpId);
                    dbSmartAspects.AddInParameter(commandMaster, "@PreviousDepartmentId", DbType.Int32, transferbo.PreviousDepartmentId);
                    dbSmartAspects.AddInParameter(commandMaster, "@CurrentDepartmentId", DbType.Int32, transferbo.CurrentDepartmentId);
                    dbSmartAspects.AddInParameter(commandMaster, "@PreviousDesignationId", DbType.Int32, transferbo.PreviousDesignationId);
                    dbSmartAspects.AddInParameter(commandMaster, "@CurrentDesignationId", DbType.Int32, transferbo.CurrentDesignationId);

                    if (transferbo.PreviousLocation != null)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousLocation", DbType.Int32, transferbo.PreviousLocation);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousLocation", DbType.Int32, DBNull.Value);

                    if (transferbo.CurrentLocation != null)
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocation", DbType.Int32, transferbo.CurrentLocation);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentLocation", DbType.Int32, DBNull.Value);

                    if (transferbo.PreviousCompanyId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousCompanyId", DbType.Int32, transferbo.PreviousCompanyId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousCompanyId", DbType.Int32, DBNull.Value);

                    if (transferbo.PreviousProjectId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousProjectId", DbType.Int32, transferbo.PreviousProjectId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousProjectId", DbType.Int32, DBNull.Value);

                    if (transferbo.PreviousReportingToId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingToId", DbType.Int32, transferbo.PreviousReportingToId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingToId", DbType.Int32, DBNull.Value);

                    if (transferbo.PreviousReportingTo2Id != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingTo2Id", DbType.Int32, transferbo.PreviousReportingTo2Id);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@PreviousReportingTo2Id", DbType.Int32, DBNull.Value);

                    if (transferbo.CurrentCompanyId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentCompanyId", DbType.Int32, transferbo.CurrentCompanyId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentCompanyId", DbType.Int32, DBNull.Value);

                    if (transferbo.CurrentProjectId != 0)
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentProjectId", DbType.Int32, transferbo.CurrentProjectId);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@CurrentProjectId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(transferbo.Description))
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, transferbo.Description);
                    else
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, DBNull.Value);
                    dbSmartAspects.AddInParameter(commandMaster, "@ReportingDate", DbType.DateTime, transferbo.ReportingDate);
                    //dbSmartAspects.AddInParameter(commandMaster, "@JoinedDate", DbType.DateTime, transferbo.JoinedDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@ReportingToId", DbType.Int32, transferbo.ReportingToId);
                    dbSmartAspects.AddInParameter(commandMaster, "@ReportingTo2Id", DbType.Int64, transferbo.ReportingTo2Id);
                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, transferbo.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                }
            }

            return (status == 1 ? true : false);
        }
        public List<EmpTransferBO> GetEmpTransfer(DateTime dateFrom, DateTime dateTo, int type, int empId)
        {
            List<EmpTransferBO> employeeList = new List<EmpTransferBO>();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTransfer_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.Int32, type);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "Transfer");
                    DataTable table = employeeDS.Tables["Transfer"];

                    employeeList = table.AsEnumerable().Select(r =>
                                   new EmpTransferBO
                                   {
                                       TransferId = r.Field<Int64>("TransferId"),
                                       TransferDate = r.Field<DateTime>("TransferDate"),
                                       EmpId = r.Field<Int64>("EmpId"),
                                       PreviousDepartmentId = r.Field<int>("PreviousDepartmentId"),
                                       CurrentDepartmentId = r.Field<int>("CurrentDepartmentId"),
                                       PreviousLocation = r.Field<int?>("PreviousLocation"),
                                       CurrentLocation = r.Field<int?>("CurrentLocation"),
                                       ReportingDate = r.Field<DateTime>("ReportingDate"),
                                       JoinedDate = r.Field<DateTime>("JoinedDate"),
                                       ReportingToId = r.Field<Int64>("ReportingToId"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       PreviousDepartmentName = r.Field<string>("PreviousDepartmentName"),
                                       CurrentDepartmentName = r.Field<string>("CurrentDepartmentName")

                                   }).ToList();
                }
            }

            return employeeList;
        }
        public EmpTransferBO GetEmpTransferById(Int64 transferId)
        {
            EmpTransferBO employee = new EmpTransferBO();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTransferById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransferId", DbType.Int64, transferId);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmpTransfer");
                    DataTable table = employeeDS.Tables["EmpTransfer"];

                    employee = table.AsEnumerable().Select(r =>
                                   new EmpTransferBO
                                   {
                                       TransferId = r.Field<Int64>("TransferId"),
                                       TransferDate = r.Field<DateTime>("TransferDate"),
                                       EmpId = r.Field<Int64>("EmpId"),
                                       PreviousDepartmentId = r.Field<int>("PreviousDepartmentId"),
                                       CurrentDepartmentId = r.Field<int>("CurrentDepartmentId"),
                                       PreviousDesignationId = r.Field<int>("PreviousDesignationId"),
                                       CurrentDesignationId = r.Field<int>("CurrentDesignationId"),
                                       PreviousLocation = r.Field<int?>("PreviousLocation"),
                                       CurrentLocation = r.Field<int?>("CurrentLocation"),
                                       ReportingDate = r.Field<DateTime>("ReportingDate"),
                                       JoinedDate = r.Field<DateTime>("JoinedDate"),
                                       ReportingToId = r.Field<Int64>("ReportingToId"),
                                       ReportingTo2Id = r.Field<Int64>("ReportingTo2Id"),
                                       ApprovedStatus = r.Field<string>("ApprovedStatus"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       PreviousDepartmentName = r.Field<string>("PreviousDepartmentName"),
                                       CurrentDepartmentName = r.Field<string>("CurrentDepartmentName"),
                                       PreviousDesignationName = r.Field<string>("PreviousDesignationName"),
                                       PreviousCompanyId = r.Field<long>("PreviousCompanyId"),
                                       CurrentCompanyId = r.Field<long>("CurrentCompanyId"),
                                       CurrentProjectId = r.Field<long>("CurrentProjectId"),
                                       PreviousProjectId = r.Field<long>("PreviousProjectId"),
                                       PreviousReportingToId = r.Field<long>("PreviousReportingToId"),
                                       PreviousReportingTo2Id = r.Field<long>("PreviousReportingTo2Id"),
                                       Description = r.Field<string>("Description"),
                                       PreviousReportingToName = r.Field<string>("PreviousReportingToName"),
                                       PreviousReportingTo2Name = r.Field<string>("PreviousReportingTo2Name"),
                                       PreviousCompanyName = r.Field<string>("PreviousCompanyName"),
                                       PreviousProjectName = r.Field<string>("PreviousProjectName"),
                                       CurrentCompanyName = r.Field<string>("CurrentCompanyName"),
                                       CurrentProjectName = r.Field<string>("CurrentProjectName")
                                   }).FirstOrDefault();
                }
            }

            return employee;
        }
        public bool UpdateEmpTransferStatus(EmpTransferBO transferbo)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateEmpTransferStatus_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@TransferId", DbType.Int32, transferbo.TransferId);
                    dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, transferbo.ApprovedStatus);
                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, transferbo.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                }
            }

            return (status > 0 ? true : false);
        }
        public bool DeleteEmpTransfer(EmpTransferBO transferbo)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PayrollEmpTransfer");
                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "TransferId");
                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, transferbo.TransferId.ToString());

                    status = dbSmartAspects.ExecuteNonQuery(command);
                }
            }

            return (status == 1 ? true : false);
        }
        // Termination & Relieving Letter
        public List<EmployeeBO> GetEmployeeByIdForLetters(int empId, int departmentId)
        {
            List<EmployeeBO> bo = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByIdForLetters_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpInfo");
                    DataTable Table = ds.Tables["EmpInfo"];

                    bo = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FirstName = r.Field<string>("FirstName"),
                        LastName = r.Field<string>("LastName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department"),
                        EmpTypeId = r.Field<int>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        DesignationId = r.Field<int>("DesignationId"),
                        GradeId = r.Field<int>("GradeId"),
                        RepotingTo = r.Field<int>("RepotingTo"),
                        RepotingTo2 = r.Field<int>("RepotingTo2"),
                        GlCompanyId = r.Field<int>("GlCompanyId"),
                        Designation = r.Field<string>("Designation"),
                        OfficialEmail = r.Field<string>("OfficialEmail"),
                        ReferenceBy = r.Field<string>("ReferenceBy"),
                        Remarks = r.Field<string>("Remarks"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        EmpDateOfBirth = r.Field<DateTime?>("EmpDateOfBirth"),
                        Gender = r.Field<string>("Gender"),
                        Religion = r.Field<string>("Religion"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        Height = r.Field<string>("Height"),
                        CountryId = r.Field<int>("CountryId"),
                        Nationality = r.Field<string>("Nationality"),
                        NationalId = r.Field<string>("NationalId"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        PIssuePlace = r.Field<string>("PIssuePlace"),
                        PIssueDate = r.Field<DateTime?>("PIssueDate"),
                        PExpireDate = r.Field<DateTime?>("PExpireDate"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PresentZipCode = r.Field<string>("PresentZipCode"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentZipCode = r.Field<string>("PermanentZipCode"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        PermanentPhone = r.Field<string>("PermanentPhone"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        AlternativeEmail = r.Field<string>("AlternativeEmail"),
                        WorkStationId = r.Field<int>("WorkStationId"),
                        DonorId = r.Field<int>("DonorId"),
                        ResignationDate = r.Field<DateTime?>("ResignationDate"),
                        EmergencyContactName = r.Field<string>("EmergencyContactName"),
                        EmergencyContactRelationship = r.Field<string>("EmergencyContactRelationship"),
                        EmergencyContactNumber = r.Field<string>("EmergencyContactNumber"),
                        EmergencyContactNumberHome = r.Field<string>("EmergencyContactNumberHome"),
                        EmergencyContactEmail = r.Field<string>("EmergencyContactEmail"),
                        ActivityCode = r.Field<string>("ActivityCode")

                    }).ToList();
                }
            }
            return bo;
        }

        //Division, District, Thana Load
        public List<SetupSearchBO> GetSetupSearchInformation(string type, string name, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SetupSearchBO> infoBOs = new List<SetupSearchBO>();
            DataSet setupDS = new DataSet();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDivisionSetupInfoBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                        dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        dbSmartAspects.LoadDataSet(cmd, setupDS, "SetupInfo");
                        DataTable table = setupDS.Tables["SetupInfo"];
                        if (type == "Division")
                        {
                            infoBOs = table.AsEnumerable().Select(r =>
                                       new SetupSearchBO
                                       {
                                           CountryId = r.Field<int?>("CountryId"),
                                           DivisionId = r.Field<int?>("DivisionId"),
                                           DivisionName = r.Field<string>("DivisionName"),
                                           //DistrictId = r.Field<int?>("DistrictId"),
                                           //DistrictName = r.Field<string>("DistrictName"),
                                           //ThanaId = r.Field<int?>("ThanaId"),
                                           //ThanaName = r.Field<string>("ThanaName")
                                       }).ToList();

                        }
                        else if (type == "District")
                        {
                            infoBOs = table.AsEnumerable().Select(r =>
                                       new SetupSearchBO
                                       {
                                           //CountryId = r.Field<int?>("CountryId"),
                                           DivisionId = r.Field<int?>("DivisionId"),
                                           //DivisionName = r.Field<string>("DivisionName"),
                                           DistrictId = r.Field<int?>("DistrictId"),
                                           DistrictName = r.Field<string>("DistrictName"),
                                           //ThanaId = r.Field<int?>("ThanaId"),
                                           //ThanaName = r.Field<string>("ThanaName")
                                       }).ToList();

                        }
                        else if (type == "Thana")
                        {
                            infoBOs = table.AsEnumerable().Select(r =>
                                       new SetupSearchBO
                                       {
                                           //CountryId = r.Field<int?>("CountryId"),
                                           //DivisionId = r.Field<int?>("DivisionId"),
                                           //DivisionName = r.Field<string>("DivisionName"),
                                           DistrictId = r.Field<int?>("DistrictId"),
                                           //DistrictName = r.Field<string>("DistrictName"),
                                           ThanaId = r.Field<int?>("ThanaId"),
                                           ThanaName = r.Field<string>("ThanaName")
                                       }).ToList();

                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return infoBOs;
        }
        public bool SaveThana(EmpThanaBO thanaBO, out int tmpId)
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("SaveThana_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@ThanaName", DbType.String, thanaBO.ThanaName);
                            dbSmartAspects.AddInParameter(commandMember, "@ThanaId", DbType.Int32, thanaBO.ThanaId);
                            dbSmartAspects.AddInParameter(commandMember, "@DistrictId", DbType.Int32, thanaBO.DistrictId);
                            dbSmartAspects.AddInParameter(commandMember, "@CreatedBy", DbType.Int32, thanaBO.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandMember, "@OutId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMember, transaction) > 0 ? true : false;
                            tmpId = Convert.ToInt32(commandMember.Parameters["@OutId"].Value);

                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;
        }
        public bool SaveDisctrict(EmpDistrictBO districtBO, out int tmpId)
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("SaveDistrict_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@DistrictName", DbType.String, districtBO.DistrictName);
                            dbSmartAspects.AddInParameter(commandMember, "@DivisionId", DbType.Int32, districtBO.DivisionId);
                            dbSmartAspects.AddInParameter(commandMember, "@DistrictId", DbType.Int32, districtBO.DistrictId);
                            dbSmartAspects.AddInParameter(commandMember, "@CreatedBy", DbType.Int32, districtBO.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandMember, "@OutId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMember, transaction) > 0 ? true : false;
                            tmpId = Convert.ToInt32(commandMember.Parameters["@OutId"].Value);

                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;
        }
        public bool SaveDivision(EmpDivisionBO divisionBO, out int tmpId)
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("SaveDivision_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@DivisionName", DbType.String, divisionBO.DivisionName);
                            dbSmartAspects.AddInParameter(commandMember, "@DivisionId", DbType.Int32, divisionBO.DivisionId);
                            dbSmartAspects.AddInParameter(commandMember, "@CountryId", DbType.Int32, divisionBO.CountryId);
                            dbSmartAspects.AddInParameter(commandMember, "@CreatedBy", DbType.Int32, divisionBO.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandMember, "@OutId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMember, transaction) > 0 ? true : false;
                            tmpId = Convert.ToInt32(commandMember.Parameters["@OutId"].Value);

                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;
        }
        public List<EmpDivisionBO> GetEmpDivisionList()
        {
            List<EmpDivisionBO> boList = new List<EmpDivisionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDivisionList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpDivision");
                    DataTable Table = ds.Tables["EmpDivision"];

                    boList = Table.AsEnumerable().Select(r => new EmpDivisionBO
                    {
                        DivisionId = r.Field<Int32>("DivisionId"),
                        DivisionName = r.Field<string>("DivisionName"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return boList;
        }
        public EmpDivisionBO GetEmpDivisionById(int Id)
        {
            EmpDivisionBO boList = new EmpDivisionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDivisionById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DivisionId", DbType.Int32, Id);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpDivision");
                    DataTable Table = ds.Tables["EmpDivision"];

                    boList = Table.AsEnumerable().Select(r => new EmpDivisionBO
                    {
                        CountryId = r.Field<Int32>("CountryId"),
                        DivisionId = r.Field<Int32>("DivisionId"),
                        DivisionName = r.Field<string>("DivisionName"),
                        Remarks = r.Field<string>("Remarks")
                    }).FirstOrDefault();
                }
            }
            return boList;
        }
        public EmpDistrictBO GetEmpDistrictById(int Id)
        {
            EmpDistrictBO boList = new EmpDistrictBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDistrictById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DistrictId", DbType.Int32, Id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpDistrict");
                    DataTable Table = ds.Tables["EmpDistrict"];

                    boList = Table.AsEnumerable().Select(r => new EmpDistrictBO
                    {
                        DistrictId = r.Field<Int32>("DistrictId"),
                        DivisionId = r.Field<Int32>("DivisionId"),
                        DistrictName = r.Field<string>("DistrictName"),
                        Remarks = r.Field<string>("Remarks")
                    }).FirstOrDefault();
                }
            }
            return boList;
        }
        public List<EmpDistrictBO> GetEmpDistrictList(int divisionId)
        {
            List<EmpDistrictBO> boList = new List<EmpDistrictBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDistrictList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DivisionId", DbType.Int32, divisionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpDistrict");
                    DataTable Table = ds.Tables["EmpDistrict"];

                    boList = Table.AsEnumerable().Select(r => new EmpDistrictBO
                    {
                        DistrictId = r.Field<Int32>("DistrictId"),
                        DivisionId = r.Field<Int32>("DivisionId"),
                        DistrictName = r.Field<string>("DistrictName"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return boList;
        }
        public EmpThanaBO GetEmpThanaListById(int Id)
        {
            EmpThanaBO boList = new EmpThanaBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpThanaListById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ThanaId", DbType.Int32, Id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpThana");
                    DataTable Table = ds.Tables["EmpThana"];

                    boList = Table.AsEnumerable().Select(r => new EmpThanaBO
                    {
                        ThanaId = r.Field<Int32>("ThanaId"),
                        DistrictId = r.Field<Int32>("DistrictId"),
                        ThanaName = r.Field<string>("ThanaName"),
                        Remarks = r.Field<string>("Remarks")
                    }).FirstOrDefault();
                }
            }
            return boList;
        }
        public List<EmpThanaBO> GetEmpThanaList(int districtId)
        {
            List<EmpThanaBO> boList = new List<EmpThanaBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpThanaList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DistrictId", DbType.Int32, districtId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpThana");
                    DataTable Table = ds.Tables["EmpThana"];

                    boList = Table.AsEnumerable().Select(r => new EmpThanaBO
                    {
                        ThanaId = r.Field<Int32>("ThanaId"),
                        DistrictId = r.Field<Int32>("DistrictId"),
                        ThanaName = r.Field<string>("ThanaName"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<EmpDivisionBO> GetEmpDivisionListForShow()
        {
            List<EmpDivisionBO> divisionList = new List<EmpDivisionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDivisionListForShow_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpDivision");
                    DataTable Table = ds.Tables["EmpDivision"];

                    divisionList = Table.AsEnumerable().Select(r => new EmpDivisionBO
                    {
                        DivisionId = r.Field<Int32>("DivisionId"),
                        DivisionName = r.Field<string>("DivisionName"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return divisionList;
        }
        public List<EmpDistrictBO> GetEmpDistrictListForShow()
        {
            List<EmpDistrictBO> districtList = new List<EmpDistrictBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDistrictListForShow_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpDistrict");
                    DataTable Table = ds.Tables["EmpDistrict"];

                    districtList = Table.AsEnumerable().Select(r => new EmpDistrictBO
                    {
                        DivisionId = r.Field<Int32>("DivisionId"),
                        DistrictId = r.Field<Int32>("DistrictId"),
                        DistrictName = r.Field<string>("DistrictName"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return districtList;
        }
        public List<EmpThanaBO> GetEmpThanaListForShow()
        {
            List<EmpThanaBO> boList = new List<EmpThanaBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpThanaListForShow_SP"))
                {

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpThana");
                    DataTable Table = ds.Tables["EmpThana"];

                    boList = Table.AsEnumerable().Select(r => new EmpThanaBO
                    {
                        ThanaId = r.Field<Int32>("ThanaId"),
                        DistrictId = r.Field<Int32>("DistrictId"),
                        ThanaName = r.Field<string>("ThanaName"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<EmpWorkStationBO> GetEmpBranchListForShow()
        {
            List<EmpWorkStationBO> boList = new List<EmpWorkStationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpBranchListForShow_SP"))
                {

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpBranch");
                    DataTable Table = ds.Tables["EmpBranch"];

                    boList = Table.AsEnumerable().Select(r => new EmpWorkStationBO
                    {
                        WorkStationId = r.Field<Int32>("WorkStationId"),
                        WorkStationName = r.Field<string>("WorkStationName")
                    }).ToList();
                }
            }
            return boList;
        }

        //Emp Transfer Report
        public List<EmpTransferReportViewBO> GetEmpTransferInfoForReport(int empId, int? preDepartId, int? currentDepartId, DateTime? transferFromDate, DateTime? transferToDate)
        {
            List<EmpTransferReportViewBO> bo = new List<EmpTransferReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTransferInfoForReport_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@PreviousDepartId", DbType.Int32, preDepartId);
                    dbSmartAspects.AddInParameter(cmd, "@CurrentDepartId", DbType.Int32, currentDepartId);
                    dbSmartAspects.AddInParameter(cmd, "@TransferFromdate", DbType.DateTime, transferFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@TransferToDate", DbType.DateTime, transferToDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTransferReport");
                    DataTable Table = ds.Tables["EmpTransferReport"];

                    bo = Table.AsEnumerable().Select(r => new EmpTransferReportViewBO
                    {
                        DisplayName = r.Field<string>("DisplayName"),
                        PreviousDepartment = r.Field<string>("PreviousDepartment"),
                        CurrentDepartment = r.Field<string>("CurrentDepartment"),
                        TransferDate = r.Field<DateTime>("TransferDate"),
                        ReportingDate = r.Field<DateTime>("ReportingDate"),
                        CurrentCompanyName = r.Field<string>("CurrentCompanyName"),
                        PreviousCompanyName = r.Field<string>("PreviousCompanyName"),
                        ReportingTo = r.Field<string>("ReportingTo"),
                        PreviousReportingTo = r.Field<string>("PreviousReportingTo"),
                        PreviousDesignation = r.Field<string>("PreviousDesignation"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation")

                    }).ToList();
                }
            }
            return bo;
        }

        public bool SaveServiceChargeDistribution(DateTime processDateFrom, DateTime processDateTo, int processYear, decimal serviceAmount, int createdBy)
        {
            int status = 0;
            //tmpId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveServiceChargeDistribution_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ProcessDateFrom", DbType.DateTime, processDateFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProcessDateTo", DbType.DateTime, processDateTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProcessYear", DbType.Int32, processYear);
                        dbSmartAspects.AddInParameter(commandMaster, "@ServiceAmount", DbType.Decimal, serviceAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                        //dbSmartAspects.AddOutParameter(commandMaster, "@ServiceProcessId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                        //tmpId = Convert.ToInt32(commandMaster.Parameters["@ServiceProcessId"].Value);
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }

            return (status > 0 ? true : false);
        }
        public List<ServiceChargeDistributionDetailsViewBO> GetEmployeeServiceChargeDistributionReport(string departmentId, DateTime dateFrom, DateTime dateTo, short processYear)
        {
            List<ServiceChargeDistributionDetailsViewBO> serviceCharge = new List<ServiceChargeDistributionDetailsViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeServiceChargeDistributionReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int16, processYear);

                    if (!string.IsNullOrEmpty(departmentId))
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "ServiceCharge");
                    DataTable Table = LeaveDS.Tables["ServiceCharge"];

                    serviceCharge = Table.AsEnumerable().Select(r => new ServiceChargeDistributionDetailsViewBO
                    {
                        ServiceProcessId = r.Field<Int64>("ServiceProcessId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ProcessDateFrom = r.Field<DateTime>("ProcessDateFrom"),
                        ProcessDateTo = r.Field<DateTime>("ProcessDateTo"),
                        ServiceAmount = r.Field<decimal>("ServiceAmount"),
                        Designation = r.Field<string>("Designation"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName")

                    }).ToList();
                }
            }
            return serviceCharge;
        }

        public List<ServiceChargeDistributionDetailsViewBO> GetEmployeeServiceChargeBankAdvice(int departmentId, int bankId, DateTime dateFrom, DateTime dateTo, short processYear)
        {
            List<ServiceChargeDistributionDetailsViewBO> serviceCharge = new List<ServiceChargeDistributionDetailsViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeServiceChargeBankAdvice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessYear", DbType.Int32, processYear);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (bankId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, bankId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "BankAdvice");
                    DataTable Table = LeaveDS.Tables["BankAdvice"];

                    serviceCharge = Table.AsEnumerable().Select(r => new ServiceChargeDistributionDetailsViewBO
                    {
                        ServiceProcessId = r.Field<Int64>("ServiceProcessId"),
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ProcessDateFrom = r.Field<DateTime>("ProcessDateFrom"),
                        ProcessDateTo = r.Field<DateTime>("ProcessDateTo"),
                        ServiceAmount = r.Field<decimal>("ServiceAmount"),
                        Designation = r.Field<string>("Designation"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),

                        BankId = r.Field<int>("BankId"),
                        BankName = r.Field<string>("BankName"),
                        AccountName = r.Field<string>("AccountName"),
                        AccountNumber = r.Field<string>("AccountNumber")

                    }).ToList();
                }
            }
            return serviceCharge;
        }

        public List<BankSalaryAdviceBO> GetBankReconciliationForReport(int companyId, int bankId, DateTime dateFrom, DateTime dateTo, short salaryYear)
        {
            List<BankSalaryAdviceBO> bankSalaryAdvice = new List<BankSalaryAdviceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBankReconciliationForReport_SP"))
                {
                    if (companyId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@SalaryYear", DbType.Int32, salaryYear);

                    if (bankId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, bankId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BankId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "BankSalaryAdvice");
                    DataTable Table = LeaveDS.Tables["BankSalaryAdvice"];

                    bankSalaryAdvice = Table.AsEnumerable().Select(r => new BankSalaryAdviceBO
                    {
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        BankId = r.Field<int?>("BankId"),
                        BankName = r.Field<string>("BankName"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        AccountName = r.Field<string>("AccountName"),
                        AccountNumber = r.Field<string>("AccountNumber"),
                        HomeTakenAmount = r.Field<decimal?>("HomeTakenAmount"),

                        Designation = r.Field<string>("Designation"),
                        LocationId = r.Field<int?>("LocationId"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DonorId = r.Field<int?>("DonorId"),
                        Project = r.Field<string>("Project")

                    }).ToList();
                }
            }
            return bankSalaryAdvice;
        }
        //Applicant Update to Employee
        public bool UpdateApplicantAsEmployee(List<int> applicantIds)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                if (applicantIds != null)
                {
                    foreach (int appId in applicantIds)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateApplicantAsEmployee_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ApplicantId", DbType.Int32, appId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            return status;
        }
        public List<EmployeeBO> GetApplicants(long jobCircularId, string reportType)
        {
            List<EmployeeBO> appList = new List<EmployeeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApplicants_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@JobCircularId", DbType.Int64, jobCircularId);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.Int64, reportType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AppointedApplicants");
                    DataTable Table = ds.Tables["AppointedApplicants"];

                    appList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FirstName = r.Field<string>("FirstName"),
                        LastName = r.Field<string>("LastName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department"),
                        EmpTypeId = r.Field<int>("EmpTypeId"),
                        EmpType = r.Field<string>("EmpType"),
                        DesignationId = r.Field<int>("DesignationId"),
                        GradeId = r.Field<int>("GradeId"),
                        RepotingTo = r.Field<int>("RepotingTo"),
                        RepotingTo2 = r.Field<int>("RepotingTo2"),
                        GlCompanyId = r.Field<int>("GlCompanyId"),
                        Designation = r.Field<string>("Designation"),
                        OfficialEmail = r.Field<string>("OfficialEmail"),
                        ReferenceBy = r.Field<string>("ReferenceBy"),
                        Remarks = r.Field<string>("Remarks"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        EmpDateOfBirth = r.Field<DateTime?>("EmpDateOfBirth"),
                        Gender = r.Field<string>("Gender"),
                        Religion = r.Field<string>("Religion"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        Height = r.Field<string>("Height"),
                        CountryId = r.Field<int>("CountryId"),
                        Nationality = r.Field<string>("Nationality"),
                        NationalId = r.Field<string>("NationalId"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        PIssuePlace = r.Field<string>("PIssuePlace"),
                        PIssueDate = r.Field<DateTime?>("PIssueDate"),
                        PExpireDate = r.Field<DateTime?>("PExpireDate"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PresentZipCode = r.Field<string>("PresentZipCode"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentZipCode = r.Field<string>("PermanentZipCode"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        PermanentPhone = r.Field<string>("PermanentPhone"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        AlternativeEmail = r.Field<string>("AlternativeEmail"),
                        WorkStationId = r.Field<int>("WorkStationId"),
                        DonorId = r.Field<int>("DonorId"),
                        ResignationDate = r.Field<DateTime?>("ResignationDate"),
                        EmergencyContactName = r.Field<string>("EmergencyContactName"),
                        EmergencyContactRelationship = r.Field<string>("EmergencyContactRelationship"),
                        EmergencyContactNumber = r.Field<string>("EmergencyContactNumber"),
                        EmergencyContactNumberHome = r.Field<string>("EmergencyContactNumberHome"),
                        EmergencyContactEmail = r.Field<string>("EmergencyContactEmail"),
                        ActivityCode = r.Field<string>("ActivityCode")

                    }).ToList();
                }
            }

            return appList;
        }

        public List<OvertimeAnalysisBO> GetPayrollOvertimeAnalysis(int departmentId, DateTime dateFrom, DateTime dateTo)
        {
            List<OvertimeAnalysisBO> overtimeAnalysis = new List<OvertimeAnalysisBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollOvertimeAnalysis_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, dateTo);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "OvertimeAnalysis");
                    DataTable Table = LeaveDS.Tables["OvertimeAnalysis"];

                    overtimeAnalysis = Table.AsEnumerable().Select(r => new OvertimeAnalysisBO
                    {
                        EmpId = r.Field<int?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        OTAmount = r.Field<decimal?>("OTAmount"),
                        TotalOTHour = r.Field<decimal?>("TotalOTHour"),
                        OTRate = r.Field<decimal?>("OTRate"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        Designation = r.Field<string>("Designation"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        LocationId = r.Field<int?>("LocationId"),
                        DonorId = r.Field<int?>("DonorId"),
                        Project = r.Field<string>("Project")

                    }).ToList();
                }
            }
            return overtimeAnalysis;
        }

        public List<OvertimeAnalysisBO> GetDepartmentWiseOvertime(int departmentId, DateTime dateFrom, DateTime dateTo)
        {
            List<OvertimeAnalysisBO> overtimeAnalysis = new List<OvertimeAnalysisBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDepartmentWiseOvertimeAnalysis_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ProcessDateTo", DbType.DateTime, dateTo);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "OvertimeAnalysis");
                    DataTable Table = LeaveDS.Tables["OvertimeAnalysis"];

                    overtimeAnalysis = Table.AsEnumerable().Select(r => new OvertimeAnalysisBO
                    {
                        OTAmount = r.Field<decimal?>("OTAmount"),
                        TotalOTHour = r.Field<decimal?>("TotalOTHour"),
                        DepartmentId = r.Field<int?>("DepartmentId"),
                        DepartmentName = r.Field<string>("DepartmentName")

                    }).ToList();
                }
            }
            return overtimeAnalysis;
        }

        public List<EmployeeBO> GetEmpForPFOpeningBalance(int departmentId)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpForPFOpeningBalance_SP"))
                {
                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmpPFOpeningBalance");
                    DataTable Table = LeaveDS.Tables["EmpPFOpeningBalance"];

                    boList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department")

                    }).ToList();
                }
            }
            return boList;
        }

        public List<EmployeeBO> GetEmployeeForHKTaskAssign()
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpForHKTaskAssign_SP"))
                {
                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "HKTaskAssignEmployee");
                    DataTable Table = LeaveDS.Tables["HKTaskAssignEmployee"];

                    boList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName")

                    }).ToList();
                }
            }
            return boList;
        }

        //---------- Employee Status
        public List<EmployeeStatusBO> GetEmployeeStatus()
        {
            string query = "SELECT * FROM PayrollEmployeeStatus";
            List<EmployeeStatusBO> boList = new List<EmployeeStatusBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet LeaveDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "EmployeeStatus");
                    DataTable Table = LeaveDS.Tables["EmployeeStatus"];

                    boList = Table.AsEnumerable().Select(r => new EmployeeStatusBO
                    {
                        EmployeeStatusId = r.Field<int>("EmployeeStatusId"),
                        EmployeeStatus = r.Field<string>("EmployeeStatus")

                    }).ToList();
                }
            }
            return boList;
        }

        //------ Employee Termination
        public bool SaveEmployeeTermination(PayrollEmpTerminationBO termination, out int tmpTerminationId)
        {
            int status = 0;
            tmpTerminationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveEmployeeTermination_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, termination.EmpId);
                    dbSmartAspects.AddInParameter(commandMaster, "@DecisionDate", DbType.DateTime, termination.DecisionDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@TerminationDate", DbType.DateTime, termination.TerminationDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@EmployeeStatusId", DbType.Int32, termination.EmployeeStatusId);
                    dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, termination.Remarks);
                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, termination.CreatedBy);
                    dbSmartAspects.AddOutParameter(commandMaster, "@TerminationId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(commandMaster);
                    tmpTerminationId = Convert.ToInt32(commandMaster.Parameters["@TerminationId"].Value);
                }
            }

            return (status == 1 ? true : false);
        }

        //---- Employee Search
        public List<EmployeeBO> GetEmpInformationForAutoSearch(string empName)
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();
            DataSet employeeDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpInformationForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, empName);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "EmployeeInfo");
                    DataTable table = employeeDS.Tables["EmployeeInfo"];

                    empList = table.AsEnumerable().Select(r =>
                                   new EmployeeBO
                                   {
                                       EmpId = r.Field<int>("EmpId"),
                                       EmpCode = r.Field<string>("EmpCode"),
                                       FirstName = r.Field<string>("FirstName"),
                                       LastName = r.Field<string>("LastName"),
                                       DisplayName = r.Field<string>("DisplayName"),
                                       EmployeeName = r.Field<string>("EmployeeName"),
                                       JoinDate = r.Field<DateTime>("JoinDate"),
                                       DepartmentId = r.Field<int>("DepartmentId"),
                                       Department = r.Field<string>("Department"),
                                       EmpTypeId = r.Field<int>("EmpTypeId"),
                                       EmpType = r.Field<string>("EmpType"),
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Designation = r.Field<string>("Designation"),
                                       OfficialEmail = r.Field<string>("OfficialEmail"),
                                       ReferenceBy = r.Field<string>("ReferenceBy"),
                                       Remarks = r.Field<string>("Remarks"),
                                       CurrencyName = r.Field<string>("CurrencyName")

                                   }).ToList();
                }
            }
            return empList;
        }


        ///-----------------Employee Bill generation, payment receive, adjustment

        public List<EmployeePaymentLedgerBO> EmployeeBillBySearch(int employeeId)
        {
            List<EmployeePaymentLedgerBO> supplierInfo = new List<EmployeePaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerBO
                    {
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefEmployeePaymentId = r.Field<Int64>("RefEmployeePaymentId"),
                        EmployeeBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool SaveEmployeeBillGeneration(EmployeeBillGenerationBO billGeneration, List<EmployeeBillGenerationDetailsBO> billGenerationDetails, out Int64 employeeBillId)
        {
            int status = 0;
            employeeBillId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmployeeBillGeneration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int64, billGeneration.EmployeeId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, billGeneration.BillDate);
                            dbSmartAspects.AddInParameter(command, "@BillCurrencyId", DbType.Int32, billGeneration.BillCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, billGeneration.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billGeneration.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@EmployeeBillId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            employeeBillId = Convert.ToInt64(command.Parameters["@EmployeeBillId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmployeeBillGenerationDetails_SP"))
                        {
                            foreach (EmployeeBillGenerationDetailsBO cpl in billGenerationDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.Int64, employeeBillId);
                                dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool UpdateEmployeeBillGeneration(EmployeeBillGenerationBO billGeneration, List<EmployeeBillGenerationDetailsBO> billGenerationDetails,
                                              List<EmployeeBillGenerationDetailsBO> billGenerationDetailsEdited, List<EmployeeBillGenerationDetailsBO> billGenerationDetailsDeleted)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeBillGeneration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.String, billGeneration.EmployeeBillId);
                            dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int64, billGeneration.EmployeeId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, billGeneration.BillDate);
                            dbSmartAspects.AddInParameter(command, "@BillCurrencyId", DbType.Int32, billGeneration.BillCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, billGeneration.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, billGeneration.CreatedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0 && billGenerationDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmployeeBillGenerationDetails_SP"))
                            {
                                foreach (EmployeeBillGenerationDetailsBO cpl in billGenerationDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.Int64, billGeneration.EmployeeBillId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && billGenerationDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeBillGenerationDetails_SP"))
                            {
                                foreach (EmployeeBillGenerationDetailsBO cpl in billGenerationDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillDetailsId", DbType.Int64, cpl.EmployeeBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.Int64, billGeneration.EmployeeBillId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int64, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && billGenerationDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteEmployeeBillGenerationDetails_SP"))
                            {
                                foreach (EmployeeBillGenerationDetailsBO cpl in billGenerationDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillDetailsId", DbType.Int64, cpl.EmployeeBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.Int64, cpl.EmployeeBillId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public List<EmployeeBillGenerationBO> GetEmployeeBillGenerationBySearch(DateTime? dateFrom, DateTime? dateTo, int employeeId)
        {
            List<EmployeeBillGenerationBO> paymentInfo = new List<EmployeeBillGenerationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillGenerationBySearch_SP"))
                {
                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new EmployeeBillGenerationBO
                    {
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        EmployeeBillNumber = r.Field<string>("EmployeeBillNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public EmployeeBillGenerationBO GetCompanyBillGeneration(Int64 employeeBillId)
        {
            EmployeeBillGenerationBO paymentInfo = new EmployeeBillGenerationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillGeneration_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeBillId", DbType.Int64, employeeBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new EmployeeBillGenerationBO
                    {
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        EmployeeBillNumber = r.Field<string>("EmployeeBillNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }

        public List<EmployeeBillGenerationDetailsBO> GetCompanyBillGenerationDetails(Int64 employeeBillId)
        {
            List<EmployeeBillGenerationDetailsBO> paymentInfo = new List<EmployeeBillGenerationDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillGenerationDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeBillId", DbType.Int64, employeeBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new EmployeeBillGenerationDetailsBO
                    {
                        EmployeeBillDetailsId = r.Field<Int64>("EmployeeBillDetailsId"),
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        BillId = r.Field<int>("BillId"),
                        Amount = r.Field<decimal>("Amount")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public List<EmployeePaymentLedgerVwBO> GetCompanyBillForBillGenerationEdit(int employeeId, Int64 employeeBillId)
        {
            List<EmployeePaymentLedgerVwBO> supplierInfo = new List<EmployeePaymentLedgerVwBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillForBillGenerationEdit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeBillId", DbType.Int64, employeeBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerVwBO
                    {
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefEmployeePaymentId = r.Field<Int64>("RefEmployeePaymentId"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        EmployeeBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<EmployeePaymentLedgerVwBO> EmployeeBillByCompanyIdAndBillGenerationFlag(int employeeId, Int64 employeeBillId)
        {
            List<EmployeePaymentLedgerVwBO> supplierInfo = new List<EmployeePaymentLedgerVwBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("EmployeeBillByEmployeeIdAndBillGenerationFlag_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeBillId", DbType.Int64, employeeBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerVwBO
                    {
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        EmployeeBillNumber = r.Field<string>("EmployeeBillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        Remarks = r.Field<string>("Remarks"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool DeleteEmployeeBillGeneration(Int64 employeeBillId)
        {
            int status = 0;
            string query = string.Empty;

            query = string.Format(@"
                            UPDATE cpl SET BillGenerationId = 0 
                            FROM PayrollEmployeePaymentLedger cpl INNER JOIN PayrollEmployeeBillGenerationDetails cbgd ON cpl.BillGenerationId = cbgd.EmployeeBillId
                            WHERE cbgd.EmployeeBillId = {0}

                            DELETE FROM PayrollEmployeeBillGenerationDetails WHERE EmployeeBillId = {0}
                            DELETE FROM PayrollEmployeeBillGeneration WHERE EmployeeBillId = {0}", employeeBillId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public List<EmployeeBillGenerationBO> GetEmployeeGeneratedBillByBillStatus(int employeeId)
        {
            List<EmployeeBillGenerationBO> paymentInfo = new List<EmployeeBillGenerationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeGeneratedBill_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new EmployeeBillGenerationBO
                    {
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        EmployeeBillNumber = r.Field<string>("EmployeeBillNumber"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public List<EmployeeBillGenerateViewBO> GetEmployeeBillForBillReceive(int employeeId, Int64 employeeBillId)
        {
            List<EmployeeBillGenerateViewBO> supplierInfo = new List<EmployeeBillGenerateViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillForReceivedPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeBillId", DbType.Int64, employeeBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeeBillGenerateViewBO
                    {
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        EmployeeBillNumber = r.Field<string>("EmployeeBillNumber"),
                        EmployeeBillDetailsId = r.Field<Int64>("EmployeeBillDetailsId"),
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        BillId = r.Field<Int32>("BillId"),
                        Amount = r.Field<decimal>("Amount"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        PaymentDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<EmployeePaymentLedgerVwBO> EmployeeBillBySearch(int employeeId, Int64 employeeBillId)
        {
            List<EmployeePaymentLedgerVwBO> supplierInfo = new List<EmployeePaymentLedgerVwBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeGeneratedBillById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeBillId", DbType.Int64, employeeBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerVwBO
                    {
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefEmployeePaymentId = r.Field<Int64>("RefEmployeePaymentId")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool SaveEmployeeBillPayment(EmployeePaymentBO employeePayment, List<EmployeePaymentDetailsBO> employeePaymentDetails, out long employeePaymentId)
        {
            int status = 0;
            employeePaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmployeePayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.String, employeePayment.EmployeeBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, employeePayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, employeePayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@EmployeePaymentAdvanceId", DbType.String, employeePayment.EmployeePaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.String, employeePayment.EmployeeId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, employeePayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, employeePayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, employeePayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, employeePayment.AccountingPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, employeePayment.AdjustmentAmount);

                            if (!string.IsNullOrEmpty(employeePayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, employeePayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(employeePayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, employeePayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (employeePayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, employeePayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (employeePayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, employeePayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (employeePayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, employeePayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (employeePayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, employeePayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            employeePaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmployeePaymentDetails_SP"))
                        {
                            foreach (EmployeePaymentDetailsBO cpl in employeePaymentDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, employeePaymentId);
                                dbSmartAspects.AddInParameter(command, "@EmployeeBillDetailsId", DbType.Int64, cpl.EmployeeBillDetailsId);
                                dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }



                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool UpdateEmployeeBillPayment(EmployeePaymentBO employeePayment, List<EmployeePaymentDetailsBO> employeePaymentDetails,
            List<EmployeePaymentDetailsBO> employeePaymentDetailsEdited, List<EmployeePaymentDetailsBO> employeePaymentDetailsDeleted)
        {
            int status = 0;
            Int64 employeePaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmployeePayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, employeePayment.PaymentId);
                            dbSmartAspects.AddInParameter(command, "@EmployeeBillId", DbType.String, employeePayment.EmployeeBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, employeePayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, employeePayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@EmployeePaymentAdvanceId", DbType.String, employeePayment.EmployeePaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.String, employeePayment.EmployeeId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, employeePayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, employeePayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, employeePayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, employeePayment.AccountingPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, employeePayment.AdjustmentAmount);

                            if (!string.IsNullOrEmpty(employeePayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, employeePayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(employeePayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, employeePayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (employeePayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, employeePayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (employeePayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, employeePayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (employeePayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, employeePayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (employeePayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, employeePayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            employeePaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0 && employeePaymentDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmployeePaymentDetails_SP"))
                            {
                                foreach (EmployeePaymentDetailsBO cpl in employeePaymentDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, employeePayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillDetailsId", DbType.Int64, cpl.EmployeeBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && employeePaymentDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmployeePaymentDetails_SP"))
                            {
                                foreach (EmployeePaymentDetailsBO cpl in employeePaymentDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentDetailsId", DbType.Int64, cpl.PaymentDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, employeePayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeeBillDetailsId", DbType.Int64, cpl.EmployeeBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@EmployeePaymentId", DbType.Int64, cpl.EmployeePaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && employeePaymentDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (EmployeePaymentDetailsBO cpl in employeePaymentDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PayrollEmployeePaymentDetails");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "PaymentDetailsId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, cpl.PaymentDetailsId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public List<EmployeePaymentBO> GetEmployeePaymentBySearch(int employeeId, DateTime? dateFrom, DateTime? dateTo, string paymentFor)
        {
            List<EmployeePaymentBO> paymentInfo = new List<EmployeePaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePaymentBySearch_SP"))
                {
                    if (employeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (paymentFor != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, paymentFor);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, DBNull.Value);
                    
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new EmployeePaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        EmployeeId = r.Field<int>("EmployeeId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AdjustmentType = r.Field<string>("AdjustmentType")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public EmployeePaymentBO GetEmployeePayment(Int64 paymentId)
        {
            EmployeePaymentBO paymentInfo = new EmployeePaymentBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePayment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new EmployeePaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        EmployeeId = r.Field<int>("EmployeeId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        CurrencyId = r.Field<int?>("CurrencyId"),
                        ConvertionRate = r.Field<decimal?>("ConvertionRate"),
                        PaymentType = r.Field<string>("PaymentType"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        AccountingPostingHeadId = r.Field<int>("AccountingPostingHeadId"),
                        EmployeePaymentAdvanceId = r.Field<Int64>("EmployeePaymentAdvanceId"),
                        AdjustmentAmount = r.Field<decimal>("AdjustmentAmount"),
                        AdjustmentType = r.Field<string>("AdjustmentType"),

                        AdjustmentAccountHeadId = r.Field<int>("AdjustmentAccountHeadId"),
                        PaymentAdjustmentAmount = r.Field<decimal>("PaymentAdjustmentAmount")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }

        public List<EmployeePaymentDetailsViewBO> GetEmployeePaymentDetails(Int64 paymentId)
        {
            List<EmployeePaymentDetailsViewBO> supplierInfo = new List<EmployeePaymentDetailsViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePaymentDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentDetailsViewBO
                    {
                        EmployeeBillId = r.Field<long?>("EmployeeBillId"),
                        PaymentDetailsId = r.Field<long>("PaymentDetailsId"),
                        PaymentId = r.Field<long>("PaymentId"),
                        EmployeeBillDetailsId = r.Field<long?>("EmployeeBillDetailsId"),
                        EmployeePaymentId = r.Field<long>("EmployeePaymentId"),

                        BillId = r.Field<long>("BillId"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DueAmount = r.Field<decimal?>("DueAmount")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<EmployeePaymentLedgerVwBO> EmployeeBillAdvanceBySearch(int employeeId)
        {
            List<EmployeePaymentLedgerVwBO> supplierInfo = new List<EmployeePaymentLedgerVwBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeAdvanceBillBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerVwBO
                    {
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefEmployeePaymentId = r.Field<Int64>("RefEmployeePaymentId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        AdvanceAmountRemaining = r.Field<decimal>("AdvanceAmountRemaining")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool DeleteEmployeePayment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            string query = string.Format(@"
                                DELETE FROM PayrollEmployeePaymentDetails
                                WHERE PaymentId = {0}

                                DELETE FROM PayrollEmployeePayment
                                WHERE PaymentId = {0}
                            ", paymentId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool ApprovedPayment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedEmployeePaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool ApprovedRefund(Int64 paymentId, int createdBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedEmployeeRefundLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        //-----Bill Adjustment

        //------- Bill Report

        public List<EmployeePaymentLedgerBO> EmployeeBillLedgerForReport(int employeeId)
        {
            List<EmployeePaymentLedgerBO> supplierInfo = new List<EmployeePaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBillLedgerForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeePaymentLedgerBO
                    {
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefEmployeePaymentId = r.Field<Int64>("RefEmployeePaymentId"),
                        EmployeeBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<EmployeeBillGenerateViewBO> EmployeeGeneratedBillForReport(int employeeId, string reportType)
        {
            List<EmployeeBillGenerateViewBO> supplierInfo = new List<EmployeeBillGenerateViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeGeneratedBillForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, employeeId);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new EmployeeBillGenerateViewBO
                    {
                        EmployeeBillId = r.Field<Int64>("EmployeeBillId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        EmployeeBillNumber = r.Field<string>("EmployeeBillNumber"),
                        EmployeeBillDetailsId = r.Field<Int64>("EmployeeBillDetailsId"),
                        EmployeePaymentId = r.Field<Int64>("EmployeePaymentId"),
                        BillId = r.Field<Int32>("BillId"),
                        Amount = r.Field<decimal>("Amount"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        EmployeeName = r.Field<string>("EmployeeName")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<EmpListForReportViewBO> GetSelectedEmployeeInfoListForReport(int companyId, int projectId, int departmentId, int designationId, string bloodGroup, int workStationId, string employeeStatus)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoListForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeStatus", DbType.String, employeeStatus);
                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    if (departmentId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    }

                    if (designationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DesignationId", DbType.Int32, designationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DesignationId", DbType.Int32, DBNull.Value);
                    }

                    if (bloodGroup != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BloodGroup", DbType.String, bloodGroup);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BloodGroup", DbType.String, DBNull.Value);
                    }

                    if (workStationId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, workStationId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@WorkStationId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        EmployeeStatus = r.Field<string>("EmployeeStatus"),
                        DisplayName = r.Field<string>("FullName"),
                        FullName = r.Field<string>("FullName"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        Gender = r.Field<string>("Gender"),
                        Height = r.Field<string>("Height"),
                        EmpDateOfBirth = r.Field<string>("EmpDateOfBirth"),
                        CountryId = r.Field<int>("CountryId"),
                        NationalId = r.Field<string>("NationalId"),
                        Religion = r.Field<string>("Religion"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        JoinDateString = r.Field<string>("JoinDateString"),
                    }).ToList();
                }
            }

            return empList;
        }

        public List<EmpListForReportViewBO> GetSelectedEmployeeList(string employeeStatus)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeStatus", DbType.String, employeeStatus);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        EmployeeStatus = r.Field<string>("EmployeeStatus"),
                        EmpId = r.Field<int>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        DisplayName = r.Field<string>("DisplayName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        NationalIdOrOtherCertificate = r.Field<string>("NationalIdOrOtherCertificate")

                    }).ToList();
                }
            }

            return empList;
        }

        public List<EmpListForReportViewBO> GetEmployeeInformationByCompanyId(int CompanyId)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInformationByCompanyId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        EmployeeStatus = r.Field<string>("EmployeeStatus"),
                        DisplayName = r.Field<string>("FullName"),
                        FullName = r.Field<string>("FullName"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        Gender = r.Field<string>("Gender"),
                        Height = r.Field<string>("Height"),
                        EmpDateOfBirth = r.Field<string>("EmpDateOfBirth"),
                        CountryId = r.Field<int>("CountryId"),
                        NationalId = r.Field<string>("NationalId"),
                        Religion = r.Field<string>("Religion"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        JoinDateString = r.Field<string>("JoinDateString")
                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmployeeBO> GetEmployeeDashboardAttendanceSummary(DateTime date)
        {
            List<EmployeeBO> empList = new List<EmployeeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeDashboardAttendanceSummary_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportDate", DbType.DateTime, date);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Payroll");
                    DataTable Table = ds.Tables["Payroll"];

                    empList = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        TransactionName = r.Field<string>("TransactionName"),
                        TransactionCount = r.Field<int>("TransactionCount")

                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmployeeStatusBO> GetEmployeeStatusListByIdList(string employeeStatus)
        {
            List<EmployeeStatusBO> empList = new List<EmployeeStatusBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeStatusListByIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeStatus", DbType.String, employeeStatus);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmployeeStatusBO
                    {
                        EmployeeStatus = r.Field<string>("EmployeeStatus"),

                    }).ToList();
                }
            }

            return empList;
        }

        public List<EmpListForReportViewBO> GetEmployeeTypeWiseListForReport(int employeeType, DateTime? dateFrom, DateTime? dateTo, string employeeStatus)
        {
            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeTypeWiseListForReport_SP"))
                {
                    if (employeeType != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeType", DbType.Int32, employeeType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeType", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@EmployeeStatus", DbType.String, employeeStatus);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmployee");
                    DataTable Table = ds.Tables["PayrollEmployee"];

                    empList = Table.AsEnumerable().Select(r => new EmpListForReportViewBO
                    {
                        EmployeeStatus = r.Field<string>("EmployeeStatus"),
                        DisplayName = r.Field<string>("FullName"),
                        FullName = r.Field<string>("FullName"),
                        BloodGroup = r.Field<string>("BloodGroup"),
                        EmpCode = r.Field<string>("EmpCode"),
                        FathersName = r.Field<string>("FathersName"),
                        MothersName = r.Field<string>("MothersName"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        MaritalStatus = r.Field<string>("MaritalStatus"),
                        Gender = r.Field<string>("Gender"),
                        Height = r.Field<string>("Height"),
                        EmpDateOfBirth = r.Field<string>("EmpDateOfBirth"),
                        CountryId = r.Field<int>("CountryId"),
                        NationalId = r.Field<string>("NationalId"),
                        Religion = r.Field<string>("Religion"),
                        PresentPhone = r.Field<string>("PresentPhone"),
                        PresentAddress = r.Field<string>("PresentAddress"),
                        PresentCity = r.Field<string>("PresentCity"),
                        PresentCountry = r.Field<string>("PresentCountry"),
                        PermanentAddress = r.Field<string>("PermanentAddress"),
                        PermanentCity = r.Field<string>("PermanentCity"),
                        PermanentCountry = r.Field<string>("PermanentCountry"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        DesignationName = r.Field<string>("DesignationName"),
                        WorkStationName = r.Field<string>("WorkStationName"),
                        JoinDateString = r.Field<string>("JoinDateString"),
                        EmployeeType = r.Field<string>("EmployeeType"),
                        EmployeeTypeExtension = r.Field<string>("EmployeeTypeExtension")


                    }).ToList();
                }
            }

            return empList;
        }
        public List<EmployeeBO> GetEmployeeBirthdayInformationforReport(DateTime? fromDate, DateTime? toDate)
        {
            List<EmployeeBO> infoList = new List<EmployeeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeBirthdayInformationforReport_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeBirthday");
                    DataTable Table = ds.Tables["EmployeeBirthday"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new EmployeeBO
                                {
                                    EmpCode = r.Field<string>("EmpCode"),
                                    EmployeeName = r.Field<string>("EmployeeName"),
                                    JoinDateDisplay = r.Field<string>("JoinDateDisplay"),
                                    DateOfBirthDisplay = r.Field<string>("DateOfBirthDisplay"),
                                    OfficialEmail = r.Field<string>("OfficialEmail"),
                                    PresentAddress = r.Field<string>("PresentAddress"),
                                    PresentPhone = r.Field<string>("PresentPhone"),
                                    PermanentAddress = r.Field<string>("PermanentAddress"),
                                    PermanentPhone = r.Field<string>("PermanentPhone"),
                                    PersonalEmail = r.Field<string>("PersonalEmail")
                                }).ToList();
                }
            }
            return infoList;
        }
        public List<EmployeeBO> GetEmployeeProvisionPeriodforReport(DateTime? fromDate, DateTime? toDate)
        {
            List<EmployeeBO> infoList = new List<EmployeeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeProvisionPeriodforReport_SP"))
                {
                    if (fromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (toDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Todate", DbType.DateTime, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeProvisionPeriod");
                    DataTable Table = ds.Tables["EmployeeProvisionPeriod"];
                    infoList = Table.AsEnumerable().Select(r =>
                                new EmployeeBO
                                {
                                    EmpCode = r.Field<string>("EmpCode"),
                                    EmployeeName = r.Field<string>("EmployeeName"),
                                    JoinDateDisplay = r.Field<string>("JoinDateDisplay"),
                                    ProvisionPeriodDisplay = r.Field<string>("ProvisionPeriodDisplay"),
                                    OfficialEmail = r.Field<string>("OfficialEmail"),
                                    PresentPhone = r.Field<string>("PresentPhone"),
                                    PermanentPhone = r.Field<string>("PermanentPhone"),
                                    PersonalEmail = r.Field<string>("PersonalEmail"),
                                    Department = r.Field<string>("Department"),
                                    Designation = r.Field<string>("Designation"),
                                    RepotingToOne = r.Field<string>("RepotingToOne"),
                                    RepotingToTwo = r.Field<string>("RepotingToTwo")
                                }).ToList();
                }
            }
            return infoList;
        }
        public List<EmployeeBO> GetEmployeeInfoForAcountManager(string searchText)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeInfoForAcountManager_SP"))
                {
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@searchText", DbType.String, searchText);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@searchText", DbType.String, DBNull.Value);
                    }
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmpDisplayNameWithCode = bo.EmpCode + "-" + bo.DisplayName;
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                               

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
