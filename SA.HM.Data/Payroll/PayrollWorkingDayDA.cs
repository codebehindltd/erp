using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class PayrollWorkingDayDA : BaseService
    {

        public PayrollWorkingDayBO GetPayrollWorkingDayInfoByID(int WorkingDayId)
        {
            PayrollWorkingDayBO workingDayBO = new PayrollWorkingDayBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollWorkingDayInfoByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@WorkingDayId", DbType.Int32, WorkingDayId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                workingDayBO.WorkingDayId = Convert.ToInt32(reader["WorkingDayId"]);
                                workingDayBO.TypeId = Convert.ToInt32(reader["TypeId"].ToString());
                                workingDayBO.WorkingPlan = Convert.ToString(reader["WorkingPlan"]);
                                if (workingDayBO.WorkingPlan == "Fixed")
                                {
                                    workingDayBO.StartTime = Convert.ToDateTime(reader["StartTime"].ToString());
                                    workingDayBO.EndTime = Convert.ToDateTime(reader["EndTime"].ToString());
                                    workingDayBO.DayOffOne = Convert.ToString(reader["DayOffOne"].ToString());
                                    workingDayBO.DayOffTwo = Convert.ToString(reader["DayOffTwo"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            return workingDayBO;
        }
        public List<PayrollWorkingDayBO> GetPayrollWorkingDayInformationBySearchCritaria(int typeId, string dayOne, string dayTwo)
        {
            List<PayrollWorkingDayBO> List = new List<PayrollWorkingDayBO>();
            string searchCriteria = string.Empty;
            searchCriteria = GenerateWhereCondition(typeId, dayOne, dayTwo);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollWorkingDayInformationBySearchCritaria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PayrollWorkingDayBO workingDayBO = new PayrollWorkingDayBO();
                                workingDayBO.WorkingDayId = Convert.ToInt32(reader["WorkingDayId"]);
                                workingDayBO.TypeId = Convert.ToInt32(reader["TypeId"].ToString());
                                workingDayBO.WorkingPlan = Convert.ToString(reader["WorkingPlan"]);
                                if (workingDayBO.WorkingPlan == "Fixed")
                                {
                                    workingDayBO.StartTime = Convert.ToDateTime(reader["StartTime"].ToString());
                                    workingDayBO.EndTime = Convert.ToDateTime(reader["EndTime"].ToString());
                                    workingDayBO.DayOffOne = Convert.ToString(reader["DayOffOne"].ToString());
                                    workingDayBO.DayOffTwo = Convert.ToString(reader["DayOffTwo"].ToString());
                                }
                                workingDayBO.TypeName = Convert.ToString(reader["TypeName"].ToString());
                                List.Add(workingDayBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
        public bool UpdatePayrollWorkingDayInfoByDayId(PayrollWorkingDayBO workingDayBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePayrollWorkingDayInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@WorkingDayId", DbType.Int32, workingDayBO.WorkingDayId);
                        dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, workingDayBO.TypeId);
                        dbSmartAspects.AddInParameter(command, "@StartTime", DbType.DateTime, workingDayBO.StartTime);
                        dbSmartAspects.AddInParameter(command, "@EndTime", DbType.DateTime, workingDayBO.EndTime);
                        dbSmartAspects.AddInParameter(command, "@WorkingPlan", DbType.String, workingDayBO.WorkingPlan);
                        dbSmartAspects.AddInParameter(command, "@DayOffOne", DbType.String, workingDayBO.DayOffOne);
                        dbSmartAspects.AddInParameter(command, "@DayOffTwo", DbType.String, workingDayBO.DayOffTwo);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, workingDayBO.LastModifiedBy);
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
        public bool UpdatePayrollWorkingDayInfoByCategoryId(PayrollWorkingDayBO workingDayBO, out int workingDayId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePayrollWorkingDayInfoByCategoryId_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, workingDayBO.TypeId);
                        dbSmartAspects.AddInParameter(command, "@StartTime", DbType.DateTime, workingDayBO.StartTime);
                        dbSmartAspects.AddInParameter(command, "@EndTime", DbType.DateTime, workingDayBO.EndTime);
                        dbSmartAspects.AddInParameter(command, "@WorkingPlan", DbType.String, workingDayBO.WorkingPlan);
                        dbSmartAspects.AddInParameter(command, "@DayOffOne", DbType.String, workingDayBO.DayOffOne);
                        dbSmartAspects.AddInParameter(command, "@DayOffTwo", DbType.String, workingDayBO.DayOffTwo);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, workingDayBO.LastModifiedBy);
                        dbSmartAspects.AddOutParameter(command, "@WorkingDayId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        workingDayId = Convert.ToInt32(command.Parameters["@WorkingDayId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool SavePayrollWorkingDayInfo(PayrollWorkingDayBO workingDayBO, out int tmpWorkingDayId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollWorkingDayInfo_SP"))
                    {

                        dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, workingDayBO.TypeId);
                        dbSmartAspects.AddInParameter(command, "@StartTime", DbType.DateTime, workingDayBO.StartTime);
                        dbSmartAspects.AddInParameter(command, "@EndTime", DbType.DateTime, workingDayBO.EndTime);
                        dbSmartAspects.AddInParameter(command, "@WorkingPlan", DbType.String, workingDayBO.WorkingPlan);
                        dbSmartAspects.AddInParameter(command, "@DayOffOne", DbType.String, workingDayBO.DayOffOne);
                        dbSmartAspects.AddInParameter(command, "@DayOffTwo", DbType.String, workingDayBO.DayOffTwo);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, workingDayBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@WorkingDayId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpWorkingDayId = Convert.ToInt32(command.Parameters["@WorkingDayId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public int GetCategoryIdCount(int CategoryId)
        {
            int categoryIdCount = 0;
            PayrollWorkingDayBO workingDayBO = new PayrollWorkingDayBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCategoryIdCount_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeId", DbType.Int32, CategoryId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                categoryIdCount = Convert.ToInt32(reader["categoryIdCount"]);
                            }
                        }
                    }
                }
            }
            return categoryIdCount;
        }
        private string GenerateWhereCondition(int TypeId, string DayOffOne, string DayOffTwo)
        {
            string Where = string.Empty, Condition = string.Empty;

            if (TypeId > 0)
            {
                Condition = " pwd.TypeId = '" + TypeId + "'";
            }

            if (DayOffOne != "All")
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " pwd.DayOffOne = '" + DayOffOne + "'";
                }
                else
                {
                    Condition += " AND pwd.DayOffOne = '" + DayOffOne + "'";
                }
            }

            if (DayOffTwo != "All")
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " pwd.DayOffTwo = '" + DayOffTwo + "'";
                }
                else
                {
                    Condition += " AND pwd.DayOffTwo = '" + DayOffTwo + "'";
                }
            }
            if (!string.IsNullOrEmpty(Condition))
            {
                Where += " WHERE " + Condition;
            }

            return Where;
        }
        public bool IsUpdateAvailable(int typeId, int workingDayId)
        {
            bool inDouble = true;
            PayrollWorkingDayBO workingDayBO = new PayrollWorkingDayBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetWorkingDayInfoByCategoryIdAndWorkingDayId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeId", DbType.Int32, typeId);
                    dbSmartAspects.AddInParameter(cmd, "@WorkingDayId", DbType.Int32, workingDayId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                inDouble = false;
                            }
                        }
                    }
                }
            }
            return inDouble;
        }
        public PayrollWorkingDayBO GetPayrollWorkingDayInfoByEmpCategoryId(int typeId)
        {
            PayrollWorkingDayBO workingDayBO = new PayrollWorkingDayBO();
            string searchCriteria = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollWorkingDayInfoByEmpCategoryId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpTypeId", DbType.Int32, typeId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                workingDayBO.WorkingDayId = Convert.ToInt32(reader["WorkingDayId"]);
                                workingDayBO.TypeId = Convert.ToInt32(reader["TypeId"].ToString());
                                workingDayBO.WorkingPlan = Convert.ToString(reader["WorkingPlan"]);
                                if (workingDayBO.WorkingPlan == "Fixed")
                                {
                                    workingDayBO.StartTime = Convert.ToDateTime(reader["StartTime"].ToString());
                                    workingDayBO.EndTime = Convert.ToDateTime(reader["EndTime"].ToString());
                                    workingDayBO.DayOffOne = Convert.ToString(reader["DayOffOne"].ToString());
                                    workingDayBO.DayOffTwo = Convert.ToString(reader["DayOffTwo"].ToString());
                                }
                                workingDayBO.TypeName = Convert.ToString(reader["TypeName"].ToString());
                            }
                        }
                    }
                }
            }
            return workingDayBO;
        }
    }
}
