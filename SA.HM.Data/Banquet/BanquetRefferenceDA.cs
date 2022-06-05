using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetRefferenceDA : BaseService
    {
        public BanquetRefferenceBO GetBanquetRefferenceInfoById(long _refferenceId)
        {

            BanquetRefferenceBO refferenceBO = new BanquetRefferenceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetRefferenceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RefferenceId", DbType.Int64, _refferenceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                refferenceBO.Id = Convert.ToInt64(reader["Id"]);
                                refferenceBO.Name = reader["Name"].ToString();
                                refferenceBO.SalesCommission = Convert.ToDecimal(reader["SalesCommission"].ToString());
                                refferenceBO.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return refferenceBO;
        }

        public List<BanquetRefferenceBO> GetBanquetRefferenceBySearchCriteria(string Name)
        {
            List<BanquetRefferenceBO> refferenceList = new List<BanquetRefferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetRefferenceBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(Name))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, Name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetRefferenceBO refferenceBO = new BanquetRefferenceBO();
                                refferenceBO.Id = Convert.ToInt64(reader["Id"]);
                                refferenceBO.Name = reader["Name"].ToString();
                                refferenceBO.SalesCommission = Convert.ToDecimal(reader["SalesCommission"].ToString());
                                refferenceBO.Description = reader["Description"].ToString();
                                refferenceList.Add(refferenceBO);
                            }
                        }
                    }
                }
            }
            return refferenceList;
        }

        public List<BanquetRefferenceBO> GetAllBanquetRefference()
        {
            List<BanquetRefferenceBO> refferenceList = new List<BanquetRefferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetRefference_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetRefferenceBO reffenceBO = new BanquetRefferenceBO();
                                reffenceBO.Id = Convert.ToInt64(reader["Id"]);
                                reffenceBO.Name = reader["Name"].ToString();
                                reffenceBO.SalesCommission = Convert.ToDecimal(reader["SalesCommission"].ToString());
                                reffenceBO.Description = reader["Description"].ToString();
                                refferenceList.Add(reffenceBO);
                            }
                        }
                    }
                }
            }
            return refferenceList;
        }

        public bool SaveBanquetRefferenceInfo(BanquetRefferenceBO refferenceBO, out long tmpRefferenceId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetRefferenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, refferenceBO.Name);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, refferenceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@SalesCommission", DbType.Decimal, refferenceBO.SalesCommission);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, refferenceBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@RefferenceId", DbType.Int64, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpRefferenceId = Convert.ToInt64(command.Parameters["@RefferenceId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool UpdateBanquetRefferenceInfo(BanquetRefferenceBO refferenceBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetRefferenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@RefferenceId", DbType.Int64, refferenceBO.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, refferenceBO.Name);
                        dbSmartAspects.AddInParameter(command, "@SalesCommission", DbType.Decimal, refferenceBO.SalesCommission);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, refferenceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, refferenceBO.LastModifiedBy);
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
