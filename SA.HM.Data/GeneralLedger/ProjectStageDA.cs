using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.TaskManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.GeneralLedger
{
    public class ProjectStageDA : BaseService
    {
        public Boolean SaveOrUpdateProjectStage(ProjectStageBO stage, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateProjectStage_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, stage.Id);
                        dbSmartAspects.AddInParameter(command, "@ProjectStage", DbType.String, stage.ProjectStage);
                        dbSmartAspects.AddInParameter(command, "@Complete", DbType.Decimal, stage.Complete);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, stage.Description);
                        dbSmartAspects.AddInParameter(command, "@DisplaySequence", DbType.Int32, stage.DisplaySequence);
                        dbSmartAspects.AddInParameter(command, "@IsFinalStage", DbType.Boolean, stage.IsFinalStage);
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
        public List<ProjectStageBO> GetStagesBySearchCriteria(string name, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ProjectStageBO> stages = new List<ProjectStageBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProjectStageBySearchCriteria_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@ProjectName", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ProjectName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ProjectStageBO TaskStage = new ProjectStageBO();

                                    TaskStage.Id = Convert.ToInt32(reader["Id"]);
                                    TaskStage.ProjectStage = (reader["ProjectStage"].ToString());                                    
                                    TaskStage.Complete = Convert.ToDecimal(reader["Complete"]);
                                    TaskStage.Description = (reader["Description"].ToString());
                                    TaskStage.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);

                                    stages.Add(TaskStage);
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
        public ProjectStageBO GetStageById(int id)
        {
            ProjectStageBO stage = new ProjectStageBO();
            string query = string.Format("SELECT * FROM GLProjectStage WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                    DataTable Table = dataSet.Tables["Stage"];

                    stage = Table.AsEnumerable().Select(r => new ProjectStageBO
                    {
                        Id = r.Field<int>("Id"),
                        ProjectStage = r.Field<string>("ProjectStage"),
                        Complete = r.Field<decimal?>("Complete"),                        
                        DisplaySequence = r.Field<int?>("DisplaySequence"),
                        Description = r.Field<string>("Description"),
                        IsFinalStage = r.Field<bool>("IsFinalStage")
                    }).FirstOrDefault();
                }
            }
            return stage;
        }
        public List<ProjectStageBO> GetAllProjectStages()
        {
            List<ProjectStageBO> stages = new List<ProjectStageBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllProjectStage_SP"))
                {
                    DataSet dataSet = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                    DataTable Table = dataSet.Tables["Stage"];

                    stages = Table.AsEnumerable().Select(r => new ProjectStageBO
                    {
                        Id = r.Field<int>("Id"),
                        ProjectStage = r.Field<string>("ProjectStage"),                        
                        Complete = r.Field<decimal?>("Complete"),
                        DisplaySequence = r.Field<int?>("DisplaySequence")
                    }).ToList();
                }
            }
            return stages;
        }
    }
}
