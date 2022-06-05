using HotelManagement.Entity.UserInformation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.UserInformation
{
    public class UserConfigurationDA : BaseService
    {
        public List<UserConfigurationBO> GetConfiguredUserByFeaturesId(long featuresId)
        {
            List<UserConfigurationBO> userConfiguredList = new List<UserConfigurationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetConfiguredUserByFeaturesId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FeaturesId", DbType.Int64, featuresId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                UserConfigurationBO userConfigured = new UserConfigurationBO();

                                userConfigured.Id = Convert.ToInt64(reader["Id"]);
                                userConfigured.UserInfoId = Convert.ToInt64(reader["UserInfoId"]);
                                userConfigured.FeaturesId = Convert.ToInt64(reader["FeaturesId"]);
                                userConfigured.IsCheckedBy = Convert.ToBoolean(reader["IsCheckedBy"]);
                                userConfigured.IsApprovedBy = Convert.ToBoolean(reader["IsApprovedBy"]);

                                userConfiguredList.Add(userConfigured);
                            }
                        }
                    }
                }               
            }
            return userConfiguredList;
        }
        public bool SaveCheckedByApprovedByUsers(List<UserConfigurationBO> userConfiguredListSave, List<UserConfigurationBO> userConfiguredListEdit, int createdBy, out Int64 tmpId)
        {
            int status = 0;
            bool returnVal = false;
            tmpId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (userConfiguredListSave.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveCheckedByApprovedByUsers_SP"))
                            {
                                foreach (UserConfigurationBO item in userConfiguredListSave)
                                {
                                    commandAdd.Parameters.Clear();
                                                
                                    dbSmartAspects.AddInParameter(commandAdd, "@FeaturesId", DbType.Int64, item.FeaturesId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@UserInfoId", DbType.Int64, item.UserInfoId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsCheckedBy", DbType.Boolean, item.IsCheckedBy);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsApprovedBy", DbType.Boolean, item.IsApprovedBy);
                                    dbSmartAspects.AddInParameter(commandAdd, "@CreatedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddOutParameter(commandAdd, "@Id", DbType.Int64, sizeof(Int64));

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);

                                    tmpId = Convert.ToInt64(commandAdd.Parameters["@Id"].Value);

                                }
                            }
                        }
                        else
                        {
                            status = 1;
                        }

                        if (status>0 && userConfiguredListEdit.Count>0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateCheckedByApprovedByUsers_SP"))
                            {
                                foreach (UserConfigurationBO item in userConfiguredListEdit)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@FeaturesId", DbType.Int64, item.FeaturesId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@UserInfoId", DbType.Int64, item.UserInfoId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsCheckedBy", DbType.Boolean, item.IsCheckedBy);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsApprovedBy", DbType.Boolean, item.IsApprovedBy);
                                    dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddInParameter(commandAdd, "@Id", DbType.Int64, item.Id);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            returnVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            returnVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return returnVal;
        }


    }
}
