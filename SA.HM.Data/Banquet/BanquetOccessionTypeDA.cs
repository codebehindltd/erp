using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetOccessionTypeDA : BaseService
    {
        public BanquetOccessionTypeBO GetBanquetThemeInfoById(long _themeId)
        {
            BanquetOccessionTypeBO catagoryBO = new BanquetOccessionTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetOccessionTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OccessionTypeId", DbType.Int64, _themeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                catagoryBO.Id = Convert.ToInt32(reader["Id"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return catagoryBO;
        }

        public List<BanquetOccessionTypeBO> GetBanquetThemeBySearchCriteria(string Name, string Code)
        {
            List<BanquetOccessionTypeBO> themeList = new List<BanquetOccessionTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetOccessionTypeBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(Name))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, Name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(Code))
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, Code);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetOccessionTypeBO themeBO = new BanquetOccessionTypeBO();
                                themeBO.Id = Convert.ToInt64(reader["Id"]);
                                themeBO.Name = reader["Name"].ToString();
                                themeBO.Code = reader["Code"].ToString();
                                themeBO.Description = reader["Description"].ToString();
                                themeList.Add(themeBO);
                            }
                        }
                    }
                }
            }
            return themeList;
        }

        public List<BanquetOccessionTypeBO> GetAllBanquetTheme()
        {
            List<BanquetOccessionTypeBO> themeList = new List<BanquetOccessionTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetTheme_SP"))
                {


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetOccessionTypeBO themeBO = new BanquetOccessionTypeBO();
                                themeBO.Id = Convert.ToInt64(reader["Id"]);
                                themeBO.Name = reader["Name"].ToString();
                                themeBO.Code = reader["Code"].ToString();
                                themeBO.Description = reader["Description"].ToString();
                                themeList.Add(themeBO);
                            }
                        }
                    }
                }
            }
            return themeList;
        }

        public bool SaveBanquetThemeInfo(BanquetOccessionTypeBO themeBO, out long tmpThemeId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetOccessionTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, themeBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, themeBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, themeBO.Description);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, themeBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OccessionTypeId", DbType.Int64, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpThemeId = Convert.ToInt64(command.Parameters["@OccessionTypeId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool UpdateBanquetThemeInfo(BanquetOccessionTypeBO themeBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetOccessionTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@OccessionTypeId", DbType.Int64, themeBO.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, themeBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, themeBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, themeBO.Description);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, themeBO.LastModifiedBy);
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
