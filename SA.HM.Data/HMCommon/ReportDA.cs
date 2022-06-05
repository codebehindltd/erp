using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Security;

namespace HotelManagement.Data.HMCommon
{
    public class ReportDA : BaseService
    {
        public List<ActivityLogReportViewBO> GetActivityLogInfo(DateTime fromDate, DateTime toDate, string userId, string moduleName)
        {
            List<ActivityLogReportViewBO> activityLogInfo = new List<ActivityLogReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActivityLogBySearchCriteriaForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@ModuleName", DbType.String, ( moduleName));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ActivityLog");
                    DataTable Table = ds.Tables["ActivityLog"];
                    activityLogInfo = Table.AsEnumerable().Select(r =>
                                new ActivityLogReportViewBO
                                {
                                    ActivityType = r.Field<string>("ActivityType"),
                                    UserName = r.Field<string>("UserName"),
                                    Module = r.Field<string>("Module"),
                                    Remarks = r.Field<string>("Remarks"),
                                    FieldName = r.Field<string>("FieldName"),
                                    PreviousData = r.Field<string>("PreviousData"),
                                    CurrentData = r.Field<string>("CurrentData"),
                                    CreatedByDate = r.Field<string>("CreatedByDate")
                                }).ToList();
                }
            }
            return activityLogInfo;
        }

        public List<GetApprovalConfigurationForReportViewBO> GetApprovalConfigurationForReport(int FeaturesId, int UserGroupId, int UserInfoId)
        {
            List<GetApprovalConfigurationForReportViewBO> approvalConfigurationLogInfo = new List<GetApprovalConfigurationForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovalConfigurationForReport_SP"))
                {
                    if (FeaturesId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@FeaturesId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FeaturesId", DbType.Int32, FeaturesId);

                    if (UserGroupId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, UserGroupId);

                    if (UserInfoId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, UserInfoId);



                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ApprovalLog");
                    DataTable Table = ds.Tables["ApprovalLog"];
                    approvalConfigurationLogInfo = Table.AsEnumerable().Select(r =>
                                new GetApprovalConfigurationForReportViewBO
                                {
                                    Features = r.Field<string>("Features"),
                                    GroupName = r.Field<string>("GroupName"),
                                    UserId = r.Field<string>("UserId"),
                                    UserName = r.Field<string>("UserName"),
                                    UserDesignation = r.Field<string>("UserDesignation"),
                                    UserEmail = r.Field<string>("UserEmail"),
                                    UserPhone = r.Field<string>("UserPhone"),
                                    IsCheckedBy = r.Field<bool?>("IsCheckedBy"),
                                    IsApprovedBy = r.Field<bool?>("IsApprovedBy")
                                }).ToList();
                }
            }
            return approvalConfigurationLogInfo;
        }

        public List<GetUserInformationForReportViewBO> GetUserInformationForReport(int UserGroupId)
        {
            List<GetUserInformationForReportViewBO> approvalInformationLogInfo = new List<GetUserInformationForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserInformationForReport_SP"))
                {
                    if (UserGroupId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, UserGroupId);



                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "InformationLog");
                    DataTable Table = ds.Tables["InformationLog"];
                    approvalInformationLogInfo = Table.AsEnumerable().Select(r =>
                                new GetUserInformationForReportViewBO
                                {
                                    GroupName = r.Field<string>("GroupName"),
                                    UserId = r.Field<string>("UserId"),
                                    UserName = r.Field<string>("UserName"),
                                    UserDesignation = r.Field<string>("UserDesignation"),
                                    UserEmail = r.Field<string>("UserEmail"),
                                    UserPhone = r.Field<string>("UserPhone")
                                }).ToList();
                }
            }
            return approvalInformationLogInfo;
        }

        public List<GetUserPermissionForReportViewBO> GetUserPermissionForReport(int UserGroupId, int moduleId, int MenuGroupId, int MenuLinksId)
        {
            List<GetUserPermissionForReportViewBO> approvalPermissionLogInfo = new List<GetUserPermissionForReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserPermissionForReport_SP"))
                {

                    if (moduleId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);

                    if (UserGroupId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, UserGroupId);

                    if (MenuGroupId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int32, MenuGroupId);

                    if (MenuLinksId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@MenuLinksId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MenuLinksId", DbType.Int32, MenuLinksId);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PermissionLog");
                    DataTable Table = ds.Tables["PermissionLog"];
                    approvalPermissionLogInfo = Table.AsEnumerable().Select(r =>
                                new GetUserPermissionForReportViewBO
                                {
                                    GroupName = r.Field<string>("GroupName"),
                                    GroupDisplayCaption = r.Field<string>("GroupDisplayCaption"),
                                    ModuleName = r.Field<string>("ModuleName"),
                                    PageDisplayCaption = r.Field<string>("PageDisplayCaption"),
                                    MenuWiseLinksId = r.Field<long>("MenuWiseLinksId"),
                                    UserGroupId = r.Field<int>("UserGroupId"),
                                    MenuGroupId = r.Field<long>("MenuGroupId"),
                                    MenuLinksId = r.Field<long>("MenuLinksId"),
                                    DisplaySequence = r.Field<int>("DisplaySequence"),
                                    IsSavePermission = r.Field<bool>("IsSavePermission"),
                                    IsUpdatePermission = r.Field<bool?>("IsUpdatePermission"),
                                    IsDeletePermission = r.Field<bool>("IsDeletePermission"),
                                    IsViewPermission = r.Field<bool>("IsViewPermission"),
                                    ActiveStat = r.Field<bool>("ActiveStat"),
                                    CreatedBy = r.Field<int?>("CreatedBy"),
                                    CreatedDate = r.Field<DateTime?>("CreatedDate"),
                                    LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                                    LastModifiedDate = r.Field<DateTime?>("LastModifiedDate")
                                }).ToList();
                }
            }
            return approvalPermissionLogInfo;
        }

        public List<MenuLinksBO> GetMenuLinksForReportPage( int moduleId, int UserGroupId, int MenuGroupId)
        {
            List<MenuLinksBO> menuLinks = new List<MenuLinksBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMenuLinksForReportPage_SP"))
                {

                    if (moduleId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ModuleId", DbType.Int32, moduleId);

                    if (UserGroupId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, UserGroupId);

                    if (MenuGroupId == 0)
                        dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int32, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MenuGroupId", DbType.Int32, MenuGroupId);


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
