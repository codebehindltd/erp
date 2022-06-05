using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class SetupDA : BaseService
    {
        /*--------------------------------------------SourceInformation------------------------------------------------------------*/
        public Boolean SaveUpdateSourceInformation(SMSourceInformationBO sMSourceInformationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSourceInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, sMSourceInformationBO.Id);

                        if (sMSourceInformationBO.SourceName != "")
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, sMSourceInformationBO.SourceName);
                        else
                            dbSmartAspects.AddInParameter(command, "@SourceName", DbType.String, DBNull.Value);

                        if (sMSourceInformationBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, sMSourceInformationBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, sMSourceInformationBO.Status);

                        if (sMSourceInformationBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMSourceInformationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMSourceInformationBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<SMSourceInformationBO> GetSourceInformationPagination(string SourceName, Int32 status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMSourceInformationBO> SourceInformationList = new List<SMSourceInformationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSourceInformationForPaging_SP"))
                    {
                        if ((SourceName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, SourceName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Int32, status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSourceInformationBO SourceInformation = new SMSourceInformationBO();

                                    SourceInformation.Id = Convert.ToInt64(reader["Id"]);
                                    SourceInformation.SourceName = (reader["SourceName"].ToString());
                                    SourceInformationList.Add(SourceInformation);
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
            return SourceInformationList;
        }
        public SMSourceInformationBO GetSourceInformationBOById(long id)
        {
            SMSourceInformationBO SourceInformation = new SMSourceInformationBO();
            string query = string.Format("select * from SMSourceInformation  where IsDeleted= 0 AND Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    SourceInformation.Id = Convert.ToInt64(reader["Id"]);
                                    SourceInformation.SourceName = (reader["SourceName"].ToString());
                                    SourceInformation.Description = (reader["Description"].ToString());
                                    if (reader["Status"] != DBNull.Value)
                                    {
                                        SourceInformation.Status = Convert.ToBoolean(reader["Status"]);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SourceInformation;

        }
        public bool DeleteSource(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMSourceInformation WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<SMSourceInformationBO> GetSourceInfoForDDL()
        {
            List<SMSourceInformationBO> sMSources = new List<SMSourceInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSourceInfoForDDL_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMSourceInformationBO sMLife = new SMSourceInformationBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    SourceName = reader["SourceName"].ToString()
                                };
                                sMSources.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return sMSources;
        }
        public List<SMSourceInformationBO> GetSourceInfoForAutoSearch(string searchItem)
        {
            List<SMSourceInformationBO> sMSources = new List<SMSourceInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSourceInfoForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchItem", DbType.String, searchItem);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMSourceInformationBO sMLife = new SMSourceInformationBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    SourceName = reader["SourceName"].ToString()
                                };
                                sMSources.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return sMSources;
        }
        /*------------------------------------------CompanyTypeInformation----------------------------------------------------------*/
        public Boolean SaveUpdateTypeInformation(SMCompanyTypeInformationBO SMCompanyTypeInformationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCompanyTypeInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, SMCompanyTypeInformationBO.Id);

                        if (SMCompanyTypeInformationBO.TypeName != "")
                            dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, SMCompanyTypeInformationBO.TypeName);
                        else
                            dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, DBNull.Value);

                        if (SMCompanyTypeInformationBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, SMCompanyTypeInformationBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, SMCompanyTypeInformationBO.Status);

                        if (SMCompanyTypeInformationBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, SMCompanyTypeInformationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, SMCompanyTypeInformationBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<SMCompanyTypeInformationBO> GetTypeInformationPagination(string TypeName, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMCompanyTypeInformationBO> TypeInformationList = new List<SMCompanyTypeInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTypeInformationForPaging_SP"))
                    {

                        if ((TypeName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, TypeName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TypeName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMCompanyTypeInformationBO TypeInformation = new SMCompanyTypeInformationBO();

                                    TypeInformation.Id = Convert.ToInt64(reader["Id"]);
                                    TypeInformation.TypeName = (reader["TypeName"].ToString());

                                    TypeInformationList.Add(TypeInformation);
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
            return TypeInformationList;
        }
        public SMCompanyTypeInformationBO GetTypeInformationById(long id)
        {
            SMCompanyTypeInformationBO TypeInformation = new SMCompanyTypeInformationBO();
            string query = string.Format("select * from SMCompanyTypeInformation  where IsDeleted= 0 AND Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    TypeInformation.Id = Convert.ToInt64(reader["Id"]);
                                    TypeInformation.TypeName = (reader["TypeName"].ToString());
                                    TypeInformation.Description = (reader["Description"].ToString());
                                    if (reader["Status"] != DBNull.Value)
                                    {
                                        TypeInformation.Status = Convert.ToBoolean(reader["Status"]);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TypeInformation;

        }
        public bool DeleteType(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMCompanyTypeInformation WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        /*--------------------------------------------SegmentInformation------------------------------------------------------------*/
        public Boolean SaveUpdateSegmentInformation(SMSegmentInformationBO sMSegmentInformationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSegmentInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, sMSegmentInformationBO.Id);

                        if (sMSegmentInformationBO.SegmentName != "")
                            dbSmartAspects.AddInParameter(command, "@SegmentName", DbType.String, sMSegmentInformationBO.SegmentName);
                        else
                            dbSmartAspects.AddInParameter(command, "@SegmentName", DbType.String, DBNull.Value);

                        if (sMSegmentInformationBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, sMSegmentInformationBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, sMSegmentInformationBO.Status);

                        if (sMSegmentInformationBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMSegmentInformationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMSegmentInformationBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<SMSegmentInformationBO> GetSegmentInformationPagination(string SegmentName, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMSegmentInformationBO> SegmentInformationList = new List<SMSegmentInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSegmentInformationForPaging_SP"))
                    {

                        if ((SegmentName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@SegmentName", DbType.String, SegmentName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SegmentName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSegmentInformationBO SegmentInformation = new SMSegmentInformationBO();

                                    SegmentInformation.Id = Convert.ToInt64(reader["Id"]);
                                    SegmentInformation.SegmentName = (reader["SegmentName"].ToString());

                                    SegmentInformationList.Add(SegmentInformation);
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
            return SegmentInformationList;
        }
        public List<SMSegmentInformationBO> GetAllSegment()
        {
            List<SMSegmentInformationBO> SegmentInformationList = new List<SMSegmentInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllSegment_SP"))
                    {

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSegmentInformationBO segmentInformation = new SMSegmentInformationBO();

                                    segmentInformation.Id = Convert.ToInt64(reader["Id"]);
                                    segmentInformation.SegmentName = (reader["SegmentName"].ToString());

                                    SegmentInformationList.Add(segmentInformation);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SegmentInformationList;
        }

        public SMSegmentInformationBO GetSegmentInformationBOById(long id)
        {
            SMSegmentInformationBO SegmentInformation = new SMSegmentInformationBO();
            string query = string.Format("select * from SMSegmentInformation  where IsDeleted= 0 AND Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    SegmentInformation.Id = Convert.ToInt64(reader["Id"]);
                                    SegmentInformation.SegmentName = (reader["SegmentName"].ToString());
                                    SegmentInformation.Description = (reader["Description"].ToString());
                                    SegmentInformation.Status = Convert.ToBoolean(reader["Status"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SegmentInformation;

        }
        public bool DeleteSegment(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMSegmentInformation WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<SMSegmentInformationBO> GetAllSegmentInformation()
        {
            List<SMSegmentInformationBO> SegmentList = new List<SMSegmentInformationBO>();
            string query = string.Format("select * from SMSegmentInformation WHERE IsDeleted = 0");

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSegmentInformationBO Segment = new SMSegmentInformationBO();
                                    Segment.Id = Convert.ToInt64(reader["Id"]);
                                    Segment.SegmentName = (reader["SegmentName"].ToString());
                                    Segment.Status = Convert.ToBoolean(reader["Status"]);
                                    SegmentList.Add(Segment);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SegmentList;

        }
        /*--------------------------------------DealProbabilityStageInformation-----------------------------------------------------*/
        public Boolean SaveUpdateProbabilityInformation(SMDealProbabilityStageInformationBO SMDealProbabilityStageInformationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateProbabilityInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, SMDealProbabilityStageInformationBO.Id);

                        if (SMDealProbabilityStageInformationBO.ProbabilityStage != "")
                            dbSmartAspects.AddInParameter(command, "@ProbabilityStage", DbType.String, SMDealProbabilityStageInformationBO.ProbabilityStage);
                        else
                            dbSmartAspects.AddInParameter(command, "@ProbabilityStage", DbType.String, DBNull.Value);

                        if (SMDealProbabilityStageInformationBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, SMDealProbabilityStageInformationBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, SMDealProbabilityStageInformationBO.Status);

                        if (SMDealProbabilityStageInformationBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, SMDealProbabilityStageInformationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, SMDealProbabilityStageInformationBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<SMDealProbabilityStageInformationBO> GetProbabilityInformationPagination(string ProbabilityStage, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMDealProbabilityStageInformationBO> ProbabilityInformationList = new List<SMDealProbabilityStageInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProbabilityInformationForPaging_SP"))
                    {

                        if ((ProbabilityStage) != "")
                            dbSmartAspects.AddInParameter(cmd, "@ProbabilityStage", DbType.String, ProbabilityStage);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ProbabilityStage", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDealProbabilityStageInformationBO ProbabilityInformation = new SMDealProbabilityStageInformationBO();

                                    ProbabilityInformation.Id = Convert.ToInt64(reader["Id"]);
                                    ProbabilityInformation.ProbabilityStage = (reader["ProbabilityStage"].ToString());

                                    ProbabilityInformationList.Add(ProbabilityInformation);
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
            return ProbabilityInformationList;
        }
        public SMDealProbabilityStageInformationBO GetProbabilityInformationBOById(long id)
        {
            SMDealProbabilityStageInformationBO ProbabilityInformation = new SMDealProbabilityStageInformationBO();
            string query = string.Format("select * from SMDealProbabilityStageInformation  where IsDeleted= 0 AND Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    ProbabilityInformation.Id = Convert.ToInt64(reader["Id"]);
                                    ProbabilityInformation.ProbabilityStage = (reader["ProbabilityStage"].ToString());
                                    ProbabilityInformation.Description = (reader["Description" +
                                        ""].ToString());
                                    ProbabilityInformation.Status = Convert.ToBoolean(reader["Status"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ProbabilityInformation;

        }
        public bool DeleteProbability(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMDealProbabilityStageInformation WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<SMDealProbabilityStageInformationBO> GetAllProbabilityStage()
        {
            List<SMDealProbabilityStageInformationBO> ProbabilityInformationList = new List<SMDealProbabilityStageInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllProbabilityStage_SP"))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDealProbabilityStageInformationBO ProbabilityInformation = new SMDealProbabilityStageInformationBO();

                                    ProbabilityInformation.Id = Convert.ToInt64(reader["Id"]);
                                    ProbabilityInformation.ProbabilityStage = (reader["ProbabilityStage"].ToString());

                                    ProbabilityInformationList.Add(ProbabilityInformation);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ProbabilityInformationList;
        }
        /*--------------------------------------------OwnershipInformation------------------------------------------------------------*/
        public Boolean SaveUpdateOwnershipInformation(SMOwnershipInformationBO sMOwnershipInformationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateOwnershipInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, sMOwnershipInformationBO.Id);

                        if (sMOwnershipInformationBO.OwnershipName != "")
                            dbSmartAspects.AddInParameter(command, "@OwnershipName", DbType.String, sMOwnershipInformationBO.OwnershipName);
                        else
                            dbSmartAspects.AddInParameter(command, "@OwnershipName", DbType.String, DBNull.Value);

                        if (sMOwnershipInformationBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, sMOwnershipInformationBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, sMOwnershipInformationBO.Status);

                        if (sMOwnershipInformationBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMOwnershipInformationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMOwnershipInformationBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<SMOwnershipInformationBO> GetOwnershipInformationPagination(string OwnershipName, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMOwnershipInformationBO> OwnershipInformationList = new List<SMOwnershipInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOwnershipInformationForPaging_SP"))
                    {

                        if ((OwnershipName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@OwnershipName", DbType.String, OwnershipName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@OwnershipName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMOwnershipInformationBO OwnershipInformation = new SMOwnershipInformationBO();

                                    OwnershipInformation.Id = Convert.ToInt64(reader["Id"]);
                                    OwnershipInformation.OwnershipName = (reader["OwnershipName"].ToString());

                                    OwnershipInformationList.Add(OwnershipInformation);
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
            return OwnershipInformationList;
        }
        public SMOwnershipInformationBO GetOwnershipInformationBOById(long id)
        {
            SMOwnershipInformationBO OwnershipInformation = new SMOwnershipInformationBO();
            string query = string.Format("select * from SMOwnershipInformation  where IsDeleted= 0 AND Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    OwnershipInformation.Id = Convert.ToInt64(reader["Id"]);
                                    OwnershipInformation.OwnershipName = (reader["OwnershipName"].ToString());
                                    OwnershipInformation.Description = (reader["Description" +
                                        ""].ToString());
                                    OwnershipInformation.Status = Convert.ToBoolean(reader["Status"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return OwnershipInformation;

        }
        public bool DeleteOwnership(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMOwnershipInformation WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        /*--------------------------------------------OwnershipInformation------------------------------------------------------------*/
        public Boolean SaveUpdateContactTitle(ContactTitleBO sContactTitleBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUpdateContactTitle_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, sContactTitleBO.Id);

                        if (sContactTitleBO.Title != "")
                            dbSmartAspects.AddInParameter(command, "@Title", DbType.String, sContactTitleBO.Title);
                        else
                            dbSmartAspects.AddInParameter(command, "@Title", DbType.String, DBNull.Value);

                        if (sContactTitleBO.TransectionType != "")
                            dbSmartAspects.AddInParameter(command, "@TransectionType", DbType.String, sContactTitleBO.TransectionType);
                        else
                            dbSmartAspects.AddInParameter(command, "@TransectionType", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, sContactTitleBO.Status);

                        if (sContactTitleBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sContactTitleBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sContactTitleBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<ContactTitleBO> GetContactTitleSearchForPagination(string title, string transectionType, Int32 status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ContactTitleBO> SourceInformationList = new List<ContactTitleBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactTitleSearchForPagination_SP"))
                    {
                        if ((title) != "")
                            dbSmartAspects.AddInParameter(cmd, "@Title", DbType.String, title);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Title", DbType.String, DBNull.Value);

                        if ((transectionType) != "0")
                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, transectionType);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Int32, status);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactTitleBO SourceInformation = new ContactTitleBO();

                                    SourceInformation.Id = Convert.ToInt64(reader["Id"]);
                                    SourceInformation.Title = (reader["Title"].ToString());
                                    SourceInformation.TransectionType = (reader["TransectionType"].ToString());
                                    SourceInformation.Status = Convert.ToBoolean(reader["Status"]);
                                    SourceInformation.ActiveStatus = (reader["ActiveStatus"].ToString());
                                    SourceInformationList.Add(SourceInformation);
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
            return SourceInformationList;
        }
        public ContactTitleBO GetContactTitleInformationById(long id)
        {
            ContactTitleBO SourceInformation = new ContactTitleBO();
            string query = string.Format("SELECT * FROM SMContactDetailsTitle  WHERE Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    SourceInformation.Id = Convert.ToInt64(reader["Id"]);
                                    SourceInformation.Title = (reader["Title"].ToString());
                                    SourceInformation.TransectionType = (reader["TransectionType"].ToString());
                                    if (reader["Status"] != DBNull.Value)
                                    {
                                        SourceInformation.Status = Convert.ToBoolean(reader["Status"]);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SourceInformation;

        }
        public bool DeleteContactTitle(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMContactDetailsTitle WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
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
