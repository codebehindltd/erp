using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class CallToActionDA : BaseService
    {
        public Boolean SaveSalesCallInfo(CallToActionBO callToAction, List<CallToActionDetailBO> callToActionDetailList, List<CallToActionDetailBO> callToActionDetailDeletedList, out long OutId)
        {
            Boolean status = false;
            int CallToActionDetailsId = 0;
            int TaskOutId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCallToAction_SP"))
                    {

                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, callToAction.Id);
                        dbSmartAspects.AddInParameter(command, "@MasterId", DbType.Int64, callToAction.MasterId);
                        dbSmartAspects.AddInParameter(command, "@FromCallToAction", DbType.String, callToAction.FromCallToAction);
                        dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, callToAction.ContactId);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, callToAction.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, callToAction.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }

                    if (status && callToActionDetailList.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveCallToActionDetails_SP"))
                        {
                            foreach (CallToActionDetailBO detailBO in callToActionDetailList)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, detailBO.Id);
                                dbSmartAspects.AddInParameter(cmd, "@CallToActionId", DbType.Int64, OutId);
                                dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, detailBO.CompanyId);
                                dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, detailBO.ContactId);
                                dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, detailBO.Type);
                                dbSmartAspects.AddInParameter(cmd, "@Date", DbType.DateTime, detailBO.Date);
                                dbSmartAspects.AddInParameter(cmd, "@Time", DbType.DateTime, detailBO.Time);
                                dbSmartAspects.AddInParameter(cmd, "@OtherActivities", DbType.String, detailBO.OtherActivities);
                                dbSmartAspects.AddInParameter(cmd, "@Description", DbType.String, detailBO.Description);
                                dbSmartAspects.AddInParameter(cmd, "@PerticipentFromOffice", DbType.String, detailBO.PerticipentFromOffice);
                                dbSmartAspects.AddInParameter(cmd, "@PerticipentFromClient", DbType.String, detailBO.PerticipentFromClient);
                                dbSmartAspects.AddInParameter(cmd, "@ReminderDayList", DbType.String, detailBO.ReminderDayList);
                                dbSmartAspects.AddOutParameter(cmd, "@OutDetailsId", DbType.Int32, sizeof(Int32));
                                status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                                CallToActionDetailsId = Convert.ToInt32(cmd.Parameters["@OutDetailsId"].Value);
                                if (status)
                                {
                                    using (DbCommand cmnd = dbSmartAspects.GetStoredProcCommand("CreateTaskForCallToAction_SP"))
                                    {
                                        cmnd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmnd, "@CallToActionDetailsId", DbType.Int64, CallToActionDetailsId);
                                        dbSmartAspects.AddInParameter(cmnd, "@Type", DbType.String, detailBO.Type);
                                        dbSmartAspects.AddInParameter(cmnd, "@TaskName", DbType.String, detailBO.TaskName);
                                        dbSmartAspects.AddInParameter(cmnd, "@Date", DbType.DateTime, detailBO.Date);
                                        dbSmartAspects.AddInParameter(cmnd, "@Time", DbType.DateTime, detailBO.Time);
                                        dbSmartAspects.AddInParameter(cmnd, "@OtherActivities", DbType.String, detailBO.OtherActivities);
                                        dbSmartAspects.AddInParameter(cmnd, "@Description", DbType.String, detailBO.Description);
                                        dbSmartAspects.AddInParameter(cmnd, "@PerticipentFromOffice", DbType.String, detailBO.TaskAssignedEmployee);

                                        status = dbSmartAspects.ExecuteNonQuery(cmnd) > 0 ? true : false;

                                    }
                                }
                            }
                        }
                    }
                    if (status && callToActionDetailDeletedList.Count > 0)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteCallToActionDetails_SP"))
                        {
                            foreach (CallToActionDetailBO detailBO in callToActionDetailList)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, detailBO.Id);
                                status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                            }
                        }
                    }
                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }
        public Boolean UpdateCallToActionDetails(CallToActionDetailBO callToActionDetailList, out long CallToActionDetailsId)
        {
            Boolean status = false;
            int TaskOutId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {

                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveCallToActionDetails_SP"))
                    {

                        cmd.Parameters.Clear();

                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, callToActionDetailList.Id);
                        dbSmartAspects.AddInParameter(cmd, "@CallToActionId", DbType.Int64, callToActionDetailList.CallToActionId);
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, callToActionDetailList.CompanyId);
                        dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, callToActionDetailList.ContactId);
                        dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, callToActionDetailList.Type);
                        dbSmartAspects.AddInParameter(cmd, "@Date", DbType.DateTime, callToActionDetailList.Date);
                        dbSmartAspects.AddInParameter(cmd, "@Time", DbType.DateTime, callToActionDetailList.Time);
                        dbSmartAspects.AddInParameter(cmd, "@OtherActivities", DbType.String, callToActionDetailList.OtherActivities);
                        dbSmartAspects.AddInParameter(cmd, "@Description", DbType.String, callToActionDetailList.Description);
                        dbSmartAspects.AddInParameter(cmd, "@PerticipentFromOffice", DbType.String, callToActionDetailList.PerticipentFromOffice);
                        dbSmartAspects.AddInParameter(cmd, "@PerticipentFromClient", DbType.String, callToActionDetailList.PerticipentFromClient);
                        dbSmartAspects.AddInParameter(cmd, "@ReminderDayList", DbType.String, callToActionDetailList.ReminderDayList);
                        dbSmartAspects.AddOutParameter(cmd, "@OutDetailsId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                        CallToActionDetailsId = Convert.ToInt32(cmd.Parameters["@OutDetailsId"].Value);
                        if (status)
                        {
                            using (DbCommand cmnd = dbSmartAspects.GetStoredProcCommand("CreateTaskForCallToAction_SP"))
                            {
                                cmnd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmnd, "@CallToActionDetailsId", DbType.Int64, CallToActionDetailsId);
                                dbSmartAspects.AddInParameter(cmnd, "@Type", DbType.String, callToActionDetailList.Type);
                                dbSmartAspects.AddInParameter(cmnd, "@TaskName", DbType.String, callToActionDetailList.TaskName);
                                dbSmartAspects.AddInParameter(cmnd, "@Date", DbType.DateTime, callToActionDetailList.Date);
                                dbSmartAspects.AddInParameter(cmnd, "@Time", DbType.DateTime, callToActionDetailList.Time);
                                dbSmartAspects.AddInParameter(cmnd, "@OtherActivities", DbType.String, callToActionDetailList.OtherActivities);
                                dbSmartAspects.AddInParameter(cmnd, "@Description", DbType.String, callToActionDetailList.Description);
                                dbSmartAspects.AddInParameter(cmnd, "@PerticipentFromOffice", DbType.String, callToActionDetailList.TaskAssignedEmployee);

                                status = dbSmartAspects.ExecuteNonQuery(cmnd) > 0 ? true : false;

                            }
                        }
                    }
                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }                
            }
            return status;
        }
        public List<CallToActionDetailBO> GetCallToActionPagination(string type, string taskName, string fromDate, string toDate, string company, string contact, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<CallToActionDetailBO> CallToActionBOList = new List<CallToActionDetailBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCallToActionForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(taskName))
                            dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, taskName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(type))
                            dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, type);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(fromDate))
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                        if (!string.IsNullOrEmpty(toDate))
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                        if (!string.IsNullOrEmpty(company))
                            dbSmartAspects.AddInParameter(cmd, "@Company", DbType.String, company);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Company", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contact))
                            dbSmartAspects.AddInParameter(cmd, "@Contact", DbType.String, contact);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Contact", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    CallToActionDetailBO CallToAction = new CallToActionDetailBO();

                                    CallToAction.Id = Convert.ToInt64(reader["Id"]);
                                    CallToAction.CompanyId = Convert.ToInt64(reader["CompanyId"]);
                                    CallToAction.ContactId = Convert.ToInt64(reader["ContactId"]);
                                    CallToAction.CallToActionId = Convert.ToInt64(reader["CallToActionId"]);
                                    CallToAction.Type = (reader["Type"].ToString());
                                    CallToAction.Date = Convert.ToDateTime(reader["Date"]);
                                    CallToAction.Time = Convert.ToDateTime(currentDate + " " + reader["Time"]);
                                    CallToAction.OtherActivities = (reader["OtherActivities"].ToString());
                                    CallToAction.Description = (reader["Description"].ToString());
                                    CallToAction.TaskName = (reader["TaskName"].ToString());
                                    CallToAction.ReminderDayList = (reader["ReminderDayList"].ToString());
                                    CallToAction.TaskAssignedEmployeeName = (reader["TaskAssignedEmployeeName"].ToString());
                                    CallToAction.PerticipentFromClientName = (reader["PerticipentFromClientName"].ToString());
                                    CallToAction.PerticipentFromOfficeName = (reader["PerticipentFromOfficeName"].ToString());
                                    CallToAction.CompanyName = (reader["CompanyName"].ToString());
                                    CallToAction.ContactName = (reader["ContactName"].ToString());
                                    CallToActionBOList.Add(CallToAction);
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
            return CallToActionBOList;
        }
        public CallToActionBO GetCallToActionId(long id)
        {
            CallToActionBO CallToAction = new CallToActionBO();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            //string query = string.Format("SELECT * FROM SMTask WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCallToActionById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CallToAction.Id = Convert.ToInt64(reader["Id"]);
                                CallToAction.MasterId = Convert.ToInt64(reader["MasterId"]);
                                CallToAction.ContactId = Convert.ToInt64(reader["ContactId"]);
                                CallToAction.CompanyId = Convert.ToInt64(reader["CompanyId"]);
                                CallToAction.CompanyName = reader["CompanyName"].ToString();
                                CallToAction.ContactName = reader["ContactName"].ToString();
                                CallToAction.FromCallToAction = reader["FromCallToAction"].ToString();
                            }
                        }
                    }

                }
            }
            return CallToAction;
        }
        public CallToActionDetailViewBO GetCallToActionDetailsById(long id)
        {
            CallToActionDetailViewBO CallToAction = new CallToActionDetailViewBO();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            //string query = string.Format("SELECT * FROM SMTask WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCallToActionDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CallToAction.Id = Convert.ToInt64(reader["Id"]);
                                CallToAction.CallToActionId = Convert.ToInt64(reader["CallToActionId"]);
                                CallToAction.CompanyId = Convert.ToInt64(reader["CompanyId"]);
                                CallToAction.TaskId = Convert.ToInt64(reader["TaskId"]);
                                CallToAction.CompanyName = (reader["CompanyName"].ToString());
                                CallToAction.ContactName = (reader["ContactName"].ToString());
                                CallToAction.ContactId = Convert.ToInt64(reader["ContactId"]);
                                CallToAction.ContactId = Convert.ToInt64(reader["ContactId"]);
                                CallToAction.CallToActionId = Convert.ToInt64(reader["CallToActionId"]);
                                CallToAction.Type = (reader["Type"].ToString());
                                CallToAction.Date = Convert.ToDateTime(reader["Date"]);
                                CallToAction.Time = Convert.ToDateTime(currentDate + " " + reader["Time"]);
                                CallToAction.OtherActivities = (reader["OtherActivities"].ToString());
                                CallToAction.Description = (reader["Description"].ToString());
                                CallToAction.TaskName = (reader["TaskName"].ToString());
                                CallToAction.ReminderDayList = (reader["ReminderDayList"].ToString());
                                CallToAction.TaskAssignedEmployee = (reader["TaskAssignedEmployee"].ToString());
                                CallToAction.PerticipentFromClient = (reader["PerticipentFromClient"].ToString());
                                CallToAction.PerticipentFromOffice = (reader["PerticipentFromOffice"].ToString());
                            }
                        }
                    }

                }
            }
            return CallToAction;
        }
        public List<CallToActionDetailBO> GetGetCallToActionDetailsByCallToActionId(long CallToActionId)
        {
            List<CallToActionDetailBO> taskList = new List<CallToActionDetailBO>();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCallToActionDetailsByCallToActionId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CallToActionId", DbType.Int32, CallToActionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CallToActionDetailBO task = new CallToActionDetailBO();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.CallToActionId = Convert.ToInt64(reader["CallToActionId"]);
                                task.Type = (reader["Type"].ToString());
                                task.Date = Convert.ToDateTime(reader["Date"]);
                                task.Time = Convert.ToDateTime(currentDate + " " + reader["Time"]);
                                task.OtherActivities = (reader["OtherActivities"].ToString());
                                task.Description = (reader["Description"].ToString());
                                task.PerticipentFromClient = (reader["PerticipentFromClient"].ToString());
                                task.PerticipentFromOffice = (reader["PerticipentFromOffice"].ToString());
                                task.ReminderDayList = (reader["ReminderDayList"].ToString());
                                task.TaskAssignedEmployee = (reader["TaskAssignedEmployee"].ToString());
                                task.TaskAssignedEmployeeName = (reader["TaskAssignedEmployeeName"].ToString());
                                task.PerticipentFromClientName = (reader["PerticipentFromClientName"].ToString());
                                task.PerticipentFromOfficeName = (reader["PerticipentFromOfficeName"].ToString());
                                task.ContactName = (reader["ContactName"].ToString());
                                task.CompanyName = (reader["CompanyName"].ToString());
                                task.ContactId = Convert.ToInt64(reader["ContactId"]);
                                task.CompanyId = Convert.ToInt64(reader["CompanyId"]);
                                taskList.Add(task);
                            }
                        }
                    }
                }
            }
            return taskList;
        }


    }
}
