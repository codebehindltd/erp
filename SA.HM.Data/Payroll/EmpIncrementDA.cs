using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpIncrementDA : BaseService
    {
        public List<EmpIncrementBO> GetAllIncrement()
        {
            List<EmpIncrementBO> IncreamentList = new List<EmpIncrementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllIncreamentInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpIncrementBO increamentBO = new EmpIncrementBO();
                                increamentBO.Id = Convert.ToInt32(reader["Id"]);
                                increamentBO.Remarks = reader["Remarks"].ToString();
                                increamentBO.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                                increamentBO.IncrementMode = reader["IncrementMode"].ToString();
                                increamentBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                increamentBO.EmployeeName = reader["EmployeeName"].ToString();
                                increamentBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                increamentBO.ShowIncrementDate = reader["ShowIncrementDate"].ToString();
                                increamentBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"].ToString());
                                increamentBO.IncrementAmount = Convert.ToDecimal(reader["IncrementAmount"].ToString());
                                increamentBO.IncrementRate = Convert.ToDecimal(reader["IncrementRate"].ToString());

                                IncreamentList.Add(increamentBO);
                            }
                        }
                    }
                }
            }
            return IncreamentList;
        }
        public List<EmpIncrementBO> GetIncrementByEmpId(int empId)
        {
            List<EmpIncrementBO> IncreamentList = new List<EmpIncrementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetIncrementByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpIncrementBO increamentBO = new EmpIncrementBO();
                                increamentBO.Id = Convert.ToInt32(reader["Id"]);
                                increamentBO.Remarks = reader["Remarks"].ToString();
                                increamentBO.BasicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                                increamentBO.IncrementMode = reader["IncrementMode"].ToString();
                                increamentBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                increamentBO.EmployeeName = reader["EmployeeName"].ToString();
                                increamentBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                increamentBO.ShowIncrementDate = reader["ShowIncrementDate"].ToString();
                                increamentBO.TotalAmount = Convert.ToDecimal(reader["TotalAmount"].ToString());
                                increamentBO.IncrementAmount = Convert.ToDecimal(reader["IncrementAmount"].ToString());
                                increamentBO.IncrementRate = Convert.ToDecimal(reader["IncrementRate"].ToString());

                                IncreamentList.Add(increamentBO);
                            }
                        }
                    }
                }
            }
            return IncreamentList;
        }

        public bool SaveEmpIncrementInfo(EmpIncrementBO increamentBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveIncreamentInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, increamentBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, increamentBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@EffectiveDate", DbType.DateTime, increamentBO.EffectiveDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, increamentBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IncrementMode", DbType.String, increamentBO.IncrementMode);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, increamentBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@Id"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public bool UpdateEmpIncrementInfo(EmpIncrementBO increamentBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpIncreamentInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, increamentBO.Id);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, increamentBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, increamentBO.Amount);
                        dbSmartAspects.AddInParameter(command, "@EffectiveDate", DbType.DateTime, increamentBO.EffectiveDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, increamentBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IncrementMode", DbType.String, increamentBO.IncrementMode);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, increamentBO.LastModifiedBy);
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

        public EmpIncrementBO GetEmpIncrementInfoById(int EditId)
        {
            EmpIncrementBO entityBO = new EmpIncrementBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpIncreamentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.Id = Convert.ToInt32(reader["Id"]);
                                entityBO.Remarks = reader["Remarks"].ToString();
                                entityBO.IncrementMode = reader["IncrementMode"].ToString();
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"].ToString());
                                entityBO.EffectiveDate = Convert.ToDateTime(reader["EffectiveDate"].ToString());
                                entityBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                entityBO.EmpCode = reader["EmpCode"].ToString();
                                if (reader["FirstName"] != DBNull.Value)
                                {
                                    entityBO.EmployeeName = reader["FirstName"].ToString();
                                }
                                if(reader["LastName"] != DBNull.Value)
                                {
                                    entityBO.EmployeeName += " " + reader["LastName"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            return entityBO;
        }

        public List<EmpIncrementBO> GetAllIncrementByEmployeeIdForGridPaging(int employeeID, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpIncrementBO> IncreamentList = new List<EmpIncrementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpIncreamentInfoByEmpIDForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Increment");
                    DataTable Table = incrementDS.Tables["Increment"];

                    IncreamentList = Table.AsEnumerable().Select(r => new EmpIncrementBO
                    {
                        Id = r.Field<int>("Id"),
                        Remarks = r.Field<string>("Remarks"),
                        IncrementMode = r.Field<string>("IncrementMode"),
                        Amount = r.Field<decimal>("Amount"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmpId = r.Field<int>("EmpId"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return IncreamentList;
        }

        public Boolean ApprovedIncrement(EmpIncrementBO increment)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedIncrement_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@IncrementId", DbType.Int32, increment.Id);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, increment.EmpId);
                        dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, increment.ApprovedStatus);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, increment.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                throw ex;
            }
            return status;
        }

        //----- Promotion 
        public bool SaveEmpPromotionInfo(EmpPromotionBO promotion, out Int64 promotionId)
        {
            Boolean status = false;
            promotionId = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollEmpPromotion_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, promotion.EmpId);

                        dbSmartAspects.AddInParameter(command, "@PromotionDate", DbType.DateTime, promotion.PromotionDate);
                        dbSmartAspects.AddInParameter(command, "@PreviousDesignationId", DbType.Int32, promotion.PreviousDesignationId);
                        dbSmartAspects.AddInParameter(command, "@PreviousGradeId", DbType.Int32, promotion.PreviousGradeId);
                        dbSmartAspects.AddInParameter(command, "@CurrentDesignationId", DbType.Int32, promotion.CurrentDesignationId);
                        dbSmartAspects.AddInParameter(command, "@CurrentGradeId", DbType.Int32, promotion.CurrentGradeId);
                        dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, promotion.ApprovalStatus);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, promotion.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, promotion.CreatedBy);

                        dbSmartAspects.AddOutParameter(command, "@PromotionId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        promotionId = Convert.ToInt32(command.Parameters["@PromotionId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public bool UpdateEmpPromotionInfo(EmpPromotionBO promotion)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePayrollEmpPromotion_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@PromotionId", DbType.Int64, promotion.PromotionId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, promotion.EmpId);
                        dbSmartAspects.AddInParameter(command, "@PromotionDate", DbType.DateTime, promotion.PromotionDate);
                        dbSmartAspects.AddInParameter(command, "@PreviousDesignationId", DbType.Int32, promotion.PreviousDesignationId);
                        dbSmartAspects.AddInParameter(command, "@PreviousGradeId", DbType.Int32, promotion.PreviousGradeId);
                        dbSmartAspects.AddInParameter(command, "@CurrentDesignationId", DbType.Int32, promotion.CurrentDesignationId);
                        dbSmartAspects.AddInParameter(command, "@CurrentGradeId", DbType.Int32, promotion.CurrentGradeId);
                        //dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, promotion.ApprovalStatus);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, promotion.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, promotion.LastModifiedBy);

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

        public bool UpdateEmpPromotionApproval(EmpPromotionBO promotion)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpPromotionApproval_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@PromotionId", DbType.Int32, promotion.PromotionId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, promotion.EmpId);
                        dbSmartAspects.AddInParameter(command, "@ApprovalStatus", DbType.String, promotion.ApprovalStatus);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, promotion.LastModifiedBy);

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

        public List<EmpPromotionBO> GetEmployeePromotion(int empId, int departmentId)
        {
            List<EmpPromotionBO> entityBO = new List<EmpPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePromotion_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];

                    entityBO = Table.AsEnumerable().Select(r => new EmpPromotionBO
                    {
                        PromotionId = r.Field<Int64>("PromotionId"),
                        EmpId = r.Field<int>("EmpId"),
                        PromotionDate = r.Field<DateTime>("PromotionDate"),
                        PreviousDesignationId = r.Field<int>("PreviousDesignationId"),
                        PreviousGradeId = r.Field<int>("PreviousGradeId"),
                        CurrentDesignationId = r.Field<int>("CurrentDesignationId"),
                        CurrentGradeId = r.Field<int>("CurrentGradeId"),
                        ApprovalStatus = r.Field<string>("ApprovalStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        PromotionDateShow = r.Field<string>("PromotionDateShow"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        PreviousDesignation = r.Field<string>("PreviousDesignation"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        PreviousGrade = r.Field<string>("PreviousGrade"),
                        CurrentGrade = r.Field<string>("CurrentGrade")

                    }).ToList();
                }
            }
            return entityBO;
        }

        public List<EmpPromotionBO> GetEmployeePromotion(int empId, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<EmpPromotionBO> entityBO = new List<EmpPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePromotion_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (fromDate != DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];

                    entityBO = Table.AsEnumerable().Select(r => new EmpPromotionBO
                    {
                        PromotionId = r.Field<Int64>("PromotionId"),
                        EmpId = r.Field<int>("EmpId"),
                        PromotionDate = r.Field<DateTime>("PromotionDate"),
                        PreviousDesignationId = r.Field<int>("PreviousDesignationId"),
                        PreviousGradeId = r.Field<int>("PreviousGradeId"),
                        CurrentDesignationId = r.Field<int>("CurrentDesignationId"),
                        CurrentGradeId = r.Field<int>("CurrentGradeId"),
                        ApprovalStatus = r.Field<string>("ApprovalStatus"),
                        Remarks = r.Field<string>("Remarks"),
                        PromotionDateShow = r.Field<string>("PromotionDateShow"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        PreviousDesignation = r.Field<string>("PreviousDesignation"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        PreviousGrade = r.Field<string>("PreviousGrade"),
                        CurrentGrade = r.Field<string>("CurrentGrade")

                    }).ToList();
                }
            }
            return entityBO;
        }
        public List<EmpPromotionBO> GetEmployeePromotionForReport(int empId, int departmentId, int fiscalYearId)
        {
            List<EmpPromotionBO> entityBO = new List<EmpPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePromotionForReport_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, fiscalYearId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "EmpPromotion");
                    DataTable Table = incrementDS.Tables["EmpPromotion"];

                    entityBO = Table.AsEnumerable().Select(r => new EmpPromotionBO
                    {
                        PromotionId = r.Field<Int64>("PromotionId"),
                        EmpId = r.Field<int>("EmpId"),
                        PromotionDate = r.Field<DateTime>("PromotionDate"),
                        PreviousDesignationId = r.Field<int>("PreviousDesignationId"),
                        PreviousGradeId = r.Field<int>("PreviousGradeId"),
                        CurrentDesignationId = r.Field<int>("CurrentDesignationId"),
                        CurrentGradeId = r.Field<int>("CurrentGradeId"),
                        ApprovalStatus = r.Field<string>("ApprovalStatus"),
                        Remarks = r.Field<string>("Remarks"),

                        EmployeeName = r.Field<string>("EmployeeName"),
                        PreviousDesignation = r.Field<string>("PreviousDesignation"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        PreviousGrade = r.Field<string>("PreviousGrade"),
                        CurrentGrade = r.Field<string>("CurrentGrade")

                    }).ToList();
                }
            }
            return entityBO;
        }

        public EmpPromotionBO GetEmployeePromotion(int promotionId)
        {
            EmpPromotionBO entityBO = new EmpPromotionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeePromotionById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PromotionId", DbType.Int32, promotionId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];

                    entityBO = Table.AsEnumerable().Select(r => new EmpPromotionBO
                    {
                        PromotionId = r.Field<Int64>("PromotionId"),
                        EmpId = r.Field<int>("EmpId"),
                        PromotionDate = r.Field<DateTime>("PromotionDate"),
                        PreviousDesignationId = r.Field<int>("PreviousDesignationId"),
                        PreviousGradeId = r.Field<int>("PreviousGradeId"),
                        CurrentDesignationId = r.Field<int>("CurrentDesignationId"),
                        CurrentGradeId = r.Field<int>("CurrentGradeId"),
                        ApprovalStatus = r.Field<string>("ApprovalStatus"),
                        Remarks = r.Field<string>("Remarks"),

                        EmployeeName = r.Field<string>("EmployeeName"),
                        PreviousDesignation = r.Field<string>("PreviousDesignation"),
                        CurrentDesignation = r.Field<string>("CurrentDesignation"),
                        PreviousGrade = r.Field<string>("PreviousGrade"),
                        CurrentGrade = r.Field<string>("CurrentGrade"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        EmpCode = r.Field<string>("EmpCode")

                    }).FirstOrDefault();
                }
            }
            return entityBO;
        }
        
        public List<EmpIncrementBO> GetEmployeeIncrement(int empId, int departmentId)
        {
            List<EmpIncrementBO> entityBO = new List<EmpIncrementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeIncrement_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];
                    try
                    {

                        entityBO = Table.AsEnumerable().Select(r => new EmpIncrementBO
                        {
                            Id = r.Field<int>("IncrementId"),
                            IncrementDate = r.Field<DateTime>("IncrementDate"),
                            IncrementMode = r.Field<string>("IncrementMode"),
                            Amount = r.Field<decimal>("Amount"),
                            EffectiveDate = r.Field<DateTime>("EffectiveDate"),
                            Remarks = r.Field<string>("Remarks"),
                            ApprovedStatus = r.Field<string>("ApprovedStatus"),
                            EmpId = r.Field<int>("EmpId"),
                            EmployeeName = r.Field<string>("EmployeeName"),
                            BasicSalary = r.Field<decimal>("BasicSalary"),
                            CreatedBy = r.Field<int>("CreatedBy"),
                            LastModifiedBy = r.Field<int>("LastModifiedBy")
                        }).ToList();
                    }
                    catch(Exception exp)
                    {
                        //Do nothing
                    }
                }
            }
            return entityBO;
        }

        public List<EmpIncrementBO> GetEmployeeIncrement(int empId, int departmentId, DateTime fromDate, DateTime toDate)
        {
            List<EmpIncrementBO> entityBO = new List<EmpIncrementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeIncrement_SP"))
                {
                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (fromDate != DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != DateTime.MinValue)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];
                    try
                    {
                        entityBO = Table.AsEnumerable().Select(r => new EmpIncrementBO
                        {
                            Id = r.Field<int>("IncrementId"),
                            IncrementDate = r.Field<DateTime>("IncrementDate"),
                            IncrementDateDisplay = r.Field<string>("IncrementDateDisplay"),
                            IncrementMode = r.Field<string>("IncrementMode"),
                            Amount = r.Field<decimal>("Amount"),
                            EffectiveDate = r.Field<DateTime>("EffectiveDate"),
                            Remarks = r.Field<string>("Remarks"),
                            ApprovedStatus = r.Field<string>("ApprovedStatus"),
                            EmpId = r.Field<int>("EmpId"),
                            EmployeeName = r.Field<string>("EmployeeName"),
                            EmployeeCodeAndName = r.Field<string>("EmployeeCodeAndName"),
                            BasicSalary = r.Field<decimal>("BasicSalary"),
                            CreatedBy = r.Field<int>("CreatedBy"),
                            LastModifiedBy = r.Field<int>("LastModifiedBy")
                        }).ToList();
                    }
                    catch (Exception exp)
                    {
                        //Do nothing
                    }
                }
            }
            return entityBO;
        }
    }
}
