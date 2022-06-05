using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantItemDA : BaseService
    {
        public List<RestaurantItemBO> GetRestaurantItemInfo()
        {
            List<RestaurantItemBO> entityBOList = new List<RestaurantItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantItemBO entityBO = new RestaurantItemBO();

                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.CategoryName = reader["CategoryName"].ToString();
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public Boolean SaveRestaurantItemInfo(RestaurantItemBO entityBO, out int tmpPKId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantItemInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, entityBO.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, entityBO.TypeId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                    dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.String, entityBO.UnitPrice);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, entityBO.ImageName);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ItemId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpPKId = Convert.ToInt32(command.Parameters["@ItemId"].Value);
                    status = SaveItemCategoryImage(tmpPKId, entityBO.RandomItemId);
                }
            }
            return status;
        }
        public Boolean UpdateRestaurantItemInfo(RestaurantItemBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantItemInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, entityBO.ItemId);
                    dbSmartAspects.AddInParameter(command, "@CategoryId", DbType.Int32, entityBO.CategoryId);
                    dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, entityBO.TypeId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                    dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.String, entityBO.UnitPrice);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, entityBO.ImageName);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, entityBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    //  status = SaveItemCategoryImage(entityBO.CategoryId, entityBO.RandomItemId);
                }
            }
            return status;
        }
        public RestaurantItemBO GetCheckDuplicateCodeByItemCode(string itemCode)
        {
            RestaurantItemBO entityBO = new RestaurantItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCheckDuplicateCodeByItemCode_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemCode", DbType.String, itemCode);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.CategoryName = reader["CategoryName"].ToString();
                                entityBO.TypeId = Convert.ToInt32(reader["TypeId"]);
                                entityBO.TypeName = reader["TypeName"].ToString();
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        
        public List<RestaurantItemBO> GetRestaurantItemInfoBySearchCriteria(string Name, string Code, int CategoryId, int TypeId)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenarateWhereCondition(Name, Code, CategoryId, TypeId);
            List<RestaurantItemBO> entityBOList = new List<RestaurantItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantItemBO entityBO = new RestaurantItemBO();
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.CategoryName = reader["CategoryName"].ToString();
                                entityBO.TypeId = Convert.ToInt32(reader["TypeId"]);
                                entityBO.TypeName = reader["TypeName"].ToString();
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();

                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());

                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
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
        public string GenarateWhereCondition(string Name, string Code, int CategoryId, int TypeId)
        {
            string Where = string.Empty;
            string Condition = string.Empty;
            if (!string.IsNullOrEmpty(Name))
            {
                Condition += " ri.Name =" + "'" + Name + "'" + "";
            }

            if (!string.IsNullOrEmpty(Code))
            {
                if (!string.IsNullOrEmpty(Condition))
                {
                    Condition += " AND ri.Code=" + "'" + Code + "'" + "";
                }
                else
                {
                    Condition += " ri.Code=" + "'" + Code + "'" + "";
                }
            }

            if (!string.IsNullOrEmpty(CategoryId.ToString()))
            {
                if (CategoryId != 0)
                {
                    if (!string.IsNullOrEmpty(Condition))
                    {
                        Condition += " AND ri.CategoryId=" + "'" + CategoryId + "'" + "";
                    }
                    else
                    {
                        Condition += " ri.CategoryId=" + "'" + CategoryId + "'" + "";
                    }
                }
            }

            if (!string.IsNullOrEmpty(TypeId.ToString()))
            {
                if (TypeId != 0)
                {
                    if (!string.IsNullOrEmpty(Condition))
                    {
                        Condition += " AND ri.TypeId=" + "'" + TypeId + "'" + "";
                    }
                    else
                    {
                        Condition += " ri.TypeId=" + "'" + TypeId + "'" + "";
                    }
                }
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
