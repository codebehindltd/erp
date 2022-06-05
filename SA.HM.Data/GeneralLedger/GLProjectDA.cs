using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLProjectDA : BaseService
    {
        public List<GLProjectBO> GetAllGLProjectInfo()
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLProjectInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.StageId = Convert.ToInt32(reader["StageId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.GLCompany = reader["GLCompany"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.CodeAndName = reader["CodeAndName"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.ProjectAmount = Convert.ToDecimal(reader["ProjectAmount"]);
                                if (reader["StartDate"] != DBNull.Value)
                                {
                                    projectBO.StartDate = Convert.ToDateTime(reader["StartDate"]);
                                }
                                else
                                {

                                }
                                if (reader["EndDate"] != DBNull.Value)
                                {
                                    projectBO.EndDate = Convert.ToDateTime(reader["EndDate"]);
                                }
                                else
                                {

                                }
                                //projectBO.StartDate = Convert.ToDateTime(reader["StartDate"]);
                                //projectBO.EndDate = Convert.ToDateTime(reader["EndDate"]);
                                projectBO.Description = reader["Description"].ToString();
                                projectList.Add(projectBO);
                            }
                        }
                    }
                }
            }
            return projectList;
        }
        public bool SaveGLProjectInfo(GLProjectBO projectBO, List<GLProjectWiseCostCenterMappingBO> newCostCenterList, long randomDocumentOwnerId, string deletedDocuments, out int tmpProjectId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLProjectInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, projectBO.Name);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, projectBO.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, projectBO.Code);
                            dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, projectBO.ShortName);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, projectBO.Description);

                            if (projectBO.StartDate != null)
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, projectBO.StartDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, DBNull.Value);

                            if (projectBO.EndDate != null)
                                dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, projectBO.EndDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, projectBO.StageId);

                            if (projectBO.ProjectCompanyId != null)
                                dbSmartAspects.AddInParameter(command, "@ProjectCompanyId", DbType.Int64, projectBO.ProjectCompanyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ProjectCompanyId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@ProjectAmount", DbType.Decimal, projectBO.ProjectAmount);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, projectBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@ProjectId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            tmpProjectId = Convert.ToInt32(command.Parameters["@ProjectId"].Value);
                        }
                        if (status)
                        {
                            foreach (var costCenter in newCostCenterList)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLProjectWiseCostCenterMapping_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, costCenter.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, tmpProjectId);

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }
                        if (status && !string.IsNullOrEmpty(deletedDocuments))
                        {
                            foreach (var item in deletedDocuments.Split(','))
                            {
                                bool deleteDocument = new DocumentsDA().DeleteDocumentsByDocumentId(Convert.ToInt64(item));
                            }
                        }
                        if (status)
                        {
                            bool update = new DocumentsDA().UpdateRandomDocumentOwnwerIdWithOwnerId(tmpProjectId, randomDocumentOwnerId);
                        }
                        if (status)
                            tran.Commit();
                        else
                            tran.Rollback();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return status;
        }
        public bool UpdateGLProjectInfo(GLProjectBO projectBO, List<GLProjectWiseCostCenterMappingBO> newCostCenterList,
                                                                            List<GLProjectWiseCostCenterMappingBO> deletedCostCenterList,
                                                                            string deletedDocuments)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLProjectInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, projectBO.ProjectId);
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, projectBO.Name);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, projectBO.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, projectBO.Code);
                            dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, projectBO.ShortName);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, projectBO.Description);

                            if (projectBO.StartDate != null)
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, projectBO.StartDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, DBNull.Value);

                            if (projectBO.EndDate != null)
                                dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, projectBO.EndDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, projectBO.StageId);

                            if (projectBO.ProjectCompanyId != null)
                                dbSmartAspects.AddInParameter(command, "@ProjectCompanyId", DbType.Int64, projectBO.ProjectCompanyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ProjectCompanyId", DbType.Int64, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@ProjectAmount", DbType.Decimal, projectBO.ProjectAmount);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, projectBO.LastModifiedBy);
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                        if (status)
                        {
                            foreach (var costCenter in newCostCenterList)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLProjectWiseCostCenterMapping_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, costCenter.CostCenterId);
                                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, projectBO.ProjectId);

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }
                        if (status)
                        {
                            HMCommonDA hMCommonDA = new HMCommonDA();
                            foreach (var costCenter in deletedCostCenterList)
                            {
                                status = hMCommonDA.DeleteInfoById("GLProjectWiseCostCenterMapping", "Id", costCenter.Id);
                            }
                        }
                        if (status && !string.IsNullOrEmpty(deletedDocuments))
                        {
                            foreach (var item in deletedDocuments.Split(','))
                            {
                                bool deleteDocument = new DocumentsDA().DeleteDocumentsByDocumentId(Convert.ToInt64(item));
                            }
                        }
                        if (status)
                            tran.Commit();
                        else
                            tran.Rollback();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return status;
        }
        public GLProjectBO GetGLProjectInfoById(int ProjectId)
        {
            GLProjectBO projectBO = new GLProjectBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProjectInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, ProjectId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Discount");

                    projectBO = ds.Tables[0].AsEnumerable().Select(r => new GLProjectBO
                    {
                        CompanyId = r.Field<int>("CompanyId"),
                        GLCompany = r.Field<string>("GLCompany"),
                        ProjectId = r.Field<int>("ProjectId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        ShortName = r.Field<string>("ShortName"),
                        Description = r.Field<string>("Description"),
                        StartDate = r.Field<DateTime?>("StartDate"),
                        EndDate = r.Field<DateTime?>("EndDate"),
                        StageId = r.Field<int?>("StageId"),
                        ProjectCompanyId = r.Field<long?>("ProjectCompanyId"),
                        ProjectStage = r.Field<string>("ProjectStage"),
                        ProjectAmount = r.Field<decimal>("ProjectAmount")
                    }).FirstOrDefault();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        projectBO.CostCenters = ds.Tables[1].AsEnumerable().Select(r => new GLProjectWiseCostCenterMappingBO
                        {
                            Id = r.Field<int>("Id"),
                            ProjectId = r.Field<int>("ProjectId"),
                            CostCenterId = r.Field<int>("CostCenterId"),
                            CostCenter = r.Field<string>("CostCenter")
                        }).ToList();
                    }
                }
            }
            return projectBO;
        }
        public List<GLProjectBO> GetProjectByCompanyId(int CompanyId)
        {
            List<GLProjectBO> List = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProjectInfoByCompanyId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.Description = reader["Description"].ToString();
                                List.Add(projectBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
        public List<GLProjectBO> GetGLProjectInfoByGLCompany(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProjectInfoByGLCompany_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.Description = reader["Description"].ToString();
                                projectList.Add(projectBO);
                            }
                        }
                    }
                }
            }
            return projectList;
        }
        public List<GLProjectBO> GetGLProjectInfoByGLCompanyNUserGroup(int companyId, int UserGroupId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProjectInfoByGLCompanyNUserGroup_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@UserGropId", DbType.Int32, UserGroupId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.IsFinalStage = Convert.ToBoolean(reader["IsFinalStage"]);
                                projectList.Add(projectBO);
                            }
                        }
                    }
                }
            }
            return projectList;
        }
        public List<GLProjectBO> GetAllGLProjectInfoBySearchCriteria(string ProjectName, string ShortName, string ProjectCode, int CompanyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLProjectInfoBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(ProjectName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, ProjectName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(ProjectCode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, ProjectCode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);
                    }

                    if (CompanyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(ShortName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ShortName", DbType.String, ShortName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ShortName", DbType.String, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.Description = reader["Description"].ToString();
                                projectList.Add(projectBO);
                            }
                        }
                    }
                }
            }
            return projectList;
        }
        public List<GLProjectBO> GetAllGLProjectInfoBySearchCriteria(string ProjectName, string ShortName, string ProjectCode, int CompanyId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGLProjectInfoPagingBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(ProjectName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, ProjectName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(ProjectCode))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, ProjectCode);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, DBNull.Value);
                    }

                    if (CompanyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(ShortName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ShortName", DbType.String, ShortName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ShortName", DbType.String, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.Description = reader["Description"].ToString();
                                projectList.Add(projectBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return projectList;
        }
        public List<GLProjectBO> GetUserGroupWiseAllGLProjectInfoBySearchCriteria(int UserGroupId, string ProjectName, string ShortName, string ProjectCode, int CompanyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUserGroupWiseAllGLProjectInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, UserGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, ProjectName);
                    dbSmartAspects.AddInParameter(cmd, "@Code", DbType.String, ProjectCode);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                    dbSmartAspects.AddInParameter(cmd, "@ShortName", DbType.String, ShortName);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLProjectBO projectBO = new GLProjectBO();
                                projectBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                projectBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                projectBO.Name = reader["Name"].ToString();
                                projectBO.Code = reader["Code"].ToString();
                                projectBO.ShortName = reader["ShortName"].ToString();
                                projectBO.Description = reader["Description"].ToString();
                                projectList.Add(projectBO);
                            }
                        }
                    }
                }
            }
            return projectList;
        }
        public GLFiscalYearViewBO GetFiscalYearViewInfoByFiscalYearId(int fiscalYearId)
        {
            GLFiscalYearViewBO gLFiscalYear = new GLFiscalYearViewBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetFiscalYearViewInfoByFiscalYearId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FiscalYearId", DbType.Int64, fiscalYearId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "Discount");

                    gLFiscalYear = ds.Tables[0].AsEnumerable().Select(r => new GLFiscalYearViewBO
                    {
                        FiscalYearId = r.Field<int>("FiscalYearId"),
                        FiscalYearName = r.Field<string>("FiscalYearName"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate"),
                        CompanyId = r.Field<int>("CompanyId")
                    }).FirstOrDefault();


                    if (gLFiscalYear != null)
                    {

                        gLFiscalYear.GLFiscalYearProjects = ds.Tables[1].AsEnumerable().Select(r => new GLFiscalYearProjectMappingBO
                        {
                            Id = r.Field<int>("Id"),
                            FiscalYearId = r.Field<int>("FiscalYearId"),
                            ProjectId = r.Field<int>("ProjectId"),
                        }).ToList();

                    }


                }
            }

            return gLFiscalYear;
        }
        public bool SaveGLProjectInfo(GLProjectBO projectBO, string costCenterList, out long tmpProjectId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLProjectInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, projectBO.ProjectId);
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, projectBO.Name);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, projectBO.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@Code", DbType.String, projectBO.Code);
                            dbSmartAspects.AddInParameter(command, "@ShortName", DbType.String, projectBO.ShortName);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, projectBO.Description);

                            if (projectBO.StartDate != null)
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, projectBO.StartDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, DBNull.Value);

                            if (projectBO.EndDate != null)
                                dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, projectBO.EndDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, projectBO.StageId);

                            if (projectBO.ProjectCompanyId != null)
                                dbSmartAspects.AddInParameter(command, "@ProjectCompanyId", DbType.Int64, projectBO.ProjectCompanyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ProjectCompanyId", DbType.Int64, DBNull.Value);
                            if (projectBO.ProjectAmount != 0)
                                dbSmartAspects.AddInParameter(command, "@ProjectAmount", DbType.Decimal, projectBO.ProjectAmount);
                            else
                                 dbSmartAspects.AddInParameter(command, "@ProjectAmount", DbType.Decimal, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, projectBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@ProjectId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            tmpProjectId = Convert.ToInt32(command.Parameters["@ProjectId"].Value);
                        }
                        if (status && !string.IsNullOrEmpty(costCenterList))
                        {

                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLProjectWiseCostCenterMapping_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.String, costCenterList);
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, tmpProjectId);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                        if (status)
                            tran.Commit();
                        else
                            tran.Rollback();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return status;
        }
        public bool UpdateStage(int stageId, int Id)
        {
            Boolean status = false;
            try
            {
                string query = string.Format(" UPDATE GLProject SET StageId = {0} Where ProjectId = {1}", stageId, Id);
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        //dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, Id);
                        //dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, stageId);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }
    }
}
