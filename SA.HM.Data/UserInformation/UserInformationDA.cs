using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.UserInformation;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.UserInformation
{
    public class UserInformationDA : BaseService
    {
        public List<UserInformationBO> GetUserInformation()
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserIdAndUserName = reader["UserIdAndUserName"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }
        public List<UserInformationBO> GetEmpAssignedUserInformation()
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAssignedUserInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserIdAndUserName = reader["UserIdAndUserName"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }
        public List<UserInformationBO> GetAllUserInformation()
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllUserInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserIdAndUserName = reader["UserIdAndUserName"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }
        //public Boolean SaveUserInformation(UserInformationBO userInformation, List<SecurityUserAdminAuthorizationBO> adminAuthorizationList, out int tmpUserInfoId)
        //{
        //    Boolean status = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUserInformation_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userInformation.UserGroupId);
        //            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, userInformation.EmpId);
        //            dbSmartAspects.AddInParameter(command, "@UserName", DbType.String, userInformation.UserName);
        //            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, userInformation.UserId);
        //            dbSmartAspects.AddInParameter(command, "@UserPassword", DbType.String, userInformation.UserPassword);
        //            dbSmartAspects.AddInParameter(command, "@UserEmail", DbType.String, userInformation.UserEmail);
        //            dbSmartAspects.AddInParameter(command, "@UserPhone", DbType.String, userInformation.UserPhone);
        //            dbSmartAspects.AddInParameter(command, "@UserDesignation", DbType.String, userInformation.UserDesignation);
        //            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, userInformation.ActiveStat);
        //            dbSmartAspects.AddInParameter(command, "@IsAdminUser", DbType.Boolean, userInformation.IsAdminUser);
        //            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, userInformation.CreatedBy);
        //            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, userInformation.SupplierId);
        //            dbSmartAspects.AddOutParameter(command, "@UserInfoId", DbType.Int32, sizeof(Int32));

        //            status = dbSmartAspects.ExecuteNonQuery(command, transction);
        //            tmpUserInfoId = Convert.ToInt32(commandMaster.Parameters["@UserInfoId"].Value);



        //            if (status)
        //            {
        //                using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveUserGroupCostCenterMappingInfo_SP"))
        //                {
        //                    foreach (UserGroupCostCenterMappingBO mappingBO in adminAuthorizationList)
        //                    {
        //                        commandMapping.Parameters.Clear();

        //                        dbSmartAspects.AddInParameter(commandMapping, "@UserGroupId", DbType.Int32, tmpUserGroupId);
        //                        dbSmartAspects.AddInParameter(commandMapping, "@CostCenterId", DbType.Int32, mappingBO.CostCenterId);
        //                        dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));

        //                        status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    return status;
        //}
        public Boolean SaveUserInformation(UserInformationBO userInformation, List<SecurityUserAdminAuthorizationBO> adminAuthorizationList, out int tmpUserInfoId)
        {
            bool retVal = false;
            int status = 0;
            int statusAuthorization = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUserInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userInformation.UserGroupId);
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, userInformation.EmpId);
                            dbSmartAspects.AddInParameter(command, "@UserName", DbType.String, userInformation.UserName);
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, userInformation.UserId);
                            dbSmartAspects.AddInParameter(command, "@UserPassword", DbType.String, userInformation.UserPassword);
                            dbSmartAspects.AddInParameter(command, "@UserEmail", DbType.String, userInformation.UserEmail);
                            dbSmartAspects.AddInParameter(command, "@UserPhone", DbType.String, userInformation.UserPhone);
                            dbSmartAspects.AddInParameter(command, "@UserDesignation", DbType.String, userInformation.UserDesignation);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, userInformation.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@IsAdminUser", DbType.Boolean, userInformation.IsAdminUser);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, userInformation.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, userInformation.SupplierId);
                            dbSmartAspects.AddOutParameter(command, "@UserInfoId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            tmpUserInfoId = Convert.ToInt32(command.Parameters["@UserInfoId"].Value);
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveSecurityUserAdminAuthorization_SP"))
                            {
                                foreach (SecurityUserAdminAuthorizationBO mappingBO in adminAuthorizationList)
                                {
                                    commandMapping.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandMapping, "@UserInfoId", DbType.Int32, tmpUserInfoId);
                                    dbSmartAspects.AddInParameter(commandMapping, "@ModuleId", DbType.Int32, mappingBO.ModuleId);
                                    dbSmartAspects.AddOutParameter(commandMapping, "@Id", DbType.Int32, sizeof(Int32));

                                    statusAuthorization = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                                    //Convert.ToInt32(command.Parameters["@Id"].Value);
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
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        //public Boolean UpdateUserInformation(UserInformationBO userInformation, List<SecurityUserAdminAuthorizationBO> adminAuthorizationList)
        //{
        //    Boolean status = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUserInformation_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInformation.UserInfoId);
        //            dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userInformation.UserGroupId);
        //            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, userInformation.EmpId);
        //            dbSmartAspects.AddInParameter(command, "@UserName", DbType.String, userInformation.UserName);
        //            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, userInformation.UserId);
        //            dbSmartAspects.AddInParameter(command, "@UserPassword", DbType.String, userInformation.UserPassword);
        //            dbSmartAspects.AddInParameter(command, "@UserEmail", DbType.String, userInformation.UserEmail);
        //            dbSmartAspects.AddInParameter(command, "@UserPhone", DbType.String, userInformation.UserPhone);
        //            dbSmartAspects.AddInParameter(command, "@UserDesignation", DbType.String, userInformation.UserDesignation);
        //            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, userInformation.ActiveStat);
        //            dbSmartAspects.AddInParameter(command, "@IsAdminUser", DbType.Boolean, userInformation.IsAdminUser);
        //            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, userInformation.SupplierId);
        //            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, userInformation.LastModifiedBy);

        //            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //        }
        //    }
        //    return status;
        //}
        public bool UpdateUserInformation(UserInformationBO userInformation, List<SecurityUserAdminAuthorizationBO> adminAuthorizationList)
        {
            bool retVal = false;
            int status = 0;
            Boolean costStatus = false;
            bool statusDeleteAllInfo = false;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUserInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInformation.UserInfoId);
                            dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, userInformation.UserGroupId);
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, userInformation.EmpId);
                            dbSmartAspects.AddInParameter(command, "@UserName", DbType.String, userInformation.UserName);
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, userInformation.UserId);
                            dbSmartAspects.AddInParameter(command, "@UserPassword", DbType.String, userInformation.UserPassword);
                            dbSmartAspects.AddInParameter(command, "@UserEmail", DbType.String, userInformation.UserEmail);
                            dbSmartAspects.AddInParameter(command, "@UserPhone", DbType.String, userInformation.UserPhone);
                            dbSmartAspects.AddInParameter(command, "@UserDesignation", DbType.String, userInformation.UserDesignation);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, userInformation.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@IsAdminUser", DbType.Boolean, userInformation.IsAdminUser);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, userInformation.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, userInformation.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0 && adminAuthorizationList != null)
                        {
                            if (adminAuthorizationList.Count > 0)
                            {
                                costStatus = UpdateAdminAuthorization(adminAuthorizationList);
                            }
                            else
                            {
                                statusDeleteAllInfo = DeleteAllUserAdminAuthorizationInfoByUserInfoId(userInformation.UserInfoId);
                            }
                        }

                        //if (status > 0 && (costStatus || statusDeleteAllInfo))
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
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }
        public Boolean DeleteAllUserAdminAuthorizationInfoByUserInfoId(int userInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllSecurityUserAdminAuthorizationInfoByUserInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        private int IsAvailableInDB(List<SecurityUserAdminAuthorizationBO> adminAuthorizationList, SecurityUserAdminAuthorizationBO adminAuthorizationBO)
        {
            int isInDB = 0;
            for (int j = 0; j < adminAuthorizationList.Count; j++)
            {
                if (adminAuthorizationList[j].UserInfoId == adminAuthorizationBO.UserInfoId && adminAuthorizationBO.ModuleId == adminAuthorizationList[j].ModuleId)
                {
                    isInDB = adminAuthorizationList[j].Id;
                }
            }
            return isInDB;
        }
        private bool UpdateAdminAuthorization(List<SecurityUserAdminAuthorizationBO> adminAuthorizationList)
        {
            //UserGroupCostCenterMappingDA costDA = new UserGroupCostCenterMappingDA();
            //List<UserGroupCostCenterMappingBO> dbCostList = new List<UserGroupCostCenterMappingBO>();
            //dbCostList = costDA.GetUserGroupCostCenterMappingByUserGroupId(adminAuthorizationList[0].UserInfoId);

            List<int> idList = new List<int>();


            for (int i = 0; i < adminAuthorizationList.Count; i++)
            {
                int succ = IsAvailableInDB(adminAuthorizationList, adminAuthorizationList[i]);
                if (succ > 0)
                {
                    //Update
                    adminAuthorizationList[i].Id = succ;
                    //bool status = costDA.UpdateUserGroupCostCenterMappingInfo(adminAuthorizationList[i]);

                    Boolean statusUpdate = false;
                    using (DbConnection conn = dbSmartAspects.CreateConnection())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSecurityUserAdminAuthorization_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, adminAuthorizationList[i].Id);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, adminAuthorizationList[i].UserInfoId);
                            dbSmartAspects.AddInParameter(command, "@ModuleId", DbType.Int32, adminAuthorizationList[i].ModuleId);
                            statusUpdate = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }

                    idList.Add(succ);
                }
                else
                {
                    //Insert

                    bool statusSave = false;
                    using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveSecurityUserAdminAuthorization_SP"))
                    {
                        //foreach (SecurityUserAdminAuthorizationBO adminAuthorizationBO in adminAuthorizationList)
                        //{
                        int Id;
                        commandMapping.Parameters.Clear();

                        dbSmartAspects.AddInParameter(commandMapping, "@UserInfoId", DbType.Int32, adminAuthorizationList[i].UserInfoId);
                        dbSmartAspects.AddInParameter(commandMapping, "@ModuleId", DbType.Int32, adminAuthorizationList[i].ModuleId);
                        dbSmartAspects.AddOutParameter(commandMapping, "@Id", DbType.Int32, sizeof(Int32));

                        statusSave = dbSmartAspects.ExecuteNonQuery(commandMapping) > 0 ? true : false;
                        Id = Convert.ToInt32(commandMapping.Parameters["@Id"].Value);

                        idList.Add(Id);
                        //}
                    }

                    //bool status = costDA.SaveUserGroupCostCenterMappingInfo(adminAuthorizationList[i], out mappingId);

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
            // Boolean deleteStatus = costDA.DeleteAllUserGroupCostCenterMappingInfoWithoutMappingIdList(adminAuthorizationList[0].UserGroupId, saveAndUpdatedIdList);


            Boolean deleteStatus = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllSecurityUserAdminAuthorizationInfoWithoutIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, adminAuthorizationList[0].UserInfoId);
                    dbSmartAspects.AddInParameter(command, "@IdList", DbType.String, saveAndUpdatedIdList);

                    deleteStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }


            return true;
        }
        public Boolean ChangeUserPassword(UserInformationBO userInformation)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ChangeUserPassword_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInformation.UserInfoId);
                    dbSmartAspects.AddInParameter(command, "@UserName", DbType.String, userInformation.UserName);
                    dbSmartAspects.AddInParameter(command, "@UserPassword", DbType.String, userInformation.UserPassword);
                    dbSmartAspects.AddInParameter(command, "@UserEmail", DbType.String, userInformation.UserEmail);
                    dbSmartAspects.AddInParameter(command, "@UserPhone", DbType.String, userInformation.UserPhone);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, userInformation.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public UserInformationBO GetUserInformationByIdNPassword(int userInfoId, string userPassword)
        {
            UserInformationBO userInformation = new UserInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByIdNPassword_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@UserPassword", DbType.String, userPassword);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);

                                if (!string.IsNullOrEmpty(reader["EmpId"].ToString()))
                                    userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);

                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();
                                userInformation.IsAdminUser = Convert.ToBoolean(reader["IsAdminUser"]);
                            }
                        }
                    }
                }
            }
            return userInformation;
        }
        public UserInformationBO GetUserInformationById(int userInfoId)
        {
            UserInformationBO userInformation = new UserInformationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);

                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();
                                userInformation.IsAdminUser = Convert.ToBoolean(reader["IsAdminUser"]);
                                userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                            }
                        }
                    }
                }
            }
            return userInformation;
        }
        public List<SecurityUserAdminAuthorizationBO> GetSecurityUserAdminAuthorizationByUserInfoId(int userInfoId)
        {
            List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserAdminAuthorizationByUserInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SecurityUserAdminAuthorizationBO adminAuthorization = new SecurityUserAdminAuthorizationBO();
                                adminAuthorization.Id = Convert.ToInt32(reader["Id"]);
                                adminAuthorization.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                adminAuthorization.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                adminAuthorizationList.Add(adminAuthorization);
                            }
                        }
                    }
                }
            }
            return adminAuthorizationList;
        }
        public UserInformationBO GetUserInformationByReservationId(int reservationId)
        {
            UserInformationBO userInformation = new UserInformationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return userInformation;
        }
        public UserInformationBO GetUserInformationByUserNameNId(string userId, string userPassword)
        {
            UserInformationBO userInformation = new UserInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByUserNameNId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@UserPassword", DbType.String, userPassword);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.GroupName = reader["GroupName"].ToString();
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                                userInformation.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();
                                userInformation.InnboardHomePage = reader["InnboardHomePage"].ToString();
                                userInformation.IsAdminUser = Convert.ToBoolean(reader["IsAdminUser"]);
                                userInformation.UserGroupType = reader["UserGroupType"].ToString();
                                userInformation.UserSignature = reader["UserSignature"].ToString();
                                userInformation.IsPaymentBillInfoHideInCompanyBillReceive = Convert.ToInt32(reader["IsPaymentBillInfoHideInCompanyBillReceive"]);
                                userInformation.IsReceiveBillInfoHideInSupplierBillPayment = Convert.ToInt32(reader["IsReceiveBillInfoHideInSupplierBillPayment"]);
                            }
                        }
                    }
                }
            }
            return userInformation;
        }

        public UserInformationBO GetUserInformationByUserIdNHashPassword(string userId, string userPassword)
        {
            UserInformationBO userInformation = new UserInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByUserIdNHashPassword_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@UserPassword", DbType.String, userPassword);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.GroupName = reader["GroupName"].ToString();
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                                userInformation.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();
                                userInformation.InnboardHomePage = reader["InnboardHomePage"].ToString();
                                userInformation.IsAdminUser = Convert.ToBoolean(reader["IsAdminUser"]);
                                userInformation.UserGroupType = reader["UserGroupType"].ToString();
                            }
                        }
                    }
                }
            }
            return userInformation;
        }

        public UserInformationBO GetUserInformationBySupplierId(int? supplierId)
        {
            UserInformationBO userInformation = new UserInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationBySupplierId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@supplierId", DbType.Int32, supplierId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.GroupName = reader["GroupName"].ToString();
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);
                                userInformation.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();
                                userInformation.InnboardHomePage = reader["InnboardHomePage"].ToString();
                                userInformation.IsAdminUser = Convert.ToBoolean(reader["IsAdminUser"]);
                                userInformation.UserGroupType = reader["UserGroupType"].ToString();
                            }
                        }
                    }
                }
            }
            return userInformation;
        }

        public Boolean DeleteUserInformationById(int userInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteUserInformationById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<UserInformationBO> GetUserInformationBySearchCriteria(string UserName, string Email, string UserId, string PhoneNo, int GroupId, bool ActiveState)
        {
            string Where = GenarateWhereCondition(GroupId, UserName, UserId, Email, PhoneNo, ActiveState);
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationBySearchCriteria_SP"))
                {
                    //dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, GroupId);
                    //dbSmartAspects.AddInParameter(cmd, "@UserName", DbType.String, UserName);
                    //dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, UserId);
                    //dbSmartAspects.AddInParameter(cmd, "@UserEmail", DbType.String, Email);
                    //dbSmartAspects.AddInParameter(cmd, "@UserPhone", DbType.String, PhoneNo);
                    //dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveState);

                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);

                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }

        public List<UserInformationBO> GetUserInformationAutoSearch(string searchTerm)
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);

                                if (!string.IsNullOrEmpty(reader["EmpId"].ToString()))
                                    userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);

                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }

        public List<UserInformationBO> GetUserInformationByUserGroup(int userGroupId)
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);

                                if (!string.IsNullOrEmpty(reader["EmpId"].ToString()))
                                    userInformation.EmpId = Convert.ToInt32(reader["EmpId"]);

                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                //userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }
        public List<UserInformationBO> GetUserInformationByUserGroupForTouchPanelLogin(int userGroupId)
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByUserGroupForTouchPanelLogin_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);

                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                //userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }

        public bool ChangeUserAccountInformation(UserInformationBO userInformationBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ChangeUserAccountInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInformationBO.UserInfoId);
                    dbSmartAspects.AddInParameter(command, "@UserName", DbType.String, userInformationBO.UserName);
                    dbSmartAspects.AddInParameter(command, "@UserEmail", DbType.String, userInformationBO.UserEmail);
                    dbSmartAspects.AddInParameter(command, "@UserPhone", DbType.String, userInformationBO.UserPhone);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, userInformationBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean UpdateUserWorkingCostCenterInfo(string userType, int userInfoId, int workingCostCenterId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUserWorkingCostCenterInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserType", DbType.String, userType);
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(command, "@WorkingCostCenterId", DbType.Int32, workingCostCenterId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public string GenarateWhereCondition(int grpId, string usrName, string usrId, string usrMail, string usrphn, bool activeStat)
        {
            string Where = string.Empty;
            //Where = "UserGroupId = '" + grpId + "' AND ActiveStat = '" + activeStat + "'";
            Where = "ActiveStat = '" + activeStat + "'";

            if (grpId > 0)
            {
                Where += " AND UserGroupId = '" + grpId + "'";
            }

            usrName = usrName.Trim();
            usrId = usrId.Trim();
            usrMail = usrMail.Trim();
            usrphn = usrphn.Trim();
            if (!string.IsNullOrEmpty(usrName))
            {
                if (!string.IsNullOrEmpty(usrId))
                {
                    if (!string.IsNullOrWhiteSpace(usrMail))
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserName LIKE '%"+usrName+ "%' AND UserId LIKE '%" + usrId + "%' AND UserEmail LIKE '%" + usrMail + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += " AND UserName LIKE '%"+usrName+ "%' AND UserId LIKE '%" + usrId + "%' AND UserEmail LIKE '%" + usrMail + "%'";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserName LIKE '%"+usrName+ "%' AND UserId LIKE '%" + usrId + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += " AND UserName LIKE '%"+usrName+ "%' AND UserId LIKE '%" + usrId + "%'";
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(usrMail))
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserName LIKE '%"+usrName+ "%' AND UserEmail LIKE '%" + usrMail + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += " AND UserName LIKE '%"+usrName+ "%' AND UserEmail LIKE '%" + usrMail + "%'";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserName LIKE '%" + usrName + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += " AND UserName LIKE '%" + usrName + "%'";
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(usrId))
                {
                    if (!string.IsNullOrWhiteSpace(usrMail))
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserId LIKE '%" + usrId + "%' AND UserEmail LIKE '%" + usrMail + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += " AND UserId LIKE '%" + usrId + "%' AND UserEmail LIKE '%" + usrMail + "%'";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserId LIKE '%" + usrId + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += " AND UserId LIKE '%" + usrId + "%'";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(usrMail))
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserEmail LIKE '%" + usrMail + "%' AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                            Where += "  AND UserEmail LIKE '%" + usrMail + "%'";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(usrphn))
                        {
                            Where += " AND UserPhone LIKE '%" + usrphn + "%'";
                        }
                        else
                        {
                        }
                    }

                }
            }
            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }
        public List<UserInformationBO> GetCashierOrWaiterInformation()
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashierOrWaiterInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                userInformation.UserName = reader["UserName"].ToString();
                                userInformation.UserId = reader["UserId"].ToString();
                                userInformation.UserPassword = reader["UserPassword"].ToString();
                                userInformation.UserEmail = reader["UserEmail"].ToString();
                                userInformation.UserPhone = reader["UserPhone"].ToString();
                                userInformation.UserDesignation = reader["UserDesignation"].ToString();
                                userInformation.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                userInformation.ActiveStatus = reader["ActiveStatus"].ToString();

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }

        public List<UserInformationBO> GetCashierWaiterInformation(int isBearer)
        {
            List<UserInformationBO> userInformationList = new List<UserInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashierWaiterInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserInformationBO userInformation = new UserInformationBO();

                                userInformation.EmpId = Convert.ToInt32(reader["UserInfoId"]);
                                userInformation.DisplayName = Convert.ToString(reader["UserName"]);

                                userInformationList.Add(userInformation);
                            }
                        }
                    }
                }
            }
            return userInformationList;
        }

        public UserInformationBO GetUserInformationForLeaveNotification(int designationId)
        {
            UserInformationBO userInfoBO = new UserInformationBO();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationForLeaveNotification_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DesignationId", DbType.Int32, designationId);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "UserInformation");
                    DataTable table = employeeDS.Tables["UserInformation"];

                    userInfoBO = table.AsEnumerable().Select(r =>
                                   new UserInformationBO
                                   {
                                       UserInfoId = r.Field<Int32>("UserInfoId"),
                                       UserGroupId = r.Field<Int32>("UserGroupId"),
                                       EmpId = r.Field<Int32>("EmpId"),
                                       UserName = r.Field<string>("UserName"),
                                       UserId = r.Field<string>("UserId"),
                                       UserEmail = r.Field<string>("UserEmail"),
                                       UserPhone = r.Field<string>("UserPhone")
                                   }).FirstOrDefault();
                }
            }

            return userInfoBO;
        }
        public UserInformationBO GetUserInformationByEmpId(int empId)
        {
            UserInformationBO userInfoBO = new UserInformationBO();

            DataSet employeeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    dbSmartAspects.LoadDataSet(cmd, employeeDS, "UserInformation");
                    DataTable table = employeeDS.Tables["UserInformation"];

                    userInfoBO = table.AsEnumerable().Select(r =>
                                   new UserInformationBO
                                   {
                                       UserInfoId = r.Field<Int32>("UserInfoId"),
                                       UserGroupId = r.Field<Int32>("UserGroupId"),
                                       EmpId = r.Field<Int32>("EmpId"),
                                       UserName = r.Field<string>("UserName"),
                                       UserId = r.Field<string>("UserId"),
                                       UserEmail = r.Field<string>("UserEmail"),
                                       UserPhone = r.Field<string>("UserPhone"),
                                       DisplayName = r.Field<string>("DisplayName")
                                   }).FirstOrDefault();
                }
            }

            return userInfoBO;
        }
    }
}
