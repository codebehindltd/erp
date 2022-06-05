using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class SalaryFormulaDA : BaseService
    {
        public List<EmpGradeBO> GetAllGrade()
        {
            EmpGradeDA empGradDA = new EmpGradeDA();
            return empGradDA.GetGradeInfo();
        }
        public List<CustomFieldBO> GetCustomFields(string CustomFieldName, string dropDownFirstValue)
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField(CustomFieldName, dropDownFirstValue);
            return fields;
        }
        public bool SaveSalaryFormulaInfo(SalaryFormulaBO salaryFormulaBO, out int tmpUserInfoId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalaryFormulaInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, salaryFormulaBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, salaryFormulaBO.GradeIdOrEmployeeId);
                        dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryFormulaBO.SalaryHeadId);
                        dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, salaryFormulaBO.DependsOn);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, salaryFormulaBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, salaryFormulaBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, salaryFormulaBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salaryFormulaBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@FormulaId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@FormulaId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool SaveBothContributionSalaryFormulaInfo(SalaryFormulaBO salaryFormulaBO, out int tmpUserInfoId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBothContributionSalaryFormulaInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, salaryFormulaBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, salaryFormulaBO.GradeIdOrEmployeeId);
                        dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryFormulaBO.SalaryHeadId);
                        dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, salaryFormulaBO.DependsOn);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, salaryFormulaBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, salaryFormulaBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, salaryFormulaBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salaryFormulaBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@FormulaId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@FormulaId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool SaveSalaryFormulaInfo(List<SalaryFormulaBO> salaryFormula, int createdBy, out int tmpFormulaId)
        {
            Boolean status = false;
            tmpFormulaId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalaryFormulaInfo_SP"))
                    {

                        foreach (SalaryFormulaBO sal in salaryFormula)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, sal.TransactionType);
                            dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, sal.GradeIdOrEmployeeId);
                            dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, sal.SalaryHeadId);
                            dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, sal.DependsOn);
                            dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, sal.Amount);
                            dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, sal.AmountType);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, sal.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                            dbSmartAspects.AddOutParameter(command, "@FormulaId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            tmpFormulaId = Convert.ToInt32(command.Parameters["@FormulaId"].Value);
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

        public bool SaveBothContributionSalaryFormulaInfo(List<SalaryFormulaBO> salaryFormula, int createdBy, out int tmpFormulaId)
        {
            Boolean status = false;
            tmpFormulaId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBothContributionSalaryFormulaInfo_SP"))
                    {

                        foreach (SalaryFormulaBO sal in salaryFormula)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, sal.TransactionType);
                            dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, sal.GradeIdOrEmployeeId);
                            dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, sal.SalaryHeadId);
                            dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, sal.DependsOn);
                            dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, sal.Amount);
                            dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, sal.AmountType);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, sal.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                            dbSmartAspects.AddOutParameter(command, "@FormulaId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            tmpFormulaId = Convert.ToInt32(command.Parameters["@FormulaId"].Value);
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

        public bool UpdateSalaryFormulaInfo(SalaryFormulaBO salaryFormulaBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalaryFormulaInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FormulaId", DbType.Int32, salaryFormulaBO.FormulaId);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, salaryFormulaBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, salaryFormulaBO.GradeIdOrEmployeeId);
                        dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryFormulaBO.SalaryHeadId);
                        dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, salaryFormulaBO.DependsOn);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, salaryFormulaBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, salaryFormulaBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, salaryFormulaBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, salaryFormulaBO.LastModifiedBy);
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

        public bool UpdateBothContributionSalaryFormulaInfo(SalaryFormulaBO salaryFormulaBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBothContributionSalaryFormulaInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FormulaId", DbType.Int32, salaryFormulaBO.FormulaId);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, salaryFormulaBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, salaryFormulaBO.GradeIdOrEmployeeId);
                        dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryFormulaBO.SalaryHeadId);
                        dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, salaryFormulaBO.DependsOn);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, salaryFormulaBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, salaryFormulaBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, salaryFormulaBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, salaryFormulaBO.LastModifiedBy);
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

        public bool UpdateSalaryFormulaInfo(List<SalaryFormulaBO> salaryFormula, int updatedBy)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalaryFormulaInfo_SP"))
                    {
                        foreach (SalaryFormulaBO sal in salaryFormula)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@FormulaId", DbType.Int32, sal.FormulaId);
                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, sal.TransactionType);
                            dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, sal.GradeIdOrEmployeeId);
                            dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, sal.SalaryHeadId);
                            dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, sal.DependsOn);
                            dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, sal.Amount);
                            dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, sal.AmountType);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, sal.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, updatedBy);
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
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

        public bool UpdateBothContributionSalaryFormulaInfo(List<SalaryFormulaBO> salaryFormula, int updatedBy)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBothContributionSalaryFormulaInfo_SP"))
                    {
                        foreach (SalaryFormulaBO sal in salaryFormula)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@FormulaId", DbType.Int32, sal.FormulaId);
                            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, sal.TransactionType);
                            dbSmartAspects.AddInParameter(command, "@GradeIdOrEmployeeId", DbType.Int32, sal.GradeIdOrEmployeeId);
                            dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, sal.SalaryHeadId);
                            dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.Int32, sal.DependsOn);
                            dbSmartAspects.AddInParameter(command, "@Amount", DbType.String, sal.Amount);
                            dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, sal.AmountType);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, sal.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, updatedBy);
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
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

        public bool DeleteSalaryFormulaInfo(List<SalaryFormulaBO> salaryFormula)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                    {
                        foreach (SalaryFormulaBO sal in salaryFormula)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PayrollSalaryFormula");
                            dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "FormulaId");
                            dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, sal.FormulaId.ToString());
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
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
        public bool DeleteBothContributionSalaryFormulaInfo(List<SalaryFormulaBO> salaryFormula)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                    {
                        foreach (SalaryFormulaBO sal in salaryFormula)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PayrollSalaryFormulaForBothContribution");
                            dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "FormulaId");
                            dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, sal.FormulaId.ToString());
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
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

        public SalaryFormulaBO getSalaryFormulaInfoById(int salaryFormulaId)
        {
            SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryFormulaInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FormulaId", DbType.Int32, salaryFormulaId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                                salaryFormulaBO.TransactionType = reader["TransactionType"].ToString();
                                salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                                salaryFormulaBO.Grade = reader["Grade"].ToString();
                                salaryFormulaBO.EmplyeeName = reader["EmplyeeName"].ToString();
                                salaryFormulaBO.EmployeeCode = reader["EmployeeCode"].ToString();
                                salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                                salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                                salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                                salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                                salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                            }
                        }
                    }
                }
            }
            return salaryFormulaBO;
        }

        public SalaryFormulaBO GetBothContributionSalaryFormulaInfoById(int salaryFormulaId)
        {
            SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBothContributionSalaryFormulaInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FormulaId", DbType.Int32, salaryFormulaId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                                salaryFormulaBO.TransactionType = reader["TransactionType"].ToString();
                                salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                                salaryFormulaBO.Grade = reader["Grade"].ToString();
                                salaryFormulaBO.EmplyeeName = reader["EmplyeeName"].ToString();
                                salaryFormulaBO.EmployeeCode = reader["EmployeeCode"].ToString();
                                salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                                salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                                salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                                salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                                salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                            }
                        }
                    }
                }
            }
            return salaryFormulaBO;
        }
        public List<SalaryFormulaBO> GetSalaryFormulaInfoByEmpId(int empId)
        {
            List<SalaryFormulaBO> salaryFormula = new List<SalaryFormulaBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryFormulaInfoByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "SalaryFormula");
                    DataTable Table = incrementDS.Tables["SalaryFormula"];

                    salaryFormula = Table.AsEnumerable().Select(r => new SalaryFormulaBO
                    {
                        FormulaId = r.Field<int>("FormulaId"),
                        TransactionType = r.Field<string>("TransactionType"),
                        GradeIdOrEmployeeId = r.Field<int>("GradeIdOrEmployeeId"),
                        Grade = r.Field<string>("Grade"),
                        EmplyeeName = r.Field<string>("EmplyeeName"),
                        EmployeeCode = r.Field<string>("EmployeeCode"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        DependsOn = r.Field<int?>("DependsOn"),
                        DependsOnHead = r.Field<string>("DependsOnHead"),
                        Amount = r.Field<decimal>("Amount"),
                        AmountType = r.Field<string>("AmountType"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();

                    //using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    //{
                    //    if (reader != null)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                    //            salaryFormulaBO.TransactionType = reader["TransactionType"].ToString();
                    //            salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                    //            salaryFormulaBO.Grade = reader["Grade"].ToString();
                    //            salaryFormulaBO.EmplyeeName = reader["EmplyeeName"].ToString();
                    //            salaryFormulaBO.EmployeeCode = reader["EmployeeCode"].ToString();
                    //            salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                    //            salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                    //            salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                    //            salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                    //            salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                    //            salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                    //            salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                    //            salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                    //        }
                    //    }
                    //}
                }
            }
            return salaryFormula;
        }
        public List<SalaryFormulaBO> GetBothContributionSalaryFormulaInfoByEmpId(int empId)
        {
            List<SalaryFormulaBO> salaryFormula = new List<SalaryFormulaBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBothContributionSalaryFormulaInfoByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "SalaryFormula");
                    DataTable Table = incrementDS.Tables["SalaryFormula"];

                    salaryFormula = Table.AsEnumerable().Select(r => new SalaryFormulaBO
                    {
                        FormulaId = r.Field<int>("FormulaId"),
                        TransactionType = r.Field<string>("TransactionType"),
                        GradeIdOrEmployeeId = r.Field<int>("GradeIdOrEmployeeId"),
                        Grade = r.Field<string>("Grade"),
                        EmplyeeName = r.Field<string>("EmplyeeName"),
                        EmployeeCode = r.Field<string>("EmployeeCode"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        DependsOn = r.Field<int?>("DependsOn"),
                        DependsOnHead = r.Field<string>("DependsOnHead"),
                        Amount = r.Field<decimal>("Amount"),
                        AmountType = r.Field<string>("AmountType"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();

                    //using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    //{
                    //    if (reader != null)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                    //            salaryFormulaBO.TransactionType = reader["TransactionType"].ToString();
                    //            salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                    //            salaryFormulaBO.Grade = reader["Grade"].ToString();
                    //            salaryFormulaBO.EmplyeeName = reader["EmplyeeName"].ToString();
                    //            salaryFormulaBO.EmployeeCode = reader["EmployeeCode"].ToString();
                    //            salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                    //            salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                    //            salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                    //            salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                    //            salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                    //            salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                    //            salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                    //            salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                    //        }
                    //    }
                    //}
                }
            }
            return salaryFormula;
        }

        public List<SalaryFormulaBO> GetSalaryFormulaByEmpGradeId(int empId)
        {
            List<SalaryFormulaBO> salaryFormula = new List<SalaryFormulaBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryFormulaByEmpGradeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "SalaryFormula");
                    DataTable Table = incrementDS.Tables["SalaryFormula"];

                    salaryFormula = Table.AsEnumerable().Select(r => new SalaryFormulaBO
                    {
                        FormulaId = r.Field<int>("FormulaId"),
                        TransactionType = r.Field<string>("TransactionType"),
                        GradeIdOrEmployeeId = r.Field<int>("GradeIdOrEmployeeId"),
                        Grade = r.Field<string>("Grade"),
                        EmplyeeName = r.Field<string>("EmplyeeName"),
                        EmployeeCode = r.Field<string>("EmployeeCode"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        DependsOn = r.Field<int?>("DependsOn"),
                        DependsOnHead = r.Field<string>("DependsOnHead"),
                        Amount = r.Field<decimal>("Amount"),
                        AmountType = r.Field<string>("AmountType"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();

                }
            }
            return salaryFormula;
        }
        public List<SalaryFormulaBO> GetBothContributionSalaryFormulaByEmpGradeId(int empId)
        {
            List<SalaryFormulaBO> salaryFormula = new List<SalaryFormulaBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBothContributionSalaryFormulaByEmpGradeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "SalaryFormula");
                    DataTable Table = incrementDS.Tables["SalaryFormula"];

                    salaryFormula = Table.AsEnumerable().Select(r => new SalaryFormulaBO
                    {
                        FormulaId = r.Field<int>("FormulaId"),
                        TransactionType = r.Field<string>("TransactionType"),
                        GradeIdOrEmployeeId = r.Field<int>("GradeIdOrEmployeeId"),
                        Grade = r.Field<string>("Grade"),
                        EmplyeeName = r.Field<string>("EmplyeeName"),
                        EmployeeCode = r.Field<string>("EmployeeCode"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        DependsOn = r.Field<int?>("DependsOn"),
                        DependsOnHead = r.Field<string>("DependsOnHead"),
                        Amount = r.Field<decimal>("Amount"),
                        AmountType = r.Field<string>("AmountType"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();

                }
            }
            return salaryFormula;
        }

        public List<SalaryFormulaBO> GetSalaryFormulaInfo()
        {
            List<SalaryFormulaBO> salaryFormulaList = new List<SalaryFormulaBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryFormulaInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();

                                salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                                salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                                salaryFormulaBO.Grade = reader["Grade"].ToString();
                                salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                                salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                                salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                                salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                                salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryFormulaList.Add(salaryFormulaBO);
                            }
                        }
                    }
                }
            }
            return salaryFormulaList;
        }
        public List<SalaryFormulaBO> GetSalaryFormulaInfoBySearchCritaria(SalaryFormulaBO salaryFormula)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenarateWhereCondition(salaryFormula);

            List<SalaryFormulaBO> salaryFormulaList = new List<SalaryFormulaBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryFormulaInfoBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrEmpty(salaryFormula.TransactionType))
                        dbSmartAspects.AddInParameter(cmd, "@TracsactionType", DbType.String, salaryFormula.TransactionType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TracsactionType", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(salaryFormula.EmployeeCode))
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, salaryFormula.EmployeeCode);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(salaryFormula.EmplyeeName))
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, salaryFormula.EmplyeeName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, DBNull.Value);

                    if (salaryFormula.GradeIdOrEmployeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GradeOrEmpId", DbType.Int32, salaryFormula.GradeIdOrEmployeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GradeOrEmpId", DbType.Int32, DBNull.Value);

                    if (salaryFormula.SalaryHeadId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SalaryHeadId", DbType.Int32, salaryFormula.SalaryHeadId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalaryHeadId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, salaryFormula.ActiveStat);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();

                                salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                                salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                                salaryFormulaBO.Grade = reader["Grade"].ToString();
                                salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                                salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                                salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                                salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                salaryFormulaBO.SalaryType = reader["SalaryType"].ToString();
                                salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                                salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryFormulaList.Add(salaryFormulaBO);
                            }
                        }
                    }
                }
            }
            return salaryFormulaList;
        }

        public List<SalaryFormulaBO> GetBothContributionSalaryFormulaInfoBySearchCritaria(SalaryFormulaBO salaryFormula)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenarateWhereCondition(salaryFormula);

            List<SalaryFormulaBO> salaryFormulaList = new List<SalaryFormulaBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBothContributionSalaryFormulaInfoBySearchCritaria_SP"))
                {
                    if (!string.IsNullOrEmpty(salaryFormula.TransactionType))
                        dbSmartAspects.AddInParameter(cmd, "@TracsactionType", DbType.String, salaryFormula.TransactionType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TracsactionType", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(salaryFormula.EmployeeCode))
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, salaryFormula.EmployeeCode);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpCode", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(salaryFormula.EmplyeeName))
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, salaryFormula.EmplyeeName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, DBNull.Value);

                    if (salaryFormula.GradeIdOrEmployeeId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GradeOrEmpId", DbType.Int32, salaryFormula.GradeIdOrEmployeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GradeOrEmpId", DbType.Int32, DBNull.Value);

                    if (salaryFormula.SalaryHeadId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SalaryHeadId", DbType.Int32, salaryFormula.SalaryHeadId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalaryHeadId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, salaryFormula.ActiveStat);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryFormulaBO salaryFormulaBO = new SalaryFormulaBO();

                                salaryFormulaBO.FormulaId = Convert.ToInt32(reader["FormulaId"].ToString());
                                salaryFormulaBO.GradeIdOrEmployeeId = Convert.ToInt32(reader["GradeIdOrEmployeeId"].ToString());
                                salaryFormulaBO.Grade = reader["Grade"].ToString();
                                salaryFormulaBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"].ToString());
                                salaryFormulaBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryFormulaBO.DependsOn = Convert.ToInt32(reader["DependsOn"].ToString());
                                salaryFormulaBO.DependsOnHead = reader["DependsOnHead"].ToString();
                                salaryFormulaBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                salaryFormulaBO.SalaryType = reader["SalaryType"].ToString();
                                salaryFormulaBO.AmountType = reader["AmountType"].ToString();
                                salaryFormulaBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryFormulaBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryFormulaList.Add(salaryFormulaBO);
                            }
                        }
                    }
                }
            }
            return salaryFormulaList;
        }
        public string GenarateWhereCondition(SalaryFormulaBO salaryFormula)
        {
            string Where = string.Empty;
            string Condition = string.Empty;
            int active = -1;
            if (salaryFormula.ActiveStat == true)
            {
                active = 1;
            }
            else
            {
                active = 0;
            }
            if (!string.IsNullOrEmpty(salaryFormula.ActiveStat.ToString()))
            {
                Condition += " sf.ActiveStat =" + "'" + active + "'" + "";
            }

            if (!string.IsNullOrEmpty(salaryFormula.SalaryHeadId.ToString()))
            {
                if (salaryFormula.SalaryHeadId != 0)
                {
                    if (!string.IsNullOrEmpty(Condition))
                    {
                        Condition += " AND sf.SalaryHeadId=" + "'" + salaryFormula.SalaryHeadId + "'" + "";
                    }
                    else
                    {
                        Condition += " sf.SalaryHeadId=" + "'" + salaryFormula.SalaryHeadId + "'" + "";
                    }
                }
            }



            if (salaryFormula.GradeIdOrEmployeeId != 0)
            {


                Condition += "AND sf.GradeIdOrEmployeeId =" + "'" + salaryFormula.GradeIdOrEmployeeId + "'" + "";

            }
            else
            {

                if (!string.IsNullOrEmpty(salaryFormula.EmplyeeName))
                {
                    if (!string.IsNullOrEmpty(Condition))
                    {
                        Condition += " AND pe.EmplyeeName=" + "'" + salaryFormula.EmplyeeName + "'" + "";
                    }
                    else
                    {
                        Condition += " pe.EmplyeeName=" + "'" + salaryFormula.EmplyeeName + "'" + "";
                    }
                }

                if (!string.IsNullOrEmpty(salaryFormula.EmployeeCode))
                {
                    if (!string.IsNullOrEmpty(Condition))
                    {
                        Condition += " AND pe.EmpCode=" + "'" + salaryFormula.EmployeeCode + "'" + "";
                    }
                    else
                    {
                        Condition += " pe.EmpCode=" + "'" + salaryFormula.EmployeeCode + "'" + "";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Condition))
            {
                Where = " WHERE " + Condition;
            }
            else
            {
                Where = Condition;
            }
            return Where;
        }

        public bool UpdateEmployeeOrGradeWiseBasicGrossSalary(List<EmpGradeBO> grade, List<EmployeeBO> employee, string transactionType)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeOrGradeWiseBasicGrossSalary_SP"))
                    {
                        if (transactionType == "Individual")
                        {
                            foreach (EmployeeBO e in employee)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, transactionType);
                                dbSmartAspects.AddInParameter(command, "@GradeId", DbType.Int32, DBNull.Value);
                                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, e.EmpId);
                                dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, e.BasicAmount);
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, e.GlCompanyId);
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, e.GlProjectId);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                        else if (transactionType == "Grade")
                        {
                            foreach (EmpGradeBO g in grade)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, transactionType);
                                dbSmartAspects.AddInParameter(command, "@GradeId", DbType.Int32, g.GradeId);
                                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, DBNull.Value);
                                dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, g.BasicAmount);
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
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
    }
}
