using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.Payroll
{
    public class DisciplinaryActionDA : BaseService
    {
        //Disciplinary Action-----------------------------------------------------------------
        public Boolean SaveDisciplinaryActionInfo(DisciplinaryActionBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDisciplinaryActionInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ActionTypeId", DbType.Int16, bo.ActionTypeId);
                        dbSmartAspects.AddInParameter(command, "@DisciplinaryActionReasonId", DbType.Int32, bo.DisciplinaryActionReasonId);
                        dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, bo.EmployeeId);
                        dbSmartAspects.AddInParameter(command, "@ProposedActionId", DbType.Int32, bo.ProposedActionId);
                        dbSmartAspects.AddInParameter(command, "@ActionBody", DbType.String, bo.ActionBody);
                        dbSmartAspects.AddInParameter(command, "@ApplicableDate", DbType.DateTime, bo.ApplicableDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);

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
        public Boolean UpdateDisciplinaryActionInfo(DisciplinaryActionBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDisciplinaryActionInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DisciplinaryActionId", DbType.Int64, bo.DisciplinaryActionId);
                        dbSmartAspects.AddInParameter(command, "@ActionTypeId", DbType.Int16, bo.ActionTypeId);
                        dbSmartAspects.AddInParameter(command, "@DisciplinaryActionReasonId", DbType.Int32, bo.DisciplinaryActionReasonId);
                        dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, bo.EmployeeId);
                        dbSmartAspects.AddInParameter(command, "@ProposedActionId", DbType.Int32, bo.ProposedActionId);
                        dbSmartAspects.AddInParameter(command, "@ActionBody", DbType.String, bo.ActionBody);
                        dbSmartAspects.AddInParameter(command, "@ApplicableDate", DbType.DateTime, bo.ApplicableDate);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bo.LastModifiedBy);

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
        public List<DisciplinaryActionBO> GetDisciplinaryActionList()
        {
            List<DisciplinaryActionBO> boList = new List<DisciplinaryActionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryAction");
                    DataTable Table = ds.Tables["DisciplinaryAction"];

                    boList = Table.AsEnumerable().Select(r => new DisciplinaryActionBO
                    {
                        DisciplinaryActionId = r.Field<Int64>("DisciplinaryActionId"),
                        ActionTypeId = r.Field<Int16>("ActionTypeId"),
                        DisciplinaryActionReasonId = r.Field<Int32>("DisciplinaryActionReasonId"),
                        ActionBody = r.Field<string>("ActionBody"),
                        ProposedActionId = r.Field<int?>("ProposedActionId"),
                        ApplicableDate = r.Field<DateTime?>("ApplicableDate")

                    }).ToList();
                }
            }
            return boList;
        }
        public DisciplinaryActionBO GetDisciplinaryActionById(int pkId)
        {
            DisciplinaryActionBO bo = new DisciplinaryActionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionId", DbType.Int64, pkId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryAction");
                    DataTable Table = ds.Tables["DisciplinaryAction"];

                    bo = Table.AsEnumerable().Select(r => new DisciplinaryActionBO
                    {
                        DisciplinaryActionId = r.Field<Int64>("DisciplinaryActionId"),
                        ActionTypeId = r.Field<Int16>("ActionTypeId"),
                        DisciplinaryActionReasonId = r.Field<Int32>("DisciplinaryActionReasonId"),
                        EmployeeId = r.Field<Int32>("EmployeeId"),
                        ActionBody = r.Field<string>("ActionBody"),
                        ProposedActionId = r.Field<int?>("ProposedActionId"),
                        ApplicableDate = r.Field<DateTime?>("ApplicableDate")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public List<DisciplinaryActionBO> GetDisciplinaryActionBySearchCriteriaForPaging(int actionTypeId, int actionReasonId, int empId, int proposedActionId, string fromDate, string toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<DisciplinaryActionBO> boList = new List<DisciplinaryActionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionInfoBySearchCriteriaForPaging_SP"))
                {
                    if (actionTypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ActionTypeId", DbType.Int16, actionTypeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ActionTypeId", DbType.Int16, DBNull.Value);
                    }
                    if (actionReasonId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionReasonId", DbType.Int32, actionReasonId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionReasonId", DbType.Int32, DBNull.Value);
                    }
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);
                    }
                    if (proposedActionId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProposedActionId", DbType.Int32, proposedActionId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProposedActionId", DbType.Int32, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryAction");
                    DataTable Table = ds.Tables["DisciplinaryAction"];

                    boList = Table.AsEnumerable().Select(r => new DisciplinaryActionBO
                    {
                        DisciplinaryActionId = r.Field<Int64>("DisciplinaryActionId"),
                        ActionTypeId = r.Field<Int16>("ActionTypeId"),
                        DisciplinaryActionReasonId = r.Field<Int32>("DisciplinaryActionReasonId"),
                        ProposedAction = r.Field<string>("ProposedAction"),
                        ProposedActionId = r.Field<int?>("ProposedActionId"),
                        EmpName = r.Field<string>("EmpName")

                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return boList;
        }

        //Disciplinary Action Type-------------------------------------------------------------
        public Boolean SaveDisciplinaryActionTypeInfo(DisciplinaryActionTypeBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDisciplinaryActionTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ActionName", DbType.String, bo.ActionName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, bo.Description);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);

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
        public Boolean UpdateDisciplinaryActionTypeInfo(DisciplinaryActionTypeBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDisciplinaryActionTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DisciplinaryActionTypeId", DbType.Int16, bo.DisciplinaryActionTypeId);
                        dbSmartAspects.AddInParameter(command, "@ActionName", DbType.String, bo.ActionName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, bo.Description);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bo.LastModifiedBy);

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
        public List<DisciplinaryActionTypeBO> GetDisciplinaryActionTypeList()
        {
            List<DisciplinaryActionTypeBO> boList = new List<DisciplinaryActionTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionTypeList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryActionType");
                    DataTable Table = ds.Tables["DisciplinaryActionType"];

                    boList = Table.AsEnumerable().Select(r => new DisciplinaryActionTypeBO
                    {
                        DisciplinaryActionTypeId = r.Field<Int16>("DisciplinaryActionTypeId"),
                        ActionName = r.Field<string>("ActionName"),
                        Description = r.Field<string>("Description")

                    }).ToList();
                }
            }
            return boList;
        }
        public DisciplinaryActionTypeBO GetDisciplinaryActionTypeById(int pkId)
        {
            DisciplinaryActionTypeBO bo = new DisciplinaryActionTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionTypeId", DbType.Int16, pkId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryActionType");
                    DataTable Table = ds.Tables["DisciplinaryActionType"];

                    bo = Table.AsEnumerable().Select(r => new DisciplinaryActionTypeBO
                    {
                        DisciplinaryActionTypeId = r.Field<Int16>("DisciplinaryActionTypeId"),
                        ActionName = r.Field<string>("ActionName"),
                        Description = r.Field<string>("Description")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }

        //Disciplinary Action Reason------------------------------------------------------------
        public Boolean SaveDisciplinaryActionReasonInfo(DisciplinaryActionReasonBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDisciplinaryActionReasonInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ActionReason", DbType.String, bo.ActionReason);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);

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
        public Boolean UpdateDisciplinaryActionReasonInfo(DisciplinaryActionReasonBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDisciplinaryActionReasonInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DisciplinaryActionReasonId", DbType.Int32, bo.DisciplinaryActionReasonId);
                        dbSmartAspects.AddInParameter(command, "@ActionReason", DbType.String, bo.ActionReason);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bo.LastModifiedBy);

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
        public List<DisciplinaryActionReasonBO> GetDisciplinaryActionReasonList()
        {
            List<DisciplinaryActionReasonBO> boList = new List<DisciplinaryActionReasonBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionReasonList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryActionReason");
                    DataTable Table = ds.Tables["DisciplinaryActionReason"];

                    boList = Table.AsEnumerable().Select(r => new DisciplinaryActionReasonBO
                    {
                        DisciplinaryActionReasonId = r.Field<Int32>("DisciplinaryActionReasonId"),
                        ActionReason = r.Field<string>("ActionReason"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return boList;
        }
        public DisciplinaryActionReasonBO GetDisciplinaryActionReasonById(int pkId)
        {
            DisciplinaryActionReasonBO bo = new DisciplinaryActionReasonBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionReasonInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionReasonId", DbType.Int32, pkId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryActionReason");
                    DataTable Table = ds.Tables["DisciplinaryActionReason"];

                    bo = Table.AsEnumerable().Select(r => new DisciplinaryActionReasonBO
                    {
                        DisciplinaryActionReasonId = r.Field<Int32>("DisciplinaryActionReasonId"),
                        ActionReason = r.Field<string>("ActionReason"),
                        Remarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }

        // For Report
        public List<DisciplinaryActionReportViewBO> GetDisciplinaryActionForReport(int empId)
        {
            List<DisciplinaryActionReportViewBO> bo = new List<DisciplinaryActionReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDisciplinaryActionForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryActionReport");
                    DataTable Table = ds.Tables["DisciplinaryActionReport"];

                    bo = Table.AsEnumerable().Select(r => new DisciplinaryActionReportViewBO
                    {
                        ApplicableDate = r.Field<DateTime>("ApplicableDate"),
                        ApplicableDateForReport = r.Field<String>("ApplicableDateForReport"),
                        DisplayName = r.Field<string>("DisplayName"),
                        ActionName = r.Field<string>("ActionName"),
                        ActionReason = r.Field<string>("ActionReason"),
                        ProposedActionId = r.Field<int?>("ProposedActionId"),
                        ProposedAction = r.Field<string>("ProposedAction"),
                        ActionBody = r.Field<string>("ActionBody")
                    }).ToList();
                }
            }
            return bo;
        }
        public List<DisciplinaryActionReportViewBO> GetDisciplinaryActionForReport(int? actionTypeId, int? actionReasonId, int? empId, int? proposedActionId, DateTime fromDate, DateTime toDate)
        {
            List<DisciplinaryActionReportViewBO> bo = new List<DisciplinaryActionReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpDisciplinaryActionForReport_SP"))
                {
                    if (actionTypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ActionTypeId", DbType.Int16, actionTypeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ActionTypeId", DbType.Int16, DBNull.Value);
                    }
                    if (actionReasonId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionReasonId", DbType.Int32, actionReasonId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DisciplinaryActionReasonId", DbType.Int32, DBNull.Value);
                    }
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, DBNull.Value);
                    }
                    if (proposedActionId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProposedActionId", DbType.Int32, proposedActionId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProposedActionId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);

                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DisciplinaryActionReport");
                    DataTable Table = ds.Tables["DisciplinaryActionReport"];

                    bo = Table.AsEnumerable().Select(r => new DisciplinaryActionReportViewBO
                    {
                        ApplicableDate = r.Field<DateTime>("ApplicableDate"),
                        ApplicableDateForReport = r.Field<String>("ApplicableDateForReport"),
                        DisplayName = r.Field<string>("DisplayName"),
                        ActionName = r.Field<string>("ActionName"),
                        ActionReason = r.Field<string>("ActionReason"),
                        ProposedActionId = r.Field<int?>("ProposedActionId"),
                        ProposedAction = r.Field<string>("ProposedAction"),
                        ActionBody = r.Field<string>("ActionBody")
                    }).ToList();
                }
            }
            return bo;
        }
    }
}
