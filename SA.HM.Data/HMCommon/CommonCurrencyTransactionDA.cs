using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class CommonCurrencyTransactionDA: BaseService
    {
        public Boolean SaveCommonCurrencyTransaction(CommonCurrencyTransactionBO cmncurrencyconvBO, out int temCurrencyConversionId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCommonCurrencyTransaction_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromConversionHeadId", DbType.Int32, cmncurrencyconvBO.FromConversionHeadId);
                    dbSmartAspects.AddInParameter(command, "@ToConversionHeadId", DbType.String, cmncurrencyconvBO.ToConversionHeadId);
                    dbSmartAspects.AddInParameter(command, "@ConversionAmount", DbType.Decimal, cmncurrencyconvBO.ConversionAmount);
                    dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, cmncurrencyconvBO.ConversionRate);
                    dbSmartAspects.AddInParameter(command, "@ConvertedAmount", DbType.Decimal, cmncurrencyconvBO.ConvertedAmount);

                    dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, cmncurrencyconvBO.TransactionType);
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, cmncurrencyconvBO.RegistrationId);
                    dbSmartAspects.AddInParameter(command, "@GuestName", DbType.String, cmncurrencyconvBO.GuestName);
                    dbSmartAspects.AddInParameter(command, "@CountryName", DbType.String, cmncurrencyconvBO.CountryName);
                    dbSmartAspects.AddInParameter(command, "@PassportNumber", DbType.String, cmncurrencyconvBO.PassportNumber);
                    dbSmartAspects.AddInParameter(command, "@TransactionDetails", DbType.String, cmncurrencyconvBO.TransactionDetails);


                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, cmncurrencyconvBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@CurrencyConversionId", DbType.Int32, sizeof(Int32));
                    
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    temCurrencyConversionId = Convert.ToInt32(command.Parameters["@CurrencyConversionId"].Value);
                }
            }
            return status;
        }

        public Boolean UpdateCommonCurrencyConversion(CommonCurrencyTransactionBO cmncurrencyconvBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCommonCurrencyConversion_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CurrencyConversionId", DbType.Int32, cmncurrencyconvBO.CurrencyConversionId);
                    dbSmartAspects.AddInParameter(command, "@FromConversionHeadId", DbType.Int32, cmncurrencyconvBO.FromConversionHeadId);
                    dbSmartAspects.AddInParameter(command, "@ToConversionHeadId", DbType.String, cmncurrencyconvBO.ToConversionHeadId);
                    dbSmartAspects.AddInParameter(command, "@ConversionAmount", DbType.Decimal, cmncurrencyconvBO.ConversionAmount);
                    dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, cmncurrencyconvBO.ConversionRate);
                    dbSmartAspects.AddInParameter(command, "@ConvertedAmount", DbType.Decimal, cmncurrencyconvBO.ConvertedAmount);
                    dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, cmncurrencyconvBO.TransactionType);
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, cmncurrencyconvBO.RegistrationId);
                    dbSmartAspects.AddInParameter(command, "@GuestName", DbType.String, cmncurrencyconvBO.GuestName);
                    dbSmartAspects.AddInParameter(command, "@CountryName", DbType.String, cmncurrencyconvBO.CountryName);
                    dbSmartAspects.AddInParameter(command, "@PassportNumber", DbType.String, cmncurrencyconvBO.PassportNumber);
                    dbSmartAspects.AddInParameter(command, "@TransactionDetails", DbType.String, cmncurrencyconvBO.TransactionDetails);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, cmncurrencyconvBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<CommonCurrencyTransactionBO> GetCommonCurrencyConversion()
        {
            List<CommonCurrencyTransactionBO> cmncurrencyconvBOList = new List<CommonCurrencyTransactionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllCommonCurrencyConversion_SP"))
                {                    
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonCurrencyTransactionBO cmncurrencyconvBO = new CommonCurrencyTransactionBO();

                                cmncurrencyconvBO.CurrencyConversionId = Convert.ToInt32(reader["CurrencyConversionId"]);
                                cmncurrencyconvBO.TransactionNumber = reader["TransactionNumber"].ToString();
                                cmncurrencyconvBO.FromConversionHeadId = Convert.ToInt32(reader["FromConversionHeadId"]);
                                cmncurrencyconvBO.ToConversionHeadId = Convert.ToInt32(reader["ToConversionHeadId"]);
                                cmncurrencyconvBO.ConversionAmount = Convert.ToDecimal(reader["ConversionAmount"]);
                                cmncurrencyconvBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                cmncurrencyconvBO.ConvertedAmount = Convert.ToDecimal(reader["ConvertedAmount"]);                          

                                cmncurrencyconvBOList.Add(cmncurrencyconvBO);
                            }
                        }
                    }
                }
            }
            return cmncurrencyconvBOList;
        }
        public List<CommonCurrencyTransactionBO> GetAllCommonCurrencyConversion(int fromId, int toId, DateTime? fromDate, DateTime? toDate)
        {
            
            List<CommonCurrencyTransactionBO> cmncurrencyconvBOList = new List<CommonCurrencyTransactionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrencyConversion_SP"))
                {
                    if (fromId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromConversionHeadId", DbType.Int32, fromId);
                    }
                    else {
                        dbSmartAspects.AddInParameter(cmd, "@FromConversionHeadId", DbType.Int32, DBNull.Value);
                    }
                    if (toId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToConversionHeadId", DbType.Int32, toId);
                    }
                    else {
                        dbSmartAspects.AddInParameter(cmd, "@ToConversionHeadId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonCurrencyTransactionBO cmncurrencyconvBO = new CommonCurrencyTransactionBO();

                                cmncurrencyconvBO.CurrencyConversionId = Convert.ToInt32(reader["CurrencyConversionId"]);
                                cmncurrencyconvBO.TransactionNumber = reader["TransactionNumber"].ToString();
                                cmncurrencyconvBO.FromConversionHeadId = Convert.ToInt32(reader["FromConversionHeadId"]);
                                cmncurrencyconvBO.ToConversionHeadId = Convert.ToInt32(reader["ToConversionHeadId"]);
                                cmncurrencyconvBO.ConversionAmount = Convert.ToDecimal(reader["ConversionAmount"]);
                                cmncurrencyconvBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                cmncurrencyconvBO.ConvertedAmount = Convert.ToDecimal(reader["ConvertedAmount"]);

                                cmncurrencyconvBOList.Add(cmncurrencyconvBO);
                            }
                        }
                    }
                }
            }
            return cmncurrencyconvBOList;
        }
        public CommonCurrencyTransactionBO GetCommonCurrencyConversionById(int editId)
        {
            CommonCurrencyTransactionBO cmnCurrencyConv = new CommonCurrencyTransactionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCurrencyConversionById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyConversionId", DbType.Int32, editId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                cmnCurrencyConv.CurrencyConversionId = Convert.ToInt32(reader["CurrencyConversionId"]);
                                cmnCurrencyConv.FromConversionHeadId = Convert.ToInt32(reader["FromConversionHeadId"]);
                                cmnCurrencyConv.ToConversionHeadId = Convert.ToInt32(reader["ToConversionHeadId"]);
                                cmnCurrencyConv.ConversionAmount = Convert.ToDecimal(reader["ConversionAmount"]);
                                cmnCurrencyConv.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                cmnCurrencyConv.ConvertedAmount = Convert.ToDecimal(reader["ConvertedAmount"]);

                                cmnCurrencyConv.TransactionType = reader["TransactionType"].ToString();
                                cmnCurrencyConv.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                cmnCurrencyConv.RoomNumber = reader["RoomNumber"].ToString();
                                cmnCurrencyConv.GuestName = reader["GuestName"].ToString();
                                cmnCurrencyConv.CountryName = reader["CountryName"].ToString();
                                cmnCurrencyConv.PassportNumber = reader["PassportNumber"].ToString();
                                cmnCurrencyConv.TransactionDetails = reader["TransactionDetails"].ToString();
                            }
                        }
                    }
                }
            }
            return cmnCurrencyConv;
        }        
        public List<CommonCurrencyTransactionBO> GetCommonCurrencyConversionInfo(string searchCriteria)
        {
            List<CommonCurrencyTransactionBO> currencyconvList = new List<CommonCurrencyTransactionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCurrencyConversion_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonCurrencyTransactionBO entityBO = new CommonCurrencyTransactionBO();

                                entityBO.CurrencyConversionId = Convert.ToInt32(reader["CurrencyConversionId"]);
                                entityBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                entityBO.FromConversionHeadId = Convert.ToInt32(reader["FromConversionHeadId"]);
                                entityBO.FromConversionHead = reader["FromConversionHead"].ToString();                                
                                entityBO.ToConversionHeadId = Convert.ToInt32(reader["ToConversionHeadId"]);
                                entityBO.ToConversionHead = reader["ToConversionHead"].ToString();                                
                                entityBO.ConversionAmount = Convert.ToDecimal(reader["ConversionAmount"]);
                                entityBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"]);
                                entityBO.ConvertedAmount = Convert.ToDecimal(reader["ConvertedAmount"]);
                                entityBO.TransactionNumber = reader["TransactionNumber"].ToString();                                

                                currencyconvList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return currencyconvList;
        }
       
            
    }
}
