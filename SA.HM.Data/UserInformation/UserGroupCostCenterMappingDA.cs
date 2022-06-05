using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.UserInformation;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.UserInformation
{
    public class UserGroupCostCenterMappingDA : BaseService
    {
        public bool SaveUserGroupCostCenterMappingInfo(UserGroupCostCenterMappingBO costCenter, out int MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUserGroupCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, costCenter.UserGroupId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool SaveUserGroupProjectMappingInfo(UserGroupCostCenterMappingBO project, out int MappingId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUserGroupProjectMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, project.UserGroupId);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, project.ProjectId);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public List<UserGroupCostCenterMappingBO> GetUserGroupCostCenterMappingByUserGroupId(int userGroupId)
        {
            List<UserGroupCostCenterMappingBO> costList = new List<UserGroupCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupCostCenterMappingByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupCostCenterMappingBO costBO = new UserGroupCostCenterMappingBO();
                                costBO.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public List<UserGroupCostCenterMappingBO> GetUserGroupProjectMappingByUserGroupId(int userGroupId)
        {
            List<UserGroupCostCenterMappingBO> costList = new List<UserGroupCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupProjectMappingByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupCostCenterMappingBO costBO = new UserGroupCostCenterMappingBO();
                                costBO.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
                                costBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                costList.Add(costBO);
                            }
                        }
                    }
                }
            }
            return costList;
        }
        public bool UpdateUserGroupCostCenterMappingInfo(UserGroupCostCenterMappingBO costCenter)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUserGroupCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, costCenter.MappingId);
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, costCenter.UserGroupId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenter.CostCenterId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateUserProjectCenterMappingInfo(UserGroupCostCenterMappingBO project)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUserProjectCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, project.MappingId);
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, project.UserGroupId);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, project.ProjectId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllUserGroupCostCenterMappingInfoWithoutMappingIdList(int userGroupId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllUserGroupCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllUserGroupProjectMappingInfoWithoutMappingIdList(int userGroupId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllUserGroupProjectMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllUserGroupCostCenterMappingInfoByUserGroupId(int userGroupId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllUserGroupCostCenterMappingInfoByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userGroupId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteAllUserGroupProjectMappingInfoByUserGroupId(int userGroupId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllUserGroupProjectMappingInfoByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userGroupId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
