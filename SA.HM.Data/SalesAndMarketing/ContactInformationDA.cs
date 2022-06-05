using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.SalesManagment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;


namespace HotelManagement.Data.SalesAndMarketing
{
    public class ContactInformationDA : BaseService
    {

        public Boolean SaveContact(ContactInformationBO contactCreationBO, string deletedDocumentId, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveContact_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, contactCreationBO.Id);


                        if (!string.IsNullOrEmpty(contactCreationBO.Name))
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, contactCreationBO.Name);
                        else
                            dbSmartAspects.AddInParameter(command, "@Name", DbType.String, DBNull.Value);

                        //if (!string.IsNullOrEmpty(contactCreationBO.ContactNo))
                        //    dbSmartAspects.AddInParameter(command, "@ContactNo", DbType.String, contactCreationBO.ContactNo);
                        //else
                        //    dbSmartAspects.AddInParameter(command, "@ContactNo", DbType.String, DBNull.Value);

                        if ((contactCreationBO.ContactOwnerId) != 0)
                            dbSmartAspects.AddInParameter(command, "@ContactOwnerId", DbType.Int32, contactCreationBO.ContactOwnerId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ContactOwnerId", DbType.Int32, 0);

                        if ((contactCreationBO.CompanyId) != 0)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, contactCreationBO.CompanyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, 0);

                        if ((contactCreationBO.SourceId) != 0)
                            dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, contactCreationBO.SourceId);
                        else
                            dbSmartAspects.AddInParameter(command, "@SourceId", DbType.Int32, 0);

                        if ((contactCreationBO.LifeCycleId) != 0)
                            dbSmartAspects.AddInParameter(command, "@LifeCycleId", DbType.Int32, contactCreationBO.LifeCycleId);
                        else
                            dbSmartAspects.AddInParameter(command, "@LifeCycleId", DbType.Int32, 0);

                        if (!string.IsNullOrEmpty(contactCreationBO.JobTitle))
                            dbSmartAspects.AddInParameter(command, "@JobTitle", DbType.String, contactCreationBO.JobTitle);
                        else
                            dbSmartAspects.AddInParameter(command, "@JobTitle", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.Department))
                            dbSmartAspects.AddInParameter(command, "@Department", DbType.String, contactCreationBO.Department);
                        else
                            dbSmartAspects.AddInParameter(command, "@Department", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.ContactType))
                            dbSmartAspects.AddInParameter(command, "@ContactType", DbType.String, contactCreationBO.ContactType);
                        else
                            dbSmartAspects.AddInParameter(command, "@ContactType", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.TicketNo))
                            dbSmartAspects.AddInParameter(command, "@TicketNo", DbType.String, contactCreationBO.TicketNo);
                        else
                            dbSmartAspects.AddInParameter(command, "@TicketNo", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.Email))
                            dbSmartAspects.AddInParameter(command, "@Email", DbType.String, contactCreationBO.Email);
                        else
                            dbSmartAspects.AddInParameter(command, "@Email", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(contactCreationBO.EmailWork))
                            dbSmartAspects.AddInParameter(command, "@EmailWork", DbType.String, contactCreationBO.EmailWork);
                        else
                            dbSmartAspects.AddInParameter(command, "@EmailWork", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.MobilePersonal))
                            dbSmartAspects.AddInParameter(command, "@MobilePersonal", DbType.String, contactCreationBO.MobilePersonal);
                        else
                            dbSmartAspects.AddInParameter(command, "@MobilePersonal", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(contactCreationBO.MobileWork))
                            dbSmartAspects.AddInParameter(command, "@MobileWork", DbType.String, contactCreationBO.MobileWork);
                        else
                            dbSmartAspects.AddInParameter(command, "@MobileWork", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.PhonePersonal))
                            dbSmartAspects.AddInParameter(command, "@PhonePersonal", DbType.String, contactCreationBO.PhonePersonal);
                        else
                            dbSmartAspects.AddInParameter(command, "@PhonePersonal", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(contactCreationBO.PhoneWork))
                            dbSmartAspects.AddInParameter(command, "@PhoneWork", DbType.String, contactCreationBO.PhoneWork);
                        else
                            dbSmartAspects.AddInParameter(command, "@PhoneWork", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.Facebook))
                            dbSmartAspects.AddInParameter(command, "@Facebook", DbType.String, contactCreationBO.Facebook);
                        else
                            dbSmartAspects.AddInParameter(command, "@Facebook", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.Whatsapp))
                            dbSmartAspects.AddInParameter(command, "@Whatsapp", DbType.String, contactCreationBO.Whatsapp);
                        else
                            dbSmartAspects.AddInParameter(command, "@Whatsapp", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(contactCreationBO.Skype))
                            dbSmartAspects.AddInParameter(command, "@Skype", DbType.String, contactCreationBO.Skype);
                        else
                            dbSmartAspects.AddInParameter(command, "@Skype", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(contactCreationBO.Twitter))
                            dbSmartAspects.AddInParameter(command, "@Twitter", DbType.String, contactCreationBO.Twitter);
                        else
                            dbSmartAspects.AddInParameter(command, "@Twitter", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactCreationBO.WorkAddress))
                            dbSmartAspects.AddInParameter(command, "@WorkAddress", DbType.String, contactCreationBO.WorkAddress);
                        else
                            dbSmartAspects.AddInParameter(command, "@WorkAddress", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@WorkStreet", DbType.String, contactCreationBO.WorkStreet);
                        dbSmartAspects.AddInParameter(command, "@WorkPostCode", DbType.String, contactCreationBO.WorkPostCode);
                        dbSmartAspects.AddInParameter(command, "@WorkCountryId", DbType.Int32, contactCreationBO.WorkCountryId);
                        dbSmartAspects.AddInParameter(command, "@WorkCityId", DbType.Int32, contactCreationBO.WorkCityId);
                        dbSmartAspects.AddInParameter(command, "@WorkStateId", DbType.Int32, contactCreationBO.WorkStateId);

                        if (!string.IsNullOrEmpty(contactCreationBO.PersonalAddress))
                            dbSmartAspects.AddInParameter(command, "@PersonalAddress", DbType.String, contactCreationBO.PersonalAddress);
                        else
                            dbSmartAspects.AddInParameter(command, "@PersonalAddress", DbType.String, DBNull.Value);
                        dbSmartAspects.AddInParameter(command, "@PersonalStreet", DbType.String, contactCreationBO.PersonalStreet);
                        dbSmartAspects.AddInParameter(command, "@PersonalPostCode", DbType.String, contactCreationBO.PersonalPostCode);
                        dbSmartAspects.AddInParameter(command, "@PersonalCountryId", DbType.Int64, contactCreationBO.PersonalCountryId);
                        dbSmartAspects.AddInParameter(command, "@PersonalCityId", DbType.Int64, contactCreationBO.PersonalCityId);
                        dbSmartAspects.AddInParameter(command, "@PersonalStateId", DbType.Int64, contactCreationBO.PersonalStateId);

                        dbSmartAspects.AddInParameter(command, "@WorkLocationId", DbType.Int64, contactCreationBO.WorkLocationId);
                        dbSmartAspects.AddInParameter(command, "@PersonalLocationId", DbType.Int64, contactCreationBO.PersonalLocationId);

                        if (contactCreationBO.DOB != null)
                            dbSmartAspects.AddInParameter(command, "@DOB", DbType.DateTime, contactCreationBO.DOB);
                        else
                            dbSmartAspects.AddInParameter(command, "@DOB", DbType.DateTime, DBNull.Value);

                        if (contactCreationBO.DateAnniversary != null)
                            dbSmartAspects.AddInParameter(command, "@DateAnniversary", DbType.DateTime, contactCreationBO.DateAnniversary);
                        else
                            dbSmartAspects.AddInParameter(command, "@DateAnniversary", DbType.DateTime, DBNull.Value);

                        if ((contactCreationBO.CreatedBy) != 0)
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, contactCreationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, DBNull.Value);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);

                    }
                    if (status && !string.IsNullOrEmpty(deletedDocumentId))
                    {
                        bool delete = DeleteContactDocument(deletedDocumentId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Boolean SaveCompanyWiseContact(List<ContactInformationBO> contacts)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyWiseContact_SP"))
                        {
                            foreach (var item in contacts)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, item.CompanyId);

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;

                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                }
            }
            return status;
        }

        // contact details related SPs
        public Boolean SaveContactDetails(List<SMContactDetailsBO> newAdded, List<SMContactDetailsBO> delatedItems, long parentId)
        {
            bool status = false;
            bool status2 = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (newAdded.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveContactDetailsItems_SP"))
                            {
                                foreach (var item in newAdded)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@ParentId", DbType.Int64, parentId);
                                    dbSmartAspects.AddInParameter(command, "@ParentType", DbType.String, item.ParentType);
                                    dbSmartAspects.AddInParameter(command, "@TransectionType", DbType.String, item.TransectionType);
                                    dbSmartAspects.AddInParameter(command, "@Title", DbType.String, item.Title);
                                    dbSmartAspects.AddInParameter(command, "@Value", DbType.String, item.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                }

                            }

                            if (status)
                            {
                                foreach (var item in newAdded)
                                {
                                    string tableName = "SMContactDetailsTitle";
                                    string pkFieldName = "Id";
                                    string pkFieldValue = "0";
                                    int IsDuplicate = 0;
                                    HMCommonDA hmCommonDA = new HMCommonDA();
                                    IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, "Title", item.Title, "TransectionType", item.TransectionType, 0, pkFieldName, pkFieldValue);

                                    if (IsDuplicate == 0)
                                    {
                                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveContactDetailsTitle_SP"))
                                        {
                                            cmd.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, item.TransectionType);
                                            dbSmartAspects.AddInParameter(cmd, "@Title", DbType.String, item.Title);

                                            status2 = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                                        }
                                    }
                                }
                            }

                        }
                        if (delatedItems.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteContactDetailsItems_SP"))
                            {
                                foreach (var item in delatedItems)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@DetailsId", DbType.Int64, item.DetailsId);
                                    dbSmartAspects.AddInParameter(command, "@ParentId", DbType.Int64, item.ParentId);

                                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;

                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                        status = false;
                    }
                }
            }
            return status;
        }
        public List<SMContactDetailsTitleBO> GetContactDetailsTitleByTransectionType(string transectionType)
        {
            List<SMContactDetailsTitleBO> sMSources = new List<SMContactDetailsTitleBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactDetailsTitleByTransectionType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, transectionType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMContactDetailsTitleBO sMLife = new SMContactDetailsTitleBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Title = reader["Title"].ToString()
                                };
                                sMSources.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return sMSources;
        }

        public List<ContactInformationBO> GetContactInformationOfClientWithoutCompany(int id)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInfoAllClientWithoutCompany_SP"))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }
        public List<SMContactDetailsTitleBO> GetContactDetailsTitleForAutoSearch(string searchItem, string type)
        {
            List<SMContactDetailsTitleBO> sMSources = new List<SMContactDetailsTitleBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactDetailsTitleForAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchItem", DbType.String, searchItem);
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, type);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMContactDetailsTitleBO sMLife = new SMContactDetailsTitleBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Title = reader["Title"].ToString()
                                };
                                sMSources.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return sMSources;
        }
        public List<SMContactDetailsBO> GetContactDetails(long parentId, string parentType)
        {
            List<SMContactDetailsBO> contactInformationList = new List<SMContactDetailsBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactDetails_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ParentId", DbType.Int32, parentId);
                        dbSmartAspects.AddInParameter(cmd, "@ParentType", DbType.String, parentType);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMContactDetailsBO detailsBO = new SMContactDetailsBO();

                                    detailsBO.DetailsId = Convert.ToInt64(reader["DetailsId"]);
                                    detailsBO.ParentId = Convert.ToInt64(reader["ParentId"]);
                                    detailsBO.ParentType = reader["ParentType"].ToString();
                                    detailsBO.TransectionType = reader["TransectionType"].ToString();
                                    detailsBO.Title = reader["Title"].ToString();
                                    detailsBO.Value = reader["Value"].ToString();

                                    contactInformationList.Add(detailsBO);
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
            return contactInformationList;
        }

        public List<ContactInformationBO> GetContactInformation()
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInfoAll_SP"))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }
        public List<ContactInformationBO> GetContactByAutoSearch(string searchTerm)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactByAutoSearch_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@searchTerm", DbType.String, searchTerm);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.WorkCountry = reader["WorkCountry"].ToString();
                                    contactInformation.WorkState = reader["WorkState"].ToString();
                                    contactInformation.WorkCity = reader["WorkCity"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformation.MobileWork = reader["MobileWork"].ToString();
                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }

        public List<ContactInformationBO> GetContactInformation(int isAdminUser, int userInfoId, string ContactName, int CompanyId, string ContactNo, string ContactEmail, Int64 countryId, Int64 stateId, Int64 cityId, Int64 areaId, int lifeCycleStage, int contactSource, Int32 contactOwnerId, Int32 dateSearchCriteria, DateTime fromDate, DateTime toDate, string contactType, string contactNumber, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInfoBySearchCriteriaForPaging_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                        dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                        if (!string.IsNullOrEmpty(ContactName))
                            dbSmartAspects.AddInParameter(cmd, "@ContactName", DbType.Int32, ContactName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactName", DbType.Int32, DBNull.Value);

                        if (CompanyId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(ContactNo))
                            dbSmartAspects.AddInParameter(cmd, "@ContactNo", DbType.String, ContactNo);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactNo", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(ContactEmail))
                            dbSmartAspects.AddInParameter(cmd, "@ContactEmail", DbType.String, ContactEmail);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactEmail", DbType.String, DBNull.Value);

                        if (countryId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.String, countryId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.String, DBNull.Value);

                        if (stateId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@StateId", DbType.String, stateId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@StateId", DbType.String, DBNull.Value);

                        if (cityId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@CityId", DbType.String, cityId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@CityId", DbType.String, DBNull.Value);

                        if (areaId > 0)
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.String, areaId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.String, DBNull.Value);

                        if (lifeCycleStage > 0)
                            dbSmartAspects.AddInParameter(cmd, "@LifeCycleStageId", DbType.String, lifeCycleStage);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@LifeCycleStageId", DbType.String, DBNull.Value);

                        if (contactSource > 0)
                            dbSmartAspects.AddInParameter(cmd, "@ContactSource", DbType.String, contactSource);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactSource", DbType.String, DBNull.Value);

                        if (contactOwnerId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ContactOwnerId", DbType.Int32, contactOwnerId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ContactOwnerId", DbType.Int32, DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(contactNumber))
                            dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, contactNumber);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(contactType))
                            dbSmartAspects.AddInParameter(cmd, "@ContactType", DbType.String, contactType);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ContactType", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@DateSearchCriteria", DbType.Int32, dateSearchCriteria);
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyName = reader["Company"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformation.ContactOwner = reader["AccountManager"].ToString();
                                    contactInformation.ContactOwnerId = Convert.ToInt32(reader["ContactOwnerId"]);
                                    contactInformation.Email = reader["Email"].ToString();
                                    contactInformation.EmailWork = reader["EmailWork"].ToString();
                                    contactInformation.MobilePersonal = reader["MobilePersonal"].ToString();
                                    contactInformation.MobileWork = reader["MobileWork"].ToString();
                                    contactInformation.JobTitle = reader["JobTitle"].ToString();
                                    contactInformation.ContactNumber = reader["ContactNumber"].ToString();
                                    contactInformation.ContactType = reader["ContactType"].ToString();
                                    contactInformation.LifeCycleStage = reader["LifeCycleStage"].ToString();
                                    if (reader["LastContactDateTime"] != DBNull.Value)
                                    {
                                        contactInformation.LastContactDateTime = Convert.ToDateTime(reader["LastContactDateTime"]);
                                    }

                                    contactInformation.AccountManager = reader["AccountManager"].ToString();
                                    contactInformation.CreatedDisplayDate = reader["CreatedDisplayDate"].ToString();
                                    contactInformation.IsDetailPanelEnableForContact = Convert.ToInt32(reader["IsDetailPanelEnableForContact"]);
                                    contactInformation.IsDetailPanelEnableForParentCompany = Convert.ToInt32(reader["IsDetailPanelEnableForParentCompany"]);

                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }

        public List<ContactInformationBO> GetContactInformationByCompanyId(int companyId)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInformationByCompanyId_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@companyId", DbType.Int32, companyId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    if (reader["ContactOwnerId"] != DBNull.Value)
                                        contactInformation.ContactOwnerId = Convert.ToInt32(reader["ContactOwnerId"]);
                                    else
                                    {
                                        contactInformation.ContactOwnerId = 0;
                                    }
                                    contactInformation.Email = reader["Email"].ToString();
                                    contactInformation.ContactNo = reader["ContactNo"].ToString();
                                    contactInformation.JobTitle = reader["JobTitle"].ToString();
                                    if (reader["LastContactDateTime"] != DBNull.Value)
                                    {
                                        contactInformation.LastContactDateTime = Convert.ToDateTime(reader["LastContactDateTime"]);

                                    }

                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }

        public List<ContactInformationBO> GetContactInformationByCompanyIdNSearchTextForAutoComplete(int companyId, string searchText)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInformationByCompanyIdNSearchTextForAutoComplete_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                        if (!string.IsNullOrWhiteSpace(searchText))
                            dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, searchText);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformation.ContactOwnerId = Convert.ToInt32(reader["ContactOwnerId"]);
                                    contactInformation.Email = reader["Email"].ToString();
                                    contactInformation.ContactNo = reader["ContactNo"].ToString();
                                    contactInformation.JobTitle = reader["JobTitle"].ToString();
                                    contactInformation.EmailWork = reader["EmailWork"].ToString();
                                    //contactInformation.PersonalAddress = reader["PersonalAddress"].ToString();
                                    contactInformation.WorkAddress = reader["WorkAddress"].ToString();
                                    //contactInformation.MobilePersonal = reader["MobilePersonal"].ToString();
                                    //if (reader["LastContactDateTime"] != DBNull.Value)
                                    //{
                                    //    contactInformation.LastContactDateTime = Convert.ToDateTime(reader["LastContactDateTime"]);

                                    //}

                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }


        public ContactInformationBO GetContactInformationById(long id)
        {
            ContactInformationBO contactInformation = new ContactInformationBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInformationById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyName = reader["Company"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformation.ContactOwner = reader["ContactOwner"].ToString();
                                    contactInformation.ContactOwnerId = Convert.ToInt32(reader["ContactOwnerId"]);
                                    contactInformation.SourceId = Convert.ToInt32(reader["SourceId"]);
                                    contactInformation.LifeCycleId = Convert.ToInt32(reader["LifeCycleId"]);
                                    contactInformation.LifeCycleStage = reader["LifeCycleStage"].ToString();
                                    contactInformation.ContactType = (reader["ContactType"]).ToString();
                                    contactInformation.SourceName = reader["SourceName"].ToString();
                                    contactInformation.Email = reader["Email"].ToString();
                                    contactInformation.EmailWork = reader["EmailWork"].ToString();
                                    contactInformation.ContactNo = reader["ContactNo"].ToString();
                                    contactInformation.JobTitle = reader["JobTitle"].ToString();
                                    contactInformation.Department = reader["Department"].ToString();
                                    contactInformation.TicketNo = reader["TicketNo"].ToString();

                                    contactInformation.MobilePersonal = reader["MobilePersonal"].ToString();
                                    contactInformation.MobileWork = reader["MobileWork"].ToString();
                                    contactInformation.PhonePersonal = reader["PhonePersonal"].ToString();
                                    contactInformation.PhoneWork = reader["PhoneWork"].ToString();

                                    contactInformation.Facebook = reader["Facebook"].ToString();
                                    contactInformation.Whatsapp = reader["Whatsapp"].ToString();
                                    contactInformation.Skype = reader["Skype"].ToString();
                                    contactInformation.Twitter = reader["Twitter"].ToString();

                                    contactInformation.WorkAddress = reader["WorkAddress"].ToString();
                                    contactInformation.WorkCountryId = Convert.ToInt32(reader["WorkCountryId"]);
                                    contactInformation.WorkStateId = Convert.ToInt32(reader["WorkStateId"]);
                                    contactInformation.WorkCityId = Convert.ToInt32(reader["WorkCityId"]);
                                    contactInformation.WorkCity = reader["WorkCity"].ToString();
                                    contactInformation.WorkState = reader["WorkState"].ToString();
                                    contactInformation.WorkCountry = reader["WorkCountry"].ToString();
                                    contactInformation.WorkLocationId = Convert.ToInt32(reader["WorkLocationId"]);
                                    contactInformation.WorkStreet = reader["WorkStreet"].ToString();
                                    contactInformation.WorkPostCode = reader["WorkPostCode"].ToString();
                                    contactInformation.PersonalAddress = reader["PersonalAddress"].ToString();
                                    contactInformation.PersonalCountryId = Convert.ToInt32(reader["PersonalCountryId"]);
                                    contactInformation.PersonalStateId = Convert.ToInt32(reader["PersonalStateId"]);
                                    contactInformation.PersonalCityId = Convert.ToInt32(reader["PersonalCityId"]);
                                    contactInformation.PersonalLocationId = Convert.ToInt32(reader["PersonalLocationId"]);
                                    contactInformation.PersonalStreet = reader["PersonalStreet"].ToString();
                                    contactInformation.PersonalPostCode = reader["PersonalPostCode"].ToString();
                                    contactInformation.PersonalCountry = reader["PersonalCountry"].ToString();
                                    contactInformation.PersonalCity = reader["PersonalCity"].ToString();
                                    contactInformation.PersonalState = reader["PersonalState"].ToString();

                                    if (reader["DOB"] != DBNull.Value)
                                    {
                                        contactInformation.DOB = Convert.ToDateTime(reader["DOB"]);
                                    }
                                    if (reader["DateAnniversary"] != DBNull.Value)
                                    {
                                        contactInformation.DateAnniversary = Convert.ToDateTime(reader["DateAnniversary"]);
                                    }
                                    if (reader["LastContactDateTime"] != DBNull.Value)
                                    {
                                        contactInformation.LastContactDateTime = Convert.ToDateTime(reader["LastContactDateTime"]);
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
            return contactInformation;
        }

        public List<ContactInformationBO> GetContactInfoForAutoComplete(string contactName, int companyId, string contactType)
        {
            List<ContactInformationBO> contactList = new List<ContactInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInfoForAutoComplete_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ContactName", DbType.String, contactName);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    if (contactType == "")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactType", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ContactType", DbType.String, contactType);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ContactInformationBO contactInformation = new ContactInformationBO();

                                contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                contactInformation.Name = reader["Name"].ToString();
                                contactInformation.CompanyName = reader["Company"].ToString();
                                contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                contactInformation.ContactOwner = reader["ContactOwner"].ToString();
                                contactInformation.ContactOwnerId = Convert.ToInt32(reader["ContactOwnerId"]);
                                contactInformation.SourceId = Convert.ToInt32(reader["SourceId"]);
                                contactInformation.LifeCycleId = Convert.ToInt32(reader["LifeCycleId"]);
                                contactInformation.LifeCycleStage = reader["LifeCycleStage"].ToString();
                                contactInformation.ContactType = (reader["ContactType"]).ToString();
                                contactInformation.SourceName = reader["SourceName"].ToString();
                                contactInformation.Email = reader["Email"].ToString();
                                contactInformation.EmailWork = reader["EmailWork"].ToString();
                                contactInformation.ContactNo = reader["ContactNo"].ToString();
                                contactInformation.JobTitle = reader["JobTitle"].ToString();
                                contactInformation.Department = reader["Department"].ToString();
                                contactInformation.TicketNo = reader["TicketNo"].ToString();

                                contactInformation.MobilePersonal = reader["MobilePersonal"].ToString();
                                contactInformation.MobileWork = reader["MobileWork"].ToString();
                                contactInformation.PhonePersonal = reader["PhonePersonal"].ToString();
                                contactInformation.PhoneWork = reader["PhoneWork"].ToString();

                                contactInformation.Facebook = reader["Facebook"].ToString();
                                contactInformation.Whatsapp = reader["Whatsapp"].ToString();
                                contactInformation.Skype = reader["Skype"].ToString();
                                contactInformation.Twitter = reader["Twitter"].ToString();

                                contactInformation.PersonalAddress = reader["PersonalAddress"].ToString();
                                contactInformation.WorkAddress = reader["WorkAddress"].ToString();

                                if (reader["DOB"] != DBNull.Value)
                                {
                                    contactInformation.DOB = Convert.ToDateTime(reader["DOB"]);

                                }
                                if (reader["DateAnniversary"] != DBNull.Value)
                                {
                                    contactInformation.DateAnniversary = Convert.ToDateTime(reader["DateAnniversary"]);

                                }
                                if (reader["LastContactDateTime"] != DBNull.Value)
                                {
                                    contactInformation.LastContactDateTime = Convert.ToDateTime(reader["LastContactDateTime"]);

                                }

                                contactList.Add(contactInformation);
                            }
                        }
                    }
                }
            }
            return contactList;
        }
        public SMContactInformationViewBO GetContactInformationByIdForView(long id)
        {
            SMContactInformationViewBO contactInformation = new SMContactInformationViewBO();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactInformationByIdForView_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "ContactInfo");

                        contactInformation = ds.Tables[0].AsEnumerable().Select(r => new SMContactInformationViewBO()
                        {
                            Id = r.Field<long>("Id"),
                            Name = r.Field<string>("Name"),
                            ContactOwner = r.Field<string>("ContactOwner"),
                            CompanyName = r.Field<string>("CompanyName"),
                            CompanyId = r.Field<int?>("CompanyId"),
                            Industry = r.Field<string>("Industry"),
                            CompanyPhone = r.Field<string>("CompanyPhone"),
                            CompanyEmail = r.Field<string>("CompanyEmail"),
                            CompanyWebsite = r.Field<string>("CompanyWebsite"),
                            CompanyAddress = r.Field<string>("CompanyAddress"),
                            CompanyLifeCycleStage = r.Field<string>("CompanyLifeCycleStage"),
                            ContactLifeCycleStage = r.Field<string>("ContactLifeCycleStage"),
                            JobTitle = r.Field<string>("JobTitle"),
                            Department = r.Field<string>("Department"),
                            EmailWork = r.Field<string>("EmailWork"),

                            Email = r.Field<string>("Email"),
                            MobilePersonal = r.Field<string>("MobilePersonal"),
                            SocialMedia = r.Field<string>("SocialMedia"),
                            Website = r.Field<string>("Website"),

                            MobileWork = r.Field<string>("MobileWork"),
                            PhonePersonal = r.Field<string>("PhonePersonal"),
                            PhoneWork = r.Field<string>("PhoneWork"),

                            Facebook = r.Field<string>("Facebook"),
                            Whatsapp = r.Field<string>("Whatsapp"),
                            Skype = r.Field<string>("Skype"),
                            Twitter = r.Field<string>("Twitter"),
                            PersonalAddress = r.Field<string>("PersonalAddress"),
                            WorkAddress = r.Field<string>("WorkAddress"),
                            DOB = r.Field<DateTime?>("DOB"),
                            DateAnniversary = r.Field<DateTime?>("DateAnniversary"),
                            LastActivityDateTime = r.Field<DateTime>("LastActivityDateTime")
                        }).FirstOrDefault();

                        contactInformation.Deals = ds.Tables[1].AsEnumerable().Select(r => new SMDealView()
                        {
                            DealId = r.Field<long>("DealId"),
                            Name = r.Field<string>("Name"),
                            Amount = r.Field<decimal?>("Amount"),
                            Stage = r.Field<string>("Stage"),
                            Complete = r.Field<decimal>("Complete")

                        }).ToList();

                        contactInformation.PastCompanys = ds.Tables[2].AsEnumerable().Select(r => new SMContactInformationViewBO()
                        {
                            CompanyId = r.Field<int?>("PreviousCompanyId"),
                            JobTitle = r.Field<string>("Title"),
                            Department = r.Field<string>("Department"),
                            CompanyName = r.Field<string>("CompanyName")

                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contactInformation;
        }
        public bool DeleteContact(long Id)
        {
            bool status;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteContact_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, Id);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            return status;

        }

        public bool DeleteCompanyFromContact(long contactId)
        {
            bool status;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteCompanyFromContact_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, contactId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                        if (status)
                        {
                            transction.Commit();
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

        // Documents //
        public Boolean DeleteContactDocument(string deletedDocumentId)
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
                                for (int i = 0; i < documentId.Count(); i++)
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

        public Boolean TransferContact(SMContactTransfer contact, bool isUpdateContact, out long id)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveContactTransferInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, contact.ContactId);
                        dbSmartAspects.AddInParameter(command, "@PreviousCompanyId", DbType.Int32, contact.PreviousCompanyId);
                        dbSmartAspects.AddInParameter(command, "@TransferredCompanyId", DbType.Int32, contact.TransferredCompanyId);
                        dbSmartAspects.AddInParameter(command, "@Title", DbType.String, contact.Title);
                        dbSmartAspects.AddInParameter(command, "@Department", DbType.String, contact.Department);
                        dbSmartAspects.AddInParameter(command, "@Mobile", DbType.String, contact.Mobile);
                        dbSmartAspects.AddInParameter(command, "@Phone", DbType.String, contact.Phone);
                        dbSmartAspects.AddInParameter(command, "@Email", DbType.String, contact.Email);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, contact.CreatedBy);
                        dbSmartAspects.AddInParameter(command, "@IsUpdateContact", DbType.Boolean, isUpdateContact);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            return status;
        }
        public List<ContactInformationBO> GetContactByCompanyIdNAccountManager(int accountManagerId, int companyId, string searchText)
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetContactByCompanyIdNAccountManager_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                        if (accountManagerId != 0)
                            dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, accountManagerId);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrWhiteSpace(searchText))
                            dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, searchText);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ContactInformationBO contactInformation = new ContactInformationBO();

                                    contactInformation.Id = Convert.ToInt64(reader["Id"]);
                                    contactInformation.Name = reader["Name"].ToString();
                                    contactInformation.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    contactInformation.ContactOwnerId = Convert.ToInt32(reader["ContactOwnerId"]);
                                    contactInformation.Email = reader["Email"].ToString();
                                    contactInformation.ContactNo = reader["ContactNo"].ToString();
                                    contactInformation.JobTitle = reader["JobTitle"].ToString();
                                    contactInformation.EmailWork = reader["EmailWork"].ToString();
                                    contactInformation.EmailWork = reader["EmailWork"].ToString();
                                    contactInformation.PersonalAddress = reader["PersonalAddress"].ToString();
                                    contactInformation.WorkAddress = reader["WorkAddress"].ToString();
                                    contactInformation.MobilePersonal = reader["MobilePersonal"].ToString();
                                    if (reader["LastContactDateTime"] != DBNull.Value)
                                    {
                                        contactInformation.LastContactDateTime = Convert.ToDateTime(reader["LastContactDateTime"]);

                                    }
                                    contactInformation.WorkCountry = reader["WorkCountry"].ToString();
                                    contactInformation.WorkState = reader["WorkState"].ToString();
                                    contactInformation.WorkCity = reader["WorkCity"].ToString();
                                    contactInformation.MobileWork = reader["MobileWork"].ToString();
                                    contactInformationList.Add(contactInformation);
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
            return contactInformationList;
        }
    }
}

