using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Security;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Security
{
    public class MenuDA : BaseService
    {
        public List<MenuGroupBO> GetMenuGroup()
        {
            List<MenuGroupBO> menuGroup = new List<MenuGroupBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuGroup_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuGroup");
                    DataTable Table = SaleServiceDS.Tables["MenuGroup"];

                    menuGroup = Table.AsEnumerable().Select(r => new MenuGroupBO
                    {
                        MenuGroupId = r.Field<Int64>("MenuGroupId"),
                        MenuGroupName = r.Field<string>("MenuGroupName"),
                        GroupDisplayCaption = r.Field<string>("GroupDisplayCaption"),
                        DisplaySequence = r.Field<int>("DisplaySequence"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();
                }
            }
            return menuGroup;
        }

        public List<MenuGroupBO> GetMenuGroupByUserGroupId(int userGroupId, string pageType)
        {
            List<MenuGroupBO> menuGroup = new List<MenuGroupBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuGroupByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@PageType", DbType.String, pageType);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuGroup");
                    DataTable Table = SaleServiceDS.Tables["MenuGroup"];

                    menuGroup = Table.AsEnumerable().Select(r => new MenuGroupBO
                    {
                        MenuGroupId = r.Field<Int64>("MenuGroupId"),
                        MenuGroupName = r.Field<string>("MenuGroupName"),
                        DisplaySequence = r.Field<int>("DisplaySequence"),
                        GroupName = r.Field<string>("GroupName"),
                        GroupIconClass = r.Field<string>("GroupIconClass")

                    }).ToList();
                }
            }
            return menuGroup;
        }
        public List<MenuLinksBO> GetMenuLinksByModuleId(int moduleId)
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksByModuleId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuLinks"];

                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        ModuleId = r.Field<Int32>("ModuleId"),
                        PageId = r.Field<string>("PageId"),
                        PageName = r.Field<string>("PageName"),
                        PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();
                }
            }
            return menuLinks;
        }

        public List<MenuLinksBO> GetMenuLinksByModuleId(int userGroupId, int moduleId)
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksByModuleNGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuLinks"];

                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        ModuleId = r.Field<Int32>("ModuleId"),
                        PageId = r.Field<string>("PageId"),
                        PageName = r.Field<string>("PageName"),
                        PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();
                }
            }
            return menuLinks;
        }
        public MenuGroupBO GetMenuGroupById(long menuGroupId)
        {
            MenuGroupBO menuGroupBO = new MenuGroupBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuGroupById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int64, menuGroupId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuGroup");
                    DataTable Table = SaleServiceDS.Tables["MenuGroup"];

                    menuGroupBO = Table.AsEnumerable().Select(r => new MenuGroupBO
                    {
                        MenuGroupId = r.Field<Int64>("MenuGroupId"),
                        MenuGroupName = r.Field<string>("MenuGroupName"),
                        GroupDisplayCaption = r.Field<string>("GroupDisplayCaption"),
                        DisplaySequence = r.Field<int>("DisplaySequence"),
                        GroupIconClass = r.Field<string>("GroupIconClass"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).FirstOrDefault();
                }
            }
            return menuGroupBO;
        }
        public MenuLinksBO GetMenuLinksByMenuLinksId(long menuLinksId)
        {
            MenuLinksBO menuLinks = new MenuLinksBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MenuLinksId", DbType.Int64, menuLinksId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuLinks"];

                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        ModuleId = r.Field<Int32>("ModuleId"),
                        PageId = r.Field<string>("PageId"),
                        PageName = r.Field<string>("PageName"),
                        PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        LinkIconClass = r.Field<string>("LinkIconClass"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).FirstOrDefault();
                }
            }
            return menuLinks;
        }
        public List<MenuWiseLinkViewBO> GetMenuWiseLinksByUserGroupId(int userGroupId, string pageType)
        {
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksByUserGroupId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@PageType", DbType.String, pageType);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuWiseLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuWiseLinks"];

                    menuWiseLinks = Table.AsEnumerable().Select(r => new MenuWiseLinkViewBO
                    {
                        MenuGroupId = r.Field<Int64>("MenuGroupId"),
                        GroupDisplaySequence = r.Field<int>("GroupDisplaySequence"),
                        MenuGroupName = r.Field<string>("MenuGroupName"),
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        LinksDisplaySequence = r.Field<int>("LinksDisplaySequence"),
                        PageId = r.Field<string>("PageId"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        PageName = r.Field<string>("PageName"),
                        ModuleId = r.Field<int>("ModuleId"),
                        GroupName = r.Field<string>("GroupName"),
                        LinkIconClass = r.Field<string>("LinkIconClass")

                    }).ToList();
                }
            }
            return menuWiseLinks;
        }
        public List<MenuWiseLinkViewBO> GetMenuLinksByModuleIdUserGroupNMenuGroupId(int userGroupId, long menuGroupId, int moduleId)
        {
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksByModuleIdUserGroupNMenuGroupId_SP"))
                {
                    if (userGroupId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, DBNull.Value);
                    }
                    
                    dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int64, menuGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuWiseLinks");
                    DataTable Table = SaleServiceDS.Tables["MenuWiseLinks"];

                    menuWiseLinks = Table.AsEnumerable().Select(r => new MenuWiseLinkViewBO
                    {
                        MenuWiseLinksId = r.Field<Int64>("MenuWiseLinksId"),
                        MenuGroupId = r.Field<Int64>("MenuGroupId"),
                        UserGroupId = r.Field<Int32>("UserGroupId"),
                        GroupDisplaySequence = r.Field<int>("GroupDisplaySequence"),
                        MenuGroupName = r.Field<string>("MenuGroupName"),
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        LinksDisplaySequence = r.Field<int>("LinksDisplaySequence"),
                        PageId = r.Field<string>("PageId"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        PageName = r.Field<string>("PageName"),
                        ModuleId = r.Field<int>("ModuleId"),
                        GroupName = r.Field<string>("GroupName"),
                        LinkIconClass = r.Field<string>("LinkIconClass"),
                        IsSavePermission = r.Field<bool>("IsSavePermission"),
                        IsUpdatePermission = r.Field<bool>("IsUpdatePermission"),
                        IsDeletePermission = r.Field<bool>("IsDeletePermission"),
                        IsViewPermission = r.Field<bool>("IsViewPermission")

                    }).ToList();
                }
            }
            return menuWiseLinks;
        }

        public List<MenuWiseLinkViewBO> GetMenuLinksByModuleIdUserIdNMenuGroupId(int userId, long menuGroupId, int moduleId)
        {
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksByModuleIdUserIdNMenuGroupId_SP"))
                {
                    if (userId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int64, menuGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                MenuWiseLinkViewBO menuWiseLink = new MenuWiseLinkViewBO();
                                menuWiseLink.MenuWiseLinksId = Convert.ToInt64(reader["MenuWiseLinksId"]);
                                menuWiseLink.MenuGroupId = Convert.ToInt64(reader["MenuGroupId"]);
                                menuWiseLink.UserGroupId = Convert.ToInt32(reader["UserGroupId"]);
                                menuWiseLink.GroupDisplaySequence = Convert.ToInt32(reader["GroupDisplaySequence"]);
                                menuWiseLink.MenuGroupName = Convert.ToString(reader["MenuGroupName"]);
                                menuWiseLink.MenuLinksId = Convert.ToInt64(reader["MenuLinksId"]);
                                menuWiseLink.LinksDisplaySequence = Convert.ToInt32(reader["LinksDisplaySequence"]);
                                menuWiseLink.PageId = Convert.ToString(reader["PageId"]);
                                menuWiseLink.PageExtension = Convert.ToString(reader["PageExtension"]);
                                menuWiseLink.PagePath = Convert.ToString(reader["PagePath"]);
                                menuWiseLink.PageType = Convert.ToString(reader["PageType"]);
                                menuWiseLink.PageName = Convert.ToString(reader["PageName"]);
                                menuWiseLink.ModuleId = Convert.ToInt32(reader["ModuleId"]);
                                menuWiseLink.GroupName = Convert.ToString(reader["GroupName"]);
                                menuWiseLink.LinkIconClass = Convert.ToString(reader["LinkIconClass"]);
                                menuWiseLink.IsSavePermission = Convert.ToBoolean(reader["IsSavePermission"]);
                                menuWiseLink.IsUpdatePermission = Convert.ToBoolean(reader["IsUpdatePermission"]);
                                menuWiseLink.IsDeletePermission = Convert.ToBoolean(reader["IsDeletePermission"]);
                                menuWiseLink.IsViewPermission = Convert.ToBoolean(reader["IsViewPermission"]);
                                menuWiseLinks.Add(menuWiseLink);
                            }
                        }
                    }
                }
            }
            return menuWiseLinks;
        }

        public List<CommonModuleNameBO> GetCommonMenuModule()
        {
            List<CommonModuleNameBO> menuGroup = new List<CommonModuleNameBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonMenuModule_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuModule");
                    DataTable Table = SaleServiceDS.Tables["MenuModule"];

                    menuGroup = Table.AsEnumerable().Select(r => new CommonModuleNameBO
                    {
                        ModuleId = r.Field<Int32>("ModuleId"),
                        TypeId = r.Field<Int32>("TypeId"),
                        ModuleName = r.Field<string>("ModuleName"),
                        GroupName = r.Field<string>("GroupName"),
                        ModulePath = r.Field<string>("ModulePath"),
                        IsReportType = r.Field<bool>("IsReportType"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();
                }
            }
            return menuGroup;
        }
        public CommonModuleNameBO GetCommonMenuModuleById(int moduleId)
        {
            CommonModuleNameBO menuGroup = new CommonModuleNameBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonMenuModuleByModuleId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);

                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "MenuModule");
                    DataTable Table = SaleServiceDS.Tables["MenuModule"];

                    menuGroup = Table.AsEnumerable().Select(r => new CommonModuleNameBO
                    {
                        ModuleId = r.Field<Int32>("ModuleId"),
                        ModuleName = r.Field<string>("ModuleName"),
                        GroupName = r.Field<string>("GroupName"),
                        ModulePath = r.Field<string>("ModulePath"),
                        IsReportType = r.Field<bool>("IsReportType"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).FirstOrDefault();
                }
            }
            return menuGroup;
        }
        public bool SaveUserGroupWiseMenuNPermission(List<MenuWiseLinksBO> securityMenuWiseLinksNelyAdded, List<MenuWiseLinksBO> securityMenuWiseLinksEdited, List<MenuWiseLinksBO> securityMenuWiseLinksDeleted, int createdBy)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (securityMenuWiseLinksNelyAdded.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveUserGroupWiseMenuLinksNPermission_SP"))
                            {
                                foreach (MenuWiseLinksBO mwl in securityMenuWiseLinksNelyAdded)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@UserGroupId", DbType.Int64, mwl.UserGroupId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupId", DbType.Int64, mwl.MenuGroupId);

                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuLinksId", DbType.Int64, mwl.MenuLinksId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplaySequence", DbType.Int32, mwl.DisplaySequence);

                                    dbSmartAspects.AddInParameter(commandAdd, "@IsSavePermission", DbType.Boolean, mwl.IsSavePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsUpdatePermission", DbType.Boolean, mwl.IsUpdatePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDeletePermission", DbType.Boolean, mwl.IsDeletePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsViewPermission", DbType.Boolean, mwl.IsViewPermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, mwl.ActiveStat);

                                    dbSmartAspects.AddInParameter(commandAdd, "@CreatedBy", DbType.Int32, createdBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }
                        else
                        {
                            status = 1;
                        }

                        if (status > 0 && securityMenuWiseLinksEdited.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateUserGroupWiseMenuLinksNPermission_SP"))
                            {
                                foreach (MenuWiseLinksBO mwl in securityMenuWiseLinksEdited)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuWiseLinksId", DbType.Int64, mwl.MenuWiseLinksId);

                                    dbSmartAspects.AddInParameter(commandAdd, "@UserGroupId", DbType.Int64, mwl.UserGroupId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupId", DbType.Int64, mwl.MenuGroupId);

                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuLinksId", DbType.Int64, mwl.MenuLinksId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplaySequence", DbType.Int32, mwl.DisplaySequence);

                                    dbSmartAspects.AddInParameter(commandAdd, "@IsSavePermission", DbType.Boolean, mwl.IsSavePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsUpdatePermission", DbType.Boolean, mwl.IsUpdatePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDeletePermission", DbType.Boolean, mwl.IsDeletePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsViewPermission", DbType.Boolean, mwl.IsViewPermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, mwl.ActiveStat);

                                    dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, createdBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }

                        if (status > 0 && securityMenuWiseLinksDeleted.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (MenuWiseLinksBO mwl in securityMenuWiseLinksDeleted)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@TableName", DbType.String, "SecurityMenuWiseLinks");
                                    dbSmartAspects.AddInParameter(commandAdd, "@TablePKField", DbType.String, "MenuWiseLinksId");
                                    dbSmartAspects.AddInParameter(commandAdd, "@TablePKId", DbType.String, mwl.MenuWiseLinksId.ToString());

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
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
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public bool SaveUserIdWiseMenuNPermission(List<MenuWiseLinksBO> securityMenuWiseLinksNelyAdded, List<MenuWiseLinksBO> securityMenuWiseLinksEdited, List<MenuWiseLinksBO> securityMenuWiseLinksDeleted, int createdBy)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (securityMenuWiseLinksNelyAdded.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveUserIdWiseMenuNPermission_SP"))
                            {
                                foreach (MenuWiseLinksBO mwl in securityMenuWiseLinksNelyAdded)
                                {
                                    commandAdd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuWiseLinksId", DbType.Int32, mwl.MenuWiseLinksId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@UserId", DbType.Int32, mwl.UserId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupId", DbType.Int64, mwl.MenuGroupId);

                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuLinksId", DbType.Int64, mwl.MenuLinksId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplaySequence", DbType.Int32, mwl.DisplaySequence);

                                    dbSmartAspects.AddInParameter(commandAdd, "@IsSavePermission", DbType.Boolean, mwl.IsSavePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsUpdatePermission", DbType.Boolean, mwl.IsUpdatePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDeletePermission", DbType.Boolean, mwl.IsDeletePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsViewPermission", DbType.Boolean, mwl.IsViewPermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, mwl.ActiveStat);

                                    dbSmartAspects.AddInParameter(commandAdd, "@CreatedBy", DbType.Int32, createdBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }
                        else
                        {
                            status = 1;
                        }

                        if (status > 0 && securityMenuWiseLinksEdited.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateUserIdWiseMenuLinksNPermission_SP"))
                            {
                                foreach (MenuWiseLinksBO mwl in securityMenuWiseLinksEdited)
                                {
                                    commandAdd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuWiseLinksByUserInfoId", DbType.Int64, mwl.MenuWiseLinksId);

                                    dbSmartAspects.AddInParameter(commandAdd, "@UserId", DbType.Int32, mwl.UserId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupId", DbType.Int64, mwl.MenuGroupId);

                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuLinksId", DbType.Int64, mwl.MenuLinksId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@DisplaySequence", DbType.Int32, mwl.DisplaySequence);

                                    dbSmartAspects.AddInParameter(commandAdd, "@IsSavePermission", DbType.Boolean, mwl.IsSavePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsUpdatePermission", DbType.Boolean, mwl.IsUpdatePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsDeletePermission", DbType.Boolean, mwl.IsDeletePermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@IsViewPermission", DbType.Boolean, mwl.IsViewPermission);
                                    dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, mwl.ActiveStat);

                                    dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, createdBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
                                }
                            }
                        }

                        if (status > 0 && securityMenuWiseLinksDeleted.Count > 0)
                        {
                            using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamicallyByUserId_SP"))
                            {
                                foreach (MenuWiseLinksBO mwl in securityMenuWiseLinksDeleted)
                                {
                                    commandAdd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandAdd, "@UserId", DbType.Int32, mwl.UserId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@TableName", DbType.String, "SecurityMenuWiseLinksByUserInfoId");
                                    dbSmartAspects.AddInParameter(commandAdd, "@TablePKField", DbType.String, "MenuWiseLinksByUserInfoId");
                                    dbSmartAspects.AddInParameter(commandAdd, "@TablePKId", DbType.String, mwl.MenuWiseLinksId.ToString());
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupId", DbType.Int64, mwl.MenuGroupId);
                                    dbSmartAspects.AddInParameter(commandAdd, "@MenuLinksId", DbType.Int64, mwl.MenuLinksId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
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
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public Boolean SaveMenuLinks(MenuLinksBO menuLinks)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveMenuLinks_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandAdd, "@ModuleId", DbType.Int32, menuLinks.ModuleId);
                            dbSmartAspects.AddInParameter(commandAdd, "@PageId", DbType.String, menuLinks.PageId);

                            dbSmartAspects.AddInParameter(commandAdd, "@PageName", DbType.String, menuLinks.PageName);
                            dbSmartAspects.AddInParameter(commandAdd, "@PageDisplayCaption", DbType.String, menuLinks.PageDisplayCaption);

                            dbSmartAspects.AddInParameter(commandAdd, "@PageExtension", DbType.String, menuLinks.PageExtension);
                            dbSmartAspects.AddInParameter(commandAdd, "@PagePath", DbType.String, menuLinks.PagePath);
                            dbSmartAspects.AddInParameter(commandAdd, "@PageType", DbType.String, menuLinks.PageType);

                            if (!string.IsNullOrEmpty(menuLinks.LinkIconClass))
                                dbSmartAspects.AddInParameter(commandAdd, "@LinkIconClass", DbType.String, menuLinks.LinkIconClass);
                            else
                                dbSmartAspects.AddInParameter(commandAdd, "@LinkIconClass", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, menuLinks.ActiveStat);
                            dbSmartAspects.AddInParameter(commandAdd, "@CreatedBy", DbType.Int32, menuLinks.CreatedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
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
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public Boolean UpdateMenuLinks(MenuLinksBO menuLinks)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateMenuLinks_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandAdd, "@MenuLinksId", DbType.Int64, menuLinks.MenuLinksId);
                            dbSmartAspects.AddInParameter(commandAdd, "@ModuleId", DbType.Int32, menuLinks.ModuleId);
                            dbSmartAspects.AddInParameter(commandAdd, "@PageId", DbType.String, menuLinks.PageId);

                            dbSmartAspects.AddInParameter(commandAdd, "@PageName", DbType.String, menuLinks.PageName);
                            dbSmartAspects.AddInParameter(commandAdd, "@PageDisplayCaption", DbType.String, menuLinks.PageDisplayCaption);

                            dbSmartAspects.AddInParameter(commandAdd, "@PageExtension", DbType.String, menuLinks.PageExtension);
                            dbSmartAspects.AddInParameter(commandAdd, "@PagePath", DbType.String, menuLinks.PagePath);
                            dbSmartAspects.AddInParameter(commandAdd, "@PageType", DbType.String, menuLinks.PageType);

                            if (!string.IsNullOrEmpty(menuLinks.LinkIconClass))
                                dbSmartAspects.AddInParameter(commandAdd, "@LinkIconClass", DbType.String, menuLinks.LinkIconClass);
                            else
                                dbSmartAspects.AddInParameter(commandAdd, "@LinkIconClass", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, menuLinks.ActiveStat);
                            dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, menuLinks.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
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
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public Boolean SaveMenuGroup(MenuGroupBO menuGroup)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("SaveMenuGroups_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupName", DbType.String, menuGroup.MenuGroupName);
                            dbSmartAspects.AddInParameter(commandAdd, "@GroupDisplayCaption", DbType.String, menuGroup.GroupDisplayCaption);

                            dbSmartAspects.AddInParameter(commandAdd, "@DisplaySequence", DbType.String, menuGroup.DisplaySequence);

                            if (!string.IsNullOrEmpty(menuGroup.GroupIconClass))
                                dbSmartAspects.AddInParameter(commandAdd, "@GroupIconClass", DbType.String, menuGroup.GroupIconClass);
                            else
                                dbSmartAspects.AddInParameter(commandAdd, "@GroupIconClass", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, menuGroup.ActiveStat);
                            dbSmartAspects.AddInParameter(commandAdd, "@CreatedBy", DbType.Int32, menuGroup.CreatedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
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
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public Boolean UpdateMenuGroup(MenuGroupBO menuGroup)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandAdd = dbSmartAspects.GetStoredProcCommand("UpdateMenuGroups_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupId", DbType.Int64, menuGroup.MenuGroupId);
                            dbSmartAspects.AddInParameter(commandAdd, "@MenuGroupName", DbType.String, menuGroup.MenuGroupName);
                            dbSmartAspects.AddInParameter(commandAdd, "@GroupDisplayCaption", DbType.String, menuGroup.GroupDisplayCaption);

                            dbSmartAspects.AddInParameter(commandAdd, "@DisplaySequence", DbType.String, menuGroup.DisplaySequence);

                            if (!string.IsNullOrEmpty(menuGroup.GroupIconClass))
                                dbSmartAspects.AddInParameter(commandAdd, "@GroupIconClass", DbType.String, menuGroup.GroupIconClass);
                            else
                                dbSmartAspects.AddInParameter(commandAdd, "@GroupIconClass", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandAdd, "@ActiveStat", DbType.Boolean, menuGroup.ActiveStat);
                            dbSmartAspects.AddInParameter(commandAdd, "@LastModifiedBy", DbType.Int32, menuGroup.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandAdd, transction);
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
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();

                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public List<MenuLinksBO> GetMenuLinksForSearch(int moduleId, string pageName, string pageType, int recordPerPage, int pageNumber, out int totalRecords)
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksForSearch_SP"))
                {
                    if (moduleId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(pageName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PageName", DbType.String, pageName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PageName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(pageType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PageType", DbType.String, pageType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PageType", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@recordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageNumber);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MenuLinks");
                    DataTable Table = ds.Tables["MenuLinks"];

                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        ModuleId = r.Field<Int32>("ModuleId"),
                        PageId = r.Field<string>("PageId"),
                        PageName = r.Field<string>("PageName"),
                        PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                        PageExtension = r.Field<string>("PageExtension"),
                        PagePath = r.Field<string>("PagePath"),
                        PageType = r.Field<string>("PageType"),
                        LinkIconClass = r.Field<string>("LinkIconClass"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return menuLinks;
        }
        public List<MenuGroupBO> GetMenuGroupForSearch(string menuGroupName, int recordPerPage, int pageNumber, out int totalRecords)
        {
            List<MenuGroupBO> menuLinks = new List<MenuGroupBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuGroupForSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(menuGroupName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MenuGroupName", DbType.String, menuGroupName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MenuGroupName", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@recordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageNumber);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MenuGroup");
                    DataTable Table = ds.Tables["MenuGroup"];

                    menuLinks = Table.AsEnumerable().Select(r => new MenuGroupBO
                    {
                        MenuGroupId = r.Field<Int64>("MenuGroupId"),
                        MenuGroupName = r.Field<string>("MenuGroupName"),
                        GroupDisplayCaption = r.Field<string>("GroupDisplayCaption"),
                        DisplaySequence = r.Field<int>("DisplaySequence"),
                        GroupIconClass = r.Field<string>("GroupIconClass"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return menuLinks;
        }

        public List<MenuLinksBO> GetMenuLinksByMenuName(string pageName, int userGroupId, int userInfoId)
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksByMenuName_SP"))
                {
                    if (!string.IsNullOrEmpty(pageName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PageName", DbType.String, pageName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PageName", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    if (userInfoId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MenuLinks");
                    DataTable Table = ds.Tables["MenuLinks"];

                    menuLinks = Table.AsEnumerable().Select(r => new MenuLinksBO
                    {
                        MenuLinksId = r.Field<Int64>("MenuLinksId"),
                        PageName = r.Field<string>("PageName"),
                        PagePath = r.Field<string>("PagePath")

                    }).ToList();

                }
            }

            return menuLinks;
        }
    }
}
