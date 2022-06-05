using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantItemCategoryDA : BaseService
    {
        public List<RestaurantItemCategoryBO> GetRestaurantItemCategoryInfo()
        {
            List<RestaurantItemCategoryBO> entityBOList = new List<RestaurantItemCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemCategoryInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantItemCategoryBO entityBO = new RestaurantItemCategoryBO();

                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();
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
        
        public Boolean SaveRestaurantItemCategoryInfo(RestaurantItemCategoryBO entityBO, out int tmpPKId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantItemCategoryInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                    dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, entityBO.ImageName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CategoryId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpPKId = Convert.ToInt32(command.Parameters["@CategoryId"].Value);
                }
            }
            status = SaveItemCategoryImage(tmpPKId, entityBO.tempCategoryId);
            return status;
        }

        public Boolean SaveItemCategoryImage(int categoryId, int randomCategoryId)
        {
            Boolean status = false;
            status = DeletePreviousCategoryImage(categoryId);
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, categoryId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, randomCategoryId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean DeletePreviousCategoryImage(int categoryId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTemporaryImage_Sp"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.String, categoryId);
                    dbSmartAspects.AddInParameter(command, "@Type", DbType.String, "ItemCategory");
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                }
            }
            return status;
        }
        public Boolean UpdateRestaurantItemCategoryInfo(RestaurantItemCategoryBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantItemCategoryInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, entityBO.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                    dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, entityBO.ImageName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, entityBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
         //   status = SaveItemCategoryImage(entityBO.CategoryId, entityBO.tempCategoryId);
            return status;
        }
        public RestaurantItemCategoryBO GetRestaurantItemCategoryInfoById(int pkId)
        {
            RestaurantItemCategoryBO entityBO = new RestaurantItemCategoryBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemCategoryInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, pkId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<RestaurantItemCategoryBO> GetRestaurantItemCategoryInfoBySearchCriteria(string Name, string Code, bool ActiveStat)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenarateWhereCondition(Name, Code, ActiveStat);
            List<RestaurantItemCategoryBO> entityBOList = new List<RestaurantItemCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemCategoryInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantItemCategoryBO entityBO = new RestaurantItemCategoryBO();

                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();
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



        public string GenarateWhereCondition(string Name, string Code, bool ActiveStat)
        {
            string Where = string.Empty;
            string Condition = string.Empty;
            int isActive = -1;
            if (ActiveStat == true)
            {
                isActive = 1;
            }
            else
            {
                isActive = 0;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                Condition += " Name =" +"'"+ Name +"'"+ "";
            }

            if (!string.IsNullOrEmpty(Code))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND Code=" + "'" + Code + "'" + "";
                }
                else
                {
                    Condition += " Code=" + "'" + Code + "'" + "";
                }
            }

            if (!string.IsNullOrEmpty(Condition))
            {

                Condition += " AND ActiveStat =" + "'" + isActive + "'" + "";
            }
            else
            {
                Condition += " ActiveStat =" + "'" + isActive +"'" + "";
            }



            if (!string.IsNullOrWhiteSpace(Condition))
            {
                Where = " WHERE " + Condition;
            }
            else
            {
                Where = Condition;
            }

            return Where;
        }

    }
}
