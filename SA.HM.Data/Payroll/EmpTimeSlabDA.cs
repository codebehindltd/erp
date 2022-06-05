using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data.Payroll
{
    public class EmpTimeSlabDA : BaseService
    {
        public List<EmpTimeSlabBO> GetAllTimeSlabInfo()
        {
            List<EmpTimeSlabBO> slabList = new List<EmpTimeSlabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTimeSlabInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTimeSlabBO slab = new EmpTimeSlabBO();

                                slab.SlabEffectDate = Convert.ToDateTime(reader["SlabEffectDate"].ToString());
                                slab.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                slab.WeekEndFirst = reader["WeekEndFirst"].ToString();
                                slab.WeekEndSecond = reader["WeekEndSecond"].ToString();
                                slab.WeekEndMode = reader["WeekEndMode"].ToString();
                                slab.EmpTimeSlabId = Int32.Parse(reader["EmpTimeSlabId"].ToString());
                                slab.TimeSlabId = Int32.Parse(reader["TimeSlabId"].ToString());
                                slab.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                slab.ActiveStatus = reader["ActiveStatus"].ToString();
                                slabList.Add(slab);
                            }
                        }
                    }
                }
            }
            return slabList;
        }



        public EmpTimeSlabBO GetTimeSlabInfoById(int EditId)
        {
            EmpTimeSlabBO slabBO = new EmpTimeSlabBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTimeSlabInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpTimeSlabId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                slabBO.SlabEffectDate = Convert.ToDateTime(reader["SlabEffectDate"].ToString());
                                slabBO.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                slabBO.WeekEndFirst = reader["WeekEndFirst"].ToString();
                                slabBO.WeekEndSecond = reader["WeekEndSecond"].ToString();
                                slabBO.WeekEndMode = reader["WeekEndMode"].ToString();
                                slabBO.EmpTimeSlabId = Int32.Parse(reader["EmpTimeSlabId"].ToString());
                                slabBO.TimeSlabId = Int32.Parse(reader["TimeSlabId"].ToString());
                                slabBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                slabBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return slabBO;
        }



        public bool SaveAllTimeSlabInfo(EmpTimeSlabBO slabBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAllTimeSlabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, slabBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@WeekEndFirst", DbType.String, slabBO.WeekEndFirst);
                    dbSmartAspects.AddInParameter(command, "@WeekEndSecond", DbType.String, slabBO.WeekEndSecond);
                    dbSmartAspects.AddInParameter(command, "@WeekEndMode", DbType.String, slabBO.WeekEndMode);
                    dbSmartAspects.AddInParameter(command, "@TimeSlabId", DbType.Int32, slabBO.TimeSlabId);
                    dbSmartAspects.AddInParameter(command, "@SlabEffectDate", DbType.DateTime, slabBO.SlabEffectDate);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, slabBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, slabBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@EmpTimeSlabId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpUserInfoId = Convert.ToInt32(command.Parameters["@EmpTimeSlabId"].Value);
                }
            }
            return status;
        }

        public bool UpdateTimeSlabInfo(EmpTimeSlabBO slabBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTimeSlabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpTimeSlabId", DbType.Int32, slabBO.EmpTimeSlabId);
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, slabBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@WeekEndFirst", DbType.String, slabBO.WeekEndFirst);
                    dbSmartAspects.AddInParameter(command, "@WeekEndSecond", DbType.String, slabBO.WeekEndSecond);
                    dbSmartAspects.AddInParameter(command, "@WeekEndMode", DbType.String, slabBO.WeekEndMode);
                    dbSmartAspects.AddInParameter(command, "@TimeSlabId", DbType.Int32, slabBO.TimeSlabId);
                    dbSmartAspects.AddInParameter(command, "@SlabEffectDate", DbType.DateTime, slabBO.SlabEffectDate);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, slabBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, slabBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;


        }

        public bool SaveEachDayTimeSlabInfo(EmpTimeSlabBO slabBO, List<EmpTimeSlabRosterBO> List, out int tmpUserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAllTimeSlabInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, slabBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@WeekEndFirst", DbType.String, slabBO.WeekEndFirst);
                        dbSmartAspects.AddInParameter(command, "@WeekEndSecond", DbType.String, slabBO.WeekEndSecond);
                        dbSmartAspects.AddInParameter(command, "@WeekEndMode", DbType.String, slabBO.WeekEndMode);
                        dbSmartAspects.AddInParameter(command, "@TimeSlabId", DbType.Int32, 0);
                        dbSmartAspects.AddInParameter(command, "@SlabEffectDate", DbType.DateTime, slabBO.SlabEffectDate);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, slabBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, slabBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@EmpTimeSlabId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@EmpTimeSlabId"].Value);
                        if (status == true)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveEachDayTimeSlabInfo_SP"))
                            {
                                foreach (EmpTimeSlabRosterBO DetailBO in List)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@DayName", DbType.String, DetailBO.DayName);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TimeSlabId", DbType.Int32, DetailBO.TimeSlabId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@EmpTimeSlabId", DbType.Int32, tmpUserInfoId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails) > 0 ? true : false;
                                }
                            }

                        }

                    }
                }
            }
            return status;
        }
        public bool UpdateEachDayTimeSlabInfo(EmpTimeSlabBO slabBO, List<EmpTimeSlabRosterBO> List)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTimeSlabInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpTimeSlabId", DbType.Int32, slabBO.EmpTimeSlabId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, slabBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@WeekEndFirst", DbType.String, slabBO.WeekEndFirst);
                        dbSmartAspects.AddInParameter(command, "@WeekEndSecond", DbType.String, slabBO.WeekEndSecond);
                        dbSmartAspects.AddInParameter(command, "@WeekEndMode", DbType.String, slabBO.WeekEndMode);
                        dbSmartAspects.AddInParameter(command, "@TimeSlabId", DbType.Int32, 0);
                        dbSmartAspects.AddInParameter(command, "@SlabEffectDate", DbType.DateTime, slabBO.SlabEffectDate);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, slabBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, slabBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        if (status == true)
                        {
                            using (DbConnection connection = dbSmartAspects.CreateConnection())
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateTimeSlabRosterInfo_SP"))
                                {
                                    foreach (EmpTimeSlabRosterBO DetailBO in List)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@DayName", DbType.String, DetailBO.DayName);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TimeSlabId", DbType.Int32, DetailBO.TimeSlabId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@EmpTimeSlabId", DbType.Int32, slabBO.EmpTimeSlabId);
                                        status = dbSmartAspects.ExecuteNonQuery(commandDetails) > 0 ? true : false;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            return status;
        }
        public List<EmpTimeSlabRosterBO> GetEmpTimeSlabRosterByEmpTimeSlabId(int EditId)
        {

            List<EmpTimeSlabRosterBO> List = new List<EmpTimeSlabRosterBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTimeSlabRosterByEmpTimeSlabId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpTimeSlabId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTimeSlabRosterBO slabBO = new EmpTimeSlabRosterBO();

                                slabBO.DayName = reader["DayName"].ToString();
                                slabBO.TimeSlabId = Int32.Parse(reader["TimeSlabId"].ToString());
                                slabBO.EmpTimeSlabId = Int32.Parse(reader["EmpTimeSlabId"].ToString());
                                List.Add(slabBO);
                            }
                        }
                    }
                }
            }
            return List;


        }
    }
}
