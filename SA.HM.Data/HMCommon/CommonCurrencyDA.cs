using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class CommonCurrencyDA : BaseService
    {
        public Boolean SaveHeadInfo(CommonCurrencyBO headBO, out int tmpConversionId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCommonCurrencyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CurrencyName", DbType.String, headBO.CurrencyName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActivaStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, headBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CurrencyId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpConversionId = Convert.ToInt32(command.Parameters["@CurrencyId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateHeadInfo(CommonCurrencyBO headBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCommonCurrencyInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, headBO.CurrencyId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyName", DbType.String, headBO.CurrencyName);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActivaStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, headBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public CommonCurrencyBO GetLocalCurrencyInfo()
        {
            CommonCurrencyBO currencyBO = new CommonCurrencyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocalCurrencyInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                currencyBO.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                currencyBO.CurrencyName = reader["CurrencyName"].ToString();
                                currencyBO.CurrencyType = reader["CurrencyType"].ToString();
                                currencyBO.ActivaStat = Convert.ToBoolean(reader["ActiveStat"]);
                            }
                        }
                    }
                }
            }
            return currencyBO;
        }
        public CommonCurrencyBO GetCommonCurrencyInfoById(int currencyId)
        {
            CommonCurrencyBO headBO = new CommonCurrencyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCurrencyInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyId", DbType.Int32, currencyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                headBO.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                headBO.CurrencyName = reader["CurrencyName"].ToString();
                                headBO.CurrencyType = reader["CurrencyType"].ToString();
                                headBO.ActivaStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return headBO;
        }
        public CommonCurrencyConversionBO GetCurrencyConversionRate(int currencyId)
        {
            CommonCurrencyConversionBO conversionBO = new CommonCurrencyConversionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrencyConversionRate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyId", DbType.Int32, currencyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                conversionBO.ConversionId = Convert.ToInt32(reader["ConversionId"]);
                                conversionBO.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                conversionBO.CurrencyName = reader["CurrencyName"].ToString();
                                conversionBO.CurrencyType = reader["CurrencyType"].ToString();
                                conversionBO.FromCurrencyId = Convert.ToInt32(reader["FromCurrencyId"]);
                                conversionBO.ToCurrencyId = Convert.ToInt32(reader["ToCurrencyId"]);
                                conversionBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                conversionBO.BillingConversionRate = Convert.ToDecimal(reader["BillingConversionRate"]);     
                            }
                        }
                    }
                }
            }
            return conversionBO;
        }
        public List<CommonCurrencyBO> GetConversionHeadInfoByType(string currencyType)
        {
            List<CommonCurrencyBO> headBOList = new List<CommonCurrencyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCurrencyInfoByType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyType", DbType.String, currencyType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonCurrencyBO headBO = new CommonCurrencyBO();

                                headBO.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                headBO.CurrencyType = reader["CurrencyType"].ToString();
                                headBO.CurrencyName = reader["CurrencyName"].ToString();
                                headBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                headBOList.Add(headBO);
                            }
                        }
                    }
                }
            }
            return headBOList;
        }
        public List<CommonCurrencyBO> GetHeadInformationBySearchCriteriaForPaging(string headName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CommonCurrencyBO> headList = new List<CommonCurrencyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCurrencyInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyName", DbType.String, headName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonCurrencyBO headBO = new CommonCurrencyBO();
                                headBO.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                headBO.CurrencyName = reader["CurrencyName"].ToString();
                                headBO.ActivaStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                headList.Add(headBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return headList;
        }
    }
}
