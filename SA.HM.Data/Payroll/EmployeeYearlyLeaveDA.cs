using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmployeeYearlyLeaveDA : BaseService
    {
        public EmployeeYearlyLeaveDA()
        {

        }

        public Boolean SaveLeaveInformation(EmployeeYearlyLeaveBO yearlyLeaveBO, out int tmpId)
        {

            Boolean status = true;
            tmpId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveYearlyLeaveInfo_SP"))
                {
                    conn.Open();
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.String, yearlyLeaveBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.String, yearlyLeaveBO.LeaveTypeId);
                    dbSmartAspects.AddInParameter(command, "@LeaveQuantity", DbType.String, yearlyLeaveBO.LeaveQuantity);
                    dbSmartAspects.AddOutParameter(command, "@YearlyLeaveId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpId = Convert.ToInt32(command.Parameters["@YearlyLeaveId"].Value);
                }
            }

            return status;
        }

        public List<EmployeeYearlyLeaveBO> GetAllLeaveByID(int employeeID)
        {
            List<EmployeeYearlyLeaveBO> lists = new List<EmployeeYearlyLeaveBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveByEmployeeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@employeeID", DbType.Int32, employeeID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeYearlyLeaveBO bo = new EmployeeYearlyLeaveBO();
                                bo.LeaveQuantity = Int32.Parse(reader["LeaveQuantity"].ToString());
                                bo.LeaveType = reader["TypeName"].ToString();
                                bo.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                bo.YearlyLeaveId = Int32.Parse(reader["YearlyLeaveId"].ToString());
                                lists.Add(bo);
                            }
                        }
                    }
                }
            }

            return lists;
        }

        public EmployeeYearlyLeaveBO GetAllLeaveByLeaveID(int EditId)
        {
            EmployeeYearlyLeaveBO bo = new EmployeeYearlyLeaveBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveByLeaveID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@YearlyLeaveId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {

                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                bo.LeaveQuantity = Int32.Parse(reader["LeaveQuantity"].ToString());
                                bo.LeaveType = reader["TypeName"].ToString();
                                bo.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                bo.LeaveTypeId = Int32.Parse(reader["LeaveTypeId"].ToString());

                            }
                        }
                    }
                }
            }

            return bo;
        }

        public bool UpdateLeaveInformation(EmployeeYearlyLeaveBO yearlyLeaveBO)
        {
            Boolean status = true;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateYearlyLeaveInfo_SP"))
                {
                    conn.Open();
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.String, yearlyLeaveBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.String, yearlyLeaveBO.LeaveTypeId);
                    dbSmartAspects.AddInParameter(command, "@LeaveQuantity", DbType.String, yearlyLeaveBO.LeaveQuantity);
                    dbSmartAspects.AddInParameter(command, "@YearlyLeaveId", DbType.String, yearlyLeaveBO.YearlyLeaveId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }

            return status;
        }

        public List<EmployeeYearlyLeaveBO> GetAllLeaveByEmployeeIDForGridPaging(int employeeID, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmployeeYearlyLeaveBO> lists = new List<EmployeeYearlyLeaveBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveByEmployeeIdForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeID", DbType.Int32, employeeID);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];


                    lists = Table.AsEnumerable().Select(r => new EmployeeYearlyLeaveBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        YearlyLeaveId = r.Field<Int32>("YearlyLeaveId"),
                        LeaveType = r.Field<string>("TypeName"),
                        LeaveQuantity = r.Field<Int32>("LeaveQuantity")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return lists;
        }

        public List<LeaveTakenNBalanceBO> GetLeaveTakenNBalanceByEmployee(int empId, DateTime currentDate)
        {
            List<LeaveTakenNBalanceBO> lists = new List<LeaveTakenNBalanceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveTakenNBalanceByEmployee_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@LeaveDate", DbType.DateTime, currentDate);
                    
                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "Leave");
                    DataTable Table = LeaveDS.Tables["Leave"];


                    lists = Table.AsEnumerable().Select(r => new LeaveTakenNBalanceBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeFullName = r.Field<string>("EmployeeFullName"), 
                        LeaveTypeID = r.Field<Int32>("LeaveTypeID"),
                        LeaveTypeName = r.Field<string>("LeaveTypeName"),
                        TotalTakenLeave = r.Field<Int32>("TotalTakenLeave"),
                        RemainingLeave = r.Field<Int32>("RemainingLeave")

                    }).ToList();
                }
            }

            return lists; 
        }

        public List<LeaveTakenNBalanceBO> GetDepartmentWiseTotalLeaveBalance(int empId)
        {
            List<LeaveTakenNBalanceBO> lists = new List<LeaveTakenNBalanceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDpartmentWiseLeaveBalanceForDashboard_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@CurrentDate", DbType.DateTime, DateTime.Now); 

                    DataSet LeaveDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, LeaveDS, "LeaveBalance");
                    DataTable Table = LeaveDS.Tables["LeaveBalance"];
                    
                    lists = Table.AsEnumerable().Select(r => new LeaveTakenNBalanceBO
                    {
                        //EmpId = r.Field<Int32>("EmpId"),
                        //EmpCode = r.Field<string>("EmpCode"),
                        //EmployeeFullName = r.Field<string>("EmployeeFullName"),
                        LeaveTypeID = r.Field<Int32>("TypeId"),
                        LeaveTypeName = r.Field<string>("LeaveTypeName"),
                        TotalLeave = r.Field<Int32?>("TotalLeave"),
                        TotalTakenLeave = r.Field<Int32?>("TakenLeave"),
                        RemainingLeave = r.Field<Int32?>("RemainingLeave")

                    }).ToList();
                }
            }

            return lists;
        }
    }
}
