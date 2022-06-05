using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class BusinessPromotionDA : BaseService
    {
        public List<BusinessPromotionBO> GetBusinessPromotionInfo()
        {
            List<BusinessPromotionBO> bpList = new List<BusinessPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBusinessPromotionInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BusinessPromotionBO bp = new BusinessPromotionBO();

                                bp.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                bp.BPHead = reader["BPHead"].ToString();
                                bp.PeriodFrom = Convert.ToDateTime(reader["PeriodFrom"]);
                                bp.PeriodTo = Convert.ToDateTime(reader["PeriodTo"]);
                                bp.IsBPPublic = Convert.ToBoolean(reader["IsBPPublic"]);
                                bp.PercentAmount = Convert.ToDecimal(reader["PercentAmount"]);
                                bp.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bp.ActiveStatus = reader["ActiveStatus"].ToString();

                                bpList.Add(bp);
                            }
                        }
                    }
                }
            }
            return bpList;
        }
        public List<BusinessPromotionBO> GetCurrentActiveBusinessPromotionInfo()
        {
            List<BusinessPromotionBO> bpList = new List<BusinessPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrentActiveBusinessPromotionInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BusinessPromotionBO bp = new BusinessPromotionBO();

                                bp.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                bp.BPHead = reader["BPHead"].ToString();
                                bp.PeriodFrom = Convert.ToDateTime(reader["PeriodFrom"]);
                                bp.PeriodTo = Convert.ToDateTime(reader["PeriodTo"]);
                                bp.IsBPPublic = Convert.ToBoolean(reader["IsBPPublic"]);
                                bp.PercentAmount = Convert.ToDecimal(reader["PercentAmount"]);
                                bp.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bp.ActiveStatus = reader["ActiveStatus"].ToString();
                                bp.BusinessPromotionIdNPercentAmount = reader["BusinessPromotionId"].ToString() + "~" + reader["PercentAmount"].ToString();

                                bpList.Add(bp);
                            }
                        }
                    }
                }
            }
            return bpList;
        }
        public Boolean SaveBusinessPromotionInfo(BusinessPromotionBO bp, List<BusinessPromotionDetailBO> businessPromotionDetailBOList, out int tmpBusinessPromotionId)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveBusinessPromotionInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@BPHead", DbType.String, bp.BPHead);
                        dbSmartAspects.AddInParameter(commandMaster, "@PeriodFrom", DbType.DateTime, bp.PeriodFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@PeriodTo", DbType.DateTime, bp.PeriodTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@TransactionType", DbType.String, bp.TransactionType);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsBPPublic", DbType.Boolean, bp.IsBPPublic);
                        dbSmartAspects.AddInParameter(commandMaster, "@PercentAmount", DbType.Decimal, bp.PercentAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, bp.ActiveStat);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, bp.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        tmpBusinessPromotionId = Convert.ToInt32(commandMaster.Parameters["@BusinessPromotionId"].Value);
                    }

                    if (status > 0)
                    {
                        using (DbCommand commandComplementary = dbSmartAspects.GetStoredProcCommand("SaveBusinessPromotionDetailInfo_SP"))
                        {
                            foreach (BusinessPromotionDetailBO businessPromotionDetailBO in businessPromotionDetailBOList)
                            {
                                commandComplementary.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandComplementary, "@BusinessPromotionId", DbType.Int32, tmpBusinessPromotionId);
                                dbSmartAspects.AddInParameter(commandComplementary, "@TransactionType", DbType.String, businessPromotionDetailBO.TransactionType);
                                dbSmartAspects.AddInParameter(commandComplementary, "@TransactionId", DbType.Int32, businessPromotionDetailBO.TransactionId);

                                status = dbSmartAspects.ExecuteNonQuery(commandComplementary, transction);
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
            }
            return retVal;
        }
        public Boolean UpdateBusinessPromotionInfo(BusinessPromotionBO bp, List<BusinessPromotionDetailBO> businessPromotionDetailBOList)
        {
            bool retVal = false;
            int status = 0;
            int tmpBusinessPromotionId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateBusinessPromotionInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@BusinessPromotionId", DbType.Int32, bp.BusinessPromotionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@BPHead", DbType.String, bp.BPHead);
                        dbSmartAspects.AddInParameter(commandMaster, "@PeriodFrom", DbType.DateTime, bp.PeriodFrom);
                        dbSmartAspects.AddInParameter(commandMaster, "@PeriodTo", DbType.DateTime, bp.PeriodTo);
                        dbSmartAspects.AddInParameter(commandMaster, "@TransactionType", DbType.String, bp.TransactionType);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsBPPublic", DbType.Boolean, bp.IsBPPublic);
                        dbSmartAspects.AddInParameter(commandMaster, "@PercentAmount", DbType.Decimal, bp.PercentAmount);
                        dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, bp.ActiveStat);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, bp.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpBusinessPromotionId = bp.BusinessPromotionId;
                    }

                    if (status > 0)
                    {
                        if (businessPromotionDetailBOList.Count() > 0 && status > 0)
                        {
                            using (DbCommand commandComplementary = dbSmartAspects.GetStoredProcCommand("SaveBusinessPromotionDetailInfo_SP"))
                            {
                                foreach (BusinessPromotionDetailBO businessPromotionDetailBO in businessPromotionDetailBOList)
                                {
                                    commandComplementary.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandComplementary, "@BusinessPromotionId", DbType.Int32, tmpBusinessPromotionId);
                                    dbSmartAspects.AddInParameter(commandComplementary, "@TransactionType", DbType.String, businessPromotionDetailBO.TransactionType);
                                    dbSmartAspects.AddInParameter(commandComplementary, "@TransactionId", DbType.Int32, businessPromotionDetailBO.TransactionId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandComplementary, transction);
                                }
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

            }
            return retVal;
        }
        public BusinessPromotionBO GetBusinessPromotionInfoById(int businessPromotionId)
        {
            BusinessPromotionBO bp = new BusinessPromotionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBusinessPromotionInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BusinessPromotionId", DbType.Int32, businessPromotionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bp.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                bp.BPHead = reader["BPHead"].ToString();
                                bp.PeriodFrom = Convert.ToDateTime(reader["PeriodFrom"]);
                                bp.PeriodTo = Convert.ToDateTime(reader["PeriodTo"]);
                                bp.TransactionType = reader["TransactionType"].ToString();
                                bp.IsBPPublic = Convert.ToBoolean(reader["IsBPPublic"]);
                                bp.PercentAmount = Convert.ToDecimal(reader["PercentAmount"]);
                                bp.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bp.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bp;
        }
        public List<BusinessPromotionBO> GetBusinessPromotionInfoBySearchCriteria(string BPHead, bool ActiveStat)
        {
            string Where = GenarateWhereConditionstring(BPHead, ActiveStat);
            List<BusinessPromotionBO> bpList = new List<BusinessPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBusinessPromotionInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BusinessPromotionBO bp = new BusinessPromotionBO();
                                bp.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                bp.BPHead = reader["BPHead"].ToString();
                                bp.PeriodFrom = Convert.ToDateTime(reader["PeriodFrom"]);
                                bp.PeriodTo = Convert.ToDateTime(reader["PeriodTo"]);
                                bp.IsBPPublic = Convert.ToBoolean(reader["IsBPPublic"]);
                                bp.PercentAmount = Convert.ToDecimal(reader["PercentAmount"]);
                                bp.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bp.ActiveStatus = reader["ActiveStatus"].ToString();

                                bpList.Add(bp);
                            }
                        }
                    }
                }
            }
            return bpList;
        }
        public string GenarateWhereConditionstring(String BPHead, bool ActiveStat)
        {

            string Where = string.Empty;
            if (!string.IsNullOrEmpty(BPHead.ToString()))
            {
                Where += "  BPHead = '" + BPHead + "'";
            }

            if (!string.IsNullOrEmpty(ActiveStat.ToString()))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND ActiveStat = '" + ActiveStat + "'";
                }
                else
                {
                    Where += "  ActiveStat = '" + ActiveStat + "'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }
        public BusinessPromotionBO LoadDiscountRelatedInformation(string discountType, int transactionId)
        {
            BusinessPromotionBO bp = new BusinessPromotionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLoadDiscountRelatedInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DiscountType", DbType.String, discountType);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, transactionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bp.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                bp.BPHead = reader["BPHead"].ToString();
                                bp.PeriodFrom = Convert.ToDateTime(reader["PeriodFrom"]);
                                bp.PeriodTo = Convert.ToDateTime(reader["PeriodTo"]);
                                bp.TransactionType = reader["TransactionType"].ToString();
                                bp.IsBPPublic = Convert.ToBoolean(reader["IsBPPublic"]);
                                bp.PercentAmount = Convert.ToDecimal(reader["PercentAmount"]);
                                bp.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bp.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bp;
        }

        public List<BusinessPromotionBO> LoadBusinessPromotionRelatedInformation(int transactionId)
        {
            List<BusinessPromotionBO> bpList = new List<BusinessPromotionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLoadBusinessPromotionRelatedInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BusinessPromotionId", DbType.Int32, transactionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BusinessPromotionBO bp = new BusinessPromotionBO();
                                bp.BusinessPromotionId = Convert.ToInt32(reader["BusinessPromotionId"]);
                                bp.TransactionType = reader["TransactionType"].ToString();
                                bp.TransactionId = Convert.ToInt32(reader["TransactionId"]);

                                bpList.Add(bp);
                            }
                        }
                    }
                }
            }
            return bpList;
        }
        
    }
}
