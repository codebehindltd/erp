using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLFiscalYearDA : BaseService
    {
        public bool SaveOrUpdateGLFiscalYear(GLFiscalYearBO fiscalYearBO, string projectIdList, out int tmpFiscalYearId)
        {
            bool status = false;
            tmpFiscalYearId = 0;

            if (fiscalYearBO.FiscalYearId == 0 || fiscalYearBO.FiscalYearId.ToString() == "")
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLFiscalYear_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, projectIdList);
                        dbSmartAspects.AddInParameter(command, "@FiscalYearName", DbType.String, fiscalYearBO.FiscalYearName);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fiscalYearBO.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, fiscalYearBO.ToDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, fiscalYearBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@FiscalYearId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpFiscalYearId = Convert.ToInt32(command.Parameters["@FiscalYearId"].Value);
                    }
                }
            }
            else
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLFiscalYear_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FiscalYearId", DbType.Int32, fiscalYearBO.FiscalYearId);
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, projectIdList);
                        dbSmartAspects.AddInParameter(command, "@FiscalYearName", DbType.String, fiscalYearBO.FiscalYearName);
                        dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fiscalYearBO.FromDate);
                        dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, fiscalYearBO.ToDate);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, fiscalYearBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public GLFiscalYearBO GetFiscalYearInfoByProjectId(int projectId)
        {
            GLFiscalYearBO fiscalYearBO = new GLFiscalYearBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLFiscalYearInfoByProjectId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                fiscalYearBO.FiscalYearId = Convert.ToInt32(reader["FiscalYearId"]);
                                fiscalYearBO.FiscalYearName = reader["FiscalYearName"].ToString();
                                fiscalYearBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                fiscalYearBO.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                fiscalYearBO.ToDate = Convert.ToDateTime(reader["ToDate"]);
                                fiscalYearBO.ReportFromDate = reader["FromDateForClientSideShow"].ToString();
                                fiscalYearBO.ReportToDate = reader["TodateForClientSideShow"].ToString();
                                fiscalYearBO.FromDateForClientSideShow = reader["FromDateForClientSideShow"].ToString();
                                fiscalYearBO.TodateForClientSideShow = reader["TodateForClientSideShow"].ToString();
                            }
                        }
                    }
                }
            }
            return fiscalYearBO;
        }
        public List<GLFiscalYearBO> GetAllFiscalYear()
        {
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllFiscalYear_SP"))
                {
                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "FiscalYear");
                    DataTable Table = CompanyDS.Tables["FiscalYear"];

                    fiscalYearList = Table.AsEnumerable().Select(r => new GLFiscalYearBO
                    {
                        FiscalYearId = r.Field<int>("FiscalYearId"),
                        FiscalYearName = r.Field<string>("FiscalYearName"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate")
                    }).ToList();
                }
            }
            return fiscalYearList;
        }
        public List<GLFiscalYearBO> GetFiscalYearListByProjectId(int projectId)
        {
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFiscalYearListByProjectId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "FiscalYear");
                    DataTable Table = CompanyDS.Tables["FiscalYear"];

                    fiscalYearList = Table.AsEnumerable().Select(r => new GLFiscalYearBO
                    {
                        FiscalYearId = r.Field<int>("FiscalYearId"),
                        FiscalYearName = r.Field<string>("FiscalYearName"),
                        FromDate = r.Field<DateTime>("FromDate"),
                        ToDate = r.Field<DateTime>("ToDate")
                    }).ToList();
                }
            }
            return fiscalYearList;
        }
        public GLFiscalYearBO GetFiscalYearId(int fiscalYearId)
        {
            GLFiscalYearBO fiscalYearBO = new GLFiscalYearBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLFiscalYearId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, fiscalYearId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                fiscalYearBO.FiscalYearId = Convert.ToInt32(reader["FiscalYearId"]);
                                fiscalYearBO.FiscalYearName = reader["FiscalYearName"].ToString();
                                fiscalYearBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                fiscalYearBO.ReportFromDate = reader["ReportFromDate"].ToString();
                                fiscalYearBO.ReportToDate = reader["ReportToDate"].ToString();
                                fiscalYearBO.FromDate = Convert.ToDateTime(reader["FromDate"]);
                                fiscalYearBO.ToDate = Convert.ToDateTime(reader["ToDate"]);
                            }
                        }
                    }
                }
            }
            return fiscalYearBO;
        }

        public GLFiscalYearBO GetFiscalYearByFiscalYearClosingFlag(bool isFiscalYearClosed)
        {
            GLFiscalYearBO fiscalYear = new GLFiscalYearBO();

            string query = "select FiscalYearId from GLFiscalYearClosingMaster where IsFiscalYearClosed = " + (isFiscalYearClosed == true ? 1 : 0);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "FiscalYear");
                    DataTable Table = CompanyDS.Tables["FiscalYear"];

                    fiscalYear = Table.AsEnumerable().Select(r => new GLFiscalYearBO
                    {
                        FiscalYearId = r.Field<int>("FiscalYearId")
                    }).FirstOrDefault();
                }
            }
            return fiscalYear;
        }

        public GLFiscalYearBO GetFiscalClosingMasterByFiscalYearId(int fiscalYearId)
        {
            GLFiscalYearBO fiscalYear = new GLFiscalYearBO();

            string query = "select FiscalYearId from GLFiscalYearClosingMaster where FiscalYearId = " + fiscalYearId;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet CompanyDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, CompanyDS, "FiscalYear");
                    DataTable Table = CompanyDS.Tables["FiscalYear"];

                    fiscalYear = Table.AsEnumerable().Select(r => new GLFiscalYearBO
                    {
                        FiscalYearId = r.Field<int>("FiscalYearId")
                    }).FirstOrDefault();
                }
            }
            return fiscalYear;
        }

        public bool FiscalYearClosingBalanceProcessing(int fiscalYearId, Int32 companyId, Int32 projectId, Int32 donorId, int createdBy)
        {
            bool status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("FiscalYearClosingProcessing_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FiscalYearId", DbType.Int32, fiscalYearId);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                        if (projectId != 0)
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                        if (donorId != 0)
                            dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                        else
                            dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);

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
    }
}
