using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class SiteSurveyNoteDA : BaseService
    {
        public Boolean SaveUpdateSiteSurveyNote(SMSiteSurveyNoteBO sMSiteSurveyNoteBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSiteSurveyNote_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, sMSiteSurveyNoteBO.Id);

                        if (sMSiteSurveyNoteBO.CompanyId != 0)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.String, sMSiteSurveyNoteBO.CompanyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.String, DBNull.Value);

                        if (sMSiteSurveyNoteBO.ContactId != 0)
                            dbSmartAspects.AddInParameter(command, "@ContactId", DbType.String, sMSiteSurveyNoteBO.ContactId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ContactId", DbType.String, DBNull.Value);

                        if (sMSiteSurveyNoteBO.DealId != 0)
                            dbSmartAspects.AddInParameter(command, "@DealId", DbType.String, sMSiteSurveyNoteBO.DealId);
                        else
                            dbSmartAspects.AddInParameter(command, "@DealId", DbType.String, DBNull.Value);

                        if (sMSiteSurveyNoteBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, sMSiteSurveyNoteBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        if (sMSiteSurveyNoteBO.SegmentId != 0)
                            dbSmartAspects.AddInParameter(command, "@SegmentId", DbType.String, sMSiteSurveyNoteBO.SegmentId);
                        else
                            dbSmartAspects.AddInParameter(command, "@SegmentId", DbType.String, DBNull.Value);

                        if (sMSiteSurveyNoteBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMSiteSurveyNoteBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, sMSiteSurveyNoteBO.LastModifiedBy);

                        dbSmartAspects.AddInParameter(command, "@IsDealNeedSiteSurvey", DbType.String, sMSiteSurveyNoteBO.IsDealNeedSiteSurvey);

                        dbSmartAspects.AddInParameter(command, "@IsSiteSurveyUnderCompany", DbType.String, sMSiteSurveyNoteBO.IsSiteSurveyUnderCompany);

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
        public List<SMSiteSurveyNoteBO> GetSiteSurveyInformationPagination(int dealId, int companyId, int contactId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMSiteSurveyNoteBO> SiteSurveyNoteList = new List<SMSiteSurveyNoteBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSiteSurveyInformationForPaging_SP"))
                    {

                        if ((companyId) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, DBNull.Value);
                        if ((contactId) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.String, contactId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.String, DBNull.Value);

                        if ((dealId) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.String, dealId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSiteSurveyNoteBO SiteSurveyNote = new SMSiteSurveyNoteBO();

                                    SiteSurveyNote.Id = Convert.ToInt64(reader["Id"]);
                                    SiteSurveyNote.Company = (reader["Company"].ToString());
                                    SiteSurveyNote.Deal = (reader["Deal"].ToString());
                                    SiteSurveyNote.Status = (reader["Status"].ToString());
                                    SiteSurveyNote.Date = (reader["Date"].ToString());
                                    SiteSurveyNote.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    SiteSurveyNote.DealId = Convert.ToInt64(reader["DealId"]);
                                    SiteSurveyNote.ContactId = Convert.ToInt32(reader["ContactId"]);
                                    SiteSurveyNote.IsDealNeedSiteSurvey = Convert.ToBoolean(reader["IsDealNeedSiteSurvey"]);
                                    SiteSurveyNote.SegmentId = Convert.ToInt64(reader["SegmentId"]);
                                    SiteSurveyNoteList.Add(SiteSurveyNote);
                                    
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
            return SiteSurveyNoteList;
        }
        public int GetSiteSurveyNoteId(int companyId, int dealId)
        {
            int SiteSurveyNoteId = 0;

            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSiteSurveyNoteByCompanyAndDealId_SP"))
                    {

                        if ((companyId) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, DBNull.Value);

                        if ((dealId) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.String, dealId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.String, DBNull.Value);



                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SiteSurveyNoteId = Convert.ToInt32(reader["Id"]);
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
            return SiteSurveyNoteId;
        }
        public Boolean SaveUpdateSiteSurveyFeedback(SMSiteSurveyFeedbackBO sMSiteSurveyFeedback, string EmpId, List<SMSiteSurveyFeedbackDetailsBO> AddedItem, List<SMSiteSurveyFeedbackDetailsBO> EditedItem, List<SMSiteSurveyFeedbackDetailsBO> DeletedItem, out long OutId)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {

                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveSiteSurveyFeedback_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandOut, "@Id", DbType.Int32, sMSiteSurveyFeedback.Id);
                            dbSmartAspects.AddInParameter(commandOut, "@SiteSurveyNoteId", DbType.Int32, sMSiteSurveyFeedback.SiteSurveyNoteId);
                            dbSmartAspects.AddInParameter(commandOut, "@SurveyFeedback", DbType.String, sMSiteSurveyFeedback.SurveyFeedback);
                            dbSmartAspects.AddInParameter(commandOut, "@UserId", DbType.Int32, sMSiteSurveyFeedback.CreatedBy);
                            dbSmartAspects.AddOutParameter(commandOut, "@OutId", DbType.Int32, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                            OutId = Convert.ToInt64(commandOut.Parameters["@OutId"].Value);

                        }
                        if (status > 0 && AddedItem.Count > 0)
                        {
                            foreach (SMSiteSurveyFeedbackDetailsBO Feedback in AddedItem)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveSiteSurveyFeedbackDetails_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SiteSurveyFeedbackId", DbType.Int64, OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, Feedback.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, Feedback.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }
                        if (status > 0 && EmpId != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateSiteSurveyEngineer_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@SiteSurveyFeedbackId", DbType.Int64, OutId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@EngineerList", DbType.String, EmpId);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }
                        if (status > 0 && EditedItem.Count > 0)
                        {
                            foreach (SMSiteSurveyFeedbackDetailsBO Edit in EditedItem)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateSiteSurveyFeedbackDetails_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Int32, Edit.Quantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Id", DbType.Int32, Edit.Id);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }
                        if (status > 0 && DeletedItem.Count > 0)
                        {
                            foreach (SMSiteSurveyFeedbackDetailsBO delete in DeletedItem)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteSiteSurveyFeedbackDetails_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Id", DbType.Int32, delete.Id);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
                return retVal;
            }
        }
        public SMSiteSurveyFeedbackBO GetSiteSurveyFeedbackByFeedbackId(long id)
        {
            SMSiteSurveyFeedbackBO sMSiteSurveyFeedbackBO = new SMSiteSurveyFeedbackBO();
            string query = string.Format("SELECT * from SMSiteSurveyFeedback  WHERE IsDeleted= 0 AND Id = {0}", id);

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

                                    sMSiteSurveyFeedbackBO.Id = Convert.ToInt64(reader["Id"]);
                                    sMSiteSurveyFeedbackBO.SurveyFeedback = (reader["SurveyFeedback"].ToString());
                                    sMSiteSurveyFeedbackBO.SiteSurveyNoteId = Convert.ToInt64(reader["SiteSurveyNoteId"]);
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

            return sMSiteSurveyFeedbackBO;

        }
        public List<SMSiteSurveyFeedbackDetailsBO> GetSiteSurveyFeedbackDetailsByFeedbackId(long id)
        {
            List<SMSiteSurveyFeedbackDetailsBO> sMSiteSurveyFeedbackDetailsList = new List<SMSiteSurveyFeedbackDetailsBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSiteSurveyFeedbackDetailsByFeedbackId_SP"))
                    {

                        if ((id) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@FeedbackId", DbType.String, id);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@FeedbackId", DbType.String, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSiteSurveyFeedbackDetailsBO SMSiteSurveyFeedbackDetailsBO = new SMSiteSurveyFeedbackDetailsBO();

                                    SMSiteSurveyFeedbackDetailsBO.Id = Convert.ToInt64(reader["Id"]);
                                    SMSiteSurveyFeedbackDetailsBO.ItemName = (reader["ItemName"].ToString());
                                    SMSiteSurveyFeedbackDetailsBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                    SMSiteSurveyFeedbackDetailsBO.UnitHead = (reader["HeadName"].ToString());
                                    SMSiteSurveyFeedbackDetailsBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                    SMSiteSurveyFeedbackDetailsBO.SiteSurveyFeedbackId = Convert.ToInt64(reader["SiteSurveyFeedbackId"]);
                                    sMSiteSurveyFeedbackDetailsList.Add(SMSiteSurveyFeedbackDetailsBO);
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

            return sMSiteSurveyFeedbackDetailsList;

        }
        public List<int> GetSiteSurveyEngineerByFeedbackId(long id)
        {
            List<int> sMSiteSurveyEngineerBOList = new List<int>();
            string query = string.Format("SELECT * from SMSiteSurveyEngineer  WHERE  SiteSurveyFeedbackId = {0}", id);

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
                                    SMSiteSurveyEngineerBO sMSiteSurveyEngineerBO = new SMSiteSurveyEngineerBO();

                                    sMSiteSurveyEngineerBO.EmpId = Convert.ToInt32(reader["EmpId"]);

                                    sMSiteSurveyEngineerBOList.Add(sMSiteSurveyEngineerBO.EmpId);
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

            return sMSiteSurveyEngineerBOList;

        }
        public int CheckFeedback(int siteSurveyId)
        {
            int FeedbackId = 0;
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CheckFeedbackBySiteSurveyNoteId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SiteSurveyNoteId", DbType.Int32, siteSurveyId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    FeedbackId = Convert.ToInt32(reader["Id"]);
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
            return FeedbackId;
        }
        public SMSiteSurveyNoteBO GetSiteSurveyNoteById(long id)
        {
            SMSiteSurveyNoteBO sMSiteSurvey = new SMSiteSurveyNoteBO();
            string query = string.Format("SELECT * from SMSiteSurveyNote  WHERE  Id = {0}", id);

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
                                    sMSiteSurvey.Id = Convert.ToInt64(reader["Id"]);
                                    
                                    if (reader["CompanyId"] == DBNull.Value)
                                    {
                                        sMSiteSurvey.CompanyId = 0;
                                    }
                                    else
                                    {
                                        sMSiteSurvey.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    }

                                    if (reader["DealId"] == DBNull.Value)
                                    {
                                        sMSiteSurvey.DealId = 0;
                                    }
                                    else
                                    {
                                        sMSiteSurvey.DealId = Convert.ToInt32(reader["DealId"]);
                                    }

                                    if (reader["ContactId"] == DBNull.Value)
                                    {
                                        sMSiteSurvey.ContactId = 0;
                                    }
                                    else
                                    {
                                        sMSiteSurvey.ContactId = Convert.ToInt32(reader["ContactId"]);
                                    }

                                    if (reader["SegmentId"] == DBNull.Value)
                                    {
                                        sMSiteSurvey.SegmentId = 0;
                                    }
                                    else
                                    {
                                        sMSiteSurvey.SegmentId = Convert.ToInt32(reader["SegmentId"]);
                                    }
                                    
                                    sMSiteSurvey.IsDealNeedSiteSurvey = Convert.ToBoolean(reader["IsDealNeedSiteSurvey"]);
                                    sMSiteSurvey.Description = (reader["Description"].ToString());
                                    sMSiteSurvey.Status= (reader["Status"].ToString());
                                    sMSiteSurvey.IsSiteSurveyUnderCompany = Convert.ToBoolean(reader["IsSiteSurveyUnderCompany"]);
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

            return sMSiteSurvey;

        }
        public SMSiteSurveyNoteBO GetSiteSurveyNoteDetailsById(long id)
        {
            SMSiteSurveyNoteBO sMSiteSurveyNoteBO = new SMSiteSurveyNoteBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSiteSurveyNoteForDetailsById_SP"))
                    {

                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                        
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {

                                    sMSiteSurveyNoteBO.Deal= (reader["Deal"].ToString());
                                    sMSiteSurveyNoteBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    sMSiteSurveyNoteBO.DealId = Convert.ToInt32(reader["DealId"]);
                                    sMSiteSurveyNoteBO.ContactId = Convert.ToInt32(reader["ContactId"]);
                                    sMSiteSurveyNoteBO.SegmentId = Convert.ToInt64(reader["SegmentId"]);
                                    sMSiteSurveyNoteBO.Segment = (reader["Segment"].ToString());
                                    sMSiteSurveyNoteBO.Description = (reader["Description"].ToString());
                                    sMSiteSurveyNoteBO.Status = (reader["Status"].ToString());
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

            return sMSiteSurveyNoteBO;

        }
        public List<SMSiteSurveyNoteBO> GetSiteSurveyInformationPaginationForFeedback(int dealId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMSiteSurveyNoteBO> SiteSurveyNoteList = new List<SMSiteSurveyNoteBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSiteSurveyInformationForFeedbackForPaging_SP"))
                    {
                        
                        if ((dealId) != 0)
                            dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.String, dealId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMSiteSurveyNoteBO SiteSurveyNote = new SMSiteSurveyNoteBO();

                                    SiteSurveyNote.Id = Convert.ToInt64(reader["Id"]);
                                    SiteSurveyNote.Company = (reader["Company"].ToString());
                                    SiteSurveyNote.Deal = (reader["Deal"].ToString());
                                    SiteSurveyNote.Status = (reader["Status"].ToString());
                                    SiteSurveyNote.Date = (reader["Date"].ToString());
                                    SiteSurveyNote.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    SiteSurveyNote.DealId = Convert.ToInt64(reader["DealId"]);
                                    SiteSurveyNote.IsDealNeedSiteSurvey = Convert.ToBoolean(reader["IsDealNeedSiteSurvey"]);
                                    SiteSurveyNoteList.Add(SiteSurveyNote);

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
            return SiteSurveyNoteList;
        }
        public Boolean ApproveSiteSurveyNote(long dealId)
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApproveSiteSurveyNote_SP"))
                    {
                        
                        dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, dealId);

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
