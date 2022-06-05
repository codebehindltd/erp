using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class TimeSlabHeadDA : BaseService
    {

        public List<TimeSlabHeadBO> GetAllTimeSlabHeadInfo()
        {

            List<TimeSlabHeadBO> List = new List<TimeSlabHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTimeSlabHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TimeSlabHeadBO headBO = new TimeSlabHeadBO();
                                headBO.TimeSlabId = Convert.ToInt32(reader["TimeSlabId"]);
                                headBO.TimeSlabHead = reader["TimeSlabHead"].ToString();
                                headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = Convert.ToString(reader["ActiveStatus"]);
                                headBO.SlabStartTime = Convert.ToDateTime(reader["SlabStartTime"].ToString());
                                headBO.SlabEndTime = Convert.ToDateTime(reader["SlabEndTime"].ToString());

                                List.Add(headBO);
                            }
                        }
                    }
                }
            }
            return List;


        }
        public TimeSlabHeadBO GetTimeSlabHeadInfoById(int EditId)
        {
            TimeSlabHeadBO headBO = new TimeSlabHeadBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTimeSlabHeadInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TimeSlabId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                headBO.TimeSlabId = Convert.ToInt32(reader["TimeSlabId"]);
                                headBO.TimeSlabHead = reader["TimeSlabHead"].ToString();
                                headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.SlabStartTime = Convert.ToDateTime(reader["SlabStartTime"].ToString());
                                headBO.SlabEndTime = Convert.ToDateTime(reader["SlabEndTime"].ToString());
                                //  headBO.CreatedDate = reader["CreatedDate"].ToString();
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return headBO;
        }

        public bool SaveTimeSlabHeadInfo(TimeSlabHeadBO headBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTimeSlabHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TimeSlabHead", DbType.String, headBO.TimeSlabHead);
                        dbSmartAspects.AddInParameter(command, "@SlabStartTime", DbType.DateTime, headBO.SlabStartTime);
                        dbSmartAspects.AddInParameter(command, "@SlabEndTime", DbType.DateTime, headBO.SlabEndTime);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, headBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@TimeSlabId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@TimeSlabId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public bool UpdateTimeSlabHeadInfo(TimeSlabHeadBO headBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    /*  */
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTimeSlabHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TimeSlabId", DbType.Int32, headBO.TimeSlabId);
                        dbSmartAspects.AddInParameter(command, "@TimeSlabHead", DbType.String, headBO.TimeSlabHead);
                        dbSmartAspects.AddInParameter(command, "@SlabStartTime", DbType.DateTime, headBO.SlabStartTime);
                        dbSmartAspects.AddInParameter(command, "@SlabEndTime", DbType.DateTime, headBO.SlabEndTime);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, headBO.LastModifiedBy);
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
    }
}
