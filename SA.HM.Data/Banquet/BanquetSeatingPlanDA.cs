using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetSeatingPlanDA : BaseService
    {
        public List<BanquetSeatingPlanBO> GetBanquetSeatingPlanBySearchCriteria(string Name, string Code, bool ActiveStat)
        {
            string searchCriteria = string.Empty;
            searchCriteria = GenerateWhereCondition(Name, Code, ActiveStat);
            List<BanquetSeatingPlanBO> entityBOList = new List<BanquetSeatingPlanBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetSeatingPlanBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetSeatingPlanBO entityBO = new BanquetSeatingPlanBO();
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.Description = reader["Description"].ToString();
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
        private string GenerateWhereCondition(string Name, string Code, bool ActiveStat)
        {
            string Where = string.Empty, Condition = string.Empty;

            if (!string.IsNullOrEmpty(Name))
            {
                Condition = " NAME = '" + Name + "'";
            }

            if (!string.IsNullOrEmpty(Code))
            {
                if (string.IsNullOrEmpty(Condition))
                {
                    Condition = " Code = '" + Code + "'";
                }
                else
                {
                    Condition += " AND Code = '" + Code + "'";
                }
            }

          if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " AND ActiveStat = " +"'"+ (!ActiveStat ? 0 : 1)+"'";
            }
            else
            {
                Condition = " ActiveStat = " +"'"+ (!ActiveStat ? 0 : 1)+"'";
            }

            if (!string.IsNullOrEmpty(Condition))
            {
                Where += " WHERE " + Condition;
            }

            return Where;
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

        public bool SaveBanquetSeatingPlanInfo(Entity.Banquet.BanquetSeatingPlanBO entityBO, out long tmpPKId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetSeatingPlanInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, entityBO.ImageName);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, entityBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@SeatingId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpPKId = Convert.ToInt64(command.Parameters["@SeatingId"].Value);
                        if (status)
                        {
                            SaveItemCategoryImage(Convert.ToInt32(tmpPKId), entityBO.RandomSeatingId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateBanquetSeatingPlanInfo(BanquetSeatingPlanBO entityBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetSeatingPlanInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SeatingId", DbType.Int64, entityBO.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ImageName", DbType.String, entityBO.ImageName);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, entityBO.LastModifiedBy);

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
        public BanquetSeatingPlanBO GetBanquetSeatingPlanInfoById(long EditId)
        {
            BanquetSeatingPlanBO entityBO = new BanquetSeatingPlanBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetSeatingPlanInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SeatingId", DbType.Int64, EditId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.Description = reader["Description"].ToString();
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

        public List<BanquetSeatingPlanBO> GetBanquetPlanInformation()
        {
            List<BanquetSeatingPlanBO> entityBOList = new List<BanquetSeatingPlanBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetPlanInformation_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetSeatingPlanBO entityBO = new BanquetSeatingPlanBO();
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
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
    }
}
