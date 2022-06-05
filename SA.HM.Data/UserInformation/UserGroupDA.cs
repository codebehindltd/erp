using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.UserInformation;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.UserInformation
{
    public class UserGroupDA : BaseService
    {
        public List<UserGroupBO> GetUserGroupInfo()
        {
            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroup_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupBO userGroup = new UserGroupBO();

                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();
                                userGroup.UserGroupType = reader["UserGroupType"].ToString();
                                userGroup.Email = reader["Email"].ToString();

                                userGroupList.Add(userGroup);
                            }
                        }
                    }
                }
            }
            return userGroupList;
        }
        public List<UserGroupBO> GetActiveUserGroupInfo(int userGroupId)
        {
            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveUserGroupInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupBO userGroup = new UserGroupBO();

                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();

                                userGroupList.Add(userGroup);
                            }
                        }
                    }
                }
            }
            return userGroupList;
        }
        public List<UserGroupBO> GetAllUserGroupInfo(int userGroupId)
        {
            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupBO userGroup = new UserGroupBO();

                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();

                                userGroupList.Add(userGroup);
                            }
                        }
                    }
                }
            }
            return userGroupList;
        }
        public List<UserGroupBO> GetRestaurantPermittedUserGroupInfo()
        {
            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantPermittedUserGroup_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupBO userGroup = new UserGroupBO();

                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();

                                userGroupList.Add(userGroup);
                            }
                        }
                    }
                }
            }
            return userGroupList;
        }
        //public Boolean SaveUserGroupInfo(UserGroupBO userGroup, List<InvUserGroupCostCenterMappingBO> costCenterList, out int tmpUserGroupId)
        //{
        //    //Boolean status = false;
        //    //using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    //{
        //    //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUserGroupInfo_SP"))
        //    //    {
        //    //        dbSmartAspects.AddInParameter(command, "@GroupName", DbType.String, userGroup.GroupName);
        //    //        dbSmartAspects.AddInParameter(command, "@DefaultHomePageId", DbType.Int32, userGroup.DefaultHomePageId);
        //    //        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, userGroup.ActiveStat);
        //    //        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, userGroup.CreatedBy);
        //    //        dbSmartAspects.AddOutParameter(command, "@UserGroupId", DbType.Int32, sizeof(Int32));

        //    //        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

        //    //        tmpUserGroupId = Convert.ToInt32(command.Parameters["@UserGroupId"].Value);
        //    //    }
        //    //}

        //    //if (status > 0)
        //    //{
        //    //    using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveInvCategoryCostCenterMappingInfo_SP"))
        //    //    {
        //    //        foreach (InvCategoryCostCenterMappingBO mappingBO in costCenterList)
        //    //        {
        //    //            commandMapping.Parameters.Clear();

        //    //            dbSmartAspects.AddInParameter(commandMapping, "@CategoryId", DbType.Int32, tmpProductCatagoryId);
        //    //            dbSmartAspects.AddInParameter(commandMapping, "@CostCenterId", DbType.Int32, mappingBO.CostCenterId);
        //    //            dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));

        //    //            status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
        //    //        }
        //    //    }
        //    //}

        //    //return status;
        //}
        public Boolean SaveUserGroupInfo(UserGroupBO userGroup, List<UserGroupCostCenterMappingBO> costCenterList, List<UserGroupCostCenterMappingBO> projectList, out int tmpUserGroupId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveUserGroupInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@GroupName", DbType.String, userGroup.GroupName);
                            dbSmartAspects.AddInParameter(commandMaster, "@DefaultHomePageId", DbType.Int32, userGroup.DefaultHomePageId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, userGroup.ActiveStat);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, userGroup.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@Email", DbType.String, userGroup.Email);
                            dbSmartAspects.AddInParameter(commandMaster, "@UserGroupType", DbType.String, userGroup.UserGroupType);
                            dbSmartAspects.AddOutParameter(commandMaster, "@UserGroupId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmpUserGroupId = Convert.ToInt32(commandMaster.Parameters["@UserGroupId"].Value);
                        }


                        if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveUserGroupCostCenterMappingInfo_SP"))
                            {
                                foreach (UserGroupCostCenterMappingBO mappingBO in costCenterList)
                                {
                                    commandMapping.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandMapping, "@UserGroupId", DbType.Int32, tmpUserGroupId);
                                    dbSmartAspects.AddInParameter(commandMapping, "@CostCenterId", DbType.Int32, mappingBO.CostCenterId);
                                    dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveUserGroupProjectMappingInfo_SP"))
                            {
                                foreach (UserGroupCostCenterMappingBO mappingBO in projectList)
                                {
                                    commandMapping.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandMapping, "@UserGroupId", DbType.Int32, tmpUserGroupId);
                                    dbSmartAspects.AddInParameter(commandMapping, "@ProjectId", DbType.Int32, mappingBO.ProjectId);
                                    dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));
                                    status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch(Exception ex)
                    {
                        transction.Rollback();
                        throw   ex;
                    }
                }
            }

            return retVal;
        }
        //public Boolean UpdateUserGroupInfo(UserGroupBO userGroup, List<InvUserGroupCostCenterMappingBO> costCenterList)
        //{
        //    Boolean status = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUserGroupInfo_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userGroup.UserGroupId);
        //            dbSmartAspects.AddInParameter(command, "@GroupName", DbType.String, userGroup.GroupName);
        //            dbSmartAspects.AddInParameter(command, "@DefaultHomePageId", DbType.Int32, userGroup.DefaultHomePageId);
        //            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, userGroup.ActiveStat);
        //            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, userGroup.LastModifiedBy);

        //            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //        }
        //    }
        //    return status;
        //}

        public bool UpdateUserGroupInfo(UserGroupBO userGroup, List<UserGroupCostCenterMappingBO> costCenterList, List<UserGroupCostCenterMappingBO> projectList)
        {
            bool retVal = false;
            int status = 0;
            Boolean costStatus = false;
            bool statusDeleteAllInfo = false;
            
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateUserGroupInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@UserGroupId", DbType.Int32, userGroup.UserGroupId);
                            dbSmartAspects.AddInParameter(commandMaster, "@GroupName", DbType.String, userGroup.GroupName);
                            dbSmartAspects.AddInParameter(commandMaster, "@DefaultHomePageId", DbType.Int32, userGroup.DefaultHomePageId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Email", DbType.String, userGroup.Email);
                            dbSmartAspects.AddInParameter(commandMaster, "@UserGroupType", DbType.String, userGroup.UserGroupType);
                            dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, userGroup.ActiveStat);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, userGroup.LastModifiedBy);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0 && costCenterList != null)
                        {
                            if (costCenterList.Count > 0)
                            {
                                costStatus = UpdateUserGroupCostCenterMapping(costCenterList);
                            }
                            else
                            {
                                UserGroupCostCenterMappingDA costDA = new UserGroupCostCenterMappingDA();
                                statusDeleteAllInfo = costDA.DeleteAllUserGroupCostCenterMappingInfoByUserGroupId(userGroup.UserGroupId);
                            }
                        }

                        if (status > 0 && projectList != null)
                        {
                            if (projectList.Count > 0)
                            {
                                costStatus = UpdateUserGroupProjectMapping(projectList);
                            }
                            else
                            {
                                UserGroupCostCenterMappingDA costDA = new UserGroupCostCenterMappingDA();
                                statusDeleteAllInfo = costDA.DeleteAllUserGroupProjectMappingInfoByUserGroupId(userGroup.UserGroupId);
                            }
                        }

                        if (status > 0 && (costStatus|| statusDeleteAllInfo))
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch(Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }
        private int IsCostcenterAvailableInDb(List<UserGroupCostCenterMappingBO> dbCostList, UserGroupCostCenterMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].UserGroupId == costBO.UserGroupId && costBO.CostCenterId == dbCostList[j].CostCenterId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        private int IsProjectAvailableInDb(List<UserGroupCostCenterMappingBO> dbCostList, UserGroupCostCenterMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].UserGroupId == costBO.UserGroupId && costBO.ProjectId == dbCostList[j].ProjectId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        private bool UpdateUserGroupCostCenterMapping(List<UserGroupCostCenterMappingBO> costCenterList)
        {
            UserGroupCostCenterMappingDA costDA = new UserGroupCostCenterMappingDA();
            List<UserGroupCostCenterMappingBO> dbCostList = new List<UserGroupCostCenterMappingBO>();
            dbCostList = costDA.GetUserGroupCostCenterMappingByUserGroupId(costCenterList[0].UserGroupId);

            List<int> idList = new List<int>();
            int mappingId;

            for (int i = 0; i < costCenterList.Count; i++)
            {
                int succ = IsCostcenterAvailableInDb(dbCostList, costCenterList[i]);
                if (succ > 0)
                {
                    //Update
                    costCenterList[i].MappingId = succ;
                    bool status = costDA.UpdateUserGroupCostCenterMappingInfo(costCenterList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveUserGroupCostCenterMappingInfo(costCenterList[i], out mappingId);
                    idList.Add(mappingId);
                }
            }

            string saveAndUpdatedIdList = string.Empty;
            for (int j = 0; j < idList.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(saveAndUpdatedIdList))
                {
                    saveAndUpdatedIdList = idList[j].ToString();
                }
                else
                {
                    saveAndUpdatedIdList = saveAndUpdatedIdList + "," + idList[j];
                }
            }
            Boolean deleteStatus = costDA.DeleteAllUserGroupCostCenterMappingInfoWithoutMappingIdList(costCenterList[0].UserGroupId, saveAndUpdatedIdList);
            return true;
        }
        private bool UpdateUserGroupProjectMapping(List<UserGroupCostCenterMappingBO> projectList)
        {
            UserGroupCostCenterMappingDA costDA = new UserGroupCostCenterMappingDA();
            List<UserGroupCostCenterMappingBO> dbCostList = new List<UserGroupCostCenterMappingBO>();
            dbCostList = costDA.GetUserGroupProjectMappingByUserGroupId(projectList[0].UserGroupId);

            List<int> idList = new List<int>();
            int mappingId;

            for (int i = 0; i < projectList.Count; i++)
            {
                int succ = IsProjectAvailableInDb(dbCostList, projectList[i]);
                if (succ > 0)
                {
                    //Update
                    projectList[i].MappingId = succ;
                    bool status = costDA.UpdateUserProjectCenterMappingInfo(projectList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveUserGroupProjectMappingInfo(projectList[i], out mappingId);
                    idList.Add(mappingId);
                }
            }

            string saveAndUpdatedIdList = string.Empty;
            for (int j = 0; j < idList.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(saveAndUpdatedIdList))
                {
                    saveAndUpdatedIdList = idList[j].ToString();
                }
                else
                {
                    saveAndUpdatedIdList = saveAndUpdatedIdList + "," + idList[j];
                }
            }
            Boolean deleteStatus = costDA.DeleteAllUserGroupProjectMappingInfoWithoutMappingIdList(projectList[0].UserGroupId, saveAndUpdatedIdList);
            return true;
        }
        public UserGroupBO GetUserGroupInfoByGroupName(string groupName)
        {
            UserGroupBO userGroup = new UserGroupBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupInfoByGroupName_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupName", DbType.String, groupName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.DefaultModuleId = Convert.ToInt32(reader["ModuleId"]);
                                userGroup.DefaultHomePageId = Convert.ToInt32(reader["DefaultHomePageId"]);
                                userGroup.DefaultHomePage = reader["DefaultHomePage"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return userGroup;
        }
        public UserGroupBO GetUserGroupInfoById(int userGroupId)
        {
            UserGroupBO userGroup = new UserGroupBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.DefaultModuleId = Convert.ToInt32(reader["ModuleId"]);
                                userGroup.DefaultHomePageId = Convert.ToInt32(reader["DefaultHomePageId"]);
                                userGroup.DefaultHomePage = reader["DefaultHomePage"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();
                                userGroup.Email = reader["Email"].ToString();
                                userGroup.UserGroupType = reader["UserGroupType"].ToString();
                            }
                        }
                    }
                }
            }
            return userGroup;
        }
        public Boolean DeleteUserGroupInfoById(int userGroupId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteUserGroupInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userGroupId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<UserGroupBO> GetUserGroupInfoBySearchCriteria(int userGroupId, string GroupName, int Status)
        {

            List<UserGroupBO> userGroupList = new List<UserGroupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    if (!string.IsNullOrWhiteSpace(GroupName))
                        dbSmartAspects.AddInParameter(cmd, "@GroupName", DbType.String, GroupName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GroupName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Int32, Status);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserGroupBO userGroup = new UserGroupBO();

                                userGroup.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userGroup.GroupName = reader["GroupName"].ToString();
                                userGroup.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userGroup.ActiveStatus = reader["ActiveStatus"].ToString();
                                userGroupList.Add(userGroup);
                            }
                        }
                    }
                }
            }
            return userGroupList;
        }
    }
}
