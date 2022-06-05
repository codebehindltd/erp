using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpAllowanceDeductionDA : BaseService
    {
        public List<EmpAllowanceDeductionBO> GetEmpAllowanceDeduction()
        {
            List<EmpAllowanceDeductionBO> List = new List<EmpAllowanceDeductionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAllowanceDeduction_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpAllowanceDeductionBO deductionBO = new EmpAllowanceDeductionBO();

                                deductionBO.EmpAllowDeductId = Int32.Parse(reader["EmpAllowDeductId"].ToString());
                                deductionBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                deductionBO.SalaryHeadId = Int32.Parse(reader["SalaryHeadId"].ToString());
                                deductionBO.SalaryHead = reader["SalaryHead"].ToString();
                                deductionBO.AllowDeductAmount = Convert.ToDecimal(reader["AllowDeductAmount"]);
                                deductionBO.EffectFrom = Convert.ToDateTime(reader["EffectFrom"].ToString());
                                deductionBO.EffectTo = Convert.ToDateTime(reader["EffectTo"].ToString());
                                deductionBO.DependsOn = Convert.ToString(reader["DependsOn"].ToString());
                                deductionBO.EffectiveYear = Convert.ToInt32(reader["EffectiveYear"].ToString());
                                deductionBO.Remarks = reader["Remarks"].ToString();

                                List.Add(deductionBO);
                            }
                        }
                    }
                }
            }
            return List;
        }

        public List<EmpAllowanceDeductionBO> GetEmpAllowanceDeductionForGridPaging(string allowDeductType, int employeeID, DateTime effectFrom, DateTime effectTo, int effectedYear, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpAllowanceDeductionBO> IncreamentList = new List<EmpAllowanceDeductionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAllowanceDeductionByEmpIdForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AllowDeductType", DbType.String, allowDeductType);

                    if (employeeID != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@EffectFrom", DbType.DateTime, effectFrom);
                    dbSmartAspects.AddInParameter(cmd, "@EffectTo", DbType.DateTime, effectTo);
                    dbSmartAspects.AddInParameter(cmd, "@EffectiveYear", DbType.Int32, effectedYear);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Increment");
                    DataTable Table = incrementDS.Tables["Increment"];

                    IncreamentList = Table.AsEnumerable().Select(r => new EmpAllowanceDeductionBO
                    {
                        EmpAllowDeductId = r.Field<int>("EmpAllowDeductId"),
                        EmpId = r.Field<int?>("EmpId"),
                        SalaryHeadId = r.Field<int>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        AllowDeductAmount = r.Field<decimal>("AllowDeductAmount"),
                        AmountType = r.Field<string>("AmountType"),
                        EffectFrom = r.Field<DateTime>("EffectFrom"),
                        EffectTo = r.Field<DateTime>("EffectTo"),
                        DependsOn = r.Field<string>("DependsOn"),
                        EffectiveYear = r.Field<int>("EffectiveYear"),
                        Remarks = r.Field<string>("Remarks"),
                        SalaryType = r.Field<string>("SalaryType"),
                        CanEditDelete = r.Field<bool>("CanEditDelete")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return IncreamentList;
        }

        public EmpAllowanceDeductionBO GetEmpAllowanceDeductionInfoByID(int EditId)
        {
            EmpAllowanceDeductionBO deductionBO = new EmpAllowanceDeductionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAllowanceDeductionInfoByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpAllowDeductId", DbType.Int32, EditId);

                    DataSet SalaryHeadDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SalaryHeadDS, "SalaryHead");
                    DataTable Table = SalaryHeadDS.Tables["SalaryHead"];

                    deductionBO = Table.AsEnumerable().Select(r => new EmpAllowanceDeductionBO
                    {
                        EmpAllowDeductId = r.Field<Int32>("EmpAllowDeductId"),
                        AllowDeductType = r.Field<string>("AllowDeductType"),
                        DepartmentId = r.Field<Int32?>("DepartmentId"),
                        EmpId = r.Field<Int32?>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        SalaryHeadId = r.Field<Int32>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        AllowDeductAmount = r.Field<decimal>("AllowDeductAmount"),
                        AmountType = r.Field<string>("AmountType"),
                        EffectFrom = r.Field<DateTime>("EffectFrom"),
                        EffectTo = r.Field<DateTime>("EffectTo"),
                        DependsOn = r.Field<string>("DependsOn"),
                        EffectiveYear = r.Field<int>("EffectiveYear"),
                        Remarks = r.Field<string>("Remarks"),
                        SalaryType = r.Field<string>("SalaryType")

                    }).FirstOrDefault();
                }
            }
            return deductionBO;
        }

        public List<EmpAllowanceDeductionBO> GetEmpAllowanceDeductionInfoByDepartmentId(int departmentId, DateTime effectFrom, DateTime effectTo, int effectiveYear)
        {
            List<EmpAllowanceDeductionBO> deductionBO = new List<EmpAllowanceDeductionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAllowanceDeductionInfoByDepartmentId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    dbSmartAspects.AddInParameter(cmd, "@EffectFrom", DbType.DateTime, effectFrom);
                    dbSmartAspects.AddInParameter(cmd, "@EffectTo", DbType.DateTime, effectTo);
                    dbSmartAspects.AddInParameter(cmd, "@EffectiveYear", DbType.Int32, effectiveYear);

                    DataSet SalaryHeadDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SalaryHeadDS, "SalaryHead");
                    DataTable Table = SalaryHeadDS.Tables["SalaryHead"];

                    deductionBO = Table.AsEnumerable().Select(r => new EmpAllowanceDeductionBO
                    {
                        EmpAllowDeductId = r.Field<Int32>("EmpAllowDeductId"),
                        AllowDeductType = r.Field<string>("AllowDeductType"),
                        DepartmentId = r.Field<Int32?>("DepartmentId"),
                        EmpId = r.Field<Int32?>("EmpId"),
                        SalaryHeadId = r.Field<Int32>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        AllowDeductAmount = r.Field<decimal>("AllowDeductAmount"),
                        AmountType = r.Field<string>("AmountType"),
                        EffectFrom = r.Field<DateTime>("EffectFrom"),
                        EffectTo = r.Field<DateTime>("EffectTo"),
                        DependsOn = r.Field<string>("DependsOn"),
                        EffectiveYear = r.Field<int>("EffectiveYear"),
                        Remarks = r.Field<string>("Remarks"),
                        SalaryType = r.Field<string>("SalaryType")

                    }).ToList();
                }
            }
            return deductionBO;
        }

        public bool SaveAllowanceDeductionInfo(EmpAllowanceDeductionBO deductionBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAllowanceDeductionInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@AllowDeductType", DbType.String, deductionBO.AllowDeductType);

                        if (deductionBO.DepartmentId != null)
                            dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, deductionBO.DepartmentId);
                        else
                            dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, DBNull.Value);

                        if (deductionBO.EmpId != null)
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, deductionBO.EmpId);
                        else
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, deductionBO.SalaryHeadId);
                        dbSmartAspects.AddInParameter(command, "@AllowDeductAmount", DbType.Decimal, deductionBO.AllowDeductAmount);
                        dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, deductionBO.AmountType);
                        dbSmartAspects.AddInParameter(command, "@EffectFrom", DbType.DateTime, deductionBO.EffectFrom);
                        dbSmartAspects.AddInParameter(command, "@EffectTo", DbType.DateTime, deductionBO.EffectTo);
                        dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.String, deductionBO.DependsOn);
                        dbSmartAspects.AddInParameter(command, "@EffectiveYear", DbType.Int32, deductionBO.EffectiveYear);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, deductionBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, deductionBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@EmpAllowDeductId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@EmpAllowDeductId"].Value);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return status;
        }

        public bool UpdateAllowanceDeductionInfo(Entity.Payroll.EmpAllowanceDeductionBO deductionBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAllowanceDeductionInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpAllowDeductId", DbType.Int32, deductionBO.EmpAllowDeductId);
                    dbSmartAspects.AddInParameter(command, "@AllowDeductType", DbType.String, deductionBO.AllowDeductType);

                    if (deductionBO.DepartmentId != null)
                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, deductionBO.DepartmentId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (deductionBO.EmpId != null)
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, deductionBO.EmpId);
                    else
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, deductionBO.SalaryHeadId);
                    dbSmartAspects.AddInParameter(command, "@AllowDeductAmount", DbType.Decimal, deductionBO.AllowDeductAmount);
                    dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, deductionBO.AmountType);
                    dbSmartAspects.AddInParameter(command, "@EffectFrom", DbType.DateTime, deductionBO.EffectFrom);
                    dbSmartAspects.AddInParameter(command, "@EffectTo", DbType.DateTime, deductionBO.EffectTo);
                    dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.String, deductionBO.DependsOn);
                    dbSmartAspects.AddInParameter(command, "@EffectiveYear", DbType.Int32, deductionBO.EffectiveYear);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, deductionBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, deductionBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool SaveAllowanceDeductionInfo(EmpAllowanceDeductionBO salaryAddDeduction, List<EmpAllowanceDeductionBO> EmpLst, List<EmpAllowanceDeductionBO> EmpEditLst, List<EmpAllowanceDeductionBO> EmpDeletedLst, out int tmpUserInfoId)
        {
            Boolean status = false;
            tmpUserInfoId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        if (EmpLst.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAllowanceDeductionInfo_SP"))
                            {
                                foreach (EmpAllowanceDeductionBO emp in EmpLst)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@AllowDeductType", DbType.String, salaryAddDeduction.AllowDeductType);

                                    if (salaryAddDeduction.DepartmentId != null)
                                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, salaryAddDeduction.DepartmentId);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, DBNull.Value);

                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, emp.EmpId);

                                    dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryAddDeduction.SalaryHeadId);
                                    dbSmartAspects.AddInParameter(command, "@AllowDeductAmount", DbType.Decimal, salaryAddDeduction.AllowDeductAmount);
                                    dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, salaryAddDeduction.AmountType);
                                    dbSmartAspects.AddInParameter(command, "@EffectFrom", DbType.DateTime, salaryAddDeduction.EffectFrom);
                                    dbSmartAspects.AddInParameter(command, "@EffectTo", DbType.DateTime, salaryAddDeduction.EffectTo);
                                    dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.String, salaryAddDeduction.DependsOn);
                                    dbSmartAspects.AddInParameter(command, "@EffectiveYear", DbType.Int32, salaryAddDeduction.EffectiveYear);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, salaryAddDeduction.Remarks);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salaryAddDeduction.CreatedBy);
                                    dbSmartAspects.AddOutParameter(command, "@EmpAllowDeductId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                    tmpUserInfoId = Convert.ToInt32(command.Parameters["@EmpAllowDeductId"].Value);
                                }
                            }
                        }

                        if (EmpEditLst.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAllowanceDeductionInfo_SP"))
                            {
                                foreach (EmpAllowanceDeductionBO emp in EmpEditLst)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmpAllowDeductId", DbType.Int32, emp.EmpAllowDeductId);
                                    dbSmartAspects.AddInParameter(command, "@AllowDeductType", DbType.String, salaryAddDeduction.AllowDeductType);

                                    if (salaryAddDeduction.DepartmentId != null)
                                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, salaryAddDeduction.DepartmentId);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, DBNull.Value);

                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, emp.EmpId);

                                    dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryAddDeduction.SalaryHeadId);
                                    dbSmartAspects.AddInParameter(command, "@AllowDeductAmount", DbType.Decimal, salaryAddDeduction.AllowDeductAmount);
                                    dbSmartAspects.AddInParameter(command, "@AmountType", DbType.String, salaryAddDeduction.AmountType);
                                    dbSmartAspects.AddInParameter(command, "@EffectFrom", DbType.DateTime, salaryAddDeduction.EffectFrom);
                                    dbSmartAspects.AddInParameter(command, "@EffectTo", DbType.DateTime, salaryAddDeduction.EffectTo);
                                    dbSmartAspects.AddInParameter(command, "@DependsOn", DbType.String, salaryAddDeduction.DependsOn);
                                    dbSmartAspects.AddInParameter(command, "@EffectiveYear", DbType.Int32, salaryAddDeduction.EffectiveYear);
                                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, salaryAddDeduction.Remarks);
                                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, salaryAddDeduction.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }

                        if (EmpDeletedLst.Count > 0)
                        {

                            using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (EmpAllowanceDeductionBO emp in EmpDeletedLst)
                                {
                                    commandReference.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandReference, "@TableName", DbType.String, "PayrollEmpAllowanceDeduction");
                                    dbSmartAspects.AddInParameter(commandReference, "@TablePKField", DbType.String, "EmpAllowDeductId");
                                    dbSmartAspects.AddInParameter(commandReference, "@TablePKId", DbType.String, emp.EmpAllowDeductId.ToString());

                                    status = dbSmartAspects.ExecuteNonQuery(commandReference) > 0 ? true : false;
                                }
                            }
                        }

                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transaction.Rollback();
                        throw ex;
                    }

                }
            }

            return status;
        }
    }
}
