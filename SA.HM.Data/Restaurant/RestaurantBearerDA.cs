using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantBearerDA : BaseService
    {
        public List<RestaurantBearerBO> GetRestaurantUserInfo(string userType)
        {
            List<RestaurantBearerBO> itemList = new List<RestaurantBearerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantUserInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserType", DbType.String, userType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearerbo = new RestaurantBearerBO();
                                bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
                                bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
                                bearerbo.IsItemCanEditDelete = Convert.ToBoolean(reader["IsItemCanEditDelete"]);
                                itemList.Add(bearerbo);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public List<RestaurantBearerBO> GetOnlyRestaurantUser()
        {
            //
            List<RestaurantBearerBO> userList = new List<RestaurantBearerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlyRestaurantUser_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearerbo = new RestaurantBearerBO();

                                bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);

                                userList.Add(bearerbo);
                            }
                        }

                    }
                }
                
            }
            return userList;
        }
        public List<RestaurantBearerBO> GetRestaurantBearerInfo(int costCenterId, int isBearer)
        {
            List<RestaurantBearerBO> itemList = new List<RestaurantBearerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearer = new RestaurantBearerBO();
                                bearer.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearer.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearer.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bearer.BearerId = Convert.ToInt32(reader["BearerId"]);
                                bearer.UserName = reader["UserName"].ToString();
                                bearer.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearer.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                itemList.Add(bearer);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public List<RestaurantBearerBO> GetActiveRestaurantBearerInfo(int costCenterId, int isBearer)
        {
            List<RestaurantBearerBO> itemList = new List<RestaurantBearerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveRestaurantBearerInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearer = new RestaurantBearerBO();
                                bearer.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearer.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearer.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bearer.BearerId = Convert.ToInt32(reader["BearerId"]);
                                bearer.UserName = reader["UserName"].ToString();
                                bearer.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearer.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                itemList.Add(bearer);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public Boolean SaveRestaurantBearerInfo(RestaurantBearerBO bearerBO, List<int> costCenterIdList, out int tmpTypeId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBearerInfo_SP"))
                {
                    foreach (int costCenterId in costCenterIdList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.String, bearerBO.UserInfoId);
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, bearerBO.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, bearerBO.ToDate);
                        dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Boolean, bearerBO.IsBearer);
                        dbSmartAspects.AddInParameter(command, "@IsChef", DbType.Boolean, bearerBO.IsChef);
                        dbSmartAspects.AddInParameter(command, "@BearerPassword", DbType.String, bearerBO.BearerPassword);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bearerBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@IsRestaurantBillCanSettle", DbType.Boolean, bearerBO.IsRestaurantBillCanSettle);
                        dbSmartAspects.AddInParameter(command, "@IsItemCanEditDelete", DbType.Boolean, bearerBO.IsItemCanEditDelete);
                        dbSmartAspects.AddInParameter(command, "@IsItemSearchEnable", DbType.Boolean, bearerBO.IsItemSearchEnable);

                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bearerBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@BearerId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                    tmpTypeId = Convert.ToInt32(command.Parameters["@BearerId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateRestaurantBearerInfo(RestaurantBearerBO bearerBO, List<int> addedCostCenterList, List<int> editedCostCenterList, List<int> deletedCostCenterList)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantBearerInfo_SP"))
                    {
                        foreach (int costCenterId in editedCostCenterList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, bearerBO.BearerId);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.String, bearerBO.UserInfoId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                            dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, bearerBO.FromDate);
                            dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Boolean, bearerBO.IsBearer);
                            dbSmartAspects.AddInParameter(command, "@IsChef", DbType.Boolean, bearerBO.IsChef);
                            dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, bearerBO.ToDate);
                            dbSmartAspects.AddInParameter(command, "@BearerPassword", DbType.String, bearerBO.BearerPassword);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bearerBO.ActiveStat);
                            dbSmartAspects.AddInParameter(command, "@IsRestaurantBillCanSettle", DbType.Boolean, bearerBO.IsRestaurantBillCanSettle);
                            dbSmartAspects.AddInParameter(command, "@IsItemCanEditDelete", DbType.Boolean, bearerBO.IsItemCanEditDelete);
                            dbSmartAspects.AddInParameter(command, "@IsItemSearchEnable", DbType.Boolean, bearerBO.IsItemSearchEnable);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bearerBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }
                    }
                    if (addedCostCenterList.Count > 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantBearerInfo_SP"))
                        {
                            foreach (int costCenterId in addedCostCenterList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.String, bearerBO.UserInfoId);
                                dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, costCenterId);
                                dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, bearerBO.FromDate);
                                dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, bearerBO.ToDate);
                                dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Boolean, bearerBO.IsBearer);
                                dbSmartAspects.AddInParameter(command, "@IsChef", DbType.Boolean, bearerBO.IsChef);
                                dbSmartAspects.AddInParameter(command, "@BearerPassword", DbType.String, bearerBO.BearerPassword);
                                dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bearerBO.ActiveStat);
                                dbSmartAspects.AddInParameter(command, "@IsRestaurantBillCanSettle", DbType.Boolean, bearerBO.IsRestaurantBillCanSettle);
                                dbSmartAspects.AddInParameter(command, "@IsItemCanEditDelete", DbType.Boolean, bearerBO.IsItemCanEditDelete);
                                dbSmartAspects.AddInParameter(command, "@IsItemSearchEnable", DbType.Boolean, bearerBO.IsItemSearchEnable);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bearerBO.LastModifiedBy);
                                dbSmartAspects.AddOutParameter(command, "@BearerId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                    }
                    if (deletedCostCenterList.Count > 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            foreach (int bearerId in deletedCostCenterList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "RestaurantBearer");
                                dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "BearerId");
                                dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, bearerId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                    }
                    if (status > 0)
                    {
                        retVal = true;
                        transction.Commit();
                    }
                    else
                    {
                        retVal = false;
                        transction.Rollback();
                    }
                }
            }
            return retVal;
        }
        public RestaurantBearerBO GetRestaurantBearerInfoById(int bearerId)
        {
            RestaurantBearerBO bearerbo = new RestaurantBearerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoById"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BearerId", DbType.Int32, bearerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
                                bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
                                bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
                                bearerbo.IsItemCanEditDelete = Convert.ToBoolean(reader["IsItemCanEditDelete"]);
                            }
                        }
                    }
                }
            }
            return bearerbo;
        }
        public List<RestaurantBearerBO> GetRestaurantBearerInfoByUserId(int empId, int isbearer, int IsChef)
        {
            List<RestaurantBearerBO> bearerList = new List<RestaurantBearerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByEmpIdNIsBearer_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, empId);

                    if (isbearer >= 0)
                        dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Boolean, (isbearer == 0 ? false : true));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Boolean, DBNull.Value);
                    if (IsChef >= 0)
                        dbSmartAspects.AddInParameter(cmd, "@IsChef", DbType.Boolean, (IsChef == 0 ? false : true));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsChef", DbType.Boolean, DBNull.Value);

                    DataSet BearerDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, BearerDS, "Bearer");
                    DataTable Table = BearerDS.Tables["Bearer"];

                    if (Table != null)
                    {
                        bearerList = Table.AsEnumerable().Select(r => new RestaurantBearerBO
                        {
                            ActiveStatus = r.Field<string>("ActiveStatus"),
                            ActiveStat = r.Field<bool>("ActiveStat"),
                            UserInfoId = r.Field<Int32>("UserInfoId"),
                            CostCenterId = r.Field<Int32>("CostCenterId"),
                            BearerId = r.Field<Int32>("BearerId"),
                            UserName = r.Field<string>("UserName"),
                            ToDate = r.Field<DateTime>("ToDate"),
                            FromDate = r.Field<DateTime>("FromDate"),
                            RestaurantBillCanSettleStatus = r.Field<string>("RestaurantBillCanSettleStatus"),
                            IsRestaurantBillCanSettle = r.Field<bool>("IsRestaurantBillCanSettle"),
                            IsItemCanEditDelete = r.Field<bool>("IsItemCanEditDelete"),
                            IsItemSearchEnableStatus = r.Field<string>("IsItemSearchEnableStatus"),
                            IsItemSearchEnable = r.Field<bool>("IsItemSearchEnable"),
                            IsBearer = r.Field<bool>("IsBearer"),
                            IsChef = r.Field<bool>("IsChef")

                        }).ToList();
                    }
                }
            }
            return bearerList;
        }
        public List<RestaurantBearerBO> GetRestaurantBearerInfoBySearchCriteria(int isBearer, string EmployeeName, DateTime? FromDate, DateTime? ToDate, bool ActiveStat)
        {
            List<RestaurantBearerBO> BearerList = new List<RestaurantBearerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoBySearchCriteria_SP"))
                {
                    if (isBearer >= 0)
                        dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Boolean, DBNull.Value);

                    if (!string.IsNullOrWhiteSpace(EmployeeName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeName", DbType.String, EmployeeName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeName", DbType.String, DBNull.Value);
                    }

                    if (FromDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                    }

                    if (ToDate != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);

                    DataSet BearerDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, BearerDS, "Bearer");
                    DataTable Table = BearerDS.Tables["Bearer"];

                    if (Table != null)
                    {
                        BearerList = Table.AsEnumerable().Select(r => new RestaurantBearerBO
                        {
                            UserInfoId = r.Field<Int32>("UserInfoId"),
                            IsBearer = r.Field<bool>("IsBearer"),
                            IsChef = r.Field<bool>("IsChef"),
                            UserName = r.Field<string>("UserName"),
                            PermittedCostcenter = r.Field<string>("PermittedCostcenter"),
                            IsBearerStatus = r.Field<string>("IsBearerStatus")

                        }).ToList();
                    }
                }
            }
            return BearerList;
        }        
        public int GetCashierInfoByLoginIdNPassword(string bearerId, string bearerPassword, int costcenterId)
        {
            int EmpID = 0;
            //RestaurantBearerBO userInformation = new RestaurantBearerBO();
            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashierInfoByLoginIdNPassword_SP"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, bearerId);
            //        dbSmartAspects.AddInParameter(cmd, "@BearerPassword", DbType.String, bearerPassword);
            //        dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costcenterId);

            //        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    EmpID = Convert.ToInt32(reader["EmpId"]);
            //                }
            //            }
            //        }
            //    }
            //}


            return EmpID;
        }
        public int BearerDuplicateCheck(int isUpdate, int empId, int bearerId, int isBearer)
        {
            Boolean status = false;
            int IsDuplicate = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BearerDuplicateCheck_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@IsUpdate", DbType.Int32, isUpdate);
                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, bearerId);
                    dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Int32, isBearer);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                IsDuplicate = Convert.ToInt32(reader["UserInfoId"]);
                            }
                        }
                    }
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return IsDuplicate;
        }
        public RestaurantBearerBO GetRestaurantBearerInfoByEmpIdNIsBearer(int empId, int isBearer)
        {
            RestaurantBearerBO bearerbo = new RestaurantBearerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByEmpIdNIsBearer_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);                                
                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
                                bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
                                bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
                                bearerbo.IsItemCanEditDelete = Convert.ToBoolean(reader["IsItemCanEditDelete"]);
                            }
                        }
                    }
                }
            }
            return bearerbo;
        }
        public RestaurantBearerBO GetRestaurantBearerInfoByCostCenterIdNEmpIdNIsBearer(int costCenterId, int empId, int isBearer)
        {
            RestaurantBearerBO bearerbo = new RestaurantBearerBO();

            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByCostCenterIdNEmpIdNIsBearer_SP"))
            //    {
            //        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
            //        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
            //        dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);

            //        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
            //        {
            //            if (reader != null)
            //            {
            //                while (reader.Read())
            //                {
            //                    bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
            //                    bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
            //                    bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
            //                    bearerbo.UserName = Convert.ToString(reader["UserName"]);
            //                    bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
            //                    bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
            //                    bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
            //                    bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
            //                    bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
            //                    bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
            //                }
            //            }
            //        }
            //    }
            //}
            return bearerbo;
        }
        public List<RestaurantBearerBO> GetRestaurantInfoForBearerByEmpIdNIsBearer(int userInfoId, int isBearer)
        {
            List<RestaurantBearerBO> bearerboList = new List<RestaurantBearerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantInfoForBearerByEmpIdNIsBearer_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearerbo = new RestaurantBearerBO();
                                bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bearerbo.CostCenter = reader["CostCenter"].ToString();
                                bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
                                bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                bearerboList.Add(bearerbo);
                            }
                        }
                    }
                }
            }
            return bearerboList;
        }
        public Boolean DeleteRestaurantBearerInfo(int userInfoId, int isBearer)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBearerInfo_SP"))
                {

                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Int32, isBearer);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public RestaurantBearerBO GetRestaurantBearerInfoByEmpId(int userInfoId)
        {
            RestaurantBearerBO bearerbo = new RestaurantBearerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByUserInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
                                bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);                                
                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
                                bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
                                bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
                                bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
                                bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
                                bearerbo.IsItemCanEditDelete = Convert.ToBoolean(reader["IsItemCanEditDelete"]);
                                bearerbo.IsBearer = Convert.ToBoolean(reader["IsBearer"]);
                                bearerbo.IsItemSearchEnable = Convert.ToBoolean(reader["IsItemSearchEnable"]);
                            }
                        }
                    }
                }
            }
            return bearerbo;
        }

        public List<RestaurantBearerBO> GetBearerInfoByAutoSearch(string beararName, int costCenterId)
        {
            List<RestaurantBearerBO> bearerList = new List<RestaurantBearerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBearerInfoByAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BeararName", DbType.String, beararName);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearerbo = new RestaurantBearerBO();

                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);

                                bearerList.Add(bearerbo);
                            }
                        }
                    }
                }
            }
            return bearerList;
        }

        public List<RestaurantBearerBO> GetRestaurantUserByAutoSearch(string beararName, int costCenterId)
        {
            List<RestaurantBearerBO> bearerList = new List<RestaurantBearerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantUserByAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BeararName", DbType.String, beararName);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBearerBO bearerbo = new RestaurantBearerBO();

                                bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
                                bearerbo.UserName = Convert.ToString(reader["UserName"]);
                                bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);

                                bearerList.Add(bearerbo);
                            }
                        }
                    }
                }
            }
            return bearerList;
        }
    }
}
