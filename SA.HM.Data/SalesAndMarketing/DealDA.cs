using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class DealDA : BaseService
    {

        public Boolean SaveOrUpdateDeal(SMDeal deal, List<SMDealWiseContactMap> deleteList, string deletedDocumentId, out long id)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateDeal_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, deal.Id);
                            dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, deal.OwnerId);
                            if (deal.CompanyId != 0)
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, deal.CompanyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, deal.Name);
                            dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, deal.Amount);
                            dbSmartAspects.AddInParameter(command, "@ExpectedRevenue", DbType.Decimal, deal.ExpectedRevenue);
                            dbSmartAspects.AddInParameter(command, "@Type", DbType.String, deal.Type);
                            dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, deal.StageId);
                            dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, deal.StartDate);
                            dbSmartAspects.AddInParameter(command, "@CloseDate", DbType.DateTime, deal.CloseDate);
                            dbSmartAspects.AddInParameter(command, "@ProbabilityStageId", DbType.Int32, deal.ProbabilityStageId);
                            dbSmartAspects.AddInParameter(command, "@SegmentId", DbType.Int32, deal.SegmentId);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, deal.Description);
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, deal.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@IsCanDelete", DbType.Boolean, deal.IsCanDelete);
                            //dbSmartAspects.AddInParameter(command, "@GLCompanyId", DbType.Int32, deal.GLCompanyId);
                            //dbSmartAspects.AddInParameter(command, "@GLProjectId", DbType.Int32, deal.GLProjectId);
                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            status = (dbSmartAspects.ExecuteNonQuery(command, transaction) > 0);

                            id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                        }
                        if (status && deal.Contacts.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDealWiseContactMap_SP"))
                            {
                                foreach (var item in deal.Contacts)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, item.ContactId);
                                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, id);

                                    status = (dbSmartAspects.ExecuteNonQuery(command, transaction) > 0);
                                }

                            }
                        }
                        if (deal.Id > 0 && status && deleteList.Count > 0)
                        {
                            string contactIdList = string.Join(",", deal.Contacts.Select(a => a.ContactId).ToList());

                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDealWiseContactMap_SP"))
                            {
                                foreach (var item in deleteList)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@ContactId", DbType.String, item.ContactId);
                                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, id);

                                    status = (dbSmartAspects.ExecuteNonQuery(command, transaction) > 0);
                                }


                            }
                        }
                        if (status && !string.IsNullOrEmpty(deletedDocumentId))
                        {
                            bool deleteDocument = DeleteDealDocument(deletedDocumentId);
                        }
                        if (status)
                        {
                            bool update = UpdateDocumentOwnwerIdWithDealId(id, deal.RandomDealId);
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();

                        }

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }

            return status;
        }

        public List<SMDeal> GetDealInfoForSearch(int ownerId, string dealNumber, string name, int stageId, int companyId, string dateType, string fromDate, string toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfoBySearchCriteriaForPaging_SP"))
                    {
                        if (ownerId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, ownerId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, DBNull.Value);
                        if (!string.IsNullOrEmpty(dealNumber))
                            dbSmartAspects.AddInParameter(cmd, "@DealNumber", DbType.String, dealNumber);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DealNumber", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (stageId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@StageId", DbType.Int32, stageId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@StageId", DbType.Int32, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, DBNull.Value);
                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(toDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }
                        dbSmartAspects.AddInParameter(cmd, "@DateType", DbType.String, dateType);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal dealBO = new SMDeal();

                                    dealBO.Id = Convert.ToInt64(reader["Id"]);
                                    dealBO.Name = reader["Name"].ToString();
                                    dealBO.Owner = reader["Owner"].ToString();
                                    dealBO.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                    dealBO.Stage = reader["Stage"].ToString();
                                    dealBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                    dealBO.Company = reader["Company"].ToString();
                                    if (reader["CompanyId"] != DBNull.Value)
                                        dealBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    if (reader["StartDate"] != DBNull.Value)
                                    {
                                        dealBO.StartDate = Convert.ToDateTime(reader["StartDate"]);

                                    }
                                    if (reader["CloseDate"] != DBNull.Value)
                                    {
                                        dealBO.CloseDate = Convert.ToDateTime(reader["CloseDate"]);

                                    }
                                    dealBO.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                    dealBO.IsQuotationReview = Convert.ToBoolean(reader["IsQuotationReview"]);
                                    dealBO.IsSiteSurvey = Convert.ToBoolean(reader["IsSiteSurvey"]);
                                    dealBO.IsCloseWon = Convert.ToBoolean(reader["IsCloseWon"]);
                                    dealBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                    dealBO.ContactId = Convert.ToInt64(reader["ContactId"]);
                                    dealBO.ContactName = reader["ContactName"].ToString();
                                    dealBO.DealNumber = reader["DealNumber"].ToString();
                                    dealList.Add(dealBO);
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
            return dealList;
        }
        public List<SMDeal> GetAllDeal(int ownerId, string dealName, int dealStageId, int companyId, string dateType, string fromDate, string toDate)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllDeal_SP"))
                    {
                        if (ownerId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, ownerId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(dealName))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, dealName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (dealStageId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@StageId", DbType.Int32, dealStageId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@StageId", DbType.Int32, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(toDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }
                        dbSmartAspects.AddInParameter(cmd, "@DateType", DbType.String, dateType);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal dealBO = new SMDeal();

                                    dealBO.Id = Convert.ToInt64(reader["Id"]);
                                    dealBO.Name = reader["Name"].ToString();
                                    dealBO.Owner = reader["Owner"].ToString();
                                    dealBO.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                    dealBO.StageId = Convert.ToInt32(reader["StageId"]);

                                    if (reader["Amount"] != DBNull.Value)
                                        dealBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                    else
                                        dealBO.Amount = 0;

                                    dealBO.Company = reader["Company"].ToString();
                                    if (reader["CompanyId"] != DBNull.Value)
                                        dealBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    if (reader["StartDate"] != DBNull.Value)
                                    {
                                        dealBO.StartDate = Convert.ToDateTime(reader["StartDate"]);

                                    }
                                    if (reader["CloseDate"] != DBNull.Value)
                                    {
                                        dealBO.CloseDate = Convert.ToDateTime(reader["CloseDate"]);
                                    }

                                    dealBO.ContactName = reader["ContactName"].ToString();
                                    dealList.Add(dealBO);
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
            return dealList;
        }
        public List<SMDeal> GetDealInfoByCompanyId(int companyId)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfoByCompanyId_SP"))
                    {
                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal dealBO = new SMDeal();

                                    dealBO.Id = Convert.ToInt64(reader["Id"]);
                                    dealBO.Name = reader["Name"].ToString();
                                    dealBO.Owner = reader["Owner"].ToString();
                                    dealBO.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                    dealBO.Stage = reader["Stage"].ToString();
                                    dealBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                    dealBO.Company = reader["Company"].ToString();
                                    dealBO.ProbabilityStage = reader["ProbabilityStage"].ToString();
                                    dealBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    //dealBO.Contact = reader["Contact"].ToString();
                                    //dealBO.ContactId = Convert.ToInt32(reader["ContactId"]);
                                    if (reader["StartDate"] != DBNull.Value)
                                    {
                                        dealBO.StartDate = Convert.ToDateTime(reader["StartDate"]);

                                    }
                                    if (reader["CloseDate"] != DBNull.Value)
                                    {
                                        dealBO.CloseDate = Convert.ToDateTime(reader["CloseDate"]);

                                    }
                                    dealBO.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                    dealList.Add(dealBO);
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
            return dealList;
        }
        public SMDeal GetDealInfoBOById(long Id)
        {
            SMDeal Deal = new SMDeal();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, Id);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");

                        Deal = ds.Tables[0].AsEnumerable().Select(r => new SMDeal
                        {
                            Id = r.Field<long>("Id"),
                            Name = r.Field<string>("Name"),
                            OwnerId = r.Field<int?>("OwnerId"),
                            Owner = r.Field<string>("Owner"),
                            StageId = r.Field<int>("StageId"),
                            Stage = r.Field<string>("Stage"),
                            Amount = r.Field<decimal?>("Amount"),
                            ExpectedRevenue = r.Field<decimal?>("ExpectedRevenue"),
                            CompanyId = r.Field<int?>("CompanyId"),
                            Company = r.Field<string>("Company"),
                            StartDate = r.Field<DateTime?>("StartDate"),
                            CloseDate = r.Field<DateTime?>("CloseDate"),
                            ProbabilityStageId = r.Field<int>("ProbabilityStageId"),
                            SegmentId = r.Field<int>("SegmentId"),
                            Description = r.Field<string>("Description"),
                            IsCanDelete = r.Field<bool>("IsCanDelete")
                            //GLCompanyId= r.Field<int>("GLCompanyId"),
                            //GLProjectId= r.Field<int>("GLProjectId")
                        }).FirstOrDefault();


                        Deal.Contacts = ds.Tables[1].AsEnumerable().Select(r => new SMDealWiseContactMap()
                        {
                            Id = r.Field<long>("Id"),
                            ContactId = r.Field<long>("ContactId"),
                            Name = r.Field<string>("Name")
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Deal;
        }

        public SMDealView GetDealInfoForViewById(long Id)
        {
            SMDealView Deal = new SMDealView();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfoForViewById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, Id);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "DealInfo");

                        Deal = ds.Tables[0].AsEnumerable().Select(r => new SMDealView
                        {
                            Name = r.Field<string>("Name"),
                            Owner = r.Field<string>("Owner"),
                            Stage = r.Field<string>("Stage"),
                            Amount = r.Field<decimal>("Amount"),
                            CompanyId = r.Field<int>("CompanyId"),
                            Company = r.Field<string>("Company"),
                            Industry = r.Field<string>("Industry"),
                            Phone = r.Field<string>("Phone"),
                            Email = r.Field<string>("Email"),
                            Website = r.Field<string>("Website"),
                            LifeCycleStage = r.Field<string>("LifeCycleStage"),
                            Complete = r.Field<decimal>("Complete"),
                            LastActivityDateTime = r.Field<DateTime>("LastActivityDateTime"),
                            ShippingStreet = r.Field<string>("ShippingStreet"),
                            ShippingState = r.Field<string>("ShippingState"),
                            ShippingCity = r.Field<string>("ShippingCity"),
                            ShippingCountry = r.Field<string>("ShippingCountry"),
                            ShippingPostCode = r.Field<string>("ShippingPostCode"),
                            ImplementationFeedback = r.Field<string>("ImplementationFeedback"),
                            ImplementationStatus = r.Field<string>("ImplementationStatus"),
                            IsCloseWon = r.Field<bool>("IsCloseWon")

                        }).FirstOrDefault();


                        Deal.Contacts = ds.Tables[1].AsEnumerable().Select(r => new SMDealWiseContactMap()
                        {
                            Id = r.Field<long>("Id"),
                            ContactId = r.Field<long>("ContactId"),
                            Name = r.Field<string>("Name"),
                            Title = r.Field<string>("Title"),
                        }).ToList();

                        Deal.CompanyContacts = ds.Tables[2].AsEnumerable().Select(r => new SMDealWiseContactMap()
                        {
                            Id = r.Field<long>("Id"),
                            Name = r.Field<string>("Name"),
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Deal;
        }
        public Boolean SoftDeleteDeal(int Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("Update SMDeal SET IsDeleted = 1 WHERE Id = {0} ", Id);

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

        public List<DealImpFeedbackBO> GetImpFeedbackInfoByDealId(int dealId)
        {
            List<DealImpFeedbackBO> dealImpFeedbackBOs = new List<DealImpFeedbackBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetImpFeedbackInfoByDealId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int32, dealId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    DealImpFeedbackBO bO = new DealImpFeedbackBO();

                                    bO.Id = Convert.ToInt64(reader["Id"]);
                                    bO.ImpEngineerId = Convert.ToInt64(reader["ImpEngineerId"]);
                                    bO.DealId = Convert.ToInt64(reader["DealId"]);
                                    dealImpFeedbackBOs.Add(bO);
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
            return dealImpFeedbackBOs;
        }
        public SMDeal GetImpFeedbackFromDealTable(int dealId)
        {
            SMDeal Deal = new SMDeal();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetImpFeedbackFromDealByDealId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int32, dealId);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    Deal.Id = Convert.ToInt32(reader["Id"]);
                                    if (reader["ImplementationDate"] != DBNull.Value)
                                    {
                                        Deal.ImplementationDate = Convert.ToDateTime(reader["ImplementationDate"]);
                                    }
                                    Deal.ImplementationFeedback = reader["ImplementationFeedback"].ToString();
                                    Deal.ImplementationStatus = reader["ImplementationStatus"].ToString();
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

            return Deal;
        }
        public bool SaveImpFeedback(List<DealImpFeedbackBO> impFeedbackBOs, int lastModifyBy, out long tmpId)
        {
            bool status = false;
            tmpId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveImpFeedback_SP"))
                        {
                            foreach (var item in impFeedbackBOs)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@ImpEngineerId", DbType.Int64, item.ImpEngineerId);
                                dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, item.DealId);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, lastModifyBy);

                                dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                tmpId = Convert.ToInt32(command.Parameters["@Id"].Value);
                            }
                        }
                        if (status == true)
                        {
                            transction.Commit();
                        }
                        else
                        {

                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;
        }
        public bool UpdateImpFeedback(List<DealImpFeedbackBO> impFeedbackBOs, int updatedBy)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateImpFeedback_SP"))
                        {
                            foreach (var item in impFeedbackBOs)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, item.DealId);
                                dbSmartAspects.AddInParameter(command, "@ImpEngineerId", DbType.Int64, item.ImpEngineerId);
                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, updatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;
                            }
                        }
                        if (status == true)
                        {

                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;

        }
        public bool DeleteImpFeedback(List<DealImpFeedbackBO> deletList)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteImpFeedback_SP"))
                        {
                            foreach (var item in deletList)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, item.DealId);
                                dbSmartAspects.AddInParameter(command, "@ImpEngineerId", DbType.Int64, item.ImpEngineerId);
                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                            }
                        }
                        if (status == true)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;

        }
        public Boolean UpdateImpFeedbackInfo(string impFeedback, DateTime impDate, int dealId, int updatedBy, string implementationStatus)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateImpFeedbackInfo_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@DealId", DbType.Int64, dealId);
                    dbSmartAspects.AddInParameter(commandDocument, "@ImplementationDate", DbType.DateTime, impDate);
                    dbSmartAspects.AddInParameter(commandDocument, "@ImplementationFeedback", DbType.String, impFeedback);
                    dbSmartAspects.AddInParameter(commandDocument, "@LastModifiedBy", DbType.Int32, updatedBy);
                    dbSmartAspects.AddInParameter(commandDocument, "@ImplementationStatus", DbType.String, implementationStatus);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }

        public Boolean UpdateDocumentOwnwerIdWithDealId(long dealId, long randomDealId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentOwnwerIdWithDealId_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@DealId", DbType.Int64, dealId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.Int64, randomDealId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }

        public Boolean DeleteDealDocument(string deletedDocumentId)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(deletedDocumentId))
            {

                string[] documentId = deletedDocumentId.Split(',');

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteDocumentsInfoByDocId_SP"))
                            {
                                for (int i = 0; i < documentId.Count() && documentId[i] != "0"; i++)
                                {
                                    commandDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDelete, "@DocumentId", DbType.Int32, int.Parse(documentId[i]));

                                    status = dbSmartAspects.ExecuteNonQuery(commandDelete, transction) > 0 ? true : false;

                                    if (!status)
                                        break;
                                }
                            }
                            if (status)
                            {
                                transction.Commit();
                            }
                            else
                            {
                                transction.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            transction.Rollback();
                            throw ex;
                        }
                    }
                    conn.Close();
                }

            }

            return status;
        }
        public Boolean SaveDealWiseContactMap(List<SMDealWiseContactMap> contactList)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDealWiseContactMap_SP"))
                        {
                            foreach (var item in contactList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, item.DealId);
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, item.ContactId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;

                                if (!status)
                                    break;
                            }

                        }
                        if (status)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
                conn.Close();
            }
            return status;
        }
        public List<SMDeal> GetAllDealByCompanyIdNContactIdForDropdown(int? CompanyId, long? ContactId)
        {
            List<SMDeal> dealList = new List<SMDeal>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllDealByCompanyIdNContactIdForDropdown_SP"))
                    {
                        if (CompanyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (ContactId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, ContactId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal sMDeal = new SMDeal();
                                    sMDeal.Id = Convert.ToInt64(reader["Id"]);
                                    sMDeal.Name = (reader["Name"].ToString());
                                    dealList.Add(sMDeal);
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
            return dealList;
        }

        public Boolean DuplicateTypeCheck(string forcastType, int id)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckDealStageForcastTypeDuplicating_SP"))
                    {



                        dbSmartAspects.AddInParameter(command, "@Id", DbType.String, id);


                        if (!String.IsNullOrEmpty(forcastType))
                            dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, forcastType);
                        else
                            dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, DBNull.Value);


                        using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    status = Convert.ToInt64(reader["STATUS"]) == 1 ? true : false;
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
            return status;
        }

        public List<SMDeal> GetDealInfoForSearchForFeedback(string dealNumber, string name, string status, int companyId, string dateType, string fromDate, string toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfoBySearchCriteriaForPagingForFeedback_SP"))
                    {
                        if (!string.IsNullOrEmpty(dealNumber))
                            dbSmartAspects.AddInParameter(cmd, "@DealNumber", DbType.String, dealNumber);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DealNumber", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(status))
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(toDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }
                        dbSmartAspects.AddInParameter(cmd, "@DateType", DbType.String, dateType);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal dealBO = new SMDeal();

                                    dealBO.Id = Convert.ToInt64(reader["Id"]);
                                    dealBO.Name = reader["Name"].ToString();
                                    dealBO.Company = reader["Company"].ToString();
                                    if (reader["CompanyId"] != DBNull.Value)
                                        dealBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    if (reader["StartDate"] != DBNull.Value)
                                    {
                                        dealBO.StartDate = Convert.ToDateTime(reader["StartDate"]);

                                    }
                                    if (reader["CloseDate"] != DBNull.Value)
                                    {
                                        dealBO.CloseDate = Convert.ToDateTime(reader["CloseDate"]);

                                    }
                                    dealBO.IsSiteSurvey = Convert.ToBoolean(reader["IsSiteSurvey"]);
                                    dealBO.ContactId = Convert.ToInt64(reader["ContactId"]);
                                    dealBO.ContactName = reader["ContactName"].ToString();
                                    dealList.Add(dealBO);
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
            return dealList;
        }
        public List<SMDeal> GetDealInfoForSearchForImplemantationFeedback(string dealNumber, string name, int companyId, string dateType, string fromDate, string toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfoBySearchCriteriaForDealImplementationFeedback_SP"))
                    {
                        if (!string.IsNullOrEmpty(dealNumber))
                            dbSmartAspects.AddInParameter(cmd, "@DealNumber", DbType.String, dealNumber);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@DealNumber", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(toDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }
                        dbSmartAspects.AddInParameter(cmd, "@DateType", DbType.String, dateType);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal dealBO = new SMDeal();

                                    dealBO.Id = Convert.ToInt64(reader["Id"]);
                                    dealBO.Name = reader["Name"].ToString();
                                    dealBO.Company = reader["Company"].ToString();
                                    if (reader["CompanyId"] != DBNull.Value)
                                        dealBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    if (reader["StartDate"] != DBNull.Value)
                                    {
                                        dealBO.StartDate = Convert.ToDateTime(reader["StartDate"]);

                                    }
                                    if (reader["CloseDate"] != DBNull.Value)
                                    {
                                        dealBO.CloseDate = Convert.ToDateTime(reader["CloseDate"]);

                                    }
                                    dealBO.IsCloseWon = Convert.ToBoolean(reader["IsCloseWon"]);
                                    dealBO.ContactId = Convert.ToInt64(reader["ContactId"]);
                                    dealBO.ContactName = reader["ContactName"].ToString();
                                    dealList.Add(dealBO);
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
            return dealList;
        }
        public List<SMDeal> GetDealByCompanyIdContactIdNAccountManager(string searchText, int contactId, int companyId, int accountManagerId)
        {
            List<SMDeal> dealList = new List<SMDeal>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealByCompanyIdContactIdNAccountManager_SP"))
                    {
                        if (companyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (contactId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, contactId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int32, DBNull.Value);

                        if (string.IsNullOrEmpty(searchText))
                            dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, searchText);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, DBNull.Value);

                        if (accountManagerId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, accountManagerId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, DBNull.Value);


                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMDeal dealBO = new SMDeal();

                                    dealBO.Id = Convert.ToInt64(reader["Id"]);
                                    dealBO.Name = reader["Name"].ToString();

                                    dealList.Add(dealBO);
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
            return dealList;
        }

    }
}
