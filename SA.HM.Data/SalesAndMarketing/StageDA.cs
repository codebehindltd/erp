using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class StageDA:BaseService
    {
        public Boolean SaveOrUpdateStage(SMDealStage stage, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateStage_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, stage.Id);
                        dbSmartAspects.AddInParameter(command, "@DealStage", DbType.String, stage.DealStage);
                        dbSmartAspects.AddInParameter(command, "@Complete", DbType.Decimal, stage.Complete);
                        dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, stage.ForcastType);
                        dbSmartAspects.AddInParameter(command, "@ForcastCategory", DbType.String, stage.ForcastCategory);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, stage.Description);
                        dbSmartAspects.AddInParameter(command, "@DisplaySequence", DbType.Int32, stage.DisplaySequence);
                        dbSmartAspects.AddInParameter(command, "@IsSiteSurvey", DbType.Int32, stage.IsSiteSurvey);
                        dbSmartAspects.AddInParameter(command, "@IsQuotationReveiw", DbType.Int32, stage.IsQuotationReveiw);
                        dbSmartAspects.AddInParameter(command, "@IsCloseWon", DbType.Int32, stage.IsCloseWon);
                        dbSmartAspects.AddInParameter(command, "@IsCloseLost", DbType.Boolean, stage.IsCloseLost);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, stage.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool UpdateDealStage(int dealSatgeId, int dealId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDealStage_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, dealId);
                        dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, dealSatgeId);
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

        public List<SMDealStage> GetAllStages()
        {
            List<SMDealStage> stages = new List<SMDealStage>();
            string query = string.Format("SELECT * FROM SMDealStage WHERE IsActive = '1'");

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                    DataTable Table = dataSet.Tables["Stage"];

                    stages = Table.AsEnumerable().Select(r => new SMDealStage
                    {
                        Id = r.Field<int>("Id"),
                        DealStage = r.Field<string>("DealStage"),
                        ForcastType = r.Field<string>("ForcastType"),
                        ForcastCategory = r.Field<string>("ForcastCategory"),
                        Complete = r.Field<decimal?>("Complete"),
                        DisplaySequence = r.Field<int?>("DisplaySequence")

                    }).ToList();
                }
            }
            return stages;
        }

        public List<SMDealStage> GetStagesBySearchCriteria(string name, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMDealStage> stages = new List<SMDealStage>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesStageBySearchCriteria_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@StageName", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@StageName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDealStage DealStage = new SMDealStage();

                                    DealStage.Id = Convert.ToInt32(reader["Id"]);
                                    DealStage.DealStage = (reader["DealStage"].ToString());
                                    DealStage.ForcastType =(reader["ForcastType"].ToString());
                                    DealStage.ForcastCategory = (reader["ForcastCategory"].ToString());
                                    DealStage.Complete = Convert.ToDecimal(reader["Complete"]);
                                    DealStage.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);

                                    stages.Add(DealStage);
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
            return stages;
        }

        public List<SMDealStage> GetAllDealStages()
        {
            List<SMDealStage> stages = new List<SMDealStage>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllDealStage"))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                    DataTable Table = dataSet.Tables["Stage"];

                    stages = Table.AsEnumerable().Select(r => new SMDealStage
                    {
                        Id = r.Field<int>("Id"),
                        DealStage = r.Field<string>("DealStage"),
                        ForcastType = r.Field<string>("ForcastType"),
                        ForcastCategory = r.Field<string>("ForcastCategory"),
                        Complete = r.Field<decimal?>("Complete"),
                        DisplaySequence = r.Field<int?>("DisplaySequence")

                    }).ToList();
                }
            }
            return stages;
        }

        public SMDealStage GetStageById(int id)
        {
            SMDealStage stage = new SMDealStage();
            string query = string.Format("SELECT * FROM SMDealStage WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                    DataTable Table = dataSet.Tables["Stage"];

                    stage = Table.AsEnumerable().Select(r => new SMDealStage
                    {
                        Id = r.Field<int>("Id"),
                        DealStage = r.Field<string>("DealStage"),
                        Complete = r.Field<decimal?>("Complete"),
                        ForcastType = r.Field<string>("ForcastType"),
                        ForcastCategory=r.Field<string>("ForcastCategory"),
                        DisplaySequence = r.Field<int?>("DisplaySequence"),
                        Description = r.Field<string>("Description"),
                        IsCloseWon = r.Field<bool>("IsCloseWon"),
                        IsCloseLost = r.Field<bool>("IsCloseLost"),
                        IsSiteSurvey = r.Field<bool>("IsSiteSurvey"),
                        IsQuotationReveiw = r.Field<bool>("IsQuotationReveiw")

                    }).FirstOrDefault();
                }
            }
            return stage;
        }
    }
}
