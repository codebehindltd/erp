using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HMCommon
{
    public class TermsNConditionsDA : BaseService
    {
        public bool SaveOrUpdateTermsNConditions(TermsNConditionsMasterBO TermsNConditionsMasterBO, string ConditionForList, out long id)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateTermsNConditions_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, TermsNConditionsMasterBO.Id);
                            dbSmartAspects.AddInParameter(command, "@Title", DbType.String, TermsNConditionsMasterBO.Title);
                            dbSmartAspects.AddInParameter(command, "@DisplaySequence", DbType.Int32, TermsNConditionsMasterBO.DisplaySequence);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, TermsNConditionsMasterBO.Description);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            status = (dbSmartAspects.ExecuteNonQuery(command, transction) > 0);

                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        }
                        if (status)
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateTermsNConditionsDetails_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@TermsNConditionsId", DbType.Int64, id);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ConditionForList", DbType.String, ConditionForList);

                                status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                            }
                        }
                        if (status)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<TermsNConditionsMasterBO> GetTermsNConditionsPagination(string title, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<TermsNConditionsMasterBO> TermsNConditionsList = new List<TermsNConditionsMasterBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTermsNConditionsForPaging_SP"))
                    {

                        if ((title) != "")
                            dbSmartAspects.AddInParameter(cmd, "@Title", DbType.String, title);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Title", DbType.String, DBNull.Value);

                        //dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);


                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TermsNConditionsMasterBO TermsNConditions = new TermsNConditionsMasterBO();

                                    TermsNConditions.Id = Convert.ToInt64(reader["Id"]);
                                    TermsNConditions.Title = (reader["Title"].ToString());
                                    TermsNConditions.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);
                                    TermsNConditions.Description = (reader["Description"].ToString());

                                    TermsNConditionsList.Add(TermsNConditions);
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
            return TermsNConditionsList;
        }

        public TermsNConditionsMasterBO GetTermsNConditionsById(long id)
        {
            TermsNConditionsMasterBO TermsNConditions = new TermsNConditionsMasterBO();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            string query = string.Format("SELECT * FROM TermsNConditionsMaster WHERE Id = {0}", id);

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
                                TermsNConditions.Id = Convert.ToInt64(reader["Id"]);
                                TermsNConditions.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);
                                TermsNConditions.Title = (reader["Title"].ToString());
                                TermsNConditions.Description = (reader["Description"].ToString());
                            }
                        }
                    }
                }
            }
            return TermsNConditions;
        }
        public List<long> GetTermsNConditionsDetailsByMasterId(long id)
        {
            List<long> DetailsList = new List<long>();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            string query = string.Format("SELECT ConditionForID FROM TermsNConditionsDetails WHERE TermsNConditionsId = {0}", id);

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

                                    DetailsList.Add(Convert.ToInt32(reader["ConditionForID"]));
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

            return DetailsList;
        }
        public List<TermsNConditionsMasterBO> GetTermsNConditionsByType(string Type)
        {
            List<TermsNConditionsMasterBO> TermsNConditionsList = new List<TermsNConditionsMasterBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTermsNConditionsByType_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, Type);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    TermsNConditionsMasterBO TermsNConditions = new TermsNConditionsMasterBO();

                                    TermsNConditions.Id = Convert.ToInt64(reader["Id"]);
                                    TermsNConditions.Title = (reader["Title"].ToString());
                                    TermsNConditions.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);
                                    TermsNConditions.Description = (reader["Description"].ToString());

                                    TermsNConditionsList.Add(TermsNConditions);
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
            return TermsNConditionsList;
        }

    }
}
