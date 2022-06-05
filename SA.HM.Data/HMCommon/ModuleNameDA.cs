using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class ModuleNameDA : BaseService
    {
        public List<ModuleNameBO> GetAllModuleInfo()
        {
            List<ModuleNameBO> entityBOList = new List<ModuleNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllModuleInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ModuleNameBO entityBO = new ModuleNameBO();

                                entityBO.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                entityBO.ModuleName = reader["ModuleName"].ToString();
                                entityBO.GroupName = reader["GroupName"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.IsReportType = Convert.ToBoolean(reader["IsReportType"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<ModuleNameBO> GetAllActiveModuleInfo()
        {
            List<ModuleNameBO> entityBOList = new List<ModuleNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllActiveModuleInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ModuleNameBO entityBO = new ModuleNameBO();

                                entityBO.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                entityBO.ModuleName = reader["ModuleName"].ToString();
                                entityBO.GroupName = reader["GroupName"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.IsReportType = Convert.ToBoolean(reader["IsReportType"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<ModuleNameBO> GetModuleInfo(int userGroupId)
        {
            List<ModuleNameBO> entityBOList = new List<ModuleNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetModuleInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.String, userGroupId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ModuleNameBO entityBO = new ModuleNameBO();

                                entityBO.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                entityBO.ModuleName = reader["ModuleName"].ToString();
                                entityBO.GroupName = reader["GroupName"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.IsReportType = Convert.ToBoolean(reader["IsReportType"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<ModuleNameBO> GetSecurityObjectTabInfo(int userGroupId, int moduleId)
        {
            List<ModuleNameBO> entityBOList = new List<ModuleNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSecurityObjectTabInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.String, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.String, moduleId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ModuleNameBO entityBO = new ModuleNameBO();

                                entityBO.ObjectTabId = Convert.ToInt32(reader["ObjectTabId"]);
                                entityBO.MenuHead = reader["MenuHead"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<ModuleNameBO> GetSecurityMenuLinksInfo(int userGroupId, int moduleId)
        {
            List<ModuleNameBO> entityBOList = new List<ModuleNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSecurityMenuLinksInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.String, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.String, moduleId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ModuleNameBO entityBO = new ModuleNameBO();

                                entityBO.MenuLinksId = Convert.ToInt32(reader["MenuLinksId"]);
                                entityBO.PageName = reader["PageName"].ToString();
                                entityBO.PageDisplayCaption = reader["PageDisplayCaption"].ToString();

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
