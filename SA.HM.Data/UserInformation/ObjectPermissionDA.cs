using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.UserInformation;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.UserInformation
{
    public class ObjectPermissionDA : BaseService
    {
        public List<ObjectPermissionBO> GetObjectPermission(int userGroupId, string groupHead)
        {
            List<ObjectPermissionBO> objectPermissionList = new List<ObjectPermissionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetObjectPermissionByUserGroupNGroupHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@ObjectGroupHead", DbType.String, groupHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ObjectPermissionBO objectPermission = new ObjectPermissionBO();

                                objectPermission.ObjectPermissionId = Convert.ToInt32(reader["ObjectPermissionId"]);
                                objectPermission.ObjectTabId = Convert.ToInt32(reader["ObjectTabId"]);
                                objectPermission.ObjectGroupHead = reader["ObjectGroupHead"].ToString();
                                objectPermission.ObjectHead = reader["ObjectHead"].ToString();
                                objectPermission.MenuHead = reader["MenuHead"].ToString();
                                objectPermission.ObjectType = reader["ObjectType"].ToString();
                                objectPermission.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                objectPermission.IsSavePermission = Convert.ToBoolean(reader["IsSavePermission"]);
                                objectPermission.SaveStatus = reader["SaveStatus"].ToString();
                                objectPermission.IsDeletePermission = Convert.ToBoolean(reader["IsDeletePermission"]);
                                objectPermission.DeleteStatus = reader["DeleteStatus"].ToString();
                                objectPermission.IsViewPermission = Convert.ToBoolean(reader["IsViewPermission"]);
                                objectPermission.ViewStatus = reader["ViewStatus"].ToString();

                                objectPermissionList.Add(objectPermission);
                            }
                        }
                    }
                }
            }
            return objectPermissionList;
        }
        public Boolean SaveObjectPermissionInfo(ObjectPermissionBO objectPermission, out int tmpObjectPermissionId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveObjectPermissionInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ObjectTabId", DbType.Int32, objectPermission.ObjectTabId);
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, objectPermission.UserGroupId);
                    dbSmartAspects.AddInParameter(command, "@IsSavePermission", DbType.Boolean, objectPermission.IsSavePermission);
                    dbSmartAspects.AddInParameter(command, "@IsDeletePermission", DbType.Boolean, objectPermission.IsDeletePermission);
                    dbSmartAspects.AddInParameter(command, "@IsViewPermission", DbType.Boolean, objectPermission.IsViewPermission);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, objectPermission.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ObjectPermissionId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpObjectPermissionId = Convert.ToInt32(command.Parameters["@ObjectPermissionId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateObjectPermissionInfo(ObjectPermissionBO objectPermission)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateObjectPermissionInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ObjectPermissionId", DbType.Int32, objectPermission.ObjectPermissionId);
                    dbSmartAspects.AddInParameter(command, "@ObjectTabId", DbType.Int32, objectPermission.ObjectTabId);
                    dbSmartAspects.AddInParameter(command, "@UserGroupId", DbType.Int32, objectPermission.UserGroupId);
                    dbSmartAspects.AddInParameter(command, "@IsSavePermission", DbType.Boolean, objectPermission.IsSavePermission);
                    dbSmartAspects.AddInParameter(command, "@IsDeletePermission", DbType.Boolean, objectPermission.IsDeletePermission);
                    dbSmartAspects.AddInParameter(command, "@IsViewPermission", DbType.Boolean, objectPermission.IsViewPermission);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, objectPermission.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<ObjectPermissionBO> GetFormPermissionByUserId(int userId, string objectType)
        {
            List<ObjectPermissionBO> objectPermissionList = new List<ObjectPermissionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormPermissionByUserId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userId);
                    dbSmartAspects.AddInParameter(cmd, "@ObjectType", DbType.String, objectType);
                    

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ObjectPermissionBO objectPermission = new ObjectPermissionBO();

                                objectPermission.ObjectPermissionId = Convert.ToInt32(reader["ObjectPermissionId"]);
                                objectPermission.ObjectTabId = Convert.ToInt32(reader["ObjectTabId"]);
                                objectPermission.ObjectGroupHead = reader["ObjectGroupHead"].ToString();
                                objectPermission.ObjectHead = reader["ObjectHead"].ToString();
                                objectPermission.MenuHead = reader["MenuHead"].ToString();
                                objectPermission.ObjectType = reader["ObjectType"].ToString();
                                objectPermission.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                objectPermission.IsSavePermission = Convert.ToBoolean(reader["IsSavePermission"]);
                                objectPermission.SaveStatus = reader["SaveStatus"].ToString();
                                objectPermission.IsDeletePermission = Convert.ToBoolean(reader["IsDeletePermission"]);
                                objectPermission.DeleteStatus = reader["DeleteStatus"].ToString();
                                objectPermission.IsViewPermission = Convert.ToBoolean(reader["IsViewPermission"]);
                                objectPermission.ViewStatus = reader["ViewStatus"].ToString();

                                objectPermissionList.Add(objectPermission);
                            }
                        }
                    }
                }
            }
            return objectPermissionList;
        }
        public ObjectPermissionBO GetFormPermissionByUserIdNForm(int userGroupId, string menuHead)
        {
            ObjectPermissionBO objectPermission = new ObjectPermissionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormPermissionByUserIdNForm_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@MenuHead", DbType.String, menuHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                objectPermission.ObjectPermissionId = Convert.ToInt32(reader["ObjectPermissionId"]);
                                objectPermission.ObjectTabId = Convert.ToInt32(reader["ObjectTabId"]);
                                objectPermission.ObjectGroupHead = reader["ObjectGroupHead"].ToString();
                                objectPermission.ObjectHead = reader["ObjectHead"].ToString();
                                objectPermission.MenuHead = reader["MenuHead"].ToString();
                                objectPermission.ObjectType = reader["ObjectType"].ToString();
                                objectPermission.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                objectPermission.IsSavePermission = Convert.ToBoolean(reader["IsSavePermission"]);
                                objectPermission.SaveStatus = reader["SaveStatus"].ToString();
                                objectPermission.IsUpdatePermission = Convert.ToBoolean(reader["IsUpdatePermission"]);
                                objectPermission.UpdateStatus = reader["UpdateStatus"].ToString();
                                objectPermission.IsDeletePermission = Convert.ToBoolean(reader["IsDeletePermission"]);
                                objectPermission.DeleteStatus = reader["DeleteStatus"].ToString();
                                objectPermission.IsViewPermission = Convert.ToBoolean(reader["IsViewPermission"]);
                                objectPermission.ViewStatus = reader["ViewStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return objectPermission;
        }
        public ObjectPermissionBO GetFormPermissionForBearerByForm(string menuHead)
        {
            ObjectPermissionBO objectPermission = new ObjectPermissionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFormPermissionForBearerByForm_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MenuHead", DbType.String, menuHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                objectPermission.ObjectPermissionId = Convert.ToInt32(reader["ObjectPermissionId"]);
                                objectPermission.ObjectTabId = Convert.ToInt32(reader["ObjectTabId"]);
                                objectPermission.ObjectGroupHead = reader["ObjectGroupHead"].ToString();
                                objectPermission.ObjectHead = reader["ObjectHead"].ToString();
                                objectPermission.MenuHead = reader["MenuHead"].ToString();
                                objectPermission.ObjectType = reader["ObjectType"].ToString();
                                objectPermission.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                objectPermission.IsSavePermission = Convert.ToBoolean(reader["IsSavePermission"]);
                                objectPermission.SaveStatus = reader["SaveStatus"].ToString();
                                objectPermission.IsDeletePermission = Convert.ToBoolean(reader["IsDeletePermission"]);
                                objectPermission.DeleteStatus = reader["DeleteStatus"].ToString();
                                objectPermission.IsViewPermission = Convert.ToBoolean(reader["IsViewPermission"]);
                                objectPermission.ViewStatus = reader["ViewStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return objectPermission;
        }
    }
}
