using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class CommonMessageDA : BaseService
    {
        public bool SaveMessage(CommonMessageBO message, List<CommonMessageDetailsBO> messageDetails, bool IsMessageSendAllGroupUser)
        {
            bool retVal = false;
            int status = 0, messageId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveMessage_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@MessageFrom", DbType.Int64, message.MessageFrom);
                            dbSmartAspects.AddInParameter(commandOut, "@MessageDate", DbType.DateTime, message.MessageDate);
                            dbSmartAspects.AddInParameter(commandOut, "@Importance", DbType.String, message.Importance);
                            dbSmartAspects.AddInParameter(commandOut, "@Subjects", DbType.String, message.Subjects);
                            dbSmartAspects.AddInParameter(commandOut, "@MessageBody", DbType.String, message.MessageBody);

                            dbSmartAspects.AddOutParameter(commandOut, "@MessageId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            messageId = Convert.ToInt32(commandOut.Parameters["@MessageId"].Value);
                        }

                        if (status > 0 && messageDetails.Count > 0 && !IsMessageSendAllGroupUser)
                        {
                            foreach (CommonMessageDetailsBO md in messageDetails)
                            {
                                using (DbCommand cmdMessageDetails = dbSmartAspects.GetStoredProcCommand("SaveMessageDetails_SP"))
                                {
                                    cmdMessageDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@MessageId", DbType.Int64, messageId);
                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@MessageTo", DbType.Int32, md.MessageTo);
                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@UserId", DbType.String, md.UserId);
                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@IsReaden", DbType.Boolean, false);
                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@IsMessageSendAllGroupUser", DbType.Boolean, IsMessageSendAllGroupUser);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdMessageDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && messageDetails.Count == 0 && IsMessageSendAllGroupUser)
                        {
                            using (DbCommand cmdMessageDetails = dbSmartAspects.GetStoredProcCommand("SaveMessageDetails_SP"))
                            {
                                cmdMessageDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdMessageDetails, "@MessageId", DbType.Int64, messageId);
                                dbSmartAspects.AddInParameter(cmdMessageDetails, "@MessageTo", DbType.Int32, message.MessageFrom);
                                dbSmartAspects.AddInParameter(cmdMessageDetails, "@UserId", DbType.String, message.MessageFromUserId);
                                dbSmartAspects.AddInParameter(cmdMessageDetails, "@IsReaden", DbType.Boolean, false);
                                dbSmartAspects.AddInParameter(cmdMessageDetails, "@IsMessageSendAllGroupUser", DbType.Boolean, IsMessageSendAllGroupUser);

                                status = dbSmartAspects.ExecuteNonQuery(cmdMessageDetails, transction);
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
        public bool SaveMessageById(CommonMessageBO message, List<CommonMessageDetailsBO> messageDetails)
        {
            bool retVal = false;
            int status = 0, messageId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveMessage_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@MessageFrom", DbType.Int64, message.MessageFrom);
                            dbSmartAspects.AddInParameter(commandOut, "@MessageDate", DbType.DateTime, message.MessageDate);
                            dbSmartAspects.AddInParameter(commandOut, "@Importance", DbType.String, message.Importance);
                            dbSmartAspects.AddInParameter(commandOut, "@Subjects", DbType.String, message.Subjects);
                            dbSmartAspects.AddInParameter(commandOut, "@MessageBody", DbType.String, message.MessageBody);

                            dbSmartAspects.AddOutParameter(commandOut, "@MessageId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            messageId = Convert.ToInt32(commandOut.Parameters["@MessageId"].Value);
                        }

                        if (status > 0 && messageDetails.Count > 0 )
                        {
                            foreach (CommonMessageDetailsBO md in messageDetails)
                            {
                                using (DbCommand cmdMessageDetails = dbSmartAspects.GetStoredProcCommand("SaveMessageDetailsById_SP"))
                                {
                                    cmdMessageDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@MessageId", DbType.Int64, messageId);
                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@MessageTo", DbType.Int32, md.MessageTo);
                                    dbSmartAspects.AddInParameter(cmdMessageDetails, "@IsReaden", DbType.Boolean, false);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdMessageDetails, transction);
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

        public bool UpdateMessageDetails(Int64 messageId, Int64 messageDetailsId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateMessageDetails_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@MessageId", DbType.Int64, messageId);
                            dbSmartAspects.AddInParameter(commandOut, "@MessageDetailsId", DbType.Int64, messageDetailsId);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
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

        public List<CommonMessageDetailsBO> GetMessageDetailsBySendUserId(Int32 userInfoId)
        {
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMessageDetailsBySenderId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MessageFrom", DbType.Int32, userInfoId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Message");
                    DataTable Table = ds.Tables["Message"];

                    messageDetails = Table.AsEnumerable().Select(r => new CommonMessageDetailsBO
                    {
                        MessageId = r.Field<Int64>("MessageId"),
                        UserName = r.Field<string>("UserName"),
                        MessageDate = r.Field<DateTime>("MessageDate"),
                        Subjects = r.Field<string>("Subjects"),
                        MessageBody = r.Field<string>("MessageBody")

                    }).ToList();
                }
            }

            return messageDetails;
        }

        public List<CommonMessageDetailsBO> GetMessageDetailsByUserId(string userId, bool? isReaden, byte? totalMessageRetrive, out Int16 TotalUnreadMessage)
        {
            List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();
            TotalUnreadMessage = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMessageDetailsByUserId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);

                    if (isReaden != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsReaden", DbType.Boolean, isReaden);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsReaden", DbType.Boolean, DBNull.Value);

                    if (totalMessageRetrive != null)
                        dbSmartAspects.AddInParameter(cmd, "@TotalMessageRetrive", DbType.Byte, totalMessageRetrive);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TotalMessageRetrive", DbType.Byte, DBNull.Value);

                    dbSmartAspects.AddOutParameter(cmd, "@TotalUnreadMessage", DbType.Int16, sizeof(Int16));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Message");
                    DataTable Table = ds.Tables["Message"];

                    messageDetails = Table.AsEnumerable().Select(r => new CommonMessageDetailsBO
                    {
                        MessageDetailsId = r.Field<Int64>("MessageDetailsId"),
                        MessageId = r.Field<Int64>("MessageId"),
                        UserName = r.Field<string>("UserName"),
                        MessageDate = r.Field<DateTime>("MessageDate"),
                        Subjects = r.Field<string>("Subjects"),
                        MessageBody = r.Field<string>("MessageBody"),
                        IsReaden = r.Field<bool>("IsReaden")

                    }).ToList();

                    TotalUnreadMessage = (Int16)cmd.Parameters["@TotalUnreadMessage"].Value;

                    //TotalUnreadMessage = Convert.ToInt32(cmd.Parameters["@TotalUnreadMessage"].Value);
                }
            }

            return messageDetails;
        }

        public CommonMessageDetailsBO GetMessageDetailsById(Int64 messageId, Int64 messageDetailsIdd)
        {
            CommonMessageDetailsBO messageDetails = new CommonMessageDetailsBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMessageDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MessageId", DbType.Int64, messageId);
                    dbSmartAspects.AddInParameter(cmd, "@MessageDetailsId", DbType.Int64, messageDetailsIdd);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Message");
                    DataTable Table = ds.Tables["Message"];

                    messageDetails = Table.AsEnumerable().Select(r => new CommonMessageDetailsBO
                    {
                        MessageDetailsId = r.Field<Int64>("MessageDetailsId"),
                        MessageId = r.Field<Int64>("MessageId"),
                        UserName = r.Field<string>("UserName"),
                        MessageDate = r.Field<DateTime>("MessageDate"),
                        Subjects = r.Field<string>("Subjects"),
                        MessageBody = r.Field<string>("MessageBody"),
                        IsReaden = r.Field<bool>("IsReaden")

                    }).FirstOrDefault();
                }
            }

            return messageDetails;
        }

        public List<CommonMessageDetailsBO> GetMessageInbox(string userId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CommonMessageDetailsBO> messageInbox = new List<CommonMessageDetailsBO>();
            totalRecords = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMessageInbox_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MessageInbox");
                    DataTable Table = ds.Tables["MessageInbox"];

                    messageInbox = Table.AsEnumerable().Select(r => new CommonMessageDetailsBO
                    {
                        MessageDetailsId = r.Field<Int64>("MessageDetailsId"),
                        MessageId = r.Field<Int64>("MessageId"),
                        UserName = r.Field<string>("UserName"),
                        MessageDate = r.Field<DateTime>("MessageDate"),
                        Subjects = r.Field<string>("Subjects"),
                        MessageBody = r.Field<string>("MessageBody"),
                        IsReaden = r.Field<bool>("IsReaden")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return messageInbox;
        }
    }
}
