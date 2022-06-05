using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class ActivityLogsDA : BaseService
    {

        public bool SaveActivityLogInformation(ActivityLogsBO activityLog)
        {
            Boolean status = false;
            int logId;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveActivityLogInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ActivityType", DbType.String, activityLog.ActivityType);
                    dbSmartAspects.AddInParameter(command, "@EntityType", DbType.String, activityLog.EntityType);
                    dbSmartAspects.AddInParameter(command, "@EntityId", DbType.Int32, activityLog.EntityId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, activityLog.Remarks);
                    dbSmartAspects.AddInParameter(command, "@Module", DbType.String, activityLog.Module);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, activityLog.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ActivityId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    logId = Convert.ToInt32(command.Parameters["@ActivityId"].Value);
                }
            }
            return status;
        }
        public bool SaveActivityLogInformation(ActivityLogsBO activityLog, out long tempId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveActivityLogInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ActivityType", DbType.String, activityLog.ActivityType);
                    dbSmartAspects.AddInParameter(command, "@EntityType", DbType.String, activityLog.EntityType);
                    dbSmartAspects.AddInParameter(command, "@EntityId", DbType.Int32, activityLog.EntityId);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, activityLog.Remarks);
                    dbSmartAspects.AddInParameter(command, "@Module", DbType.String, activityLog.Module);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, activityLog.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ActivityId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tempId = Convert.ToInt32(command.Parameters["@ActivityId"].Value);
                }
            }
            return status;
        }
        public bool SaveActivityLogDetails(ActivityLogDetailsBO activityLog, out long tempId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveActivityLogDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ActivityId", DbType.Int64, activityLog.ActivityId);
                    dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, activityLog.FieldName);
                    dbSmartAspects.AddInParameter(command, "@PreviousData", DbType.String, activityLog.PreviousData);
                    dbSmartAspects.AddInParameter(command, "@CurrentData", DbType.String, activityLog.CurrentData);
                    dbSmartAspects.AddInParameter(command, "@DetailDescription", DbType.String, activityLog.DetailDescription);

                    dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tempId = Convert.ToInt32(command.Parameters["@Id"].Value);
                }
            }
            return status;
        }
        public List<ActivityLogDetailsBO> GetSelectedFieldsByFormName(string formName)
        {
            List<ActivityLogDetailsBO> bOs = new List<ActivityLogDetailsBO>();
            string query = string.Format("SELECT * from ActivityLogDetailsSetup  WHERE IsSaveActivity = 1 AND PageId = '{0}'", formName);
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
                                    ActivityLogDetailsBO logDetailsBO = new ActivityLogDetailsBO()
                                    {
                                        FormName = (reader["PageId"].ToString()),
                                        FieldName = (reader["FieldName"].ToString()),
                                        IsSaveActivity = Convert.ToBoolean(reader["IsSaveActivity"])
                                    };
                                    bOs.Add(logDetailsBO);

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
            return bOs;
        }
    }
}
