using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.HMCommon
{
    public class DashboardDA : BaseService
    {
        public List<UserDashboardItemMappingBO> GetItemList(long userId)
        {
            List<UserDashboardItemMappingBO> boList = new List<UserDashboardItemMappingBO>();

            DataSet ItemDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserDashboardMappingItemInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int64, userId);

                    dbSmartAspects.LoadDataSet(cmd, ItemDS, "ItemList");
                    DataTable table = ItemDS.Tables["ItemList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new UserDashboardItemMappingBO
                                   {
                                       Id = r.Field<long>("Id"),
                                       UserId = r.Field<long>("UserId"),
                                       ItemId = r.Field<long>("ItemId"),
                                       Panel = r.Field<int>("Panel"),
                                       Div = r.Field<int>("Div")

                                   }).ToList();
                }
            }

            return boList;
        }

        public List<UserDashboardItemMappingBO> GetItemListByUserGroupId(long userGroupId)
        {
            List<UserDashboardItemMappingBO> boList = new List<UserDashboardItemMappingBO>();

            DataSet ItemDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserDashboardMappingItemInfoByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int64, userGroupId);

                    dbSmartAspects.LoadDataSet(cmd, ItemDS, "ItemList");
                    DataTable table = ItemDS.Tables["ItemList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new UserDashboardItemMappingBO
                                   {
                                       Id = r.Field<long>("Id"),
                                       UserId = r.Field<long>("UserId"),
                                       ItemId = r.Field<long>("ItemId"),
                                       Panel = r.Field<int>("Panel"),
                                       Div = r.Field<int>("Div")

                                   }).ToList();
                }
            }

            return boList;
        }

        public Boolean SaveDashboardManagement(List<DashboardManagementBO> boList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDashboardManagement_SP"))
                {
                    foreach (DashboardManagementBO bo in boList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int64, bo.UserId);
                        dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int64, bo.ItemId);
                        dbSmartAspects.AddInParameter(command, "@Panel", DbType.Int32, bo.Panel);
                        dbSmartAspects.AddInParameter(command, "@DivName", DbType.String, bo.DivName);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }

        public Boolean DeleteDashboardManagement(long userId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDashboardManagement_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int64, userId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<DashboardManagementBO> GetDashboardManagement(long userId)
        {
            List<DashboardManagementBO> boList = new List<DashboardManagementBO>();

            DataSet ItemDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDashboardManagement_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int64, userId);

                    dbSmartAspects.LoadDataSet(cmd, ItemDS, "ItemList");
                    DataTable table = ItemDS.Tables["ItemList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new DashboardManagementBO
                                   {
                                       Id = r.Field<long>("Id"),
                                       UserId = r.Field<long>("UserId"),
                                       ItemId = r.Field<long>("ItemId"),
                                       Panel = r.Field<int>("Panel"),
                                       DivName = r.Field<string>("DivName")

                                   }).ToList();
                }
            }

            return boList;
        }

        public List<DashboardManagementBO> GetDashboardManagementByUserGroupId(long userGroupId)
        {
            List<DashboardManagementBO> boList = new List<DashboardManagementBO>();

            DataSet ItemDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDashboardManagementByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int64, userGroupId);

                    dbSmartAspects.LoadDataSet(cmd, ItemDS, "ItemList");
                    DataTable table = ItemDS.Tables["ItemList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new DashboardManagementBO
                                   {
                                       UserId = r.Field<long>("UserId")

                                   }).ToList();
                }
            }

            return boList;
        }

        public List<DashboardItemBO> GetDashboardItem(long userGroupId)
        {
            List<DashboardItemBO> boList = new List<DashboardItemBO>();

            DataSet ItemDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDashboardItemInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    dbSmartAspects.LoadDataSet(cmd, ItemDS, "ItemList");
                    DataTable table = ItemDS.Tables["ItemList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new DashboardItemBO
                                   {
                                       UserMappingId = r.Field<long>("UserMappingId"),
                                       Id = r.Field<long>("Id"),                                       
                                       ItemName = r.Field<string>("ItemName"),
                                       ActiveStatus = r.Field<string>("ActiveStatus")

                                   }).ToList();
                }
            }

            return boList;
        }

        public Boolean SaveUserDashboardMapping(UserDashboardItemMappingBO bo)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUserDashboardMapping_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int64, bo.UserId);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int64, bo.ItemId);
                    dbSmartAspects.AddInParameter(command, "@Panel", DbType.Int32, bo.Panel);
                    dbSmartAspects.AddInParameter(command, "@Div", DbType.String, bo.Div);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean DeleteUserDashboardMapping(long userId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteUserDashboardMapping_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int64, userId);                        

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

        public Boolean DeleteUserDashboardItemByItemId_SP(long itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteUserDashboardItemByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int64, itemId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean DeleteDashboardManagementByItemId_SP(long itemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDashboardManagementByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int64, itemId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
