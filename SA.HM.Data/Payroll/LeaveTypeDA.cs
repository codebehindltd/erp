using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class LeaveTypeDA : BaseService
    {
        public List<LeaveTypeBO> GetLeaveTypeInfo()
        {
            List<LeaveTypeBO> leaveTypeList = new List<LeaveTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveTypeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LeaveTypeBO leaveType = new LeaveTypeBO();

                                leaveType.LeaveTypeId = Convert.ToInt32(reader["LeaveTypeId"]);
                                leaveType.TypeName = reader["TypeName"].ToString();
                                leaveType.YearlyLeave = Int32.Parse(reader["YearlyLeave"].ToString());

                                leaveType.CanCarryForward = Convert.ToBoolean(reader["CanCarryForward"].ToString());
                                leaveType.MaxDayCanCarryForwardYearly = Convert.ToByte(reader["MaxDayCanCarryForwardYearly"].ToString());
                                leaveType.MaxDayCanKeepAsCarryForwardLeave = Convert.ToByte(reader["MaxDayCanKeepAsCarryForwardLeave"].ToString());
                                leaveType.CanEncash = Convert.ToBoolean(reader["CanEncash"].ToString());
                                leaveType.MaxDayCanEncash = Convert.ToByte(reader["MaxDayCanEncash"].ToString());

                                leaveType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                leaveType.ActiveStatus = reader["ActiveStatus"].ToString();

                                leaveTypeList.Add(leaveType);
                            }
                        }
                    }
                }
            }
            return leaveTypeList;
        }
        public List<LeaveTypeBO> GetActiveLeaveTypeInfo()
        {
            List<LeaveTypeBO> boList = new List<LeaveTypeBO>();            

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveLeaveTypeInfo_SP"))
                {
                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "LeaveType");
                    DataTable table = SupplierDS.Tables["LeaveType"];

                    boList = table.AsEnumerable().Select(r => new LeaveTypeBO
                    {
                        LeaveTypeId = r.Field<int>("LeaveTypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        YearlyLeave = r.Field<int>("YearlyLeave"),
                        CanCarryForward = r.Field<bool>("CanCarryForward"),
                        MaxDayCanCarryForwardYearly = r.Field<byte>("MaxDayCanCarryForwardYearly"),
                        MaxDayCanKeepAsCarryForwardLeave = r.Field<byte>("MaxDayCanKeepAsCarryForwardLeave"),
                        CanEncash = r.Field<bool>("CanEncash"),
                        MaxDayCanEncash = r.Field<byte>("MaxDayCanEncash"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();
                }
            }
            return boList;
        }
        public Boolean SaveLeaveTypeInfo(LeaveTypeBO leaveTypeBO, out int tmpLeaveTypeId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLeaveTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, leaveTypeBO.TypeName);
                        dbSmartAspects.AddInParameter(command, "@YearlyLeave", DbType.Int32, leaveTypeBO.YearlyLeave);
                        dbSmartAspects.AddInParameter(command, "@LeaveModeId", DbType.Int32, leaveTypeBO.LeaveModeId);
                        dbSmartAspects.AddInParameter(command, "@CanCarryForward", DbType.Boolean, leaveTypeBO.CanCarryForward);
                        dbSmartAspects.AddInParameter(command, "@MaxDayCanCarryForwardYearly", DbType.Byte, leaveTypeBO.MaxDayCanCarryForwardYearly);
                        dbSmartAspects.AddInParameter(command, "@MaxDayCanKeepAsCarryForwardLeave", DbType.Byte, leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave);
                        dbSmartAspects.AddInParameter(command, "@CanEncash", DbType.Boolean, leaveTypeBO.CanEncash);
                        dbSmartAspects.AddInParameter(command, "@MaxDayCanEncash", DbType.Byte, leaveTypeBO.MaxDayCanEncash);

                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, leaveTypeBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, leaveTypeBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@LeaveTypeId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpLeaveTypeId = Convert.ToInt32(command.Parameters["@LeaveTypeId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateLeaveTypeInfo(LeaveTypeBO leaveTypeBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateLeaveTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@LeaveTypeId", DbType.Int32, leaveTypeBO.LeaveTypeId);
                        dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, leaveTypeBO.TypeName);
                        dbSmartAspects.AddInParameter(command, "@YearlyLeave", DbType.Int32, leaveTypeBO.YearlyLeave);
                        dbSmartAspects.AddInParameter(command, "@LeaveModeId", DbType.Int32, leaveTypeBO.LeaveModeId);
                        dbSmartAspects.AddInParameter(command, "@CanCarryForward", DbType.Boolean, leaveTypeBO.CanCarryForward);
                        dbSmartAspects.AddInParameter(command, "@MaxDayCanCarryForwardYearly", DbType.Byte, leaveTypeBO.MaxDayCanCarryForwardYearly);
                        dbSmartAspects.AddInParameter(command, "@MaxDayCanKeepAsCarryForwardLeave", DbType.Byte, leaveTypeBO.MaxDayCanKeepAsCarryForwardLeave);
                        dbSmartAspects.AddInParameter(command, "@CanEncash", DbType.Boolean, leaveTypeBO.CanEncash);
                        dbSmartAspects.AddInParameter(command, "@MaxDayCanEncash", DbType.Byte, leaveTypeBO.MaxDayCanEncash);

                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, leaveTypeBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, leaveTypeBO.LastModifiedBy);

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
        public LeaveTypeBO GetLeaveTypeInfoById(int leaveTypeId)
        {
            LeaveTypeBO leaveType = new LeaveTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLeaveTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LeaveTypeId", DbType.Int32, leaveTypeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                leaveType.LeaveTypeId = Convert.ToInt32(reader["LeaveTypeId"]);
                                leaveType.TypeName = reader["TypeName"].ToString();
                                leaveType.YearlyLeave = Int32.Parse(reader["YearlyLeave"].ToString());
                                leaveType.LeaveModeId = Int32.Parse(reader["LeaveModeId"].ToString());
                                if (reader["CanCarryForward"] != DBNull.Value)
                                {
                                    leaveType.CanCarryForward = Convert.ToBoolean(reader["CanCarryForward"].ToString());
                                }
                                leaveType.MaxDayCanCarryForwardYearly = Convert.ToByte(reader["MaxDayCanCarryForwardYearly"].ToString());
                                leaveType.MaxDayCanKeepAsCarryForwardLeave = Convert.ToByte(reader["MaxDayCanKeepAsCarryForwardLeave"].ToString());
                                leaveType.CanEncash = Convert.ToBoolean(reader["CanEncash"].ToString());
                                leaveType.MaxDayCanEncash = Convert.ToByte(reader["MaxDayCanEncash"].ToString());

                                leaveType.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                leaveType.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return leaveType;
        }
    }
}
