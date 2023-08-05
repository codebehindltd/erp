using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestInformationDA : BaseService
    {
        public List<GuestInformationBO> GetRegistrationDetailByRegiId(int registrationId)
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationDetailByRegiId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    registrationDetail.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    registrationDetail.GuestDOB = null;
                                }
                                registrationDetail.GuestSex = reader["GuestSex"].ToString();
                                registrationDetail.GuestEmail = reader["GuestEmail"].ToString();
                                registrationDetail.GuestPhone = reader["GuestPhone"].ToString();
                                registrationDetail.GuestAddress1 = reader["GuestAddress1"].ToString();
                                registrationDetail.GuestAddress2 = reader["GuestAddress2"].ToString();
                                registrationDetail.GuestCity = reader["GuestCity"].ToString();
                                registrationDetail.GuestZipCode = reader["GuestZipCode"].ToString();
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.GuestNationality = reader["GuestNationality"].ToString();
                                registrationDetail.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                registrationDetail.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                registrationDetail.NationalId = reader["NationalId"].ToString();
                                registrationDetail.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["PIssueDate"].ToString()))
                                {
                                    registrationDetail.PIssueDate = Convert.ToDateTime(reader["PIssueDate"]);
                                }
                                else
                                {
                                    registrationDetail.PIssueDate = null;
                                }
                                registrationDetail.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["PExpireDate"].ToString()))
                                {
                                    registrationDetail.PExpireDate = Convert.ToDateTime(reader["PExpireDate"]);
                                }
                                else
                                {
                                    registrationDetail.PExpireDate = null;
                                }
                                registrationDetail.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["VIssueDate"].ToString()))
                                {
                                    registrationDetail.VIssueDate = Convert.ToDateTime(reader["VIssueDate"]);
                                }
                                else
                                {
                                    registrationDetail.VIssueDate = null;
                                }
                                registrationDetail.CountryName = reader["CountryName"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<GuestInformationBO> GetGuestInfoBySearchCriteria(string guestName, string companyName, string email, string phoneNumber, string nId, string doB, string passport, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GuestInformationBO> guestList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInfoForBlock_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    if (!string.IsNullOrWhiteSpace(guestName))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(email))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, email); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(phoneNumber))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, phoneNumber); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(nId))
                    { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, nId); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(passport))
                    { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, passport); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(doB))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.String, doB); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(companyName))
                    { dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName); }
                    else
                    { dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value); }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO guestInfo = new GuestInformationBO();

                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                guestInfo.GuestDOBShow = reader["GuestDOBShow"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }

                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                guestInfo.CompanyName = reader["CompanyName"].ToString();
                                guestInfo.GuestBlock = Convert.ToBoolean(reader["GuestBlock"]);

                                guestList.Add(guestInfo);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;

                }
            }
            return guestList;
        }
        public List<GuestInformationBO> GetGuestInformationBySearchCriteria(string GuestName, string GuestEmail, string GuestPhone, string NationalId, string PassportNumber, DateTime? GuestDOB, string CompanyName, string RoomNo, DateTime? FromDate, DateTime? ToDate, string RegistrationNo)
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    if (!string.IsNullOrWhiteSpace(GuestName))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, GuestName); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestEmail))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, GuestEmail); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestPhone))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, GuestPhone); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(NationalId))
                    { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, NationalId); }
                    else { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(PassportNumber))
                    { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, PassportNumber); }
                    else { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, DBNull.Value); }

                    if (GuestDOB != null)
                    { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.DateTime, GuestDOB); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.DateTime, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(CompanyName))
                    { dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, CompanyName); }
                    else { dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(RoomNo))
                    { dbSmartAspects.AddInParameter(cmd, "@RoomNo", DbType.String, RoomNo); }
                    else { dbSmartAspects.AddInParameter(cmd, "@RoomNo", DbType.String, DBNull.Value); }

                    if (FromDate != null)
                    { dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate); }
                    else { dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value); }

                    if (ToDate != null)
                    { dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate); }
                    else { dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(RegistrationNo))
                    { dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, RegistrationNo); }
                    else { dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, DBNull.Value); }


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.GuestName = reader["GuestName"].ToString();

                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    registrationDetail.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    registrationDetail.GuestDOB = null;
                                }

                                registrationDetail.GuestSex = reader["GuestSex"].ToString();
                                registrationDetail.GuestEmail = reader["GuestEmail"].ToString();
                                registrationDetail.GuestPhone = reader["GuestPhone"].ToString();
                                registrationDetail.GuestAddress1 = reader["GuestAddress1"].ToString();
                                registrationDetail.GuestAddress2 = reader["GuestAddress2"].ToString();
                                registrationDetail.GuestCity = reader["GuestCity"].ToString();
                                registrationDetail.GuestZipCode = reader["GuestZipCode"].ToString();
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.CountryName = reader["CountryName"].ToString();
                                registrationDetail.GuestNationality = reader["GuestNationality"].ToString();
                                registrationDetail.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                registrationDetail.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                registrationDetail.NationalId = reader["NationalId"].ToString();
                                registrationDetail.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    registrationDetail.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PIssueDate = null;
                                }
                                registrationDetail.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    registrationDetail.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PExpireDate = null;
                                }
                                registrationDetail.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    registrationDetail.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    registrationDetail.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VExpireDate = null;
                                }
                                registrationDetail.RoomNumber = reader["RoomNumber"].ToString();

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }

            //registrationDetailList = registrationDetailList.GroupBy(test => test.GuestName).Select(group => group.First()).ToList();
            return registrationDetailList;
        }
        public List<GuestInformationBO> GetGuestInformationBySearchCriteriaForPaging(string GuestName, string GuestEmail, string GuestPhone, string NationalId, string PassportNumber, string GuestDOB, string CompanyName, string RoomNo, DateTime? FromDate, DateTime? ToDate, string RegistrationNo, string ReservationNumber, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationBySearchCriteriaForPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrWhiteSpace(GuestName))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, GuestName); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestEmail))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, GuestEmail); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestPhone))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, GuestPhone); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(NationalId))
                    { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, NationalId); }
                    else { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(PassportNumber))
                    { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, PassportNumber); }
                    else { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestDOB))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.String, GuestDOB); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.String, DBNull.Value); }


                    if (!string.IsNullOrWhiteSpace(CompanyName))
                    { dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, CompanyName); }
                    else { dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(RoomNo))
                    { dbSmartAspects.AddInParameter(cmd, "@RoomNo", DbType.String, RoomNo); }
                    else { dbSmartAspects.AddInParameter(cmd, "@RoomNo", DbType.String, DBNull.Value); }

                    if (FromDate != null)
                    { dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate); }
                    else { dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value); }

                    if (ToDate != null)
                    { dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate); }
                    else { dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(RegistrationNo))
                    { dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, RegistrationNo); }
                    else { dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(ReservationNumber))
                    { dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, ReservationNumber); }
                    else { dbSmartAspects.AddInParameter(cmd, "@ReservationNumber", DbType.String, DBNull.Value); }
                    
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.GuestName = reader["GuestName"].ToString();

                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    registrationDetail.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    registrationDetail.GuestDOB = null;
                                }

                                registrationDetail.GuestSex = reader["GuestSex"].ToString();
                                registrationDetail.GuestEmail = reader["GuestEmail"].ToString();
                                registrationDetail.GuestPhone = reader["GuestPhone"].ToString();
                                registrationDetail.GuestAddress1 = reader["GuestAddress1"].ToString();
                                registrationDetail.GuestAddress2 = reader["GuestAddress2"].ToString();
                                registrationDetail.GuestCity = reader["GuestCity"].ToString();
                                registrationDetail.GuestZipCode = reader["GuestZipCode"].ToString();
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.CountryName = reader["CountryName"].ToString();
                                registrationDetail.GuestNationality = reader["GuestNationality"].ToString();
                                registrationDetail.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                registrationDetail.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                registrationDetail.NationalId = reader["NationalId"].ToString();
                                registrationDetail.PassportNumber = reader["PassportNumber"].ToString();
                                //registrationDetail.RoomNumber = reader["RoomNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    registrationDetail.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PIssueDate = null;
                                }
                                registrationDetail.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    registrationDetail.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PExpireDate = null;
                                }
                                registrationDetail.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    registrationDetail.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    registrationDetail.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VExpireDate = null;
                                }

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return registrationDetailList;
        }
        public GuestInformationBO GetGuestInformationBySearchCriteriaForAutoPopup(string GuestName, string GuestDOB, string GuestEmail, string GuestPhone, string CountryId, string NationalId, string PassportNumber)
        {
            GuestInformationBO guestInfo = new GuestInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationByAutoPopupSearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(GuestName))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, GuestName); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestEmail))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, GuestEmail); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestPhone))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, GuestPhone); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(NationalId))
                    { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, NationalId); }
                    else { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(PassportNumber))
                    { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, PassportNumber); }
                    else { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestDOB))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.String, GuestDOB); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestDOB", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrEmpty(CountryId))
                    { dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, Convert.ToInt32(CountryId)); }
                    else { dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, DBNull.Value); }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }
                                guestInfo.GuestSex = reader["GuestSex"].ToString();
                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();

                                guestInfo.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestInfo.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestInfo.GuestCity = reader["GuestCity"].ToString();
                                guestInfo.GuestZipCode = reader["GuestZipCode"].ToString();

                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                guestInfo.GuestNationality = reader["GuestNationality"].ToString();
                                guestInfo.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestInfo.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    guestInfo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PIssueDate = null;
                                }
                                guestInfo.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    guestInfo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PExpireDate = null;
                                }
                                guestInfo.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    guestInfo.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    guestInfo.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VExpireDate = null;
                                }
                                guestInfo.CountryName = reader["CountryName"].ToString();
                                guestInfo.AdditionalRemarks = reader["AdditionalRemarks"].ToString();
                                guestInfo.GuestBlock = Convert.ToBoolean(reader["GuestBlock"]);
                            }
                        }
                    }
                }
            }
            return guestInfo;
        }
        public GuestInformationBO GetGuestInformationByGuestId(int GuestId, int RegistrationId = 0)
        {
            GuestInformationBO guestInfo = new GuestInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationByGuestId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.String, GuestId);
                    if (RegistrationId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, RegistrationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, DBNull.Value);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.Title = reader["Title"].ToString();
                                guestInfo.FirstName = reader["FirstName"].ToString();
                                guestInfo.LastName = reader["LastName"].ToString();
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }
                                guestInfo.GuestSex = reader["GuestSex"].ToString();
                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();

                                guestInfo.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestInfo.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestInfo.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                                guestInfo.ProfessionName = reader["ProfessionName"].ToString();
                                guestInfo.GuestCity = reader["GuestCity"].ToString();
                                guestInfo.GuestZipCode = reader["GuestZipCode"].ToString();

                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                guestInfo.GuestNationality = reader["GuestNationality"].ToString();
                                guestInfo.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestInfo.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    guestInfo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PIssueDate = null;
                                }
                                guestInfo.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    guestInfo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PExpireDate = null;
                                }
                                guestInfo.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    guestInfo.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    guestInfo.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VExpireDate = null;
                                }
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                if (!string.IsNullOrEmpty(reader["SatyedNights"].ToString()))
                                    guestInfo.TotalStayedNight = Convert.ToInt32(reader["SatyedNights"].ToString());
                                else
                                    guestInfo.TotalStayedNight = null;
                                //guestInfo.GuestPreferences = reader["GuestPreferences"].ToString();
                                guestInfo.MealPlan = reader["MealPlan"].ToString();
                                guestInfo.Remarks = reader["Remarks"].ToString();
                                guestInfo.GuestBlock = Convert.ToBoolean(reader["GuestBlock"]);
                                guestInfo.Description = reader["Description"].ToString();
                                guestInfo.AdditionalRemarks = reader["AdditionalRemarks"].ToString();
                            }
                        }
                    }
                }
            }
            return guestInfo;
        }
        public GuestInformationBO GetGuestInformationByGuestIdNew(int GuestId)
        {

            GuestInformationBO guestInfo = new GuestInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationByGuestIdNew_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.String, GuestId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.Title = reader["Title"].ToString();
                                guestInfo.FirstName = reader["FirstName"].ToString();
                                guestInfo.LastName = reader["LastName"].ToString();
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }
                                guestInfo.GuestSex = reader["GuestSex"].ToString();
                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();

                                guestInfo.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestInfo.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestInfo.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                                guestInfo.ProfessionName = reader["ProfessionName"].ToString();
                                guestInfo.GuestCity = reader["GuestCity"].ToString();
                                guestInfo.GuestZipCode = reader["GuestZipCode"].ToString();

                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                guestInfo.GuestNationality = reader["GuestNationality"].ToString();
                                guestInfo.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestInfo.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    guestInfo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PIssueDate = null;
                                }
                                guestInfo.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    guestInfo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PExpireDate = null;
                                }
                                guestInfo.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    guestInfo.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    guestInfo.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VExpireDate = null;
                                }
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                if (!string.IsNullOrEmpty(reader["SatyedNights"].ToString()))
                                    guestInfo.TotalStayedNight = Convert.ToInt32(reader["SatyedNights"].ToString());
                                else
                                    guestInfo.TotalStayedNight = null;
                                //guestInfo.GuestPreferences = reader["GuestPreferences"].ToString();
                                guestInfo.RoomId = Convert.ToInt32(reader["RoomId"].ToString());
                                guestInfo.ShowDOB = reader["ShowDOB"].ToString();
                                guestInfo.ShowPIssueDate = reader["ShowPIssueDate"].ToString();
                                guestInfo.ShowPExpireDate = reader["ShowPExpireDate"].ToString();
                                guestInfo.ShowVIssueDate = reader["ShowVIssueDate"].ToString();
                                guestInfo.ShowVExpireDate = reader["ShowVExpireDate"].ToString();
                                guestInfo.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                guestInfo.GuestBlock = Convert.ToBoolean(reader["GuestBlock"]);
                                guestInfo.Description = reader["Description"].ToString();
                                guestInfo.AdditionalRemarks = reader["AdditionalRemarks"].ToString();
                            }
                        }
                    }
                }
            }
            return guestInfo;
        }
        public GuestInformationBO GetOnlineGuestInformationByGuestId(int GuestId)
        {

            GuestInformationBO guestInfo = new GuestInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineGuestInformationByGuestId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.String, GuestId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.Title = reader["Title"].ToString();
                                guestInfo.FirstName = reader["FirstName"].ToString();
                                guestInfo.LastName = reader["LastName"].ToString();
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }
                                guestInfo.GuestSex = reader["GuestSex"].ToString();
                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();

                                guestInfo.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestInfo.GuestAddress2 = reader["GuestAddress2"].ToString();
                                //guestInfo.ProfessionId = Int32.Parse(reader["ProfessionId"].ToString());
                                //guestInfo.ProfessionName = reader["ProfessionName"].ToString();
                                guestInfo.GuestCity = reader["GuestCity"].ToString();
                                guestInfo.GuestZipCode = reader["GuestZipCode"].ToString();

                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                guestInfo.GuestNationality = reader["GuestNationality"].ToString();
                                guestInfo.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestInfo.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                //guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                //if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                //{
                                //    guestInfo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.PIssueDate = null;
                                //}
                                //guestInfo.PIssuePlace = reader["PIssuePlace"].ToString();
                                //if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                //{
                                //    guestInfo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.PExpireDate = null;
                                //}
                                //guestInfo.VisaNumber = reader["VisaNumber"].ToString();
                                //if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                //{
                                //    guestInfo.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.VIssueDate = null;
                                //}
                                //if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                //{
                                //    guestInfo.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.VExpireDate = null;
                                //}
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                //if (!string.IsNullOrEmpty(reader["SatyedNights"].ToString()))
                                //    guestInfo.TotalStayedNight = Convert.ToInt32(reader["SatyedNights"].ToString());
                                //else
                                //    guestInfo.TotalStayedNight = null;
                                //guestInfo.GuestPreferences = reader["GuestPreferences"].ToString();
                                guestInfo.RoomId = Convert.ToInt32(reader["RoomId"].ToString());
                                guestInfo.ShowDOB = reader["ShowDOB"].ToString();
                                guestInfo.ShowPIssueDate = reader["ShowPIssueDate"].ToString();
                                guestInfo.ShowPExpireDate = reader["ShowPExpireDate"].ToString();
                                guestInfo.ShowVIssueDate = reader["ShowVIssueDate"].ToString();
                                guestInfo.ShowVExpireDate = reader["ShowVExpireDate"].ToString();
                            }
                        }
                    }
                }
            }
            return guestInfo;
        }
        public List<GuestInformationBO> GetGuestInformationDetailByRegiId(int registrationId)
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationDetailByRegiId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    registrationDetail.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    registrationDetail.GuestDOB = null;
                                }
                                registrationDetail.GuestSex = reader["GuestSex"].ToString();
                                registrationDetail.GuestEmail = reader["GuestEmail"].ToString();
                                registrationDetail.CountryName = reader["CountryName"].ToString();
                                registrationDetail.GuestPhone = reader["GuestPhone"].ToString();
                                registrationDetail.GuestAddress1 = reader["GuestAddress1"].ToString();
                                registrationDetail.GuestAddress2 = reader["GuestAddress2"].ToString();
                                registrationDetail.GuestCity = reader["GuestCity"].ToString();
                                registrationDetail.GuestZipCode = reader["GuestZipCode"].ToString();
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.GuestNationality = reader["GuestNationality"].ToString();
                                registrationDetail.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                registrationDetail.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                registrationDetail.NationalId = reader["NationalId"].ToString();
                                registrationDetail.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    registrationDetail.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PIssueDate = null;
                                }
                                registrationDetail.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    registrationDetail.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PExpireDate = null;
                                }
                                registrationDetail.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    registrationDetail.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    registrationDetail.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VExpireDate = null;
                                }
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.RegistrationId = Int32.Parse(reader["RegistrationId"].ToString());

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<GuestInformationBO> GetGuestInformationDetailByResId(int reservationId, bool IsQueryFromReservation)
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationDetailByResId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@IsQueryFromReservation", DbType.Boolean, IsQueryFromReservation);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    registrationDetail.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    registrationDetail.GuestDOB = null;
                                }

                                registrationDetail.GuestSex = reader["GuestSex"].ToString();
                                registrationDetail.CountryName = reader["CountryName"].ToString();
                                registrationDetail.GuestEmail = reader["GuestEmail"].ToString();
                                registrationDetail.GuestPhone = reader["GuestPhone"].ToString();

                                registrationDetail.GuestAddress1 = reader["GuestAddress1"].ToString();
                                registrationDetail.GuestAddress2 = reader["GuestAddress2"].ToString();
                                registrationDetail.GuestCity = reader["GuestCity"].ToString();
                                registrationDetail.GuestZipCode = reader["GuestZipCode"].ToString();
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.GuestNationality = reader["GuestNationality"].ToString();
                                registrationDetail.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                registrationDetail.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                registrationDetail.NationalId = reader["NationalId"].ToString();
                                registrationDetail.PassportNumber = reader["PassportNumber"].ToString();
                                registrationDetail.ReservationMode = reader["ReservationMode"].ToString();
                                registrationDetail.ClassificationId = Int32.Parse(reader["ClassificationId"].ToString());

                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    registrationDetail.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PIssueDate = null;
                                }
                                registrationDetail.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    registrationDetail.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PExpireDate = null;
                                }
                                registrationDetail.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    registrationDetail.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    registrationDetail.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VExpireDate = null;
                                }
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());

                                registrationDetail.RoomId = Convert.ToInt32(reader["RoomId"]);
                                registrationDetail.RoomNumber = reader["RoomNumber"].ToString();
                                registrationDetail.AdditionalRemarks = reader["AdditionalRemarks"].ToString();


                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<GuestInformationBO> GetGuestInformationDetailByResIdNew(int reservationId, bool IsQueryFromReservation)
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationDetailByResIdNew_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@IsQueryFromReservation", DbType.Boolean, IsQueryFromReservation);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.Title = reader["Title"].ToString();
                                registrationDetail.FirstName = reader["FirstName"].ToString();
                                registrationDetail.LastName = reader["LastName"].ToString();
                                registrationDetail.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    registrationDetail.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    registrationDetail.GuestDOB = null;
                                }
                                registrationDetail.GuestDOBShow = reader["GuestDOBShow"].ToString();
                                registrationDetail.GuestSex = reader["GuestSex"].ToString();
                                registrationDetail.CountryName = reader["CountryName"].ToString();
                                registrationDetail.GuestEmail = reader["GuestEmail"].ToString();
                                registrationDetail.GuestPhone = reader["GuestPhone"].ToString();

                                registrationDetail.GuestAddress1 = reader["GuestAddress1"].ToString();
                                registrationDetail.GuestAddress2 = reader["GuestAddress2"].ToString();
                                registrationDetail.GuestCity = reader["GuestCity"].ToString();
                                registrationDetail.GuestZipCode = reader["GuestZipCode"].ToString();
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                registrationDetail.GuestNationality = reader["GuestNationality"].ToString();
                                registrationDetail.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                registrationDetail.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                registrationDetail.NationalId = reader["NationalId"].ToString();
                                registrationDetail.PassportNumber = reader["PassportNumber"].ToString();
                                registrationDetail.ReservationMode = reader["ReservationMode"].ToString();


                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    registrationDetail.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PIssueDate = null;
                                }
                                registrationDetail.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    registrationDetail.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.PExpireDate = null;
                                }
                                registrationDetail.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    registrationDetail.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    registrationDetail.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    registrationDetail.VExpireDate = null;
                                }
                                registrationDetail.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());

                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public List<GuestInformationBO> GetInHouseGuestBreakfastInfo()
        {
            List<GuestInformationBO> inHouseGuestBreakfastInfoList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInHouseGuestInfoForBreakfast_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO inHouseGuestBreakfastInfo = new GuestInformationBO();
                                inHouseGuestBreakfastInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                inHouseGuestBreakfastInfo.GuestName = reader["GuestName"].ToString();
                                inHouseGuestBreakfastInfo.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                inHouseGuestBreakfastInfo.RegistrationNumber = reader["RRNumber"].ToString();
                                inHouseGuestBreakfastInfo.RoomNumber = reader["RoomNumber"].ToString();
                                inHouseGuestBreakfastInfo.GuestNationality = reader["GuestNationality"].ToString();
                                inHouseGuestBreakfastInfo.CompanyName = reader["GuestCompany"].ToString();
                                if (!string.IsNullOrEmpty(reader["DateIn"].ToString()))
                                {
                                    inHouseGuestBreakfastInfo.ArriveDate = Convert.ToDateTime(reader["DateIn"].ToString());
                                }
                                else
                                {
                                    inHouseGuestBreakfastInfo.ArriveDate = DateTime.MinValue;
                                }
                                inHouseGuestBreakfastInfo.ArriveDateString = Convert.ToDateTime(reader["DateIn"].ToString()).ToString("MM/dd/yyyy");
                                inHouseGuestBreakfastInfo.ExpectedCheckOutDateString = Convert.ToDateTime(reader["DateOut"].ToString()).ToString("MM/dd/yyyy");

                                if (!string.IsNullOrEmpty(reader["DateOut"].ToString()))
                                {
                                    inHouseGuestBreakfastInfo.ExpectedCheckOutDate = Convert.ToDateTime(reader["DateOut"].ToString());
                                }
                                else
                                {
                                    inHouseGuestBreakfastInfo.ExpectedCheckOutDate = DateTime.MinValue;
                                }
                                inHouseGuestBreakfastInfo.Remarks = reader["HotelRemarks"].ToString();
                                inHouseGuestBreakfastInfo.POSRemarks = reader["POSRemarks"].ToString();


                                inHouseGuestBreakfastInfoList.Add(inHouseGuestBreakfastInfo);
                            }
                        }

                        /*GuestInformationBO inHouseGuestBreakfastInfo1 = new GuestInformationBO();
                        inHouseGuestBreakfastInfo1.GuestId = 1;
                        inHouseGuestBreakfastInfo1.GuestName = "Mehedi";
                        inHouseGuestBreakfastInfo1.RegistrationId = 1;
                        inHouseGuestBreakfastInfo1.RegistrationNumber = "EZ0002";
                        inHouseGuestBreakfastInfo1.RoomNumber = "101";
                        inHouseGuestBreakfastInfo1.GuestNationality = "Bangladeshi";
                        inHouseGuestBreakfastInfo1.CompanyName = "Data Grid";

                        inHouseGuestBreakfastInfo1.ArriveDate = DateTime.Now;
                        inHouseGuestBreakfastInfo1.ArriveDateString = DateTime.Now.ToString("MM/dd/yyyy");
                        inHouseGuestBreakfastInfo1.ExpectedCheckOutDateString = DateTime.Now.ToString("MM/dd/yyyy");

                        inHouseGuestBreakfastInfo1.ExpectedCheckOutDate = DateTime.Now;
                        

                        inHouseGuestBreakfastInfo1.Remarks = "Hotel Remarkssss";
                        inHouseGuestBreakfastInfo1.POSRemarks = "POS Remarksss";


                        inHouseGuestBreakfastInfoList.Add(inHouseGuestBreakfastInfo1);

                        GuestInformationBO inHouseGuestBreakfastInfo2 = new GuestInformationBO();
                        inHouseGuestBreakfastInfo2.GuestId = 2;
                        inHouseGuestBreakfastInfo2.GuestName = "Mehedi";
                        inHouseGuestBreakfastInfo2.RegistrationId = 2;
                        inHouseGuestBreakfastInfo2.RegistrationNumber = "EZ00033";
                        inHouseGuestBreakfastInfo2.RoomNumber = "10121";
                        inHouseGuestBreakfastInfo2.GuestNationality = "Bangladeshi";
                        inHouseGuestBreakfastInfo2.CompanyName = "Data Grid";

                        inHouseGuestBreakfastInfo2.ArriveDate = DateTime.Now;

                        inHouseGuestBreakfastInfo2.ExpectedCheckOutDate = DateTime.Now;

                        inHouseGuestBreakfastInfo2.ArriveDateString = DateTime.Now.ToString("MM/dd/yyyy");
                        inHouseGuestBreakfastInfo2.ExpectedCheckOutDateString = DateTime.Now.ToString("MM/dd/yyyy");


                        inHouseGuestBreakfastInfo2.Remarks = "Hotel Remarkssss";
                        inHouseGuestBreakfastInfo2.POSRemarks = "POS Remarksss";


                        inHouseGuestBreakfastInfoList.Add(inHouseGuestBreakfastInfo2);*/
                    }
                }
            }
            return inHouseGuestBreakfastInfoList;
        }
        public bool SaveGuestInhouseBreakfastInfo(List<GuestInformationBO> boList, UserInformationBO userInfoBo)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestInhouseBreakfastInfo_SP"))
                {
                    foreach (GuestInformationBO bo in boList)
                    {
                        command.Parameters.Clear();
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, bo.RegistrationId);
                        dbSmartAspects.AddInParameter(command, "@GuestId", DbType.Int32, bo.GuestId);
                        dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, userInfoBo.EmpId);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            
            return status;
        }
        public List<GuestInformationBO> GetGuestOnlineInformationDetailByResId(int reservationId, bool IsQueryFromReservation)
        {
            List<GuestInformationBO> GuestList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationDetailByResIdOnline_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@IsQueryFromReservation", DbType.Boolean, IsQueryFromReservation);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO guestInfo = new GuestInformationBO();

                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.Title = reader["Title"].ToString();
                                guestInfo.FirstName = reader["FirstName"].ToString();
                                guestInfo.LastName = reader["LastName"].ToString();
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }
                                guestInfo.GuestDOBShow = reader["GuestDOBShow"].ToString();
                                guestInfo.GuestSex = reader["GuestSex"].ToString();
                                guestInfo.CountryName = reader["CountryName"].ToString();
                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();

                                guestInfo.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestInfo.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestInfo.GuestCity = reader["GuestCity"].ToString();
                                guestInfo.GuestZipCode = reader["GuestZipCode"].ToString();
                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestInfo.GuestNationality = reader["GuestNationality"].ToString();
                                guestInfo.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestInfo.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                //registrationDetail.ReservationMode = reader["ReservationMode"].ToString();


                                //if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                //{
                                //    guestInfo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.PIssueDate = null;
                                //}
                                //guestInfo.PIssuePlace = reader["PIssuePlace"].ToString();
                                //if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                //{
                                //    guestInfo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.PExpireDate = null;
                                //}
                                //guestInfo.VisaNumber = reader["VisaNumber"].ToString();
                                //if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                //{
                                //    guestInfo.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.VIssueDate = null;
                                //}
                                //if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                //{
                                //    guestInfo.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                //}
                                //else
                                //{
                                //    guestInfo.VExpireDate = null;
                                //}
                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());

                                GuestList.Add(guestInfo);
                            }
                        }
                    }
                }
            }
            return GuestList;
        }
        public bool SaveGuestRegistrationInformation(int tempRegId, int GuestId)
        {
            Boolean status = false;
            if (!IsAvailableGuestRegistration(tempRegId, GuestId))
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestRegistrationInfo_SP"))
                    {
                        command.Parameters.Clear();
                        dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, tempRegId);
                        dbSmartAspects.AddInParameter(command, "@GuestId", DbType.Int32, GuestId);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            else
            {
                status = true;
            }
            return status;
        }
        public bool IsAvailableGuestRegistration(int tempRegId, int GuestId)
        {
            bool isAvailable = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("IsAvailableGuestRegistration_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, tempRegId);
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.Int32, GuestId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                isAvailable = true;
                            }
                        }
                    }
                }
                conn.Close();
            }
            return isAvailable;
        }
        public bool UpdateGuestBlockInfo(int guestId, bool isGuestBlock, string description)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestBlockInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, guestId);
                    dbSmartAspects.AddInParameter(command, "@isGuestBlock", DbType.Boolean, isGuestBlock);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, description);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateGuestInformation(GuestInformationBO registrationDetailBO, string guestDeletedDoc, List<GuestPreferenceMappingBO> preferenList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGuestInformation_SP"))
                    {

                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestName", DbType.String, registrationDetailBO.GuestName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDOB", DbType.DateTime, registrationDetailBO.GuestDOB);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSex", DbType.String, registrationDetailBO.GuestSex);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestEmail", DbType.String, registrationDetailBO.GuestEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestPhone", DbType.String, registrationDetailBO.GuestPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress1", DbType.String, registrationDetailBO.GuestAddress1);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress2", DbType.String, registrationDetailBO.GuestAddress2);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProfessionId", DbType.Int32, registrationDetailBO.ProfessionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCity", DbType.String, registrationDetailBO.GuestCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestZipCode", DbType.String, registrationDetailBO.GuestZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCountryId", DbType.Int32, registrationDetailBO.GuestCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestNationality", DbType.String, registrationDetailBO.GuestNationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDrivinlgLicense", DbType.String, registrationDetailBO.GuestDrivinlgLicense);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAuthentication", DbType.String, registrationDetailBO.GuestAuthentication);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, registrationDetailBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, registrationDetailBO.PassportNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, registrationDetailBO.PIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, registrationDetailBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, registrationDetailBO.PExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisaNumber", DbType.String, registrationDetailBO.VisaNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIssueDate", DbType.DateTime, registrationDetailBO.VIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VExpireDate", DbType.DateTime, registrationDetailBO.VExpireDate);
                        //dbSmartAspects.AddInParameter(commandMaster, "@GuestPreferences", DbType.String, registrationDetailBO.GuestPreferences);
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction) > 0 ? true : false;

                        using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                        {
                            string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                            commandPreference.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                            dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                            dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                        }
                    }

                    if (status)
                    {
                        if (!string.IsNullOrEmpty(guestDeletedDoc))
                        {
                            status = false;
                            string[] documentId = guestDeletedDoc.Split(',');

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

                conn.Close();

            }
            bool success = UpdateDocumentInformation(registrationDetailBO);
            return status;
        }
        public bool UpdateGuestInformationNew(GuestInformationBO registrationDetailBO, string reservationId, string guestDeletedDoc, List<GuestPreferenceMappingBO> preferenList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGuestInformationNew_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ReservationId", DbType.Int32, reservationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Title", DbType.String, registrationDetailBO.Title);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, registrationDetailBO.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, registrationDetailBO.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestName", DbType.String, registrationDetailBO.GuestName);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDOB", DbType.DateTime, registrationDetailBO.GuestDOB);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestSex", DbType.String, registrationDetailBO.GuestSex);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestEmail", DbType.String, registrationDetailBO.GuestEmail);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestPhone", DbType.String, registrationDetailBO.GuestPhone);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress1", DbType.String, registrationDetailBO.GuestAddress1);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress2", DbType.String, registrationDetailBO.GuestAddress2);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProfessionId", DbType.Int32, registrationDetailBO.ProfessionId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCity", DbType.String, registrationDetailBO.GuestCity);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestZipCode", DbType.String, registrationDetailBO.GuestZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestCountryId", DbType.Int32, registrationDetailBO.GuestCountryId);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestNationality", DbType.String, registrationDetailBO.GuestNationality);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestDrivinlgLicense", DbType.String, registrationDetailBO.GuestDrivinlgLicense);
                        dbSmartAspects.AddInParameter(commandMaster, "@GuestAuthentication", DbType.String, registrationDetailBO.GuestAuthentication);
                        dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, registrationDetailBO.NationalId);
                        dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, registrationDetailBO.PassportNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.DateTime, registrationDetailBO.PIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, registrationDetailBO.PIssuePlace);
                        dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.DateTime, registrationDetailBO.PExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VisaNumber", DbType.String, registrationDetailBO.VisaNumber);
                        dbSmartAspects.AddInParameter(commandMaster, "@VIssueDate", DbType.DateTime, registrationDetailBO.VIssueDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@VExpireDate", DbType.DateTime, registrationDetailBO.VExpireDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@ClassificationId", DbType.Int32, registrationDetailBO.ClassificationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@AdditionalRemarks", DbType.String, registrationDetailBO.AdditionalRemarks);
                        dbSmartAspects.AddInParameter(commandMaster, "@RoomId", DbType.Int32, registrationDetailBO.RoomId);
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction) > 0 ? true : false;

                        using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                        {
                            string preferences = string.Join(",", preferenList.Select(i => i.PreferenceId));

                            commandPreference.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                            dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferences);

                            dbSmartAspects.ExecuteNonQuery(commandPreference, transction);

                        }
                    }
                    if (status)
                    {
                        if (!string.IsNullOrEmpty(guestDeletedDoc))
                        {
                            status = false;
                            string[] documentId = guestDeletedDoc.Split(',');

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

                conn.Close();

            }
            bool success = UpdateDocumentInformation(registrationDetailBO);
            return status;
        }
        public bool UpdateGuestInformationWithOutDocument(GuestInformationBO registrationDetailBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGuestInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestName", DbType.String, registrationDetailBO.GuestName);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestDOB", DbType.DateTime, registrationDetailBO.GuestDOB);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestSex", DbType.String, registrationDetailBO.GuestSex);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestEmail", DbType.String, registrationDetailBO.GuestEmail);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestPhone", DbType.String, registrationDetailBO.GuestPhone);

                    dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress1", DbType.String, registrationDetailBO.GuestAddress1);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestAddress2", DbType.String, registrationDetailBO.GuestAddress2);
                    dbSmartAspects.AddInParameter(commandMaster, "@ProfessionId", DbType.Int32, registrationDetailBO.ProfessionId);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestCity", DbType.String, registrationDetailBO.GuestCity);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestZipCode", DbType.String, registrationDetailBO.GuestZipCode);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestCountryId", DbType.Int32, registrationDetailBO.GuestCountryId);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestNationality", DbType.String, registrationDetailBO.GuestNationality);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestDrivinlgLicense", DbType.String, registrationDetailBO.GuestDrivinlgLicense);
                    dbSmartAspects.AddInParameter(commandMaster, "@GuestAuthentication", DbType.String, registrationDetailBO.GuestAuthentication);
                    dbSmartAspects.AddInParameter(commandMaster, "@NationalId", DbType.String, registrationDetailBO.NationalId);
                    dbSmartAspects.AddInParameter(commandMaster, "@PassportNumber", DbType.String, registrationDetailBO.PassportNumber);
                    dbSmartAspects.AddInParameter(commandMaster, "@PIssueDate", DbType.String, registrationDetailBO.PIssueDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@PIssuePlace", DbType.String, registrationDetailBO.PIssuePlace);
                    dbSmartAspects.AddInParameter(commandMaster, "@PExpireDate", DbType.String, registrationDetailBO.PExpireDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@VisaNumber", DbType.String, registrationDetailBO.VisaNumber);
                    dbSmartAspects.AddInParameter(commandMaster, "@VIssueDate", DbType.String, registrationDetailBO.VIssueDate);
                    dbSmartAspects.AddInParameter(commandMaster, "@VExpireDate", DbType.String, registrationDetailBO.VExpireDate);
                    //dbSmartAspects.AddInParameter(commandMaster, "@GuestPreferences", DbType.String, registrationDetailBO.GuestPreferences);
                    status = dbSmartAspects.ExecuteNonQuery(commandMaster) > 0 ? true : false;
                }
                conn.Close();
            }
            return status;
        }
        public bool UpdateDocumentInformation(GuestInformationBO registrationDetailBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateDocumentInfo_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@GuestId", DbType.Int32, registrationDetailBO.GuestId);
                    dbSmartAspects.AddInParameter(commandDocument, "@TempOwnerId", DbType.String, registrationDetailBO.tempOwnerId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<GuestInformationBO> GetAllGuestInformation()
        {
            List<GuestInformationBO> registrationDetailList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGuestInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO registrationDetail = new GuestInformationBO();

                                registrationDetail.GuestId = Convert.ToInt32(reader["GuestId"]);
                                registrationDetail.GuestName = reader["GuestName"].ToString();
                                registrationDetailList.Add(registrationDetail);
                            }
                        }
                    }
                }
            }
            return registrationDetailList;
        }
        public int CountNumberOfGuestByRegistrationId(int RegistrationId)
        {
            int numberOfGuest = -1;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CountNumberOfGuestByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, RegistrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                numberOfGuest = Convert.ToInt32(reader["NoOfGuest"]);
                            }

                        }
                    }
                }
            }
            return numberOfGuest;
        }
        public List<GuestInformationBO> GetGuestInformationByRegistrationId(int RegistrationId)
        {
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, RegistrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO guestBO = new GuestInformationBO();

                                guestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                guestBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                guestBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                guestBO.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestBO.ArriveDate = Convert.ToDateTime(reader["CheckInDate"]);
                                guestBO.Title = reader["Title"].ToString();
                                guestBO.FirstName = reader["FirstName"].ToString();
                                guestBO.LastName = reader["LastName"].ToString();
                                guestBO.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestBO.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestBO.GuestDOB = null;
                                }

                                if (!string.IsNullOrEmpty(reader["ProfessionName"].ToString()))
                                {
                                    guestBO.ProfessionName = reader["ProfessionName"].ToString();
                                }
                                else
                                    guestBO.ProfessionName = string.Empty;

                                guestBO.GuestSex = reader["GuestSex"].ToString();
                                guestBO.GuestEmail = reader["GuestEmail"].ToString();
                                guestBO.CountryName = reader["CountryName"].ToString();
                                guestBO.GuestPhone = reader["GuestPhone"].ToString();
                                guestBO.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestBO.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestBO.GuestCity = reader["GuestCity"].ToString();
                                guestBO.GuestZipCode = reader["GuestZipCode"].ToString();
                                guestBO.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestBO.GuestNationality = reader["GuestNationality"].ToString();
                                guestBO.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestBO.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestBO.NationalId = reader["NationalId"].ToString();
                                guestBO.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    guestBO.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    guestBO.PIssueDate = null;
                                }
                                guestBO.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    guestBO.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    guestBO.PExpireDate = null;
                                }
                                guestBO.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    guestBO.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    guestBO.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    guestBO.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    guestBO.VExpireDate = null;
                                }
                                guestBO.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestBO.PaxInRate = Convert.ToDecimal(reader["PaxInRate"].ToString());
                                guestBO.CurrencyTypeHead = reader["CurrencyTypeHead"].ToString();
                                guestBO.ReferanceName = reader["ReferanceName"].ToString();
                                guestBO.CompanyName = reader["CompanyName"].ToString();
                                guestBO.MealPlan = reader["MealPlan"].ToString();
                                guestBO.Remarks = reader["Remarks"].ToString();
                                guestBO.POSRemarks = reader["POSRemarks"].ToString();
                                guestBO.AdditionalRemarks = reader["AdditionalRemarks"].ToString();
                                list.Add(guestBO);
                            }

                        }
                    }
                }
            }
            return list;
        }
        public List<GuestInformationBO> GetGuestInformationByRegistrationIdForSwap(int RegistrationId)
        {
            List<GuestInformationBO> list = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, RegistrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO guestBO = new GuestInformationBO();

                                guestBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);
                                guestBO.RoomRate = Convert.ToDecimal(reader["RoomRate"]);
                                guestBO.ExpectedCheckOutDate = Convert.ToDateTime(reader["ExpectedCheckOutDate"]);
                                guestBO.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestBO.ArriveDate = Convert.ToDateTime(reader["CheckInDate"]);
                                guestBO.Title = reader["Title"].ToString();
                                guestBO.FirstName = reader["FirstName"].ToString();
                                guestBO.LastName = reader["LastName"].ToString();
                                guestBO.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestBO.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestBO.GuestDOB = null;
                                }

                                if (!string.IsNullOrEmpty(reader["ProfessionName"].ToString()))
                                {
                                    guestBO.ProfessionName = reader["ProfessionName"].ToString();
                                }
                                else
                                    guestBO.ProfessionName = string.Empty;

                                guestBO.GuestSex = reader["GuestSex"].ToString();
                                guestBO.GuestEmail = reader["GuestEmail"].ToString();
                                guestBO.CountryName = reader["CountryName"].ToString();
                                guestBO.GuestPhone = reader["GuestPhone"].ToString();
                                guestBO.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestBO.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestBO.GuestCity = reader["GuestCity"].ToString();
                                guestBO.GuestZipCode = reader["GuestZipCode"].ToString();
                                guestBO.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestBO.GuestNationality = reader["GuestNationality"].ToString();
                                guestBO.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestBO.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestBO.NationalId = reader["NationalId"].ToString();
                                guestBO.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    guestBO.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    guestBO.PIssueDate = null;
                                }
                                guestBO.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    guestBO.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    guestBO.PExpireDate = null;
                                }
                                guestBO.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    guestBO.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    guestBO.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    guestBO.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    guestBO.VExpireDate = null;
                                }
                                guestBO.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestBO.PaxInRate = Convert.ToDecimal(reader["PaxInRate"].ToString());
                                guestBO.CurrencyTypeHead = reader["CurrencyTypeHead"].ToString();
                                guestBO.ReferanceName = reader["ReferanceName"].ToString();
                                guestBO.CompanyName = reader["CompanyName"].ToString();
                                guestBO.MealPlan = reader["MealPlan"].ToString();
                                guestBO.Remarks = reader["Remarks"].ToString();
                                guestBO.POSRemarks = reader["POSRemarks"].ToString();
                                list.Add(guestBO);
                            }

                        }
                    }
                }
            }
            return list;
        }

        public DataTable GetGuestInformationDetailsByDateRange(DateTime fromDate, DateTime toDate, string reportType)
        {
            DataTable Table = new DataTable();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationDetailsByDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestInformation");
                    Table = ds.Tables["GuestInformation"];
                }
            }

            return Table;
        }
        public List<SBReportViewBO> GetGuestInformationDetails(DateTime startDate, DateTime endDate, string reportType)
        {
            List<SBReportViewBO> guestList = new List<SBReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationDetailsByDateRange_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, endDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestInformationDetails");
                    DataTable Table = ds.Tables["GuestInformationDetails"];

                    if (reportType == "SB")
                    {
                        guestList = Table.AsEnumerable().Select(r =>
                                    new SBReportViewBO
                                    {
                                        HotelCode = r.Field<string>("HotelCode"),
                                        FirstName = r.Field<string>("FirstName"),
                                        MiddleName = r.Field<string>("MiddleName"),
                                        LastName = r.Field<string>("LastName"),
                                        CountryName = r.Field<string>("CountryName"),
                                        PassportNumber = r.Field<string>("PassportNumber"),
                                        GuestSex = r.Field<string>("GuestSex"),
                                        GuestDOB = r.Field<string>("GuestDOB"),
                                        Profession = r.Field<string>("Profession"),
                                        CheckInDate = r.Field<string>("CheckInDate"),
                                        CheckOutDate = r.Field<string>("CheckOutDate"),
                                        BookingRefference = r.Field<string>("BookingRefference"),
                                        RoomNumber = r.Field<string>("RoomNumber"),
                                        Remarks = r.Field<string>("Remarks"),
                                        SortingOrderByArrivalNChekOutDate = r.Field<int>("SortingOrderByArrivalNChekOutDate")
                                    }).ToList();
                    }
                    else if (reportType == "NRB")
                    {
                        guestList = Table.AsEnumerable().Select(r =>
                                    new SBReportViewBO
                                    {
                                        HotelCode = r.Field<string>("HotelCode"),
                                        FirstName = r.Field<string>("FirstName"),
                                        MiddleName = r.Field<string>("MiddleName"),
                                        LastName = r.Field<string>("LastName"),
                                        GuestCompany = r.Field<string>("GuestCompany"),
                                        CountryName = r.Field<string>("CountryName"),
                                        PassportNumber = r.Field<string>("PassportNumber"),
                                        GuestSex = r.Field<string>("GuestSex"),
                                        GuestDOB = r.Field<string>("GuestDOB"),
                                        Profession = r.Field<string>("Profession"),
                                        CheckInDate = r.Field<string>("CheckInDate"),
                                        CheckOutDate = r.Field<string>("CheckOutDate"),
                                        BookingRefference = r.Field<string>("BookingRefference"),
                                        RoomNumber = r.Field<string>("RoomNumber"),
                                        Remarks = r.Field<string>("Remarks"),
                                        SortingOrderByArrivalNChekOutDate = r.Field<int>("SortingOrderByArrivalNChekOutDate")
                                    }).ToList();
                    }
                    else if (reportType == "Police")
                    {
                        guestList = Table.AsEnumerable().Select(r =>
                                        new SBReportViewBO
                                        {
                                            RoomNumber = r.Field<string>("RoomNumber"),
                                            CheckInDate = r.Field<string>("CheckInDate"),
                                            GuestName = r.Field<string>("GuestName"),
                                            GuestFather = r.Field<string>("GuestFather"),
                                            GuestCompany = r.Field<string>("GuestCompany"),
                                            GuestAddress = r.Field<string>("GuestAddress"),
                                            GuestNationality = r.Field<string>("GuestNationality"),
                                            ProfessionName = r.Field<string>("ProfessionName"),
                                            VisitPurpose = r.Field<string>("VisitPurpose"),
                                            Identification = r.Field<string>("Identification"),
                                            GuestMobile = r.Field<string>("GuestMobile"),
                                            CheckOutDate = r.Field<string>("CheckOutDate"),
                                            Remarks = r.Field<string>("Remarks")
                                        }).ToList();
                    }
                }
            }
            return guestList;
        }
        public List<PromotionalGuestInfoByDateRangeReportBO> GetRoomReservationInfoByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<PromotionalGuestInfoByDateRangeReportBO> guestInfo = new List<PromotionalGuestInfoByDateRangeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPromotionalGuestInfoByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    guestInfo = Tabel.AsEnumerable().Select(r => new PromotionalGuestInfoByDateRangeReportBO
                    {
                        BPHead = r.Field<string>("BPHead"),
                        BusinessPromotionId = r.Field<int?>("BusinessPromotionId"),
                        TotalGuest = r.Field<int?>("TotalGuest")

                    }).ToList();
                }

                return guestInfo;
            }
        }
        public List<CorporateGuestInfoByDateRangeBO> GetCorporateGuestInfoByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<CorporateGuestInfoByDateRangeBO> guestInfo = new List<CorporateGuestInfoByDateRangeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCorporateGuestInfoByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    guestInfo = Tabel.AsEnumerable().Select(r => new CorporateGuestInfoByDateRangeBO
                    {
                        CompanyName = r.Field<string>("CompanyName"),
                        ArrivedTime = r.Field<int?>("ArrivedTime")

                    }).ToList();
                }

                return guestInfo;
            }
        }
        public List<GuestHouseInfoForReportBO> GetTodaysExpectedArrivalRoomId()
        {
            List<GuestHouseInfoForReportBO> guestHouse = new List<GuestHouseInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTodaysExpectedArrivalRoomId_SP"))
                {
                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "GuestHouse");
                    DataTable Tabel = reservationDS.Tables["GuestHouse"];

                    guestHouse = Tabel.AsEnumerable().Select(r => new GuestHouseInfoForReportBO
                    {
                        RoomId = r.Field<int>("RoomId")
                    }).ToList();
                }

                return guestHouse;
            }
        }
        public List<GuestHouseInfoForReportBO> GetGuestHouseInfoForReport(DateTime fromDate, DateTime toDate, string reportType, string guestCompany)
        {
            List<GuestHouseInfoForReportBO> guestHouse = new List<GuestHouseInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@GuestCompany", DbType.String, guestCompany);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "GuestHouse");
                    DataTable Tabel = reservationDS.Tables["GuestHouse"];

                    guestHouse = Tabel.AsEnumerable().Select(r => new GuestHouseInfoForReportBO
                    {
                        ReportName = r.Field<string>("ReportName"),
                        RRNumber = r.Field<string>("RRNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        GuestNationality = r.Field<string>("GuestNationality"),
                        GuestPassportNumber = r.Field<string>("GuestPassportNumber"),
                        GuestCompany = r.Field<string>("GuestCompany"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        TotalPerson = r.Field<int?>("TotalPerson"),
                        ProbableArrivalTime = r.Field<string>("ProbableArrivalTime"),
                        DateIn = r.Field<string>("DateIn"),
                        DateOut = r.Field<string>("DateOut"),
                        RoomNumberOrPickInfo = r.Field<string>("RoomNumberOrPickInfo"),
                        ReportTable = r.Field<string>("ReportTable"),
                        ReportType = r.Field<string>("ReportType"),
                        ReservedBy = r.Field<string>("ReservedBy"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        GuestReferance = r.Field<string>("GuestReferance"),
                        Quantity = r.Field<int?>("Quantity"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomNumberList = r.Field<string>("RoomNumberList"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        CurrencyHead = r.Field<string>("CurrencyHead"),
                        RoomRate = r.Field<decimal>("RoomRate"),
                        AirportPickUp = r.Field<string>("AirportPickUp"),
                        AirportDrop = r.Field<string>("AirportDrop"),
                        ArrivalFlightName = r.Field<string>("ArrivalFlightName"),
                        ArrivalFlightNumber = r.Field<string>("ArrivalFlightNumber"),
                        ArrivalTimeString = r.Field<string>("ArrivalTimeString"),
                        UserName = r.Field<string>("UserName"),
                        CheckOutBy = r.Field<string>("CheckOutBy"),
                        Remarks = r.Field<string>("Remarks"),
                        VipGuestTypeId = r.Field<int?>("VipGuestTypeId"),
                        BlockCode = r.Field<string>("BlockCode"),
                        MktCode = r.Field<string>("MktCode"),
                        NumberOfPersonAdult = r.Field<int?>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int?>("NumberOfPersonChild"),
                        GroupName = r.Field<string>("GroupName"),
                        MealPlan = r.Field<string>("MealPlan"),
                        Email = r.Field<string>("Email"),
                        BillNo = r.Field<string>("BillNumber"),

                    }).ToList();
                }

                return guestHouse;
            }
        }
        public List<GuestHouseInfoForReportBO> GetInHouseGuestInfoForReport(DateTime fromDate, DateTime toDate, string reportType, string guestCompany)
        {
            List<GuestHouseInfoForReportBO> guestHouse = new List<GuestHouseInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInHouseGuestInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@GuestCompany", DbType.String, guestCompany);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "GuestHouse");
                    DataTable Tabel = reservationDS.Tables["GuestHouse"];

                    guestHouse = Tabel.AsEnumerable().Select(r => new GuestHouseInfoForReportBO
                    {
                        ReportName = r.Field<string>("ReportName"),
                        RRNumber = r.Field<string>("RRNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        GuestNationality = r.Field<string>("GuestNationality"),
                        GuestPassportNumber = r.Field<string>("GuestPassportNumber"),
                        GuestCompany = r.Field<string>("GuestCompany"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        TotalPerson = r.Field<int?>("TotalPerson"),
                        ProbableArrivalTime = r.Field<string>("ProbableArrivalTime"),
                        DateIn = r.Field<string>("DateIn"),
                        DateOut = r.Field<string>("DateOut"),
                        RoomNumberOrPickInfo = r.Field<string>("RoomNumberOrPickInfo"),
                        ReportTable = r.Field<string>("ReportTable"),
                        ReportType = r.Field<string>("ReportType"),
                        ReservedBy = r.Field<string>("ReservedBy"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        GuestReferance = r.Field<string>("GuestReferance"),
                        Quantity = r.Field<int?>("Quantity"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomNumberList = r.Field<string>("RoomNumberList"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        CurrencyHead = r.Field<string>("CurrencyHead"),
                        RoomRate = r.Field<decimal>("RoomRate"),
                        AirportPickUp = r.Field<string>("AirportPickUp"),
                        AirportDrop = r.Field<string>("AirportDrop"),
                        ArrivalFlightName = r.Field<string>("ArrivalFlightName"),
                        ArrivalFlightNumber = r.Field<string>("ArrivalFlightNumber"),
                        ArrivalTimeString = r.Field<string>("ArrivalTimeString"),
                        UserName = r.Field<string>("UserName"),
                        CheckOutBy = r.Field<string>("CheckOutBy"),
                        Remarks = r.Field<string>("Remarks"),
                        VipGuestTypeId = r.Field<int?>("VipGuestTypeId"),
                        BlockCode = r.Field<string>("BlockCode"),
                        MktCode = r.Field<string>("MktCode"),
                        NumberOfPersonAdult = r.Field<int?>("NumberOfPersonAdult"),
                        NumberOfPersonChild = r.Field<int?>("NumberOfPersonChild"),
                        GroupName = r.Field<string>("GroupName"),
                        MealPlan = r.Field<string>("MealPlan"),
                        Email = r.Field<string>("Email"),
                        BillNo = r.Field<string>("BillNumber"),
                        LocalCurrencyHead = r.Field<string>("LocalCurrencyHead"),
                        TotalRoomRateUSD = r.Field<decimal>("TotalRoomRateUSD"),
                        TotalRoomRateLocalCurrency = r.Field<decimal>("TotalRoomRateLocalCurrency")
                    }).ToList();
                }

                return guestHouse;
            }
        }
        public List<GuestStayedNightBO> GetGuestTotalStayovers(string guestName, string companyName, string passportNumber, string mobileNumber, int? minNoOfNights)
        {
            List<GuestStayedNightBO> guestInfo = new List<GuestStayedNightBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestStayedNight_SP"))
                {
                    if (!string.IsNullOrEmpty(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(companyName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(passportNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, passportNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(mobileNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, mobileNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, DBNull.Value);
                    }

                    if (minNoOfNights != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@NumberOfNight", DbType.Int32, minNoOfNights);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@NumberOfNight", DbType.Int32, DBNull.Value);
                    }

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "GuestStayedNight_SP");
                    DataTable Tabel = reservationDS.Tables["GuestStayedNight_SP"];

                    guestInfo = Tabel.AsEnumerable().Select(r => new GuestStayedNightBO
                    {
                        GuestName = r.Field<string>("GuestName"),
                        GuestPhone = r.Field<string>("GuestPhone"),
                        GuestEmail = r.Field<string>("GuestEmail"),
                        DOB = r.Field<string>("DOB"),
                        CompanyName = r.Field<string>("CompanyName"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        SatyedNights = r.Field<int?>("SatyedNights")

                    }).ToList();
                }

                return guestInfo;
            }
        }
        public List<RoomReservationInfoByDateRangeReportBO> GetReservationInfoForRegistration(int reservationId, DateTime? checkInDate, DateTime? checkOutDate, string guestName, string companyName, string reservationNo)
        {
            List<RoomReservationInfoByDateRangeReportBO> guestlist = new List<RoomReservationInfoByDateRangeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomReservationInfoFromRegistration_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, checkInDate);
                    dbSmartAspects.AddInParameter(cmd, "@CheckOutDate", DbType.DateTime, checkOutDate);
                    if (!string.IsNullOrEmpty(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(companyName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(reservationNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, reservationNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, DBNull.Value);
                    }

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "ReservationInfo");
                    DataTable Tabel = reservationDS.Tables["ReservationInfo"];

                    guestlist = Tabel.AsEnumerable().Select(r => new RoomReservationInfoByDateRangeReportBO
                    {
                        ReservationId = r.Field<int?>("ReservationId"),
                        ReservationDetailId = r.Field<int?>("ReservationDetailId"),
                        GuestName = r.Field<string>("GuestName"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        RCheckInDate = r.Field<DateTime>("RCheckInDate"),
                        CheckIn = r.Field<string>("CheckInDate"),
                        RCheckOutDate = r.Field<DateTime>("RCheckOutDate"),
                        DateOut = r.Field<string>("CheckOutDate"),
                        RoomId = r.Field<int?>("RoomId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        RoomType = r.Field<string>("RoomType"),
                        PartialRegistration = r.Field<int?>("PartialRegistration")
                    }).ToList();
                }

                return guestlist;
            }
        }
        public List<RoomReservationInfoByDateRangeReportBO> GetReservationInfoForRegistrationWithPaging(int reservationId, DateTime? checkInDate, DateTime? checkOutDate, string guestName, string companyName, string reservationNo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RoomReservationInfoByDateRangeReportBO> guestlist = new List<RoomReservationInfoByDateRangeReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationInfoForRegistrationWithPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, checkInDate);
                    dbSmartAspects.AddInParameter(cmd, "@CheckOutDate", DbType.DateTime, checkOutDate);
                    if (!string.IsNullOrEmpty(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(companyName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(reservationNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, reservationNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReservationNo", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "ReservationInfo");
                    DataTable Tabel = reservationDS.Tables["ReservationInfo"];

                    guestlist = Tabel.AsEnumerable().Select(r => new RoomReservationInfoByDateRangeReportBO
                    {
                        ReservationId = r.Field<int?>("ReservationId"),
                        ReservationDetailId = r.Field<int?>("ReservationDetailId"),
                        GuestName = r.Field<string>("GuestName"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        RCheckInDate = r.Field<DateTime>("RCheckInDate"),
                        CheckIn = r.Field<string>("CheckInDate"),
                        RCheckOutDate = r.Field<DateTime>("RCheckOutDate"),
                        DateOut = r.Field<string>("CheckOutDate"),
                        RoomId = r.Field<int?>("RoomId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        RoomType = r.Field<string>("RoomType"),
                        PartialRegistration = r.Field<int?>("PartialRegistration")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }


            }
            return guestlist;
        }
        public List<GuestInformationBO> GetPrevGuestInfoForReservation(string GuestName, int CountryId, string GuestPhone, string GuestEmail, string passportNumber, string nationalId, int companyId)
        {
            List<GuestInformationBO> guestInfoList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPrevGuestInfoForReservation_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(GuestName))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, GuestName); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestEmail))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, GuestEmail); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestEmail", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(GuestPhone))
                    { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, GuestPhone); }
                    else { dbSmartAspects.AddInParameter(cmd, "@GuestPhone", DbType.String, DBNull.Value); }

                    if (CountryId != 0)
                    { dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, Convert.ToInt32(CountryId)); }
                    else { dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(passportNumber))
                    { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, passportNumber); }
                    else { dbSmartAspects.AddInParameter(cmd, "@PassportNumber", DbType.String, DBNull.Value); }

                    if (!string.IsNullOrWhiteSpace(nationalId))
                    { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, nationalId); }
                    else { dbSmartAspects.AddInParameter(cmd, "@NationalId", DbType.String, DBNull.Value); }

                    if (companyId != 0)
                    { dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, Convert.ToInt32(companyId)); }
                    else { dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value); }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO guestInfo = new GuestInformationBO();

                                guestInfo.GuestId = Convert.ToInt32(reader["GuestId"]);
                                guestInfo.GuestName = reader["GuestName"].ToString();
                                if (!string.IsNullOrEmpty(reader["GuestDOB"].ToString()))
                                {
                                    guestInfo.GuestDOB = Convert.ToDateTime(reader["GuestDOB"].ToString());
                                }
                                else
                                {
                                    guestInfo.GuestDOB = null;
                                }
                                guestInfo.GuestSex = reader["GuestSex"].ToString();
                                guestInfo.GuestEmail = reader["GuestEmail"].ToString();
                                guestInfo.GuestPhone = reader["GuestPhone"].ToString();

                                guestInfo.GuestAddress1 = reader["GuestAddress1"].ToString();
                                guestInfo.GuestAddress2 = reader["GuestAddress2"].ToString();
                                guestInfo.GuestCity = reader["GuestCity"].ToString();
                                guestInfo.GuestZipCode = reader["GuestZipCode"].ToString();

                                guestInfo.GuestCountryId = Int32.Parse(reader["GuestCountryId"].ToString());
                                guestInfo.CountryName = reader["CountryName"].ToString();

                                guestInfo.GuestNationality = reader["GuestNationality"].ToString();
                                guestInfo.GuestDrivinlgLicense = reader["GuestDrivinlgLicense"].ToString();
                                guestInfo.GuestAuthentication = reader["GuestAuthentication"].ToString();
                                guestInfo.NationalId = reader["NationalId"].ToString();
                                guestInfo.PassportNumber = reader["PassportNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["PIssueDate"].ToString()))
                                {
                                    guestInfo.PIssueDate = Convert.ToDateTime(reader["PIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PIssueDate = null;
                                }
                                guestInfo.PIssuePlace = reader["PIssuePlace"].ToString();
                                if (!string.IsNullOrEmpty(reader["PExpireDate"].ToString()))
                                {
                                    guestInfo.PExpireDate = Convert.ToDateTime(reader["PExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.PExpireDate = null;
                                }
                                guestInfo.VisaNumber = reader["VisaNumber"].ToString();
                                if (!string.IsNullOrEmpty(reader["VIssueDate"].ToString()))
                                {
                                    guestInfo.VIssueDate = Convert.ToDateTime(reader["VIssueDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VIssueDate = null;
                                }
                                if (!string.IsNullOrEmpty(reader["VExpireDate"].ToString()))
                                {
                                    guestInfo.VExpireDate = Convert.ToDateTime(reader["VExpireDate"].ToString());
                                }
                                else
                                {
                                    guestInfo.VExpireDate = null;
                                }
                                guestInfo.CountryName = reader["CountryName"].ToString();
                                guestInfo.ShowDOB = reader["ShowDOB"].ToString();
                                guestInfo.RegistrationNumber = reader["RegistrationNumber"].ToString();
                                guestInfo.RoomNumber = reader["RoomNumber"].ToString();
                                guestInfo.ShowCheckInDate = reader["ShowCheckInDate"].ToString();
                                guestInfo.ShowCheckOutDate = reader["ShowCheckOutDate"].ToString();

                                guestInfoList.Add(guestInfo);
                            }
                        }
                    }
                }
            }
            return guestInfoList;
        }
        public List<GuestInformationBO> GetMultipleAireportPickupDropInfo(int reservationId)
        {
            List<GuestInformationBO> guestInfoList = new List<GuestInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMultipleAireportPickupDropInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestInformationBO guestInfo = new GuestInformationBO();

                                guestInfo.GuestName = reader["GuestName"].ToString();
                                guestInfo.APDId = Convert.ToInt32(reader["APDId"].ToString());
                                guestInfo.ArrivalFlightName = reader["ArrivalFlightName"].ToString();

                                guestInfoList.Add(guestInfo);
                            }
                        }
                    }
                }
            }
            return guestInfoList;
        }
        public GuestRegistrationMappingBO GetGuestIdFMRoomId(int roomId)
        {
            GuestRegistrationMappingBO entityBO = new GuestRegistrationMappingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestIdFMRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestInfo");
                    DataTable Table = ds.Tables["GuestInfo"];
                    entityBO = Table.AsEnumerable().Select(r =>
                                new GuestRegistrationMappingBO
                                {
                                    GuestId = r.Field<long>("GuestId")
                                }).FirstOrDefault();
                }
            }
            return entityBO;
        }
        public GuestReservationMappingBO GetReservationIdFMRoomId(int roomId)
        {
            GuestReservationMappingBO entityBO = new GuestReservationMappingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationIdFMRoomId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestInfo");
                    DataTable Table = ds.Tables["GuestInfo"];
                    entityBO = Table.AsEnumerable().Select(r =>
                                new GuestReservationMappingBO
                                {
                                    GuestId = r.Field<long>("GuestId"),
                                    ReservationId = r.Field<long>("ReservationId")
                                }).FirstOrDefault();
                }
            }
            return entityBO;
        }
        public List<GuestHouseInfoForReportBO> GetGuestExpectedDepartureList(string guestName, string companyName, string regNumber, string roomNumber, DateTime? checkInDate, DateTime? checkOutDate)
        {
            List<GuestHouseInfoForReportBO> guestHouse = new List<GuestHouseInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestExpectedDepartureList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CheckInDate", DbType.DateTime, checkInDate);
                    dbSmartAspects.AddInParameter(cmd, "@CheckOutDate", DbType.DateTime, checkOutDate);
                    if (!string.IsNullOrEmpty(guestName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, guestName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@GuestName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(companyName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(regNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, regNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RegistrationNo", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(roomNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, roomNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumber", DbType.String, DBNull.Value);
                    }

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "GuestHouse");
                    DataTable Tabel = reservationDS.Tables["GuestHouse"];

                    guestHouse = Tabel.AsEnumerable().Select(r => new GuestHouseInfoForReportBO
                    {
                        RRNumber = r.Field<string>("RRNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        //GuestNationality = r.Field<string>("GuestNationality"),
                        GuestCompany = r.Field<string>("GuestCompany"),
                        ProbableArrivalTime = r.Field<string>("ProbableArrivalTime"),
                        DateIn = r.Field<string>("DateIn"),
                        DateOut = r.Field<string>("DateOut"),
                        RoomNumberList = r.Field<string>("RoomNumberList"),
                        IsStopChargePosting = r.Field<int>("IsStopChargePosting"),
                    }).ToList();
                }

                return guestHouse;
            }
        }
        public List<SettlementSummaryBO> GetSettlementSummaryInfo(DateTime fromDate, DateTime toDate)
        {
            List<SettlementSummaryBO> settlementSummary = new List<SettlementSummaryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSettlementSummaryInfo_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet settlementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, settlementDS, "SettlementSummary");
                    DataTable Tabel = settlementDS.Tables["SettlementSummary"];

                    settlementSummary = Tabel.AsEnumerable().Select(r => new SettlementSummaryBO
                    {
                        BillNumber = r.Field<string>("BillNumber"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        FolioNumber = r.Field<int>("FolioNumber"),
                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        BillAmount = r.Field<decimal>("BillAmount"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        SettlementAmount = r.Field<decimal>("SettlementAmount"),
                        UserName = r.Field<string>("UserName")

                    }).ToList();
                }

            }
            return settlementSummary;
        }
        public List<CorporateGuestInfoByDateRangeBO> GetCorporateGuestInfoByDateRangeAndCompanyList(DateTime fromDate, DateTime toDate, string CompanyList)
        {
            List<CorporateGuestInfoByDateRangeBO> guestInfo = new List<CorporateGuestInfoByDateRangeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCorporateGuestInfoByDateRangeNCompanyId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, CompanyList);

                    DataSet reservationDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, reservationDS, "Reservation");
                    DataTable Tabel = reservationDS.Tables["Reservation"];

                    guestInfo = Tabel.AsEnumerable().Select(r => new CorporateGuestInfoByDateRangeBO
                    {
                        CompanyName = r.Field<string>("CompanyName"),
                        ArrivedTime = r.Field<int?>("ArrivedTime")

                    }).ToList();
                }

                return guestInfo;
            }
        }
    }
}
