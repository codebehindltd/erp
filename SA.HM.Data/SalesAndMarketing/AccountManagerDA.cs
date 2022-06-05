using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesAndMarketing;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class AccountManagerDA : BaseService
    {
        public Boolean SaveUpdateAccountManager(List<AccountManagerBO> addedManager, List<AccountManagerBO> deletedManager, int createdBy)
        {
            Boolean status = true;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAccountManager_SP"))
                    {
                        foreach (AccountManagerBO am in addedManager)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, am.DepartmentId);
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, am.EmpId);
                            dbSmartAspects.AddInParameter(command, "@SortName", DbType.String, am.SortName);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                            status = true;
                        }
                    }

                    using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                    {
                        foreach (AccountManagerBO am in deletedManager)
                        {
                            cmdOutDetails.Parameters.Clear();

                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TableName", DbType.String, "SMAccountManager");
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKField", DbType.String, "AccountManagerId");
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKId", DbType.String, am.AccountManagerId.ToString());

                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transaction) > 0 ? true : false;
                            status = true;
                        }
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }
        public List<AccountManagerBO> GetAccountManager(int departmentId)
        {
            List<AccountManagerBO> viewList = new List<AccountManagerBO>();
            string query = string.Format("Select * From SMAccountManager Where DepartmentId = {0}", departmentId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                    DataTable Table = ds.Tables["SalesCall"];

                    viewList = Table.AsEnumerable().Select(r => new AccountManagerBO
                    {
                        AccountManagerId = r.Field<int>("AccountManagerId"),
                        EmpId = r.Field<int>("EmpId"),
                        SortName = r.Field<string>("SortName")

                    }).ToList();
                }
            }
            return viewList;

        }        
        public List<AccountManagerBO> GetAccountManager(int isAdminUser, string type, int userInfoId)
        {
            List<AccountManagerBO> viewList = new List<AccountManagerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountManager_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                    DataTable Table = ds.Tables["SalesCall"];

                    viewList = Table.AsEnumerable().Select(r => new AccountManagerBO
                    {
                        AccountManagerId = r.Field<int>("AccountManagerId"),
                        UserInfoId = r.Field<int>("UserInfoId"),
                        EmpId = r.Field<int>("EmpId"),
                        SortName = r.Field<string>("SortName"),
                        DisplayName = r.Field<string>("DisplayName")

                    }).ToList();
                }
            }
            return viewList;

        }
        public AccountManagerBO GetAccountManagerById(int id)
        {
            AccountManagerBO managerBO = new AccountManagerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountManagerById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AccountManager");
                    DataTable Table = ds.Tables["AccountManager"];

                    managerBO = Table.AsEnumerable().Select(r => new AccountManagerBO
                    {
                        AccountManagerId = r.Field<int>("AccountManagerId"),
                        EmpId = r.Field<int>("EmpId"),
                        SortName = r.Field<string>("SortName"),
                        DisplayName = r.Field<string>("DisplayName"),
                        OfficialEmail = r.Field<string>("OfficialEmail")

                    }).FirstOrDefault();
                }
            }
            return managerBO;

        }
        //new account manager
        public Boolean SaveAccountManagerInfo(AccountManagerBO accountManagerBO, out int OutId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveAccountManagerInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, accountManagerBO.AncestorId);
                            dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, accountManagerBO.EmpId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Type", DbType.String, accountManagerBO.Type);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, accountManagerBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@OutId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));


                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            //int ERR = Convert.ToInt32(commandMaster.Parameters["@Err"].Value);
                            OutId = Convert.ToInt32(commandMaster.Parameters["@OutId"].Value);
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
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }


            return retVal;
        }
        public List<AccountManagerBO> GetAccountManagerByAutoSearch(string searchText)
        {
            List<AccountManagerBO> viewList = new List<AccountManagerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNewAccountManagerByAutoSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(@searchText))
                        dbSmartAspects.AddInParameter(cmd, "@searchText", DbType.String, @searchText);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@searchText", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                    DataTable Table = ds.Tables["SalesCall"];

                    viewList = Table.AsEnumerable().Select(r => new AccountManagerBO
                    {
                        AccountManagerId = r.Field<int>("AccountManagerId"),
                        EmpId = r.Field<int>("EmpId"),
                        DisplayName = r.Field<string>("DisplayName")

                    }).ToList();
                }
            }
            return viewList;

        }
        public bool UpdateAccountManagerInfo(AccountManagerBO accountManagerBO, out int NodeId)
        {
            bool retVal = false;
            NodeId = 0;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateAccountManagerInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@AccountManagerId", DbType.Int32, accountManagerBO.AccountManagerId);
                            dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, accountManagerBO.AncestorId);
                            dbSmartAspects.AddInParameter(commandMaster, "@EmpId", DbType.Int32, accountManagerBO.EmpId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Type", DbType.String, accountManagerBO.Type);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, accountManagerBO.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0 )
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
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }
            return retVal;
        }
        public List<AccountManagerBO> GetAccountManagerInfoForPagination(string accountManager, string supervisonName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<AccountManagerBO> AccountManagerList = new List<AccountManagerBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountManagerInfoForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(accountManager))
                            dbSmartAspects.AddInParameter(cmd, "@AccountManager", DbType.String, accountManager);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@AccountManager", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(supervisonName))
                            dbSmartAspects.AddInParameter(cmd, "@SupervisonName", DbType.String, supervisonName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SupervisonName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    AccountManagerBO AccountManager = new AccountManagerBO();

                                    AccountManager.AccountManagerId = Convert.ToInt32(reader["AccountManagerId"]);
                                    AccountManager.AccountManager = (reader["AccountManager"].ToString());
                                    AccountManager.SupervisonName = (reader["SupervisonName"].ToString());
                                    AccountManagerList.Add(AccountManager);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AccountManagerList;
        }
        public AccountManagerBO GetNewAccountManagerInfoById(int id)
        {
            AccountManagerBO managerBO = new AccountManagerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountManagerInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "AccountManager");
                    DataTable Table = ds.Tables["AccountManager"];

                    managerBO = Table.AsEnumerable().Select(r => new AccountManagerBO
                    {
                        AccountManagerId = r.Field<int>("AccountManagerId"),
                        EmpId = r.Field<int>("EmpId"),
                        AncestorId = r.Field<int>("AncestorId"),
                        AccountManager = r.Field<string>("AccountManager"),
                        SupervisonName = r.Field<string>("SupervisonName"),
                    }).FirstOrDefault();
                }
            }
            return managerBO;

        }
        public List<AccountManagerBO> GetAccountManagerInfoByCustomString(string customString)
        {
            List<AccountManagerBO> nodeMatrixBOList = new List<AccountManagerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAccountManagerInfoByCustomString_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomString", DbType.String, customString);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AccountManagerBO nodeMatrixBO = new AccountManagerBO();

                                nodeMatrixBO.AccountManagerId = Convert.ToInt32(reader["AccountManagerId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                nodeMatrixBO.AccountManager = reader["AccountManager"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
    }
}
