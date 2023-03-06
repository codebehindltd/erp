using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestCompanyDA : BaseService
    {
        public List<GuestCompanyBO> GetGuestCompanyInfo()
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();

                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }

        public List<GuestCompanyBO> GetCompanyInfoForAirTicket(string searchTerm)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            using(DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using(DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInfoForAirTicket_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);
                    using(IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if(reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                companyList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return companyList;
        }
        public List<GuestCompanyBO> GetCompanyInfoForAccountApproval(string searchTerm)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInfoForAirTicket_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                companyList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return companyList;
        }
        public GuestCompanyBO GetCompanyBenefitsForFillForm(int companyId)
        {
            GuestCompanyBO companyInfo = new GuestCompanyBO();
            companyInfo.CompanyId = companyId;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInformationByCompanyId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                companyInfo.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                                if (reader["CreditLimitExpire"] != DBNull.Value)
                                    companyInfo.CreditLimitExpire = Convert.ToDateTime(reader["CreditLimitExpire"]);
                                if (reader["ShortCreditLimit"] != DBNull.Value)
                                    companyInfo.ShortCreditLimit = Convert.ToDecimal(reader["ShortCreditLimit"]);
                                if (reader["ShortCreditLimitExpire"] != DBNull.Value)
                                    companyInfo.ShortCreditLimitExpire = Convert.ToDateTime(reader["ShortCreditLimitExpire"]);
                                if (reader["TransportFareFactory"] != DBNull.Value)
                                    companyInfo.TransportFareFactory = Convert.ToDecimal(reader["TransportFareFactory"]);
                                if (reader["TransportFareDepo"] != DBNull.Value)
                                    companyInfo.TransportFareDepo = Convert.ToDecimal(reader["TransportFareDepo"]);
                                if (reader["SalesCommission"] != DBNull.Value)
                                    companyInfo.SalesCommission = Convert.ToDecimal(reader["SalesCommission"]);
                                if (reader["LegalAction"] != DBNull.Value)
                                    companyInfo.LegalAction = reader["LegalAction"].ToString();
                            }
                        }
                    }
                }
            }
            return companyInfo;
        }

        public List<GuestCompanyBO> GetCompanyLegalActionForFillForm(int companyId)
        {
            List<GuestCompanyBO> legalActionList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLegalActionInfoByCompanyId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO legalAction = new GuestCompanyBO();
                                legalAction.Id = Convert.ToInt64(reader["Id"]);
                                legalAction.CompanyId = companyId;
                                if (reader["TransactionDate"] != DBNull.Value)
                                    legalAction.TransactionDate = Convert.ToDateTime(reader["TransactionDate"]);
                                if (reader["Remarks"] != DBNull.Value)
                                    legalAction.DetailDescription = reader["Remarks"].ToString();
                                if (reader["CallToAction"] != DBNull.Value)
                                    legalAction.CallToAction = reader["CallToAction"].ToString();
                                legalActionList.Add(legalAction);
                            }
                        }
                    }
                }
            }
            return legalActionList;
        }
        public GuestCompanyBO GetLastLegalActionId()
        {
            GuestCompanyBO guestCompany = new GuestCompanyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLastLegalActionId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestCompany.LastId = Convert.ToInt64(reader["LastId"]);
                            }
                        }
                    }
                }
            }
            return guestCompany;
        }

        public List<GuestCompanyBO> GetGuestCompanyInfoByUserId(int userInfoId)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoByUserId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int64, userInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();

                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<GuestCompanyBO> GetGuestGroupNameInfo()
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestGroupNameInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.GroupName = reader["GroupName"].ToString();
                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<GuestCompanyBO> GetAffiliatedGuestCompanyInfo()
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAffiliatedGuestCompanyInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();

                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public Boolean SaveGuestCompanyInfo(GuestCompanyBO guestCompany, int[] CRMCompanyIds, out int tmpCompanyId)
        {
            Boolean status = false;
            Boolean statusEmail = false;
            Boolean statusWeb = false;
            Boolean statusPhone = false;
            Boolean statusFax = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestCompanyInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyName", DbType.String, guestCompany.CompanyName);
                        // AncestorId OwnershipId CompanyType  TicketNumber 
                        //dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, guestCompany.CompanyAddress);
                        dbSmartAspects.AddInParameter(command, "@EmailAddress", DbType.String, guestCompany.EmailAddress);
                        dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, guestCompany.WebAddress);
                        dbSmartAspects.AddInParameter(command, "@ContactNumber", DbType.String, guestCompany.ContactNumber);
                        dbSmartAspects.AddInParameter(command, "@Fax", DbType.String, guestCompany.Fax);
                        //dbSmartAspects.AddInParameter(command, "@ContactDesignation", DbType.String, guestCompany.ContactDesignation);
                        //dbSmartAspects.AddInParameter(command, "@TelephoneNumber", DbType.String, guestCompany.TelephoneNumber);
                        //dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, guestCompany.ContactPerson);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestCompany.Remarks);
                        dbSmartAspects.AddInParameter(command, "@BranchCode", DbType.String, guestCompany.BranchCode);
                        //dbSmartAspects.AddInParameter(command, "@SignupStatusId", DbType.String, guestCompany.SignupStatusId);
                        //dbSmartAspects.AddInParameter(command, "@AffiliatedDate", DbType.DateTime, guestCompany.AffiliatedDate);
                        dbSmartAspects.AddInParameter(command, "@DiscountPercent", DbType.Decimal, guestCompany.DiscountPercent);
                        dbSmartAspects.AddInParameter(command, "@CreditLimit", DbType.Decimal, guestCompany.CreditLimit);
                        dbSmartAspects.AddInParameter(command, "@CompanyOwnerId", DbType.Int32, guestCompany.CompanyOwnerId);
                        //dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, guestCompany.LocationId);
                        dbSmartAspects.AddInParameter(command, "@IndustryId", DbType.Int32, guestCompany.IndustryId);
                        //dbSmartAspects.AddInParameter(command, "@IsMember", DbType.Boolean, false);
                        dbSmartAspects.AddInParameter(command, "@AnnualRevenue", DbType.Decimal, guestCompany.AnnualRevenue);
                        dbSmartAspects.AddInParameter(command, "@NumberOfEmployee", DbType.Int32, guestCompany.NumberOfEmployee);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, guestCompany.CreatedBy);
                        //new add
                        dbSmartAspects.AddInParameter(command, "@AncestorId", DbType.Int32, guestCompany.AncestorId);
                        dbSmartAspects.AddInParameter(command, "@OwnershipId", DbType.Int32, guestCompany.OwnershipId);
                        dbSmartAspects.AddInParameter(command, "@CompanyType", DbType.Int32, guestCompany.CompanyType);
                        dbSmartAspects.AddInParameter(command, "@TicketNumber", DbType.String, guestCompany.TicketNumber);
                        dbSmartAspects.AddInParameter(command, "@LifeCycleStageId", DbType.Int64, guestCompany.LifeCycleStageId);

                        dbSmartAspects.AddInParameter(command, "@BillingStreet", DbType.String, guestCompany.BillingStreet);
                        dbSmartAspects.AddInParameter(command, "@BillingPostCode", DbType.String, guestCompany.BillingPostCode);
                        dbSmartAspects.AddInParameter(command, "@BillingCountryId", DbType.Int32, guestCompany.BillingCountryId);
                        dbSmartAspects.AddInParameter(command, "@BillingCityId", DbType.Int32, guestCompany.BillingCityId);
                        dbSmartAspects.AddInParameter(command, "@BillingStateId", DbType.Int32, guestCompany.BillingStateId);

                        dbSmartAspects.AddInParameter(command, "@ShippingStreet", DbType.String, guestCompany.ShippingStreet);
                        dbSmartAspects.AddInParameter(command, "@ShippingPostCode", DbType.String, guestCompany.ShippingPostCode);
                        dbSmartAspects.AddInParameter(command, "@ShippingCountryId", DbType.Int64, guestCompany.ShippingCountryId);
                        dbSmartAspects.AddInParameter(command, "@ShippingCityId", DbType.Int64, guestCompany.ShippingCityId);
                        dbSmartAspects.AddInParameter(command, "@ShippingStateId", DbType.Int64, guestCompany.ShippingStateId);

                        dbSmartAspects.AddInParameter(command, "@BillingLocationId", DbType.Int64, guestCompany.BillingLocationId);
                        dbSmartAspects.AddInParameter(command, "@ShippingLocationId", DbType.Int64, guestCompany.ShippingLocationId);

                        dbSmartAspects.AddInParameter(command, "@BillingAddress", DbType.String, guestCompany.BillingAddress);
                        dbSmartAspects.AddInParameter(command, "@ShippingAddress", DbType.String, guestCompany.ShippingAddress);

                        dbSmartAspects.AddOutParameter(command, "@CompanyId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpCompanyId = Convert.ToInt32(command.Parameters["@CompanyId"].Value);


                        HMCommonDA hmCommonDA = new HMCommonDA();
                        Boolean uploadStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(tmpCompanyId, guestCompany.RandomProductId);
                    }

                    if (status == true)
                    {
                        if (CRMCompanyIds != null && CRMCompanyIds.Length > 0)
                        {
                            string companyIdsStringDelimitted = string.Join(",", CRMCompanyIds.Select(x => x.ToString()).ToArray());

                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SMGLCompanyAndGuestCompanyMapping_Save_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, tmpCompanyId);
                                dbSmartAspects.AddInParameter(command, "@GLCompanyIds", DbType.String, companyIdsStringDelimitted);

                                bool saveStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                        //if (detailsView.Eamils.Count > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyContactDetails_SP"))
                        //    {
                        //        foreach (var item in detailsView.Eamils)
                        //        {
                        //            command.Parameters.Clear();
                        //            dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, tmpCompanyId);
                        //            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, item.TransactionType);
                        //            dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, item.FieldName);
                        //            dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, item.FieldValue);

                        //            statusEmail = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //        }
                        //    }
                        //}
                        //if (detailsView.Webs.Count > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyContactDetails_SP"))
                        //    {
                        //        foreach (var item in detailsView.Webs)
                        //        {
                        //            command.Parameters.Clear();
                        //            dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, tmpCompanyId);
                        //            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, item.TransactionType);
                        //            dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, item.FieldName);
                        //            dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, item.FieldValue);

                        //            statusWeb = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //        }
                        //    }
                        //}
                        //if (detailsView.Phones.Count > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyContactDetails_SP"))
                        //    {
                        //        foreach (var item in detailsView.Phones)
                        //        {
                        //            command.Parameters.Clear();
                        //            dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, tmpCompanyId);
                        //            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, item.TransactionType);
                        //            dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, item.FieldName);
                        //            dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, item.FieldValue);

                        //            statusPhone = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //        }
                        //    }
                        //}
                        //if (detailsView.Faxs.Count > 0)
                        //{
                        //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyContactDetails_SP"))
                        //    {
                        //        foreach (var item in detailsView.Faxs)
                        //        {
                        //            command.Parameters.Clear();
                        //            dbSmartAspects.AddInParameter(command, "@TransactionId", DbType.Int64, tmpCompanyId);
                        //            dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, item.TransactionType);
                        //            dbSmartAspects.AddInParameter(command, "@FieldName", DbType.String, item.FieldName);
                        //            dbSmartAspects.AddInParameter(command, "@FieldValue", DbType.String, item.FieldValue);

                        //            statusFax = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean SaveGuestCompanyInfo(GuestCompanyBO guestCompany, out int tmpCompanyId)
        {
            return this.SaveGuestCompanyInfo(guestCompany, null, out tmpCompanyId);
        }
        public Boolean UpdateGuestCompanyInfo(GuestCompanyBO guestCompany, int[] CRMCompanyIds)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestCompanyInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, guestCompany.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@CompanyName", DbType.String, guestCompany.CompanyName);
                        dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, guestCompany.CompanyAddress);
                        dbSmartAspects.AddInParameter(command, "@EmailAddress", DbType.String, guestCompany.EmailAddress);
                        dbSmartAspects.AddInParameter(command, "@WebAddress", DbType.String, guestCompany.WebAddress);

                        dbSmartAspects.AddInParameter(command, "@ContactNumber", DbType.String, guestCompany.ContactNumber);
                        //dbSmartAspects.AddInParameter(command, "@ContactDesignation", DbType.String, guestCompany.ContactDesignation);
                        //dbSmartAspects.AddInParameter(command, "@TelephoneNumber", DbType.String, guestCompany.TelephoneNumber);
                        //dbSmartAspects.AddInParameter(command, "@ContactPerson", DbType.String, guestCompany.ContactPerson);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, guestCompany.Remarks);
                        dbSmartAspects.AddInParameter(command, "@BranchCode", DbType.String, guestCompany.BranchCode);
                        dbSmartAspects.AddInParameter(command, "@Fax", DbType.String, guestCompany.Fax);

                        //dbSmartAspects.AddInParameter(command, "@SignupStatusId", DbType.String, guestCompany.SignupStatusId);
                        //dbSmartAspects.AddInParameter(command, "@AffiliatedDate", DbType.DateTime, guestCompany.AffiliatedDate);
                        dbSmartAspects.AddInParameter(command, "@DiscountPercent", DbType.Decimal, guestCompany.DiscountPercent);
                        dbSmartAspects.AddInParameter(command, "@CreditLimit", DbType.Decimal, guestCompany.CreditLimit);
                        dbSmartAspects.AddInParameter(command, "@CompanyOwnerId", DbType.Int32, guestCompany.CompanyOwnerId);
                        //dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, guestCompany.LocationId);
                        dbSmartAspects.AddInParameter(command, "@IndustryId", DbType.Int32, guestCompany.IndustryId);
                        //dbSmartAspects.AddInParameter(command, "@IsMember", DbType.Boolean, false);
                        dbSmartAspects.AddInParameter(command, "@AnnualRevenue", DbType.Decimal, guestCompany.AnnualRevenue);
                        dbSmartAspects.AddInParameter(command, "@NumberOfEmployee", DbType.Int32, guestCompany.NumberOfEmployee);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, guestCompany.LastModifiedBy);

                        //new added 
                        dbSmartAspects.AddInParameter(command, "@AncestorId", DbType.Int32, guestCompany.AncestorId);
                        dbSmartAspects.AddInParameter(command, "@OwnershipId", DbType.Int32, guestCompany.OwnershipId);
                        dbSmartAspects.AddInParameter(command, "@CompanyType", DbType.Int32, guestCompany.CompanyType);
                        dbSmartAspects.AddInParameter(command, "@TicketNumber", DbType.String, guestCompany.TicketNumber);
                        dbSmartAspects.AddInParameter(command, "@LifeCycleStageId", DbType.Int64, guestCompany.LifeCycleStageId);

                        dbSmartAspects.AddInParameter(command, "@BillingStreet", DbType.String, guestCompany.BillingStreet);
                        dbSmartAspects.AddInParameter(command, "@BillingPostCode", DbType.String, guestCompany.BillingPostCode);
                        dbSmartAspects.AddInParameter(command, "@BillingCountryId", DbType.Int32, guestCompany.BillingCountryId);
                        dbSmartAspects.AddInParameter(command, "@BillingCityId", DbType.Int32, guestCompany.BillingCityId);
                        dbSmartAspects.AddInParameter(command, "@BillingStateId", DbType.Int32, guestCompany.BillingStateId);

                        dbSmartAspects.AddInParameter(command, "@ShippingStreet", DbType.String, guestCompany.ShippingStreet);
                        dbSmartAspects.AddInParameter(command, "@ShippingPostCode", DbType.String, guestCompany.ShippingPostCode);
                        dbSmartAspects.AddInParameter(command, "@ShippingCountryId", DbType.Int32, guestCompany.ShippingCountryId);
                        dbSmartAspects.AddInParameter(command, "@ShippingCityId", DbType.Int32, guestCompany.ShippingCityId);
                        dbSmartAspects.AddInParameter(command, "@ShippingStateId", DbType.Int32, guestCompany.ShippingStateId);

                        dbSmartAspects.AddInParameter(command, "@BillingLocationId", DbType.Int64, guestCompany.BillingLocationId);
                        dbSmartAspects.AddInParameter(command, "@ShippingLocationId", DbType.Int64, guestCompany.ShippingLocationId);

                        dbSmartAspects.AddInParameter(command, "@BillingAddress", DbType.String, guestCompany.BillingAddress);
                        dbSmartAspects.AddInParameter(command, "@ShippingAddress", DbType.String, guestCompany.ShippingAddress);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                    if (status == true)
                    {
                        if (CRMCompanyIds != null && CRMCompanyIds.Length > 0)
                        {
                            string companyIdsStringDelimitted = string.Join(",", CRMCompanyIds.Select(x => x.ToString()).ToArray());

                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SMGLCompanyAndGuestCompanyMapping_Save_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, guestCompany.CompanyId);
                                dbSmartAspects.AddInParameter(command, "@GLCompanyIds", DbType.String, companyIdsStringDelimitted);

                                bool saveStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
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
        public Boolean UpdateGuestCompanyInfo(GuestCompanyBO guestCompany)
        {
            return this.UpdateGuestCompanyInfo(guestCompany, null);
        }
        public bool CompanyDeleteValidation(Int64 companyId)
        {
            Boolean status = true;
            Boolean deleteStatus = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CompanyDeleteValidation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                        dbSmartAspects.AddOutParameter(command, "@DataCount", DbType.Int64, sizeof(Int32));

                        deleteStatus = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        Int64 dataCount = Convert.ToInt64(command.Parameters["@DataCount"].Value);
                        if (dataCount > 0)
                        {
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
        public GuestCompanyBO GetGuestCompanyInfoById(int companyId)
        {
            GuestCompanyBO guestCompany = new GuestCompanyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.EmailAddressWithoutLabel = reader["EmailAddressWithoutLabel"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactNumberWithoutLabel = reader["ContactNumberWithoutLabel"].ToString();
                                guestCompany.Fax = reader["Fax"].ToString();
                                //guestCompany.ContactDesignation = reader["ContactDesignation"].ToString();
                                //guestCompany.TelephoneNumber = reader["TelephoneNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.BranchCode = reader["BranchCode"].ToString();
                                //guestCompany.SignupStatus = reader["SignupStatus"].ToString();

                                guestCompany.SignupStatusId = Convert.ToInt32(reader["SignupStatusId"]);
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.CreditLimit = Convert.ToDecimal(reader["CreditLimit"]);
                                guestCompany.CompanyOwnerId = Convert.ToInt32(reader["CompanyOwnerId"]);

                                guestCompany.MatrixInfo = Convert.ToString(reader["MatrixInfo"]);
                                guestCompany.LocationId = Convert.ToInt32(reader["LocationId"]);
                                guestCompany.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);

                                //new add
                                guestCompany.CompanyOwnerName = Convert.ToString(reader["CompanyOwnerName"]);
                                guestCompany.TypeName = Convert.ToString(reader["TypeName"]);
                                guestCompany.IndustryName = Convert.ToString(reader["IndustryName"]);
                                guestCompany.OwnershipName = Convert.ToString(reader["OwnershipName"]);
                                guestCompany.LifeCycleStage = reader["LifeCycleStage"].ToString();

                                guestCompany.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                guestCompany.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                guestCompany.AncestorId = Convert.ToInt32(reader["AncestorId"]);

                                guestCompany.CompanyType = Convert.ToInt32(reader["CompanyType"]);
                                guestCompany.OwnershipId = Convert.ToInt32(reader["OwnershipId"]);
                                guestCompany.LifeCycleStageId = Convert.ToInt32(reader["LifeCycleStageId"]);


                                guestCompany.BillingCountryId = Convert.ToInt32(reader["BillingCountryId"]);
                                guestCompany.BillingStateId = Convert.ToInt32(reader["BillingStateId"]);
                                guestCompany.BillingCityId = Convert.ToInt32(reader["BillingCityId"]);
                                guestCompany.BillingCity = reader["BillingCity"].ToString();
                                guestCompany.BillingState = reader["BillingState"].ToString();
                                guestCompany.BillingCountry = reader["BillingCountry"].ToString();
                                guestCompany.BillingLocationId = Convert.ToInt32(reader["BillingLocationId"]);
                                guestCompany.BillingStreet = reader["BillingStreet"].ToString();
                                guestCompany.BillingPostCode = reader["BillingPostCode"].ToString();

                                guestCompany.ShippingCountryId = Convert.ToInt32(reader["ShippingCountryId"]);
                                guestCompany.ShippingStateId = Convert.ToInt32(reader["ShippingStateId"]);
                                guestCompany.ShippingCityId = Convert.ToInt32(reader["ShippingCityId"]);
                                guestCompany.ShippingLocationId = Convert.ToInt32(reader["ShippingLocationId"]);
                                guestCompany.ShippingStreet = reader["ShippingStreet"].ToString();
                                guestCompany.ShippingPostCode = reader["ShippingPostCode"].ToString();
                                guestCompany.ShippingCountry = reader["ShippingCountry"].ToString();
                                guestCompany.ShippingCity = reader["ShippingCity"].ToString();
                                guestCompany.ShippingState = reader["ShippingState"].ToString();

                                guestCompany.TicketNumber = reader["TicketNumber"].ToString();
                                guestCompany.BillingAddress = reader["BillingAddress"].ToString();
                                guestCompany.ShippingAddress = reader["ShippingAddress"].ToString();


                                if (reader["NumberOfEmployee"] != DBNull.Value)
                                    guestCompany.NumberOfEmployee = Convert.ToInt32(reader["NumberOfEmployee"]);
                                if (reader["AnnualRevenue"] != DBNull.Value)
                                    guestCompany.AnnualRevenue = Convert.ToDecimal(reader["AnnualRevenue"]);
                                guestCompany.IndustryName = reader["IndustryName"].ToString();
                                //if(reader["DealStageWiseCompanyStatusId"] != DBNull.Value)
                                //    guestCompany.DealStageWiseCompanyStatusId = Convert.ToInt64(reader["DealStageWiseCompanyStatusId"]);
                                //guestCompany.DealStageWiseCompanyStatus = reader["DealStageWiseCompanyStatus"].ToString();
                                guestCompany.IsClient = Convert.ToBoolean(reader["IsClient"]);
                            }
                        }
                    }
                }
            }
            return guestCompany;
        }
        public List<int> GetGuestCRMCompanyInfoById(int companyId)
        {
            List<int> CRMCompanyIds = new List<int>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSMGLCompanyAndGuestCompanyMappingInfoById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CRMCompanyIds.Add(Convert.ToInt32(reader["GLCompanyId"]));
                            }
                        }
                    }
                }
            }
            return CRMCompanyIds;
        }
        //write get method details GetCompanyContactDetailsByCriteria_SP
        public List<HotelCompanyContactDetailsBO> GetCompanyContactDetailsByCriteria(int transId, string transType, string fieldName)
        {
            List<HotelCompanyContactDetailsBO> detailsBOs = new List<HotelCompanyContactDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyContactDetailsByCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(transType))
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, DBNull.Value);
                    if (!string.IsNullOrEmpty(fieldName))
                        dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, fieldName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FieldName", DbType.String, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int64, transId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelCompanyContactDetailsBO bO = new HotelCompanyContactDetailsBO();
                                bO.Id = Convert.ToInt32(reader["Id"]);
                                bO.TransactionId = Convert.ToInt32(reader["TransactionId"]);
                                bO.FieldName = reader["FieldName"].ToString();
                                bO.TransactionType = reader["TransactionType"].ToString();
                                bO.FieldValue = reader["FieldValue"].ToString();

                                detailsBOs.Add(bO);
                            }
                        }
                    }
                }
            }
            return detailsBOs;
        }
        public GuestCompanyBO GetGuestCompanyInfoForSalesCallById(int companyId)
        {
            GuestCompanyBO guestCompany = new GuestCompanyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoForSalesCallById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactDesignation = reader["ContactDesignation"].ToString();
                                guestCompany.TelephoneNumber = reader["TelephoneNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.SignupStatus = reader["SignupStatus"].ToString();
                                if (reader["DiscountPercent"] != DBNull.Value)
                                    guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.CompanyOwnerId = Convert.ToInt32(reader["CompanyOwnerId"]);
                                guestCompany.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);

                                if (!string.IsNullOrEmpty(reader["LocationId"].ToString()))
                                    guestCompany.LocationId = Convert.ToInt32(reader["LocationId"]);
                                else
                                    guestCompany.LocationId = 0;

                                if (!string.IsNullOrEmpty(reader["Balance"].ToString()))
                                    guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);
                                else
                                    guestCompany.Balance = 0.00M;

                                guestCompany.FirstInitialDateString = reader["FirstInitialDateString"].ToString();
                                guestCompany.FirstInitialTimeString = (reader["FirstInitialTimeString"].ToString().Replace("AM", "")).Replace("PM", "");
                                guestCompany.LastFollowUpDateString = reader["LastFollowUpDateString"].ToString();
                                guestCompany.LastFollowUpTimeString = (reader["LastFollowUpTimeString"].ToString().Replace("AM", "")).Replace("PM", "");
                            }
                        }
                    }
                }
            }
            return guestCompany;
        }
        public GuestCompanyBO GetGuestCompanyInfoByRegistrationId(int registrationId)
        {
            GuestCompanyBO guestCompany = new GuestCompanyBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactDesignation = reader["ContactDesignation"].ToString();
                                guestCompany.TelephoneNumber = reader["TelephoneNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                            }
                        }
                    }
                }
            }
            return guestCompany;
        }
        public List<GuestCompanyBO> GetGuestCompanyInfoBySearchCriteria(int isAdminUser, int userInfoId, string companyName, Int32 companyType, string contactNumber, string companyEmail, Int64 countryId, Int64 stateId, Int64 cityId, Int64 areaId, int lifeCycleStage, Int32 companyOwnerId, Int32 dateSearchCriteria, DateTime fromDate, DateTime toDate, string companyNumber, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    if (!string.IsNullOrEmpty(companyName))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.Int32, companyName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.Int32, DBNull.Value);

                    if (companyType > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyType", DbType.Int32, companyType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyType", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(contactNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, contactNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(companyEmail))
                        dbSmartAspects.AddInParameter(cmd, "@EmailAddress", DbType.String, companyEmail);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmailAddress", DbType.String, DBNull.Value);

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

                    if (companyOwnerId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyOwnerId", DbType.Int32, companyOwnerId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyOwnerId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(companyNumber))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyNumber", DbType.String, companyNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyNumber", DbType.String, DBNull.Value);

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
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.ParentCompanyId = Convert.ToInt32(reader["ParentCompanyId"]);
                                guestCompany.ParentCompany = reader["ParentCompany"].ToString();
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                guestCompany.CompanyNumber = reader["CompanyNumber"].ToString();
                                guestCompany.CompanyContact = reader["CompanyContact"].ToString();
                                guestCompany.CompanyEmail = reader["CompanyEmail"].ToString();
                                guestCompany.StateName = reader["StateName"].ToString();
                                guestCompany.CityName = reader["CityName"].ToString();
                                guestCompany.CountryName = reader["CountryName"].ToString();
                                guestCompany.AssociateContacts = reader["AssociateContacts"].ToString();
                                guestCompany.LifeCycleStage = reader["LifeCycleStage"].ToString();
                                guestCompany.AccountManager = reader["AccountManager"].ToString();
                                guestCompany.CreatedDisplayDate = reader["CreatedDisplayDate"].ToString();
                                guestCompany.IsDetailPanelEnableForCompany = Convert.ToInt32(reader["IsDetailPanelEnableForCompany"]);
                                guestCompany.IsDetailPanelEnableForParentCompany = Convert.ToInt32(reader["IsDetailPanelEnableForParentCompany"]);
                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return roomTypeList;
        }

        public List<GuestCompanyBO> GetALLGuestCompanyInfo()
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetALLGuestCompanyInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();

                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }

        public string GenarateWhereConditionstring(string CompanyName, string EmailAddress, string ContactNumber, string ContactPerson)
        {
            string Where = string.Empty;
            if (!string.IsNullOrEmpty(CompanyName))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " OR CompanyName LIKE '%" + CompanyName + "%'";
                }
                else
                {
                    Where += "  CompanyName LIKE '%" + CompanyName + "%'";
                }
            }

            if (!string.IsNullOrEmpty(EmailAddress))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " OR EmailAddress LIKE '%" + EmailAddress + "%'";
                }
                else
                {
                    Where += " EmailAddress LIKE '%" + EmailAddress + "%'";
                }
            }
            if (!string.IsNullOrEmpty(ContactNumber))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " OR ContactNumber LIKE '%" + ContactNumber + "%'";
                }
                else
                {
                    Where += "  ContactNumber LIKE '%" + ContactNumber + "%'";
                }
            }

            if (!string.IsNullOrEmpty(ContactPerson))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " OR ContactPerson LIKE '%" + ContactPerson + "%'";
                }
                else
                {
                    Where += " ContactPerson LIKE '%" + ContactPerson + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where + " AND (IsMember = 0 OR IsMember IS NULL)";
            }
            else
            {
                Where = " WHERE IsMember = 0 OR IsMember IS NULL";
            }

            return Where;
        }
        public List<GuestCompanyBO> GetGuestCompanyInfo(string companyName)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInformation_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();

                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();

                                guestCompany.BillingStreet = reader["BillingStreet"].ToString();
                                guestCompany.BillingCity = reader["BillingCity"].ToString();
                                guestCompany.BillingState = reader["BillingState"].ToString();
                                guestCompany.BillingCountry = reader["BillingCountry"].ToString();
                                guestCompany.BillingPostCode = reader["BillingPostCode"].ToString();

                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);
                                guestCompany.LifeCycleStageId = Convert.ToInt32(reader["LifeCycleStageId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<GuestCompanyBO> GetGLCompanyWiseGuestCompanyInfo(int userInfoId, string companyName, int costcenterId)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLCompanyWiseGuestCompanyInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costcenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();

                                guestCompany.BillingStreet = reader["BillingStreet"].ToString();
                                guestCompany.BillingCity = reader["BillingCity"].ToString();
                                guestCompany.BillingState = reader["BillingState"].ToString();
                                guestCompany.BillingCountry = reader["BillingCountry"].ToString();
                                guestCompany.BillingPostCode = reader["BillingPostCode"].ToString();

                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);
                                guestCompany.LifeCycleStageId = Convert.ToInt32(reader["LifeCycleStageId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<GuestCompanyBO> GetGuestCompanyInfoByCompanyName(string companyName)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoByCompanyName_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();

                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();

                                guestCompany.BillingStreet = reader["BillingStreet"].ToString();
                                guestCompany.BillingCity = reader["BillingCity"].ToString();
                                guestCompany.BillingState = reader["BillingState"].ToString();
                                guestCompany.BillingCountry = reader["BillingCountry"].ToString();
                                guestCompany.BillingPostCode = reader["BillingPostCode"].ToString();

                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);
                                guestCompany.LifeCycleStageId = Convert.ToInt32(reader["LifeCycleStageId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public Boolean SaveHotelCompanyPaymentLedger(HotelCompanyPaymentLedgerBO hotelCompanyPaymentLedgerBO, List<CompanyPaymentLedgerVwBo> companyPaymentLedger, out long tmpCompanyPaymentId)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveHotelCompanyPaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, hotelCompanyPaymentLedgerBO.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, hotelCompanyPaymentLedgerBO.BillNumber);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, hotelCompanyPaymentLedgerBO.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, hotelCompanyPaymentLedgerBO.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, hotelCompanyPaymentLedgerBO.CurrencyId);
                            dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, hotelCompanyPaymentLedgerBO.ConvertionRate);
                            dbSmartAspects.AddInParameter(command, "@DRAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.DRAmount);
                            dbSmartAspects.AddInParameter(command, "@CRAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.CRAmount);
                            dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.CurrencyAmount);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, hotelCompanyPaymentLedgerBO.AccountsPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, hotelCompanyPaymentLedgerBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@PaymentStatus", DbType.String, hotelCompanyPaymentLedgerBO.PaymentStatus);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, hotelCompanyPaymentLedgerBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@CompanyPaymentId", DbType.Int64, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            tmpCompanyPaymentId = Convert.ToInt64(command.Parameters["@CompanyPaymentId"].Value);
                        }

                        if (status > 0 && companyPaymentLedger != null)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPaymentLedgerForBillPayment_SP"))
                            {
                                foreach (CompanyPaymentLedgerVwBo cp in companyPaymentLedger)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cp.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cp.BillId);
                                    dbSmartAspects.AddInParameter(command, "@IsBillGenerated", DbType.Boolean, cp.IsBillGenerated);
                                    dbSmartAspects.AddInParameter(command, "@PaidAmount", DbType.Decimal, cp.PaidAmount);
                                    dbSmartAspects.AddInParameter(command, "@PaidAmountCurrent", DbType.Decimal, cp.PaidAmountCurrent);
                                    dbSmartAspects.AddInParameter(command, "@RefCompanyPaymentId", DbType.Int64, tmpCompanyPaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public Boolean SaveCompanyAccountApprovalInfo(GuestCompanyBO BenefitList, List<GuestCompanyBO> LegalActions, List<int> deletedLegalActionInfoList, out int tmpCompanyId)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commBen = dbSmartAspects.GetStoredProcCommand("UpdateCompanyAccountApprovalBenefitInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commBen, "@CompanyId", DbType.Int32, BenefitList.CompanyId);
                            dbSmartAspects.AddInParameter(commBen, "@CompanyName", DbType.String, BenefitList.CompanyName);
                            dbSmartAspects.AddInParameter(commBen, "@CreditLimit", DbType.Decimal, BenefitList.CreditLimit);
                            dbSmartAspects.AddInParameter(commBen, "@CreditLimitExpire", DbType.DateTime, BenefitList.CreditLimitExpire);
                            dbSmartAspects.AddInParameter(commBen, "@ShortCreditLimit", DbType.Decimal, BenefitList.ShortCreditLimit);
                            dbSmartAspects.AddInParameter(commBen, "@ShortCreditLimitExpire", DbType.DateTime, BenefitList.ShortCreditLimitExpire);
                            dbSmartAspects.AddInParameter(commBen, "@TransportFareFactory", DbType.Decimal, BenefitList.TransportFareFactory);
                            dbSmartAspects.AddInParameter(commBen, "@TransportFareDepo", DbType.Decimal, BenefitList.TransportFareDepo);
                            dbSmartAspects.AddInParameter(commBen, "@SalesCommission", DbType.Decimal, BenefitList.SalesCommission);
                            dbSmartAspects.AddInParameter(commBen, "@LegalAction", DbType.String, BenefitList.LegalAction);
                            dbSmartAspects.AddInParameter(commBen, "@AccountsApprovedBy", DbType.Int32, BenefitList.AccountsApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commBen, transaction);
                            tmpCompanyId = Convert.ToInt32(BenefitList.CompanyId);
                        }

                        if (deletedLegalActionInfoList.Count > 0)
                        {
                            foreach (int ai in deletedLegalActionInfoList)
                            {
                                using (DbCommand cmdDelLa = dbSmartAspects.GetStoredProcCommand("DeleteLegalActionById_SP"))
                                {
                                    cmdDelLa.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdDelLa, "@Id", DbType.Int64, ai);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelLa, transaction);
                                }
                            }
                        }

                        if (LegalActions.Count > 0)
                        {
                            using (DbCommand commApprove = dbSmartAspects.GetStoredProcCommand("SaveCompanyAccountApprovalLegalActionInfo_SP"))
                            {
                                foreach (GuestCompanyBO gc in LegalActions)
                                {
                                    commApprove.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commApprove, "@Id", DbType.Int64, gc.Id);
                                    dbSmartAspects.AddInParameter(commApprove, "@CompanyId", DbType.Int32, gc.CompanyId);
                                    dbSmartAspects.AddInParameter(commApprove, "@TransactionDate", DbType.DateTime, gc.TransactionDate);
                                    dbSmartAspects.AddInParameter(commApprove, "@DetailDescription", DbType.String, gc.DetailDescription);
                                    dbSmartAspects.AddInParameter(commApprove, "@CallToAction", DbType.String, gc.CallToAction);
                                    dbSmartAspects.AddInParameter(commApprove, "@IsPreviousDataExists", DbType.Int32, gc.IsPreviousDataExists);
                                    dbSmartAspects.AddInParameter(commApprove, "@CreatedBy", DbType.Int32, BenefitList.AccountsApprovedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commApprove, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public Boolean UpdateHotelCompanyPaymentLedger(HotelCompanyPaymentLedgerBO hotelCompanyPaymentLedgerBO, List<CompanyPaymentLedgerVwBo> companyPaymentLedger)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHotelCompanyPaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, hotelCompanyPaymentLedgerBO.CompanyPaymentId);
                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, hotelCompanyPaymentLedgerBO.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, hotelCompanyPaymentLedgerBO.BillNumber);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, hotelCompanyPaymentLedgerBO.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, hotelCompanyPaymentLedgerBO.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, hotelCompanyPaymentLedgerBO.CurrencyId);
                            dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, hotelCompanyPaymentLedgerBO.ConvertionRate);
                            dbSmartAspects.AddInParameter(command, "@DRAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.DRAmount);
                            dbSmartAspects.AddInParameter(command, "@CRAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.CRAmount);
                            dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.CurrencyAmount);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, hotelCompanyPaymentLedgerBO.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, hotelCompanyPaymentLedgerBO.AccountsPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, hotelCompanyPaymentLedgerBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@PaymentStatus", DbType.String, hotelCompanyPaymentLedgerBO.PaymentStatus);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, hotelCompanyPaymentLedgerBO.LastModifiedBy);
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0 && companyPaymentLedger != null)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPaymentLedgerForBillPaymentUpdation_SP"))
                            {
                                foreach (CompanyPaymentLedgerVwBo cp in companyPaymentLedger)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cp.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cp.BillId);
                                    dbSmartAspects.AddInParameter(command, "@IsBillGenerated", DbType.Boolean, cp.IsBillGenerated);
                                    dbSmartAspects.AddInParameter(command, "@PaidAmount", DbType.Decimal, cp.PaidAmount);
                                    dbSmartAspects.AddInParameter(command, "@PaidAmountCurrent", DbType.Decimal, cp.PaidAmountCurrent);

                                    if (cp.IsBillGenerated == true)
                                        dbSmartAspects.AddInParameter(command, "@RefCompanyPaymentId", DbType.Int64, hotelCompanyPaymentLedgerBO.CompanyPaymentId);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@RefCompanyPaymentId", DbType.Int64, 0);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public List<HotelCompanyPaymentLedgerBO> GetHotelCompanyPaymentLedger(DateTime? dateFrom, DateTime? dateTo, string paymentId, bool isInvoice)
        {
            List<HotelCompanyPaymentLedgerBO> paymentInfo = new List<HotelCompanyPaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelCompanyPaymentLedger_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.String, paymentId);
                    dbSmartAspects.AddInParameter(cmd, "@IsInvoice", DbType.Boolean, isInvoice);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new HotelCompanyPaymentLedgerBO
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate")

                    }).ToList();
                }
            }
            return paymentInfo;
        }
        public HotelCompanyPaymentLedgerBO GetSupplierPaymentLedgerById(long id)
        {
            HotelCompanyPaymentLedgerBO paymentInfo = new HotelCompanyPaymentLedgerBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentLedgerById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyPaymentId", DbType.Int64, id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new HotelCompanyPaymentLedgerBO
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        //CurrencyName = r.Field<string>("CurrencyName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        Remarks = r.Field<string>("Remarks"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyId = r.Field<int>("CompanyId"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        TelephoneNumber = r.Field<string>("TelephoneNumber"),
                        AccountsPostingHeadId = r.Field<Int64>("AccountsPostingHeadId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount")

                    }).SingleOrDefault();
                }
            }
            return paymentInfo;
        }
        //--** Company Ledger
        public List<CompanyPaymentLedgerReportVwBo> GetCompanyLedger(int userInfoId, int glCompanyId, int companyId, DateTime dateFrom, DateTime dateTo, string paymentStatus, string reportType, string searchNarration, string fromAmount, string toAmount)
        {
            List<CompanyPaymentLedgerReportVwBo> supplierInfo = new List<CompanyPaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentLedgerReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    if (glCompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, glCompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@GLCompanyId", DbType.Int32, DBNull.Value);

                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (paymentStatus != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, paymentStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, DBNull.Value);

                    if (searchNarration != "")
                        dbSmartAspects.AddInParameter(cmd, "@SearchNarration", DbType.String, searchNarration);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SearchNarration", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(fromAmount))
                        dbSmartAspects.AddInParameter(cmd, "@FromAmount", DbType.Decimal, Convert.ToDecimal(fromAmount));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromAmount", DbType.Decimal, DBNull.Value);

                    if (!string.IsNullOrEmpty(toAmount))
                        dbSmartAspects.AddInParameter(cmd, "@ToAmount", DbType.Decimal, Convert.ToDecimal(toAmount));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToAmount", DbType.Decimal, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerReportVwBo
                    {
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        PaymentDateDisplay = r.Field<string>("PaymentDateDisplay"),
                        Narration = r.Field<string>("Narration"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        BalanceCommulative = r.Field<decimal?>("BalanceCommulative"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<CompanyPaymentLedgerReportVwBo> GetCompanyARAging(string reportType, int companyId, DateTime asOfDate, int intervalBands, string intervalType)
        {
            List<CompanyPaymentLedgerReportVwBo> supplierInfo = new List<CompanyPaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyARAging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@AsOfDate", DbType.Date, asOfDate);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalBands", DbType.Int32, intervalBands);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalType", DbType.String, intervalType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerReportVwBo
                    {
                        CompanyId = r.Field<Int32>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ColumnOrderDisplay = r.Field<Int32>("ColumnOrderDisplay"),
                        ColumnAgingTitle = r.Field<string>("ColumnAgingTitle"),
                        ColumnAgingBalance = r.Field<decimal>("ColumnAgingBalance")
                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<CompanyPaymentLedgerReportVwBo> GetCompanyARAgingDetail(string reportType, int companyId, DateTime asOfDate, int intervalBands, string intervalType)
        {
            List<CompanyPaymentLedgerReportVwBo> supplierInfo = new List<CompanyPaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyARAgingDetail_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@AsOfDate", DbType.Date, asOfDate);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalBands", DbType.Int32, intervalBands);
                    dbSmartAspects.AddInParameter(cmd, "@IntervalType", DbType.String, intervalType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerReportVwBo
                    {
                        CompanyId = r.Field<Int32>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentDateDisplay = r.Field<string>("PaymentDateDisplay"),
                        BillNumber = r.Field<string>("BillNumber"),
                        TransactionAge = r.Field<Int32>("TransactionAge"),
                        DueAmount = r.Field<decimal>("DueAmount")
                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<CompanyPaymentLedgerReportVwBo> GetCompanyPaymentInfoForReport(int costcenterId, string reportType, int companyId, DateTime dateFrom, DateTime dateTo, Int32 receivedBy, string collectionType, string adjustmentNodeHead)
        {
            List<CompanyPaymentLedgerReportVwBo> supplierInfo = new List<CompanyPaymentLedgerReportVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (costcenterId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costcenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (receivedBy > 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.String, receivedBy);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.String, DBNull.Value);

                    if (collectionType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, collectionType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@AdjustmentNodeHead", DbType.String, adjustmentNodeHead);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerReportVwBo
                    {
                        //PaymentId = r.Field<Int32>("PaymentId"),
                        CompanyBillId = r.Field<Int64>("CompanyBillId"),
                        BillDetails = r.Field<string>("BillDetails"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDateDisplay = r.Field<string>("PaymentDateDisplay"),
                        CompanyName = r.Field<string>("CompanyName"),
                        BillAmount = r.Field<decimal>("BillAmount"),
                        AdjustmentAmount = r.Field<decimal>("AdjustmentAmount"),
                        AdjustmentDetails = r.Field<string>("AdjustmentDetails"),
                        CollectionAmount = r.Field<decimal>("CollectionAmount"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        InvoiceDetails = r.Field<string>("InvoiceDetails"),
                        Remarks = r.Field<string>("Remarks"),
                        TransactionBy = r.Field<string>("TransactionBy")
                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<GuestCompanyBO> GetCompanyInfoBySearchCriteria(string searchText)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();
            string query = "SELECT (ISNULL(CompanyNumber, '') + ' - ' + CompanyName) AS CompanyCodeAndName, * FROM HotelGuestCompany WHERE (ISNULL(CompanyNumber, '') + ' - ' + CompanyName) LIKE '%" + searchText + "%'";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "Supplier");
                    DataTable table = SupplierDS.Tables["Supplier"];

                    companyList = table.AsEnumerable().Select(r => new GuestCompanyBO
                    {
                        CompanyId = r.Field<int>("CompanyId"),
                        //CompanyName = r.Field<string>("CompanyName"),
                        CompanyName = r.Field<string>("CompanyCodeAndName"),
                        ContactNumber = r.Field<string>("ContactNumber")
                    }).ToList();
                }
            }
            return companyList;
        }
        //--- Company Site
        public Boolean SaveCompanySite(CompanySiteBO site, out int siteId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanySite_SP"))
                    {
                        command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, site.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@SiteName", DbType.String, site.SiteName);

                        dbSmartAspects.AddInParameter(command, "@BusinessContactName", DbType.String, site.BusinessContactName);
                        dbSmartAspects.AddInParameter(command, "@BusinessContactEmail", DbType.String, site.BusinessContactEmail);
                        dbSmartAspects.AddInParameter(command, "@BusinessContactPhone", DbType.String, site.BusinessContactPhone);

                        dbSmartAspects.AddInParameter(command, "@TechnicalContactName", DbType.String, site.TechnicalContactName);
                        dbSmartAspects.AddInParameter(command, "@TechnicalContactEmail", DbType.String, site.TechnicalContactEmail);
                        dbSmartAspects.AddInParameter(command, "@TechnicalContactPhone", DbType.String, site.TechnicalContactPhone);

                        dbSmartAspects.AddInParameter(command, "@BillingContactName", DbType.String, site.BillingContactName);
                        dbSmartAspects.AddInParameter(command, "@BillingContactEmail", DbType.String, site.BillingContactEmail);
                        dbSmartAspects.AddInParameter(command, "@BillingContactPhone", DbType.String, site.BillingContactPhone);

                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, site.Remarks);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, site.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@SiteId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        siteId = Convert.ToInt32(command.Parameters["@SiteId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateCompanySite(CompanySiteBO site)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanySite_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SiteId", DbType.Int32, site.SiteId);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, site.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@SiteName", DbType.String, site.SiteName);

                        dbSmartAspects.AddInParameter(command, "@BusinessContactName", DbType.String, site.BusinessContactName);
                        dbSmartAspects.AddInParameter(command, "@BusinessContactEmail", DbType.String, site.BusinessContactEmail);
                        dbSmartAspects.AddInParameter(command, "@BusinessContactPhone", DbType.String, site.BusinessContactPhone);

                        dbSmartAspects.AddInParameter(command, "@TechnicalContactName", DbType.String, site.TechnicalContactName);
                        dbSmartAspects.AddInParameter(command, "@TechnicalContactEmail", DbType.String, site.TechnicalContactEmail);
                        dbSmartAspects.AddInParameter(command, "@TechnicalContactPhone", DbType.String, site.TechnicalContactPhone);

                        dbSmartAspects.AddInParameter(command, "@BillingContactName", DbType.String, site.BillingContactName);
                        dbSmartAspects.AddInParameter(command, "@BillingContactEmail", DbType.String, site.BillingContactEmail);
                        dbSmartAspects.AddInParameter(command, "@BillingContactPhone", DbType.String, site.BillingContactPhone);

                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, site.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, site.LastModifiedBy);

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
        public CompanySiteBO GetCompanySiteById(int siteId)
        {
            CompanySiteBO site = new CompanySiteBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanySiteById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SiteId", DbType.Int32, siteId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                site.SiteId = Convert.ToInt32(reader["SiteId"]);
                                site.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                site.SiteName = reader["SiteName"].ToString();
                                site.BusinessContactName = reader["BusinessContactName"].ToString();
                                site.BusinessContactEmail = reader["BusinessContactEmail"].ToString();
                                site.BusinessContactPhone = reader["BusinessContactPhone"].ToString();
                                site.TechnicalContactName = reader["TechnicalContactName"].ToString();
                                site.TechnicalContactEmail = reader["TechnicalContactEmail"].ToString();
                                site.TechnicalContactPhone = reader["TechnicalContactPhone"].ToString();
                                site.BillingContactName = reader["BillingContactName"].ToString();
                                site.BillingContactEmail = reader["BillingContactEmail"].ToString();
                                site.BillingContactPhone = reader["BillingContactPhone"].ToString();
                                site.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return site;
        }
        public List<CompanySiteBO> GetCompanySiteBySearchCriteria(string siteName, string businessContactName, string businessContactEmail, string businessContactPhone)
        {
            List<CompanySiteBO> roomTypeList = new List<CompanySiteBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanySiteBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(siteName))
                        dbSmartAspects.AddInParameter(cmd, "@SiteName", DbType.String, siteName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SiteName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(businessContactName))
                        dbSmartAspects.AddInParameter(cmd, "@BusinessContactName", DbType.String, businessContactName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BusinessContactName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(businessContactEmail))
                        dbSmartAspects.AddInParameter(cmd, "@BusinessContactEmail", DbType.String, businessContactEmail);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BusinessContactEmail", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(businessContactPhone))
                        dbSmartAspects.AddInParameter(cmd, "@BusinessContactPhone", DbType.String, businessContactPhone);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BusinessContactPhone", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CompanySiteBO site = new CompanySiteBO();

                                site.SiteId = Convert.ToInt32(reader["SiteId"]);
                                site.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                site.SiteName = reader["SiteName"].ToString();
                                site.BusinessContactName = reader["BusinessContactName"].ToString();
                                site.BusinessContactEmail = reader["BusinessContactEmail"].ToString();
                                site.BusinessContactPhone = reader["BusinessContactPhone"].ToString();
                                site.TechnicalContactName = reader["TechnicalContactName"].ToString();
                                site.TechnicalContactEmail = reader["TechnicalContactEmail"].ToString();
                                site.TechnicalContactPhone = reader["TechnicalContactPhone"].ToString();
                                site.BillingContactName = reader["BillingContactName"].ToString();
                                site.BillingContactEmail = reader["BillingContactEmail"].ToString();
                                site.BillingContactPhone = reader["BillingContactPhone"].ToString();
                                site.Remarks = reader["Remarks"].ToString();

                                roomTypeList.Add(site);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<CompanySiteBO> GetCompanySiteByCompanyId(int companyId)
        {
            List<CompanySiteBO> roomTypeList = new List<CompanySiteBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanySiteByCompanyId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CompanySiteBO site = new CompanySiteBO();

                                site.SiteId = Convert.ToInt32(reader["SiteId"]);
                                site.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                site.SiteName = reader["SiteName"].ToString();
                                site.BusinessContactName = reader["BusinessContactName"].ToString();
                                site.BusinessContactEmail = reader["BusinessContactEmail"].ToString();
                                site.BusinessContactPhone = reader["BusinessContactPhone"].ToString();
                                site.TechnicalContactName = reader["TechnicalContactName"].ToString();
                                site.TechnicalContactEmail = reader["TechnicalContactEmail"].ToString();
                                site.TechnicalContactPhone = reader["TechnicalContactPhone"].ToString();
                                site.BillingContactName = reader["BillingContactName"].ToString();
                                site.BillingContactEmail = reader["BillingContactEmail"].ToString();
                                site.BillingContactPhone = reader["BillingContactPhone"].ToString();
                                site.Remarks = reader["Remarks"].ToString();

                                roomTypeList.Add(site);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        //-- Company Bill
        public Boolean UpdateCompanyPaymentLedgerForBillGeneration(List<CompanyPaymentLedgerVwBo> companyPaymentLedger)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPaymentLedgerForBillGeneration_SP"))
                {
                    foreach (CompanyPaymentLedgerVwBo cpl in companyPaymentLedger)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                        dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                        dbSmartAspects.AddInParameter(command, "@IsBillGenerated", DbType.Boolean, cpl.IsBillGenerated);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public List<CompanyPaymentLedgerVwBo> GetCompanyBillByPaymentRefId(Int64 refCompanyPaymentId, int companyId)
        {
            List<CompanyPaymentLedgerVwBo> supplierInfo = new List<CompanyPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillByPaymentRefId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RefCompanyPaymentId", DbType.Int64, refCompanyPaymentId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerVwBo
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        PaidAmountCurrent = r.Field<decimal>("PaidAmountCurrent"),
                        IsBillGenerated = r.Field<bool>("IsBillGenerated"),
                        RefCompanyPaymentId = r.Field<Int64>("RefCompanyPaymentId")

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public Boolean UpdateCompanyPaymentLedgerForBillAdjustment(int companyId, List<CompanyPaymentLedgerVwBo> companyPaymentLedger)
        {
            int status = 0;
            string query = string.Format(@"UPDATE HotelGuestCompany
                                            SET    Balance = dbo.FnCompanyCurrentBalance(GETDATE(), GETDATE(), {0})
                                            WHERE  CompanyId = {0}", companyId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPaymentLedgerForBillAdjustment_SP"))
                        {
                            foreach (CompanyPaymentLedgerVwBo cpl in companyPaymentLedger)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@IsBillAdjusted", DbType.Boolean, cpl.IsBillAdjusted);
                                dbSmartAspects.AddInParameter(command, "@PaidAmount", DbType.Decimal, cpl.PaidAmount);
                                dbSmartAspects.AddInParameter(command, "@PaidAmountCurrent", DbType.Decimal, cpl.PaidAmountCurrent);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }

                        if (status > 0 && companyPaymentLedger != null)
                        {
                            using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public List<CompanyPaymentLedgerVwBo> CompanyBillByCompanyIdAndBillGenerationFlag(int companyId, Int64 companyBillId, int currencyId)
        {
            List<CompanyPaymentLedgerVwBo> supplierInfo = new List<CompanyPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CompanyBillByCompanyIdAndBillGenerationFlag_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyBillId", DbType.Int64, companyBillId);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyId", DbType.Int64, currencyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerVwBo
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        CompanyBillNumber = r.Field<string>("CompanyBillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        VatAmount = r.Field<decimal>("VatAmount"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        Remarks = r.Field<string>("Remarks"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        CurrencyId = r.Field<Int32>("CurrencyId")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        //------------------------------Company Bill Process, Receive and Adjustment
        public List<CompanyPaymentLedgerVwBo> CompanyBillBySearch(int companyId)
        {
            List<CompanyPaymentLedgerVwBo> supplierInfo = new List<CompanyPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillBySearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerVwBo
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefCompanyPaymentId = r.Field<Int64>("RefCompanyPaymentId"),
                        CompanyBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<CompanyPaymentLedgerVwBo> CompanyBillBySearch(int companyId, Int64 companyBillId)
        {
            List<CompanyPaymentLedgerVwBo> supplierInfo = new List<CompanyPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyGeneratedBillById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyBillId", DbType.Int32, companyBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerVwBo
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefCompanyPaymentId = r.Field<Int64>("RefCompanyPaymentId"),
                        CompanyBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public bool SaveCompanyBillGeneration(CompanyBillGenerationBO billGeneration, List<CompanyBillGenerationDetailsBO> billGenerationDetails, out Int64 companyBillId)
        {
            int status = 0;
            companyBillId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyBillGeneration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, billGeneration.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, billGeneration.BillDate);
                            dbSmartAspects.AddInParameter(command, "@BillCurrencyId", DbType.Int32, billGeneration.BillCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, billGeneration.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billGeneration.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@CompanyBillId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            companyBillId = Convert.ToInt64(command.Parameters["@CompanyBillId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyBillGenerationDetails_SP"))
                        {
                            foreach (CompanyBillGenerationDetailsBO cpl in billGenerationDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.Int64, companyBillId);
                                dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);
                                dbSmartAspects.AddInParameter(command, "@ModuleName", DbType.String, cpl.ModuleName);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool UpdateCompanyBillGeneration(CompanyBillGenerationBO billGeneration, List<CompanyBillGenerationDetailsBO> billGenerationDetails,
                                              List<CompanyBillGenerationDetailsBO> billGenerationDetailsEdited, List<CompanyBillGenerationDetailsBO> billGenerationDetailsDeleted)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBillGeneration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.String, billGeneration.CompanyBillId);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, billGeneration.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, billGeneration.BillDate);
                            dbSmartAspects.AddInParameter(command, "@BillCurrencyId", DbType.Int32, billGeneration.BillCurrencyId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, billGeneration.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, billGeneration.CreatedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0 && billGenerationDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyBillGenerationDetails_SP"))
                            {
                                foreach (CompanyBillGenerationDetailsBO cpl in billGenerationDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.Int64, billGeneration.CompanyBillId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);
                                    dbSmartAspects.AddInParameter(command, "@ModuleName", DbType.String, cpl.ModuleName);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && billGenerationDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyBillGenerationDetails_SP"))
                            {
                                foreach (CompanyBillGenerationDetailsBO cpl in billGenerationDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.Int64, billGeneration.CompanyBillId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int64, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);
                                    dbSmartAspects.AddInParameter(command, "@ModuleName", DbType.String, cpl.ModuleName);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && billGenerationDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteCompanyBillGenerationDetails_SP"))
                            {
                                foreach (CompanyBillGenerationDetailsBO cpl in billGenerationDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.Int64, cpl.CompanyBillId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool DeleteCompanyBillGeneration(Int64 companyBillId)
        {
            int status = 0;
            string query = string.Empty;

            query = string.Format(@"
                            UPDATE cpl SET BillGenerationId = 0 
                            FROM HotelCompanyPaymentLedger cpl INNER JOIN HotelCompanyBillGenerationDetails cbgd ON cpl.BillGenerationId = cbgd.CompanyBillId
                            WHERE cbgd.CompanyBillId = {0}

                            DELETE FROM HotelCompanyBillGenerationDetails WHERE CompanyBillId = {0}
                            DELETE FROM HotelCompanyBillGeneration WHERE CompanyBillId = {0}", companyBillId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }
        public List<CompanyBillGenerationBO> GetCompanyBillGenerationBySearch(DateTime? dateFrom, DateTime? dateTo, int companyId)
        {
            List<CompanyBillGenerationBO> paymentInfo = new List<CompanyBillGenerationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillGenerationBySearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new CompanyBillGenerationBO
                    {
                        CompanyBillId = r.Field<Int64>("CompanyBillId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        CompanyBillNumber = r.Field<string>("CompanyBillNumber"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        Remarks = r.Field<string>("Remarks"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        //------------
        public CompanyBillGenerationBO GetCompanyBillGeneration(Int64 companyBillId)
        {
            CompanyBillGenerationBO paymentInfo = new CompanyBillGenerationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillGeneration_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyBillId", DbType.Int64, companyBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new CompanyBillGenerationBO
                    {
                        CompanyBillId = r.Field<Int64>("CompanyBillId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        CompanyBillNumber = r.Field<string>("CompanyBillNumber"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId"),
                        Remarks = r.Field<string>("Remarks"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }

        public List<CompanyBillGenerationDetailsBO> GetCompanyBillGenerationDetails(Int64 companyBillId)
        {
            List<CompanyBillGenerationDetailsBO> paymentInfo = new List<CompanyBillGenerationDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillGenerationDetails_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyBillId", DbType.Int64, companyBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new CompanyBillGenerationDetailsBO
                    {
                        CompanyBillDetailsId = r.Field<Int64>("CompanyBillDetailsId"),
                        CompanyBillId = r.Field<Int64>("CompanyBillId"),
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        BillId = r.Field<int>("BillId"),
                        Amount = r.Field<decimal>("Amount"),
                        ModuleName = r.Field<string>("ModuleName")
                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public List<CompanyPaymentLedgerVwBo> GetCompanyBillForBillGenerationEdit(int companyId, Int64 companyBillId)
        {
            List<CompanyPaymentLedgerVwBo> supplierInfo = new List<CompanyPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillForBillGenerationEdit_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyBillId", DbType.Int64, companyBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentLedgerVwBo
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefCompanyPaymentId = r.Field<Int64>("RefCompanyPaymentId"),
                        CompanyBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<CompanyBillGenerationBO> GetCompanyGeneratedBillByBillStatus(int companyId)
        {
            List<CompanyBillGenerationBO> paymentInfo = new List<CompanyBillGenerationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyGeneratedBill_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new CompanyBillGenerationBO
                    {
                        CompanyBillId = r.Field<Int64>("CompanyBillId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        CompanyBillNumber = r.Field<string>("CompanyBillNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        BillCurrencyId = r.Field<int>("BillCurrencyId")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        //------- Company Bill Receive
        public List<CompanyBillGenerateViewBO> GetCompanyBillForBillReceive(int companyId, Int64 companyBillId)
        {
            List<CompanyBillGenerateViewBO> supplierInfo = new List<CompanyBillGenerateViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyBillForReceivedPayment_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyBillId", DbType.Int64, companyBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyBillGenerateViewBO
                    {
                        CompanyBillId = r.Field<Int64>("CompanyBillId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        CompanyBillNumber = r.Field<string>("CompanyBillNumber"),
                        CompanyBillDetailsId = r.Field<Int64>("CompanyBillDetailsId"),
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        BillId = r.Field<Int32>("BillId"),
                        Amount = r.Field<decimal>("Amount"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        BillCurrencyId = r.Field<int>("BillCurrencyId"),
                        PaymentDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool SaveCompanyBillPayment(CompanyPaymentBO companyPayment, List<CompanyPaymentDetailsBO> companyPaymentDetails, out long companyPaymentId)
        {
            int status = 0;
            //Int64 companyPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.Int64, companyPayment.CompanyBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, companyPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, companyPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@CompanyPaymentAdvanceId", DbType.Int64, companyPayment.CompanyPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, companyPayment.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, companyPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, companyPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, companyPayment.AdjustmentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, companyPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, companyPayment.AccountingPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, companyPayment.ApprovedStatus);

                            if (!string.IsNullOrEmpty(companyPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, companyPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(companyPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, companyPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (companyPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, companyPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (companyPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, companyPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, DBNull.Value);

                            if (companyPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, companyPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (companyPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, companyPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, DBNull.Value);

                            if (companyPayment.ChequeDate != null)
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, companyPayment.ChequeDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, companyPayment.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            companyPaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyPaymentDetails_SP"))
                        {
                            foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, companyPaymentId);
                                dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool SaveCompanyBillPaymentTransaction(CompanyPaymentBO companyPayment, List<CompanyPaymentDetailsBO> companyPaymentDetails, List<CompanyPaymentDetailsBO> ReceiveInformationDetails, List<CompanyPaymentDetailsBO> ReceiveInformationDeletedDetails, out long companyPaymentId)
        {
            int status = 0;
            //Int64 companyPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.Int64, companyPayment.CompanyBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, companyPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, companyPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@CompanyPaymentAdvanceId", DbType.Int64, companyPayment.CompanyPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, companyPayment.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, companyPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, companyPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, companyPayment.AdjustmentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, companyPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, companyPayment.AccountingPostingHeadId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, companyPayment.ApprovedStatus);

                            if (!string.IsNullOrEmpty(companyPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, companyPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(companyPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, companyPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (companyPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, companyPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (companyPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, companyPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, DBNull.Value);

                            if (companyPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, companyPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (companyPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, companyPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, DBNull.Value);

                            if (companyPayment.ChequeDate != null)
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, companyPayment.ChequeDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, companyPayment.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            companyPaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyPaymentDetails_SP"))
                        {
                            foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, companyPaymentId);
                                dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateHotelCompanyPaymentTransectionDetails_SP"))
                        {
                            foreach (CompanyPaymentDetailsBO rfd in ReceiveInformationDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, rfd.PaymentMode);
                                dbSmartAspects.AddInParameter(command, "@PaymentHeadId", DbType.Int64, rfd.PaymentHeadId);
                                dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, rfd.PaymentAmount);
                                dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, rfd.PaymentDate);
                                dbSmartAspects.AddInParameter(command, "@CurrencyTypeId", DbType.Int64, rfd.CurrencyTypeId);
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, rfd.ConversionRate);
                                //dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, rfd.ChequeDate);
                                if (rfd.PaymentMode == "Bank")
                                    dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, rfd.ChequeDate);
                                else
                                    dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, DBNull.Value);
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, rfd.ChequeNumber);
                                dbSmartAspects.AddInParameter(command, "@DetailId", DbType.Int64, rfd.DetailId);
                                dbSmartAspects.AddInParameter(command, "@HotelCompanyPaymentId", DbType.Int64, companyPaymentId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            }
                        }



                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool UpdateCompanyBillPayment(CompanyPaymentBO companyPayment, List<CompanyPaymentDetailsBO> companyPaymentDetails,
            List<CompanyPaymentDetailsBO> companyPaymentDetailsEdited, List<CompanyPaymentDetailsBO> companyPaymentDetailsDeleted)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, companyPayment.PaymentId);
                            dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.String, companyPayment.CompanyBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, companyPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, companyPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@CompanyPaymentAdvanceId", DbType.String, companyPayment.CompanyPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.String, companyPayment.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, companyPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.String, companyPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, companyPayment.AdjustmentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, companyPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, companyPayment.AccountingPostingHeadId);

                            if (!string.IsNullOrEmpty(companyPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, companyPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(companyPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, companyPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (companyPayment.ChequeDate != null)
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, companyPayment.ChequeDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, DBNull.Value);

                            if (companyPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, companyPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (companyPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, companyPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (companyPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, companyPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (companyPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, companyPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, companyPayment.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0 && companyPaymentDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyPaymentDetails_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, companyPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && companyPaymentDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPaymentDetails_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentDetailsId", DbType.Int64, cpl.PaymentDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, companyPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && companyPaymentDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "HotelCompanyPaymentDetails");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "PaymentDetailsId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, cpl.PaymentDetailsId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }


        public bool UpdateCompanyBillPaymentTransaction(CompanyPaymentBO companyPayment, List<CompanyPaymentDetailsBO> companyPaymentDetails,
                    List<CompanyPaymentDetailsBO> companyPaymentDetailsEdited, List<CompanyPaymentDetailsBO> companyPaymentDetailsDeleted, List<CompanyPaymentDetailsBO> ReceiveInformationDetails, List<CompanyPaymentDetailsBO> ReceiveInformationDeletedDetails)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, companyPayment.PaymentId);
                            dbSmartAspects.AddInParameter(command, "@CompanyBillId", DbType.String, companyPayment.CompanyBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, companyPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, companyPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@CompanyPaymentAdvanceId", DbType.String, companyPayment.CompanyPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.String, companyPayment.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, companyPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.String, companyPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, companyPayment.AdjustmentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, companyPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, companyPayment.AccountingPostingHeadId);

                            if (!string.IsNullOrEmpty(companyPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, companyPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(companyPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, companyPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (companyPayment.ChequeDate != null)
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, companyPayment.ChequeDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, DBNull.Value);

                            if (companyPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, companyPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (companyPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, companyPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (companyPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, companyPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (companyPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, companyPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, companyPayment.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0 && companyPaymentDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCompanyPaymentDetails_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, companyPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && companyPaymentDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCompanyPaymentDetails_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentDetailsId", DbType.Int64, cpl.PaymentDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, companyPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyBillDetailsId", DbType.Int64, cpl.CompanyBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@CompanyPaymentId", DbType.Int64, cpl.CompanyPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && companyPaymentDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in companyPaymentDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "HotelCompanyPaymentDetails");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "PaymentDetailsId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, cpl.PaymentDetailsId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && ReceiveInformationDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateHotelCompanyPaymentTransectionDetails_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO rfd in ReceiveInformationDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentMode", DbType.String, rfd.PaymentMode);
                                    dbSmartAspects.AddInParameter(command, "@PaymentHeadId", DbType.Int64, rfd.PaymentHeadId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, rfd.PaymentAmount);
                                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, rfd.PaymentDate);
                                    dbSmartAspects.AddInParameter(command, "@CurrencyTypeId", DbType.Int64, rfd.CurrencyTypeId);
                                    dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, rfd.ConversionRate);
                                    //dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, rfd.ChequeDate);
                                    if (rfd.PaymentMode == "Bank")
                                        dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, rfd.ChequeDate);
                                    else
                                        dbSmartAspects.AddInParameter(command, "@ChequeDate", DbType.DateTime, DBNull.Value);
                                    dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, rfd.ChequeNumber);
                                    dbSmartAspects.AddInParameter(command, "@DetailId", DbType.Int64, rfd.DetailId);
                                    dbSmartAspects.AddInParameter(command, "@HotelCompanyPaymentId", DbType.Int64, supplierIdPaymentId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }
                        if (status > 0 && ReceiveInformationDeletedDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (CompanyPaymentDetailsBO cpl in ReceiveInformationDeletedDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "HotelCompanyPaymentTransectionDetails");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "PaymentTransectionId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, cpl.DetailId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public List<CompanyPaymentBO> GetCompanyPaymentBySearch(int userInfoId, int companyId, DateTime? dateFrom, DateTime? dateTo, string paymentFor)
        {
            List<CompanyPaymentBO> paymentInfo = new List<CompanyPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentBySearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (paymentFor != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, paymentFor);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new CompanyPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        CompanyId = r.Field<int>("CompanyId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AdjustmentType = r.Field<string>("AdjustmentType"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public CompanyPaymentBO GetCompanyPayment(Int64 paymentId)
        {
            CompanyPaymentBO paymentInfo = new CompanyPaymentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPayment_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                paymentInfo.PaymentId = Convert.ToInt64(reader["PaymentId"]);
                                paymentInfo.CompanyBillId = Convert.ToInt64(reader["CompanyBillId"]);
                                paymentInfo.PaymentDate = Convert.ToDateTime(reader["PaymentDate"]);

                                paymentInfo.LedgerNumber = reader["LedgerNumber"].ToString();
                                paymentInfo.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                paymentInfo.AdvanceAmount = Convert.ToDecimal(reader["AdvanceAmount"]);
                                paymentInfo.Remarks = reader["Remarks"].ToString();
                                paymentInfo.ChequeNumber = reader["ChequeNumber"].ToString();
                                if (reader["ChequeDate"] != DBNull.Value)
                                {
                                    paymentInfo.ChequeDate = Convert.ToDateTime(reader["ChequeDate"]);
                                }

                                paymentInfo.CurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                                paymentInfo.ConvertionRate = Convert.ToDecimal(reader["ConvertionRate"]);
                                paymentInfo.PaymentType = reader["PaymentType"].ToString();
                                paymentInfo.PaymentFor = reader["PaymentFor"].ToString();
                                paymentInfo.AccountingPostingHeadId = Convert.ToInt32(reader["AccountingPostingHeadId"]);

                                paymentInfo.AdjustmentType = reader["AdjustmentType"].ToString();
                                paymentInfo.CompanyPaymentAdvanceId = Convert.ToInt64(reader["CompanyPaymentAdvanceId"]);
                                paymentInfo.CurrencyType = reader["CurrencyType"].ToString();
                                paymentInfo.AdjustmentAmount = Convert.ToDecimal(reader["AdjustmentAmount"]);
                                paymentInfo.AdjustmentAccountHeadId = Convert.ToInt32(reader["AdjustmentAccountHeadId"]);
                                paymentInfo.PaymentAdjustmentAmount = Convert.ToDecimal(reader["PaymentAdjustmentAmount"]);
                            }
                        }
                    }
                }
            }
            return paymentInfo;
        }

        public List<CompanyPaymentDetailsViewBO> GetCompanyPaymentDetails(Int64 paymentId)
        {
            List<CompanyPaymentDetailsViewBO> supplierInfo = new List<CompanyPaymentDetailsViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyPaymentDetails_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentDetailsViewBO
                    {
                        CompanyBillId = r.Field<long?>("CompanyBillId"),
                        PaymentDetailsId = r.Field<long>("PaymentDetailsId"),
                        PaymentId = r.Field<long>("PaymentId"),
                        CompanyBillDetailsId = r.Field<long?>("CompanyBillDetailsId"),
                        CompanyPaymentId = r.Field<long>("CompanyPaymentId"),

                        BillId = r.Field<long>("BillId"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DueAmount = r.Field<decimal?>("DueAmount")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<CompanyPaymentDetailsBO> GetCompanyPaymentTransectionDetails(Int64 paymentId)
        {
            List<CompanyPaymentDetailsBO> supplierInfo = new List<CompanyPaymentDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelCompanyPaymentTransectionDetailsById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new CompanyPaymentDetailsBO
                    {
                        PaymentMode = r.Field<string>("PaymentMode"),
                        PaymentHeadName = r.Field<string>("PaymentHeadName"),
                        CurrencyTypeName = r.Field<string>("CurrencyTypeName"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        Totalamount = r.Field<decimal>("Totalamount"),

                        ConversionRate = r.Field<decimal>("ConvertionRate"),
                        ChequeDate = r.Field<DateTime?>("ChequeDate"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        PaymentHeadId = r.Field<long>("PaymentHeadId"),
                        PaymentDetailsId = r.Field<long>("PaymentTransectionId"),
                        CompanyPaymentId = r.Field<long>("HotelCompanyPaymentId"),
                        CurrencyTypeId = r.Field<long>("CurrencyTypeId")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool CheckedPayment(Int64 paymentId, int checkedBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckedCompanyPaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.String, checkedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool ApprovedPayment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedCompanyPaymentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool ApprovedPaymentAdjustment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedCompanyPaymentAdjustmentLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool ApprovedRefund(Int64 paymentId, int createdBy)
        {
            int status = 0;
            Int64 supplierIdPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedCompanyRefundLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, paymentId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, createdBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public bool DeletePayment(Int64 paymentId, int createdBy)
        {
            int status = 0;
            string query = string.Format(@"
                                DELETE FROM HotelCompanyPaymentDetails
                                WHERE PaymentId = {0}

                                DELETE FROM HotelCompanyPayment
                                WHERE PaymentId = {0}
                            ", paymentId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetSqlStringCommand(query))
                        {
                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = 0;
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return status > 0 ? true : false;
        }

        public List<HotelCompanyPaymentLedgerBO> CompanyBillAdvanceBySearch(int companyId)
        {
            List<HotelCompanyPaymentLedgerBO> supplierInfo = new List<HotelCompanyPaymentLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyAdvanceBillBySearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new HotelCompanyPaymentLedgerBO
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefCompanyPaymentId = r.Field<Int64>("RefCompanyPaymentId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        AdvanceAmountRemaining = r.Field<decimal>("AdvanceAmountRemaining")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public HotelCompanyPaymentLedgerBO GetCompanyLedger(Int64 restaurantBillId)
        {
            string companyLedgerDeleteQuery = string.Empty;
            companyLedgerDeleteQuery = string.Format("SELECT * FROM HotelCompanyPaymentLedger WHERE BillId = {0}", restaurantBillId);

            HotelCompanyPaymentLedgerBO paymentInfo = new HotelCompanyPaymentLedgerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(companyLedgerDeleteQuery))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new HotelCompanyPaymentLedgerBO
                    {
                        CompanyPaymentId = r.Field<Int64>("CompanyPaymentId")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }

        public bool UpdateCompanyOrContactLifeCycleStage(int companyId, long contactId, int dealStageId, string dealType)
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {

                    //using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateDealStageWiseCompanyStatus_SP"))
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateCompanyLifeCycleStage_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, contactId);
                        dbSmartAspects.AddInParameter(cmd, "@DealStageId", DbType.Int32, dealStageId);
                        dbSmartAspects.AddInParameter(cmd, "@DealType", DbType.String, dealType);

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

        public bool CheckCompanyOrContactIsClient(int companyId, long contactId)
        {
            bool status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CheckCompanyOrContactIsClient_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, contactId);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "CheckInfo");
                        DataTable Table = ds.Tables["CheckInfo"];

                        status = Table.AsEnumerable().Select(r => status = r.Field<bool>("IsCilent")).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public List<GuestCompanyBO> GetCompanyInfoByNameForAutoComplete(string searchText)
        {
            List<GuestCompanyBO> result = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInfoByNameForAutoComplete_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, searchText);

                    DataSet company = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, company, "Company");
                    DataTable Table = company.Tables["Company"];

                    result = Table.AsEnumerable().Select(r => new GuestCompanyBO
                    {
                        CompanyId = r.Field<Int32>("CompanyId"),
                        CompanyNameWithCode = r.Field<string>("CompanyNameWithCode"),
                        Lvl = r.Field<int?>("Lvl"),
                        Hierarchy = r.Field<string>("Hierarchy")

                    }).ToList();
                }
            }
            return result;
        }

        public List<GuestCompanyBO> GetCompanyInfoByHierarchy(int companyId, string hierarchy)
        {
            List<GuestCompanyBO> companyList = new List<GuestCompanyBO>();

            DataSet Company = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyInfoByHierarchy_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@Hierarchy", DbType.String, hierarchy);

                    dbSmartAspects.LoadDataSet(cmd, Company, "Company");
                    DataTable table = Company.Tables["Company"];

                    companyList = table.AsEnumerable().Select(r => new GuestCompanyBO
                    {
                        CompanyId = r.Field<int>("CompanyId"),
                        CompanyNumber = r.Field<string>("CompanyNumber"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyNameWithCode = r.Field<string>("CompanyNameWithCode"),
                        Lvl = r.Field<int?>("Lvl"),
                        Hierarchy = r.Field<string>("Hierarchy")

                    }).ToList();

                    Company.Dispose();
                }
            }

            return companyList;
        }

        public List<GuestCompanyBO> GetGuestCompanyInfoByCompanyNameAccountManagerIdAndIndustryId(string companyName, int AccountManagerId, int industryId)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoByCompanyNameAccountManagerIdAndIndustryId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    if (AccountManagerId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, AccountManagerId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AccountManagerId", DbType.Int32, DBNull.Value);

                    if (industryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@IndustryId", DbType.Int32, industryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IndustryId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();

                                guestCompany.BillingStreet = reader["BillingStreet"].ToString();
                                guestCompany.BillingCity = reader["BillingCity"].ToString();
                                guestCompany.BillingState = reader["BillingState"].ToString();
                                guestCompany.BillingCountry = reader["BillingCountry"].ToString();
                                guestCompany.BillingPostCode = reader["BillingPostCode"].ToString();

                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);
                                guestCompany.LifeCycleStageId = Convert.ToInt32(reader["LifeCycleStageId"]);

                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<GuestCompanyBO> GetGuestCompanyInfoByAccountManager(int accountManagerId)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoByAccountManager_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@AccountManagerID", DbType.Int32, accountManagerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.Balance = Convert.ToDecimal(reader["Balance"]);
                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }
        public List<GuestCompanyBO> GetGuestCompanyInfoBySearchCriteriaForReport(int isAdminUser, int userInfoId, string companyName, Int32 companyType, string contactNumber, string companyEmail, Int64 countryId, Int64 stateId, Int64 cityId, Int64 areaId, int lifeCycleStage, Int32 companyOwnerId, Int32 dateSearchCriteria, DateTime fromDate, DateTime toDate, string companyNumber, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GuestCompanyBO> roomTypeList = new List<GuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestCompanyInfoBySearchCriteriaForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    dbSmartAspects.AddInParameter(cmd, "@IsAdminUser", DbType.Int32, isAdminUser);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    if (!string.IsNullOrEmpty(companyName))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);

                    if (companyType > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyType", DbType.Int32, companyType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyType", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(contactNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, contactNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ContactNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(companyEmail))
                        dbSmartAspects.AddInParameter(cmd, "@EmailAddress", DbType.String, companyEmail);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmailAddress", DbType.String, DBNull.Value);

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

                    if (companyOwnerId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyOwnerId", DbType.Int32, companyOwnerId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyOwnerId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(companyNumber))
                        dbSmartAspects.AddInParameter(cmd, "@CompanyNumber", DbType.String, companyNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyNumber", DbType.String, DBNull.Value);

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
                                GuestCompanyBO guestCompany = new GuestCompanyBO();
                                guestCompany.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                guestCompany.ParentCompanyId = Convert.ToInt32(reader["ParentCompanyId"]);
                                guestCompany.ParentCompany = reader["ParentCompany"].ToString();
                                guestCompany.CompanyName = reader["CompanyName"].ToString();
                                guestCompany.CompanyAddress = reader["CompanyAddress"].ToString();
                                guestCompany.EmailAddress = reader["EmailAddress"].ToString();
                                guestCompany.WebAddress = reader["WebAddress"].ToString();
                                guestCompany.ContactNumber = reader["ContactNumber"].ToString();
                                guestCompany.ContactPerson = reader["ContactPerson"].ToString();
                                guestCompany.Remarks = reader["Remarks"].ToString();
                                guestCompany.DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                                guestCompany.NodeId = Convert.ToInt32(reader["NodeId"]);
                                guestCompany.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                guestCompany.CompanyNumber = reader["CompanyNumber"].ToString();
                                guestCompany.CompanyContact = reader["CompanyContact"].ToString();
                                guestCompany.CompanyEmail = reader["CompanyEmail"].ToString();
                                guestCompany.StateName = reader["StateName"].ToString();
                                guestCompany.CityName = reader["CityName"].ToString();
                                guestCompany.CountryName = reader["CountryName"].ToString();
                                guestCompany.AssociateContacts = reader["AssociateContacts"].ToString();
                                guestCompany.LifeCycleStage = reader["LifeCycleStage"].ToString();
                                guestCompany.AccountManager = reader["AccountManager"].ToString();
                                guestCompany.CreatedDisplayDate = reader["CreatedDisplayDate"].ToString();
                                guestCompany.IsDetailPanelEnableForCompany = Convert.ToInt32(reader["IsDetailPanelEnableForCompany"]);
                                guestCompany.IsDetailPanelEnableForParentCompany = Convert.ToInt32(reader["IsDetailPanelEnableForParentCompany"]);
                                roomTypeList.Add(guestCompany);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return roomTypeList;
        }
    }
}
