using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLAccountConfigurationDA : BaseService
    {
        public bool SaveAccountConfigurationInfo(GLAccountConfigurationBO configurationBO, out int tmpConfigurationId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAccountConfigurationInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AccountType", DbType.String, configurationBO.AccountType);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.String, configurationBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, configurationBO.ProjectId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, configurationBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ConfigurationId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpConfigurationId = Convert.ToInt32(command.Parameters["@ConfigurationId"].Value);
                }
            }
            return status;
        }
        public bool UpdateAccountConfigurationInfo(GLAccountConfigurationBO configurationBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAccountConfigurationInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ConfigurationId", DbType.Int32, configurationBO.ConfigurationId);
                    dbSmartAspects.AddInParameter(command, "@AccountType", DbType.String, configurationBO.AccountType);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, configurationBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, configurationBO.ProjectId);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, configurationBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<GLAccountConfigurationBO> GetAllAccountConfigurationInfo()
        {
            List<GLAccountConfigurationBO> configurationList = new List<GLAccountConfigurationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllAccountConfigurationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();
                                configurationBO.ConfigurationId = Convert.ToInt32(reader["ConfigurationId"]);
                                configurationBO.AccountType = reader["AccountType"].ToString();
                                configurationBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                configurationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                configurationBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                configurationList.Add(configurationBO);
                            }
                        }
                    }
                }
            }
            return configurationList;
        }
        public GLAccountConfigurationBO GetAccountConfigurationInfoById(int ConfigurationId)
        {
            GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountConfigurationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ConfigurationId", DbType.Int32, ConfigurationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                configurationBO.ConfigurationId = Convert.ToInt32(reader["ConfigurationId"]);
                                configurationBO.AccountType = reader["AccountType"].ToString();
                                configurationBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                configurationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                configurationBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                            }
                        }
                    }
                }
            }
            return configurationBO;
        }
        public GLAccountConfigurationBO GetAccountConfigurationInfoByProjectIdNAccountType(int projectId, string accountType)
        {
            GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountConfigurationInfoByProjectIdNAccountType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@AccountType", DbType.String, accountType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                configurationBO.ConfigurationId = Convert.ToInt32(reader["ConfigurationId"]);
                                configurationBO.AccountType = reader["AccountType"].ToString();
                                configurationBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                configurationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                configurationBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                configurationBO.HeadCount = Convert.ToInt32(reader["HeadCount"]);
                            }
                        }
                    }
                }
            }
            return configurationBO;
        }
        public List<GLAccountConfigurationBO> GetCashAndCashEquivalantTransactionalHead(int projectId, string accountType)
        {
            List<GLAccountConfigurationBO> configurationBOList = new List<GLAccountConfigurationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashAndCashEquivalantTransactionalHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@AccountType", DbType.String, accountType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();
                                configurationBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                configurationBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                configurationBO.NodeNumber = reader["NodeNumber"].ToString();
                                configurationBO.NodeHead = reader["NodeHead"].ToString();
                                configurationBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                configurationBO.Hierarchy = reader["Hierarchy"].ToString();
                                configurationBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                configurationBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                configurationBO.NodeType = Convert.ToString(reader["NodeType"]);

                                configurationBOList.Add(configurationBO);
                            }
                        }
                    }
                }
            }
            return configurationBOList;
        }
        public List<GLAccountConfigurationBO> GetAllAccountConfigurationInfoByProjectIdNAccountType(int projectId, string accountType)
        {
            List<GLAccountConfigurationBO> configurationBOList = new List<GLAccountConfigurationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountConfigurationInfoByProjectIdNAccountType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@AccountType", DbType.String, accountType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();

                                //configurationBO.ConfigurationId = Convert.ToInt32(reader["ConfigurationId"]);
                                //configurationBO.AccountType = reader["AccountType"].ToString();
                                //configurationBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                //configurationBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                configurationBO.NodeId = Convert.ToInt64(reader["NodeId"]);

                                configurationBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                configurationBO.NodeNumber = reader["NodeNumber"].ToString();
                                configurationBO.NodeHead = reader["NodeHead"].ToString();
                                configurationBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                configurationBO.Hierarchy = reader["Hierarchy"].ToString();
                                configurationBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                configurationBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                configurationBO.NodeType = Convert.ToString(reader["NodeType"]);

                                configurationBOList.Add(configurationBO);
                            }
                        }
                    }
                }
            }
            return configurationBOList;
        }
        public List<GLAccountConfigurationBO> GetAccountConfigurationByProjectID(int ProjectId)
        {
            List<GLAccountConfigurationBO> configurationList = new List<GLAccountConfigurationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountConfigurationByProjectID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, ProjectId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLAccountConfigurationBO configurationBO = new GLAccountConfigurationBO();
                                configurationBO.AccountTypeName = reader["AccountTypeName"].ToString();
                                configurationBO.NodeHead = reader["NodeHead"].ToString();
                                configurationBO.ProjectName = reader["ProjectName"].ToString();
                                configurationBO.ConfigurationId = Int32.Parse(reader["ConfigurationId"].ToString());
                                configurationList.Add(configurationBO);
                            }
                        }
                    }
                }
            }
            return configurationList;
        }
        public string GetVoucherNumber(DateTime voucherDate, string voucherType, int projectId, int userId)
        {
            string result = "0";
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.DateTime, voucherDate);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherMode", DbType.String, voucherType);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result = reader["VoucherNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return result;
        }
        public string GetVoucherNumberForCheckDuplicate(string voucherNo, string voucherType, int projectId)
        {
            string result = "0";
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherNumberForCheckDuplicate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@VoucherNo", DbType.String, voucherNo);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherMode", DbType.String, voucherType);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result = reader["CountVoucherNumber"].ToString();
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int GetAccountConfigurationInfoByProjectAndAccountType(int projectId, string accountType, int nodeId)
        {
            int dataCount = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountConfigurationInfoByProjectAndAccountType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@AccountType", DbType.String, accountType);
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, nodeId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                dataCount++;
                            }
                        }
                    }
                }
            }
            return dataCount;
        }
    }
}
