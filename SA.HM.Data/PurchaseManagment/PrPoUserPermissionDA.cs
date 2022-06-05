using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.PurchaseManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PrPoUserPermissionDA : BaseService
    {
        //public List<RestaurantBearerBO> GetRestaurantBearerInfo(int costCenterId, int isBearer)
        //{
        //    List<RestaurantBearerBO> itemList = new List<RestaurantBearerBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfo_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
        //            dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        RestaurantBearerBO bearer = new RestaurantBearerBO();
        //                        bearer.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        bearer.EmpId = Convert.ToInt32(reader["EmpId"]);
        //                        bearer.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        bearer.BearerId = Convert.ToInt32(reader["BearerId"]);
        //                        bearer.EmployeeName = reader["EmployeeName"].ToString();
        //                        bearer.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
        //                        bearer.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
        //                        itemList.Add(bearer);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return itemList;
        //}
        public Boolean SavePRPOUserPermissionInfo(PrPoUserPermissionBO permissionBO,  List<PrPoUserPermissionBO> userPermissionList, out int tmpTypeId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePRPOUserPermissionInfo_SP"))
                {
                    foreach (PrPoUserPermissionBO po in userPermissionList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, permissionBO.EmpId);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.String, permissionBO.UserInfoId);
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, po.CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, permissionBO.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, permissionBO.ToDate);
                        dbSmartAspects.AddInParameter(command, "@IsPRAllow", DbType.Boolean, po.IsPRAllow);
                        dbSmartAspects.AddInParameter(command, "@IsPOAllow", DbType.String, po.IsPOAllow);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, permissionBO.ActiveStat);                        
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, permissionBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int64, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                    tmpTypeId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdatePRPOUserPermissionInfo(PrPoUserPermissionBO permissionBO, List<PrPoUserPermissionBO> addedCostCenterList, List<PrPoUserPermissionBO> editedCostCenterList, List<PrPoUserPermissionBO> deletedCostCenterList)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePRPOUserPermissionInfo_SP"))
                    {
                        foreach (PrPoUserPermissionBO po in editedCostCenterList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, po.MappingId);
                            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, po.EmpId);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.String, permissionBO.UserInfoId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, po.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, permissionBO.FromDate);
                            dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, permissionBO.ToDate);
                            dbSmartAspects.AddInParameter(command, "@IsPRAllow", DbType.Boolean, po.IsPRAllow);
                            dbSmartAspects.AddInParameter(command, "@IsPOAllow", DbType.Boolean, po.IsPOAllow);
                            dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, permissionBO.ActiveStat);                            
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, permissionBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }
                    }
                    if (addedCostCenterList.Count > 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePRPOUserPermissionInfo_SP"))
                        {
                            foreach (PrPoUserPermissionBO po in addedCostCenterList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, po.EmpId);
                                dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.String, permissionBO.UserInfoId);
                                dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, po.CostCenterId);
                                dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, permissionBO.FromDate);
                                dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, permissionBO.ToDate);
                                dbSmartAspects.AddInParameter(command, "@IsPRAllow", DbType.Boolean, po.IsPRAllow);
                                dbSmartAspects.AddInParameter(command, "@IsPOAllow", DbType.String, po.IsPOAllow);
                                dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, permissionBO.ActiveStat);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, permissionBO.CreatedBy);
                                dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int64, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                    }
                    if (deletedCostCenterList.Count > 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                        {
                            foreach (PrPoUserPermissionBO po in deletedCostCenterList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PRPOUserPermission");
                                dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "MappingId");
                                dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, po.MappingId);

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
        //public RestaurantBearerBO GetRestaurantBearerInfoById(int bearerId)
        //{
        //    RestaurantBearerBO bearerbo = new RestaurantBearerBO();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoById"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@BearerId", DbType.Int32, bearerId);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        bearerbo.EmpId = Convert.ToInt32(reader["EmpId"]);
        //                        bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
        //                        bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
        //                        bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
        //                        bearerbo.EmployeeName = Convert.ToString(reader["EmployeeName"]);
        //                        bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
        //                        bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return bearerbo;
        //}
        public List<PrPoUserPermissionBO> GetPRPOUserPermissionByUserInfoId(int empId)
        {
            List<PrPoUserPermissionBO> bearerList = new List<PrPoUserPermissionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPRPOUserPermissionByUserInfoId"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, empId);                    

                    DataSet BearerDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, BearerDS, "Bearer");
                    DataTable Table = BearerDS.Tables["Bearer"];

                    if (Table != null)
                    {
                        bearerList = Table.AsEnumerable().Select(r => new PrPoUserPermissionBO
                        {
                            MappingId = r.Field<Int64>("MappingId"),
                            ActiveStatus = r.Field<string>("ActiveStatus"),
                            ActiveStat = r.Field<bool>("ActiveStat"),
                            EmpId = r.Field<Int32>("EmpId"),
                            UserInfoId = r.Field<Int32>("UserInfoId"),
                            CostCenterId = r.Field<Int32>("CostCenterId"),                            
                            UserName = r.Field<string>("UserName"),
                            ToDate = r.Field<DateTime>("ToDate"),
                            FromDate = r.Field<DateTime>("FromDate"),
                            IsPRAllow = r.Field<bool>("IsPRAllow"),
                            IsPOAllow = r.Field<bool>("IsPOAllow")

                        }).ToList();
                    }
                }
            }
            return bearerList;
        }
        public List<PrPoUserPermissionBO> GetPRPOUserPermissionInfoBySearchCriteria(string UserName, DateTime? FromDate, DateTime? ToDate, bool ActiveStat)
        {
            List<PrPoUserPermissionBO> boList = new List<PrPoUserPermissionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPRPOUserPermissionBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(UserName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserName", DbType.String, UserName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserName", DbType.String, DBNull.Value);
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
                        boList = Table.AsEnumerable().Select(r => new PrPoUserPermissionBO
                        {
                            //ActiveStatus = r.Field<string>("ActiveStatus"),
                            UserInfoId = r.Field<Int32>("UserInfoId"),
                            //CostCenterId = r.Field<Int32>("CostCenterId"),
                            //MappingId = r.Field<Int64>("MappingId"),
                            UserName = r.Field<string>("UserName"),
                            //ToDate = r.Field<DateTime>("ToDate"),
                            //FromDate = r.Field<DateTime>("FromDate")
                            //PermittedCostcenter = r.Field<string>("PermittedCostcenter")

                        }).ToList();
                    }
                }
            }
            return boList;
        }
        //public int GetRestaurantBearerInfoByPassword(string bearerPassword)
        //{
        //    int EmpID = -1;
        //    RestaurantBearerBO userInformation = new RestaurantBearerBO();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByPassword_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@BearerPassword", DbType.String, bearerPassword);
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        EmpID = Convert.ToInt32(reader["EmpId"]);
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    return EmpID;
        //}
        //public int GetRestaurantBearerInfoByLoginIdNPassword(string bearerId, string bearerPassword)
        //{
        //    int EmpID = 0;
        //    RestaurantBearerBO userInformation = new RestaurantBearerBO();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBearerInfoByLoginIdNPassword_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, bearerId);
        //            dbSmartAspects.AddInParameter(cmd, "@BearerPassword", DbType.String, bearerPassword);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        EmpID = Convert.ToInt32(reader["EmpId"]);
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    return EmpID;
        //}
        //public int BearerDuplicateCheck(int isUpdate, int empId, int bearerId, int isBearer)
        //{
        //    Boolean status = false;
        //    int IsDuplicate = 0;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("BearerDuplicateCheck_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(command, "@IsUpdate", DbType.Int32, isUpdate);
        //            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, empId);
        //            dbSmartAspects.AddInParameter(command, "@BearerId", DbType.Int32, bearerId);
        //            dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Int32, isBearer);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {

        //                        IsDuplicate = Convert.ToInt32(reader["EmpId"]);
        //                    }
        //                }
        //            }
        //            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //        }
        //    }
        //    return IsDuplicate;
        //}
        //public PrPoUserPermissionBO GetPRPOUserPermissionByUserInfoId(int userInfoId)
        //{
        //    PrPoUserPermissionBO bearerbo = new PrPoUserPermissionBO();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPRPOUserPermissionByUserInfoId"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);                    

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        bearerbo.EmpId = Convert.ToInt32(reader["EmpId"]);
        //                        bearerbo.UserInfoId = Convert.ToInt32(reader["UserInfoId"]);
        //                        bearerbo.UserName = Convert.ToString(reader["UserName"]);
        //                        bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        bearerbo.MappingId = Convert.ToInt64(reader["MappingId"]);
        //                        bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
        //                        bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
        //                        bearerbo.IsPRAllow = Convert.ToBoolean(reader["IsPRAllow"]);
        //                        bearerbo.IsPOAllow = Convert.ToBoolean(reader["IsPOAllow"]);                                
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return bearerbo;
        //}
        //public RestaurantBearerBO GetRestaurantBearerInfoByCostCenterIdNEmpIdNIsBearer(int costCenterId, int empId, int isBearer)
        //{
        //    RestaurantBearerBO bearerbo = new RestaurantBearerBO();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByCostCenterIdNEmpIdNIsBearer_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
        //            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
        //            dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        bearerbo.EmpId = Convert.ToInt32(reader["EmpId"]);
        //                        bearerbo.EmployeeName = Convert.ToString(reader["EmployeeName"]);
        //                        bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
        //                        bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
        //                        bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
        //                        bearerbo.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
        //                        bearerbo.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return bearerbo;
        //}
        //public List<RestaurantBearerBO> GetRestaurantInfoForBearerByEmpIdNIsBearer(int empId, int isBearer)
        //{
        //    List<RestaurantBearerBO> bearerboList = new List<RestaurantBearerBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantInfoForBearerByEmpIdNIsBearer_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
        //            dbSmartAspects.AddInParameter(cmd, "@IsBearer", DbType.Int32, isBearer);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        RestaurantBearerBO bearerbo = new RestaurantBearerBO();
        //                        bearerbo.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        bearerbo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
        //                        bearerbo.EmpId = Convert.ToInt32(reader["EmpId"]);
        //                        bearerbo.EmployeeName = Convert.ToString(reader["EmployeeName"]);
        //                        bearerbo.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        bearerbo.CostCenter = reader["CostCenter"].ToString();
        //                        bearerbo.BearerId = Convert.ToInt32(reader["BearerId"]);
        //                        bearerbo.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
        //                        bearerbo.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
        //                        bearerboList.Add(bearerbo);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return bearerboList;
        //}
        //public RestaurantBearerBO GetRestaurantBearerInfoByIdNPassword(int empId, string userPassword)
        //{
        //    RestaurantBearerBO bearer = new RestaurantBearerBO();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBearerInfoByIdNPassword_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
        //            dbSmartAspects.AddInParameter(cmd, "@UserPassword", DbType.String, userPassword);
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {

        //                        bearer.ActiveStatus = reader["ActiveStatus"].ToString();
        //                        bearer.EmpId = Convert.ToInt32(reader["EmpId"]);
        //                        bearer.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        bearer.BearerId = Convert.ToInt32(reader["BearerId"]);
        //                        bearer.EmployeeName = reader["EmployeeName"].ToString();
        //                        bearer.ToDate = Convert.ToDateTime(reader["ToDate"].ToString());
        //                        bearer.FromDate = Convert.ToDateTime(reader["FromDate"].ToString());
        //                        bearer.RestaurantBillCanSettleStatus = reader["RestaurantBillCanSettleStatus"].ToString();
        //                        bearer.IsRestaurantBillCanSettle = Convert.ToBoolean(reader["IsRestaurantBillCanSettle"]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return bearer;
        //}
        //public Boolean DeleteRestaurantBearerInfo(int bearerId, int isBearer)
        //{
        //    Boolean status = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantBearerInfo_SP"))
        //        {

        //            dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, bearerId);
        //            dbSmartAspects.AddInParameter(command, "@IsBearer", DbType.Int32, isBearer);

        //            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
        //        }
        //    }
        //    return status;
        //}
    }
}
