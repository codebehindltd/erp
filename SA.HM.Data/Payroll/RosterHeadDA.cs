using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Data.Payroll
{
    public class RosterHeadDA : BaseService
    {
        public List<RosterHeadBO> GetRosterHeadInfo()
        {
            List<RosterHeadBO> bpList = new List<RosterHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRosterHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RosterHeadBO entityBO = new RosterHeadBO();

                                entityBO.RosterId = Convert.ToInt32(reader["RosterId"]);
                                entityBO.RosterName = reader["RosterName"].ToString();
                                entityBO.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                entityBO.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                bpList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return bpList;
        }
        public List<RosterHeadBO> GetCurrentActiveRosterHeadInfo()
        {
            List<RosterHeadBO> bpList = new List<RosterHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrentActiveRosterHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RosterHeadBO entityBO = new RosterHeadBO();

                                entityBO.RosterId = Convert.ToInt32(reader["RosterId"]);
                                entityBO.RosterName = reader["RosterName"].ToString();
                                entityBO.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                entityBO.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                bpList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return bpList;
        }
        public Boolean SaveRosterHeadInfo(RosterHeadBO entityBO, out int tmpRosterId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRosterHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RosterName", DbType.String, entityBO.RosterName);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, entityBO.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, entityBO.ToDate);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@RosterId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpRosterId = Convert.ToInt32(command.Parameters["@RosterId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateRosterHeadInfo(RosterHeadBO entityBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRosterHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RosterId", DbType.Int32, entityBO.RosterId);
                        dbSmartAspects.AddInParameter(command, "@RosterName", DbType.String, entityBO.RosterName);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, entityBO.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, entityBO.ToDate);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, entityBO.LastModifiedBy);

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
        public RosterHeadBO GetRosterHeadInfoById(int rosterId)
        {
            RosterHeadBO entityBO = new RosterHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRosterHeadInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RosterId", DbType.Int32, rosterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.RosterId = Convert.ToInt32(reader["RosterId"]);
                                entityBO.RosterName = reader["RosterName"].ToString();
                                entityBO.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                entityBO.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }

        public List<RosterHeadBO> GetRosterHeadInfoBySearchCriteria(string rosterHead)
        {
            List<RosterHeadBO> entityBOList = new List<RosterHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRosterHeadInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RosterHead", DbType.String, rosterHead);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RosterHeadBO entityBO = new RosterHeadBO();

                                entityBO.RosterId = Convert.ToInt32(reader["RosterId"]);
                                entityBO.RosterName = reader["RosterName"].ToString();
                                entityBO.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                entityBO.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
    }
}
