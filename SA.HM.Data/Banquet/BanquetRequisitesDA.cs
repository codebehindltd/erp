using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetRequisitesDA : BaseService
    {
        public List<BanquetRequisitesBO> GetBanquetRequisitesInfoBySearchCriteria(string Name, string Code, bool ActiveStat)
        {
            List<BanquetRequisitesBO> entityBOList = new List<BanquetRequisitesBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetRequisitesBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(Name))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, Name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(Code))
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, Code);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetRequisitesBO entityBO = new BanquetRequisitesBO();
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);
                                entityBO.ExpenseAccountsPostingHeadId = Convert.ToInt64(reader["ExpenseAccountsPostingHeadId"]);
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public BanquetRequisitesBO GetBanquetRequisitesInfoById(long EditId)
        {
            BanquetRequisitesBO entityBO = new BanquetRequisitesBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetRequisitesInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitesId", DbType.Int64, EditId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);
                                entityBO.ExpenseAccountsPostingHeadId = Convert.ToInt64(reader["ExpenseAccountsPostingHeadId"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public bool SaveBanquetRequisitesInfo(BanquetRequisitesBO entityBO, out long tmpRequisitesId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetRequisitesInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                        dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.Decimal, entityBO.UnitPrice);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, entityBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command,"@RequisitesId", DbType.Int64, sizeof(Int32));
                        if(entityBO.AccountsPostingHeadId == 0)
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, DBNull.Value);
                        else
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, entityBO.AccountsPostingHeadId);
                        if (entityBO.ExpenseAccountsPostingHeadId == 0)
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int32, DBNull.Value);
                        else
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int64, entityBO.ExpenseAccountsPostingHeadId);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpRequisitesId = Convert.ToInt64(command.Parameters["@RequisitesId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateBanquetRequisitesInfo(BanquetRequisitesBO entityBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetRequisitesInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RequisitesId", DbType.Int64, entityBO.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, entityBO.Code);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                        dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.Decimal, entityBO.UnitPrice);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, entityBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, entityBO.LastModifiedBy);
                        if (entityBO.AccountsPostingHeadId == 0)
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int32, DBNull.Value);
                        else
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, entityBO.AccountsPostingHeadId);
                        if (entityBO.ExpenseAccountsPostingHeadId == 0)
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int32, DBNull.Value);
                        else
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int64, entityBO.ExpenseAccountsPostingHeadId);
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
        public List<BanquetRequisitesBO> GetRequisitesInfo()
        {
            List<BanquetRequisitesBO> entityBOList = new List<BanquetRequisitesBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRequisitesInfo_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetRequisitesBO entityBO = new BanquetRequisitesBO();
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);
                                entityBO.ExpenseAccountsPostingHeadId = Convert.ToInt64(reader["ExpenseAccountsPostingHeadId"]);
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
