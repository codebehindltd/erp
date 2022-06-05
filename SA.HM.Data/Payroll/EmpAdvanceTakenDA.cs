using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpAdvanceTakenDA : BaseService
    {
        public Boolean SaveEmpAdvanceTakenInfo(EmpAdvanceTakenBO advTknInfo, out int advanceTakenId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpAdvanceTakenInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, advTknInfo.EmpId);
                    dbSmartAspects.AddInParameter(command, "@AdvanceDate", DbType.DateTime, advTknInfo.AdvanceDate);
                    dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, advTknInfo.AdvanceAmount);
                    dbSmartAspects.AddInParameter(command, "@PayMonth", DbType.String, advTknInfo.PayMonth);
                    dbSmartAspects.AddInParameter(command, "@IsDeductFromSalary", DbType.Boolean, advTknInfo.IsDeductFromSalary);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, advTknInfo.Remarks);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, advTknInfo.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@AdvanceTakenId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    advanceTakenId = Convert.ToInt32(command.Parameters["@AdvanceTakenId"].Value);
                }
            }
            return status;
        }

        public Boolean UpdateEmpAdvanceTakenInfo(EmpAdvanceTakenBO advTknInfo)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpAdvanceTakenInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AdvanceId", DbType.Int32, advTknInfo.AdvanceId);
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, advTknInfo.EmpId);
                    dbSmartAspects.AddInParameter(command, "@AdvanceDate", DbType.DateTime, advTknInfo.AdvanceDate);
                    dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, advTknInfo.AdvanceAmount);
                    dbSmartAspects.AddInParameter(command, "@PayMonth", DbType.String, advTknInfo.PayMonth);
                    dbSmartAspects.AddInParameter(command, "@IsDeductFromSalary", DbType.Boolean, advTknInfo.IsDeductFromSalary);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, advTknInfo.Remarks);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, advTknInfo.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<EmpAdvanceTakenBO> GetAdvanceTakenInfoBySearchCriteriaForPagination(int? empId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpAdvanceTakenBO> advtknList = new List<EmpAdvanceTakenBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAdvanceTakenInfoBySearchCriteriaForPagination_SP"))
                {
                    if (empId != null)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmpAdvanceTaken");
                    DataTable Table = ds.Tables["PayrollEmpAdvanceTaken"];

                    advtknList = Table.AsEnumerable().Select(r => new EmpAdvanceTakenBO
                    {
                        AdvanceId = r.Field<Int32>("AdvanceId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        AdvanceDate = r.Field<DateTime>("AdvanceDate"),
                        AdvanceAmount = r.Field<Decimal>("AdvanceAmount"),
                        PayMonth = r.Field<string>("PayMonth"),
                        IsDeductFromSalary = r.Field<Boolean>("IsDeductFromSalary"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        //ApprovedDate = r.Field<DateTime>("ApprovedDate"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return advtknList;
        }

        public EmpAdvanceTakenBO GetAdvanceTakenInfoById(int advanceTakenId)
        {
            EmpAdvanceTakenBO advanceTakenBO = new EmpAdvanceTakenBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAdvanceTakenInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AdvanceId", DbType.Int32, advanceTakenId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollEmpAdvanceTaken");
                    DataTable Table = ds.Tables["PayrollEmpAdvanceTaken"];

                    advanceTakenBO = Table.AsEnumerable().Select(r => new EmpAdvanceTakenBO
                    {
                        AdvanceId = r.Field<Int32>("AdvanceId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmpName = r.Field<string>("EmployeeName"),
                        AdvanceDate = r.Field<DateTime>("AdvanceDate"),
                        AdvanceAmount = r.Field<Decimal>("AdvanceAmount"),
                        PayMonth = r.Field<string>("PayMonth"),
                        IsDeductFromSalary = r.Field<Boolean>("IsDeductFromSalary"),
                        Remarks = r.Field<string>("Remarks"),

                    }).FirstOrDefault();
                }
            }
            return advanceTakenBO;
        }

        public Boolean DeleteAdvtknInfoById(int advanceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAdvTknInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AdvanceId", DbType.Int32, advanceId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool UpdateAdvanceTakenNApprovedStatus(int advanceId, string cancel, string approved, int modifiedBy)
        {
            Boolean status = false;            
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateAdvanceTakenNApprovedStatus_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@AdvanceId", DbType.Int32, advanceId);
                        //dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);


                        if (!string.IsNullOrEmpty(approved))
                            dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approved);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, cancel);

                        dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.String, modifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false; ;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }

            return status;
        }
    }
}
