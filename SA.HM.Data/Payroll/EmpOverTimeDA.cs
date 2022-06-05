using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Data.Payroll
{
    public class EmpOverTimeDA : BaseService
    {
        public EmpOverTimeBO GetOverTimeInfoByID(int EditId)
        {
            EmpOverTimeBO empOverTimeBO = new EmpOverTimeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpOverTimeById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OverTimeId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                empOverTimeBO.OverTimeId = Convert.ToInt32(reader["OverTimeId"]);
                                empOverTimeBO.OverTimeDate = Convert.ToDateTime(reader["OverTimeDate"].ToString());
                                empOverTimeBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                //empOverTimeBO.OverTimeHour = Convert.ToDecimal(reader["OverTimeHour"].ToString());
                            }
                        }
                    }
                }
            }
            return empOverTimeBO;
        }
        public List<EmpOverTimeBO> GetAllOverTimeByImployeeID(int EmployeeID)
        {
            List<EmpOverTimeBO> List = new List<EmpOverTimeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllOverTimeByImployeeID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, EmployeeID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpOverTimeBO empOverTimeBO = new EmpOverTimeBO();
                                empOverTimeBO.OverTimeId = Convert.ToInt32(reader["OverTimeId"]);
                                empOverTimeBO.OverTimeDate = Convert.ToDateTime(reader["OverTimeDate"].ToString());
                                empOverTimeBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                //empOverTimeBO.OverTimeHour = Convert.ToDecimal(reader["OverTimeHour"].ToString());
                                List.Add(empOverTimeBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
        public List<EmpOverTimeBO> GetAllOverTimeByEmployeeIdForGridPaging(int employeeID, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpOverTimeBO> IncreamentList = new List<EmpOverTimeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOverTimeByEmpIDForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Increment");
                    DataTable Table = incrementDS.Tables["Increment"];

                    IncreamentList = Table.AsEnumerable().Select(r => new EmpOverTimeBO
                    {
                        OverTimeId = r.Field<Int64>("OverTimeId"),
                        OTHour = r.Field<decimal>("OTHour"),
                        OverTimeDate = r.Field<DateTime>("OverTimeDate"),
                        EmpId = r.Field<int>("EmpId")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return IncreamentList;
        }
        public bool UpdateOverTimeInfo(EmpOverTimeBO overTimeBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateOverTimeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@OverTimeId", DbType.Int32, overTimeBO.OverTimeId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, overTimeBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@OverTimeDate", DbType.DateTime, overTimeBO.OverTimeDate);
                        //dbSmartAspects.AddInParameter(command, "@OverTimeHour", DbType.Decimal, overTimeBO.OverTimeHour);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, overTimeBO.LastModifiedBy);
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

        public bool SaveOvertime(List<EmpOverTimeBO> overTimeBO, int createdBy, out int overTimeId)
        {
            Boolean status = false;
            try
            {
                overTimeId = 0;

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOverTimeInfo_SP"))
                            {
                                foreach (EmpOverTimeBO ot in overTimeBO)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, ot.EmpId);
                                    dbSmartAspects.AddInParameter(command, "@OverTimeDate", DbType.DateTime, ot.OverTimeDate);
                                    //dbSmartAspects.AddInParameter(command, "@EntryTime", DbType.DateTime, ot.EntryTime);
                                    //dbSmartAspects.AddInParameter(command, "@ExitTime", DbType.DateTime, ot.ExitTime);
                                    dbSmartAspects.AddInParameter(command, "@TotalHour", DbType.Int32, ot.TotalHour);
                                    dbSmartAspects.AddInParameter(command, "@OTHour", DbType.Int32, ot.OTHour);
                                    dbSmartAspects.AddInParameter(command, "@ApprovedOTHour", DbType.Int32, ot.ApprovedOTHour);
                                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                                    dbSmartAspects.AddOutParameter(command, "@OverTimeId", DbType.Int32, sizeof(Int64));
                                    

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                    overTimeId = Convert.ToInt32(command.Parameters["@OverTimeId"].Value);
                                }
                            }

                            if (status)
                                transaction.Commit();
                            else
                                transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
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

        public List<OvertimeHourOfEmployeeeBO> GetOvertimeHourOfEmployeee(int? employeeId, int? departmentId, DateTime? attendanceDate)
        {
            List<OvertimeHourOfEmployeeeBO> overtime = new List<OvertimeHourOfEmployeeeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOvertimeHourOfEmployeee_SP"))
                {
                    if (employeeId != null)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (employeeId != null)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (attendanceDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@AttenDanceDate", DbType.DateTime, attendanceDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AttenDanceDate", DbType.DateTime, DBNull.Value);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];

                    overtime = Table.AsEnumerable().Select(r => new OvertimeHourOfEmployeeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        AttendanceDate = r.Field<DateTime>("AttendanceDate"),
                        EntryTime = r.Field<DateTime?>("EntryTime"),
                        ExitTime = r.Field<DateTime?>("ExitTime"),
                        TotalHour = r.Field<int?>("TotalHour"),
                        OTHour = r.Field<int?>("OTHour"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Designation = r.Field<string>("Designation")

                    }).ToList();
                }
            }

            return overtime;
        }

        public List<EmpOverTimeBO> GetEmpOverTime(int? employeeId, int? departmentId, DateTime? attendanceFromDate, DateTime? attendanceToDate)
        {
            List<EmpOverTimeBO> overtime = new List<EmpOverTimeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeOvertime_SP"))
                {
                    if (employeeId != null)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, employeeId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (departmentId != null)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);

                    if (attendanceFromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@AttendanceFromDate", DbType.DateTime, attendanceFromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AttendanceFromDate", DbType.DateTime, DBNull.Value);

                    if (attendanceToDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@AttendanceToDate", DbType.DateTime, attendanceToDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AttendanceToDate", DbType.DateTime, DBNull.Value);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "Overtime");
                    DataTable Table = incrementDS.Tables["Overtime"];

                    overtime = Table.AsEnumerable().Select(r => new EmpOverTimeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        OverTimeDate = r.Field<DateTime>("OverTimeDate"),
                        EntryTime = r.Field<DateTime>("EntryTime"),
                        ExitTime = r.Field<DateTime>("ExitTime"),
                        TotalHour = r.Field<int>("TotalHour"),
                        OTHour = r.Field<int>("OTHour"),
                        ApprovedOTHour = r.Field<int>("ApprovedOTHour"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Designation = r.Field<string>("Designation"),
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Department = r.Field<string>("Department")

                    }).ToList();
                }
            }

            return overtime;
        }
    }
}
