using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.CostofGoodsSold;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.CostofGoodsSold
{
    public class CgsTransactionDA: BaseService
    {
        public Boolean SaveTransactionHeadInfo(CgsTransactionHeadBO tranHeadBO, out int tmpTranHeadId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTransactionHeadInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, tranHeadBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Type", DbType.String, tranHeadBO.Type);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, tranHeadBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, tranHeadBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TransactionHeadId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpTranHeadId = Convert.ToInt32(command.Parameters["@TransactionHeadId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateTransactionHeadInfo(CgsTransactionHeadBO tranHeadBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTransactionHeadInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TransactionHeadId", DbType.Int32, tranHeadBO.TransactionHeadId);
                    dbSmartAspects.AddInParameter(command, "@Name", DbType.String, tranHeadBO.Name);
                    dbSmartAspects.AddInParameter(command, "@Type", DbType.String, tranHeadBO.Type);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, tranHeadBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, tranHeadBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public CgsTransactionHeadBO GetTransHeadInfoById(int transId)
        {
            CgsTransactionHeadBO transBO = new CgsTransactionHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTransHeadInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionHeadId", DbType.Int32, transId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                transBO.TransactionHeadId = Convert.ToInt32(reader["TransactionHeadId"]);
                                transBO.Name = reader["Name"].ToString();
                                transBO.Type = reader["Type"].ToString();
                                transBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                transBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return transBO;
        }
        public List<CgsTransactionHeadBO> GetTransHeadInfoBySearchCriteriaForPaging(string name, string type, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CgsTransactionHeadBO> transBOList = new List<CgsTransactionHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTransHeadInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
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
                                CgsTransactionHeadBO transBO = new CgsTransactionHeadBO();
                                transBO.TransactionHeadId = Convert.ToInt32(reader["TransactionHeadId"]);
                                transBO.Name = reader["Name"].ToString();
                                transBO.Type = reader["Type"].ToString();
                                transBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                transBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                transBOList.Add(transBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return transBOList;
        }

        public bool SaveMonthlyTransactionInfo(CgsMonthlyTransactionBO monthlyTranBO, out int tmpMonthlyTranId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMonthlyTransactionInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, monthlyTranBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@InputDate", DbType.DateTime, monthlyTranBO.InputDate);
                    dbSmartAspects.AddInParameter(command, "@EmpType ", DbType.String, monthlyTranBO.EmpType);
                    dbSmartAspects.AddInParameter(command, "@Amount ", DbType.Decimal, monthlyTranBO.Amount);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, monthlyTranBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@MonthlyTranId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpMonthlyTranId = Convert.ToInt32(command.Parameters["@MonthlyTranId"].Value);
                }
            }
            return status;
        }
        public bool UpdateMonthlyTransactionInfo(CgsMonthlyTransactionBO monthlyTranBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMonthlyTransactionnfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MonthlyTranId", DbType.Int32, monthlyTranBO.MonthlyTransactionId);
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, monthlyTranBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@InputDate", DbType.DateTime, monthlyTranBO.InputDate);
                    dbSmartAspects.AddInParameter(command, "@EmpType", DbType.Decimal, monthlyTranBO.EmpType);
                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, monthlyTranBO.Amount);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, monthlyTranBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public CgsMonthlyTransactionBO GetMonthlyTransInfoById(int transId)
        {
            CgsMonthlyTransactionBO transBO = new CgsMonthlyTransactionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthlyTransInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MonthlyTransactionId", DbType.Int32, transId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                transBO.MonthlyTransactionId = Convert.ToInt32(reader["MonthlyTransactionId"]);
                                transBO.EmpType = reader["EmpType"].ToString();
                                transBO.InputDate = Convert.ToDateTime(reader["InputDate"]);
                                transBO.Amount = Convert.ToDecimal(reader["Amount"]);
                            }
                        }
                    }
                }
            }
            return transBO;
        }
        public List<CgsMonthlyTransactionBO> GetMonthlyTransactionInfoBySearchCriteriaForPaging(int empId, DateTime? inputDate, string empType, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CgsMonthlyTransactionBO> transactionList = new List<CgsMonthlyTransactionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMonthlyTransactionInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@InputDate", DbType.DateTime, inputDate);
                    dbSmartAspects.AddInParameter(cmd, "@EmpType", DbType.String, empType);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CgsMonthlyTransactionBO transactionBO = new CgsMonthlyTransactionBO();
                                transactionBO.MonthlyTransactionId = Convert.ToInt32(reader["MonthlyTransactionId"]);
                                transactionBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                transactionBO.EmpType = reader["EmpType"].ToString();
                                transactionBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                transactionBO.InputDate = Convert.ToDateTime(reader["InputDate"]);
                                transactionList.Add(transactionBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return transactionList;
        }        
    }
}
