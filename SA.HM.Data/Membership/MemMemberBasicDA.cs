using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Membership;
using System.Collections;

namespace HotelManagement.Data.Membership
{
    public class MemMemberBasicDA : BaseService
    {
        public Boolean SaveMemberBasicInfo(MemMemberBasicsBO memBasicInfo, out int tmpMemberId, List<MemMemberReferenceBO> referenceList, List<MemMemberFamilyMemberBO> familyMemberList, List<OnlineMemberEducationBO> educationList)
        {
            bool status = false;
            int tmpCompanyId = 0;
            int membersave = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("SaveMemMemberBasicInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@CompanyId", DbType.Int32, tmpCompanyId);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberType", DbType.Int32, memBasicInfo.TypeId);
                            dbSmartAspects.AddInParameter(commandMember, "@MembershipNumber", DbType.String, memBasicInfo.MembershipNumber);
                            dbSmartAspects.AddInParameter(commandMember, "@NameTitle", DbType.String, memBasicInfo.NameTitle);
                            dbSmartAspects.AddInParameter(commandMember, "@FirstName", DbType.String, memBasicInfo.FirstName);
                            dbSmartAspects.AddInParameter(commandMember, "@MiddleName", DbType.String, memBasicInfo.MiddleName);
                            dbSmartAspects.AddInParameter(commandMember, "@LastName", DbType.String, memBasicInfo.LastName);
                            dbSmartAspects.AddInParameter(commandMember, "@FullName", DbType.String, memBasicInfo.FullName);
                            dbSmartAspects.AddInParameter(commandMember, "@FatherName", DbType.String, memBasicInfo.FatherName);
                            dbSmartAspects.AddInParameter(commandMember, "@MotherName", DbType.String, memBasicInfo.MotherName);
                            dbSmartAspects.AddInParameter(commandMember, "@BirthDate", DbType.DateTime, memBasicInfo.BirthDate);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberGender", DbType.Int32, memBasicInfo.MemberGender);
                            dbSmartAspects.AddInParameter(commandMember, "@BloodGroup", DbType.Int32, memBasicInfo.BloodGroup);
                            dbSmartAspects.AddInParameter(commandMember, "@MaritalStatus", DbType.Int32, memBasicInfo.MaritalStatus);
                            dbSmartAspects.AddInParameter(commandMember, "@Nationality", DbType.Int32, memBasicInfo.Nationality);
                            dbSmartAspects.AddInParameter(commandMember, "@PassportNumber", DbType.String, memBasicInfo.PassportNumber);
                            dbSmartAspects.AddInParameter(commandMember, "@Occupation", DbType.String, memBasicInfo.Occupation);
                            dbSmartAspects.AddInParameter(commandMember, "@Organization", DbType.String, memBasicInfo.Organization);
                            dbSmartAspects.AddInParameter(commandMember, "@Designation", DbType.String, memBasicInfo.Designation);
                            dbSmartAspects.AddInParameter(commandMember, "@MonthlyIncome", DbType.Decimal, memBasicInfo.MonthlyIncome);
                            dbSmartAspects.AddInParameter(commandMember, "@AnnualTurnover", DbType.Decimal, memBasicInfo.AnnualTurnover);
                            dbSmartAspects.AddInParameter(commandMember, "@SecurityDeposit", DbType.Decimal, memBasicInfo.SecurityDeposit);
                            dbSmartAspects.AddInParameter(commandMember, "@RegistrationDate", DbType.DateTime, memBasicInfo.RegistrationDate);
                            dbSmartAspects.AddInParameter(commandMember, "@ExpiryDate", DbType.DateTime, memBasicInfo.ExpiryDate);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberAddress", DbType.String, memBasicInfo.MemberAddress);
                            dbSmartAspects.AddInParameter(commandMember, "@MailAddress", DbType.String, memBasicInfo.MailAddress);
                            dbSmartAspects.AddInParameter(commandMember, "@MobileNumber", DbType.String, memBasicInfo.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMember, "@OfficePhone", DbType.String, memBasicInfo.OfficePhone);
                            dbSmartAspects.AddInParameter(commandMember, "@HomeFax", DbType.String, memBasicInfo.HomeFax);
                            dbSmartAspects.AddInParameter(commandMember, "@OfficeFax", DbType.String, memBasicInfo.OfficeFax);
                            dbSmartAspects.AddInParameter(commandMember, "@PersonalEmail", DbType.String, memBasicInfo.PersonalEmail);
                            dbSmartAspects.AddInParameter(commandMember, "@OfficeEmail", DbType.String, memBasicInfo.OfficeEmail);
                            dbSmartAspects.AddInParameter(commandMember, "@CreatedBy", DbType.Int32, memBasicInfo.CreatedBy);

                            dbSmartAspects.AddInParameter(commandMember, "@OfficeAddress", DbType.String, memBasicInfo.OfficeAddress);
                            dbSmartAspects.AddInParameter(commandMember, "@Height", DbType.Int64, memBasicInfo.Height);
                            dbSmartAspects.AddInParameter(commandMember, "@Weight", DbType.Int64, memBasicInfo.Weight);
                            dbSmartAspects.AddInParameter(commandMember, "@ReligionId", DbType.Int32, memBasicInfo.ReligionId);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeRelationId", DbType.Int32, memBasicInfo.NomineeRelationId);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeDOB", DbType.DateTime, memBasicInfo.NomineeDOB);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeName", DbType.String, memBasicInfo.NomineeName);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeFather", DbType.String, memBasicInfo.NomineeFather);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeMother", DbType.String, memBasicInfo.NomineeMother);

                            dbSmartAspects.AddInParameter(commandMember, "@MemberPassword", DbType.String, memBasicInfo.MemberPassword);

                            dbSmartAspects.AddOutParameter(commandMember, "@MemberId", DbType.Int32, sizeof(Int32));
                            membersave = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);

                            tmpMemberId = Convert.ToInt32(commandMember.Parameters["@MemberId"].Value);
                        }

                        if (membersave > 0)
                        {
                            int countReference = 0;
                            int countFamilyMember = 0;
                            int countEducation = 0;

                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("SaveMemMemberReferenceInfo_SP"))
                                {
                                    foreach (MemMemberReferenceBO bo in referenceList)
                                    {
                                        commandReference.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandReference, "@MemberId", DbType.Int32, tmpMemberId);
                                        dbSmartAspects.AddInParameter(commandReference, "@Arbitrator", DbType.String, bo.Arbitrator);
                                        dbSmartAspects.AddInParameter(commandReference, "@ArbitratorMode", DbType.String, bo.ArbitratorMode);
                                        dbSmartAspects.AddInParameter(commandReference, "@Relationship", DbType.String, bo.Relationship);

                                        countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transaction);
                                    }
                                }
                            }

                            if (familyMemberList != null)
                            {
                                using (DbCommand commandFamilyMember = dbSmartAspects.GetStoredProcCommand("SaveMemFamilyMemberInfo_SP"))
                                {
                                    foreach (MemMemberFamilyMemberBO bo in familyMemberList)
                                    {
                                        commandFamilyMember.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberId", DbType.Int32, tmpMemberId);
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberName", DbType.String, bo.MemberName);
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberDOB", DbType.DateTime, bo.MemberDOB);
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@Occupation", DbType.String, bo.Occupation);
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@Relationship", DbType.String, bo.Relationship);
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@UsageMode", DbType.String, bo.UsageMode);

                                        countFamilyMember += dbSmartAspects.ExecuteNonQuery(commandFamilyMember, transaction);
                                    }
                                }
                            }
                            if (educationList != null)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("SaveOnlineMemEducation_SP"))
                                {
                                    foreach (var item in educationList)
                                    {
                                        commandEducation.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandEducation, "@MemberId", DbType.Int32, tmpMemberId);
                                        dbSmartAspects.AddInParameter(commandEducation, "@Degree", DbType.String, item.Degree);
                                        dbSmartAspects.AddInParameter(commandEducation, "@Institution", DbType.String, item.Institution);
                                        dbSmartAspects.AddInParameter(commandEducation, "@PassingYear", DbType.Int32, item.PassingYear);
                                        dbSmartAspects.AddInParameter(commandEducation, "@CreatedBy", DbType.Int32, item.CreatedBy);

                                        countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transaction);
                                    }
                                }
                            }
                        }

                        if (membersave > 0)
                        {
                            transaction.Commit();
                            status = true;
                        }
                        else
                        {
                            transaction.Rollback();
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

        public Boolean UpdateMemberBasicInfo(MemMemberBasicsBO memBasicInfo, List<MemMemberReferenceBO> referenceList, ArrayList arrayReferenceDelete, List<MemMemberFamilyMemberBO> familyMemberList, ArrayList arrayFamilyMemberDelete, List<OnlineMemberEducationBO> educationList, ArrayList arrayEducationDelete, int isNewChartOfAccountsHeadCreateForMember)
        {
            bool status = false;
            int update = 0;
            int tmpMemberId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("UpdateMemMemberBasicInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMember, "@MemberType", DbType.Int32, memBasicInfo.TypeId);
                        dbSmartAspects.AddInParameter(commandMember, "@CompanyId", DbType.Int32, memBasicInfo.CompanyId);
                        dbSmartAspects.AddInParameter(commandMember, "@MemberId", DbType.Int32, memBasicInfo.MemberId);
                        dbSmartAspects.AddInParameter(commandMember, "@MembershipNumber", DbType.String, memBasicInfo.MembershipNumber);
                        dbSmartAspects.AddInParameter(commandMember, "@NameTitle", DbType.String, memBasicInfo.NameTitle);
                        dbSmartAspects.AddInParameter(commandMember, "@FirstName", DbType.String, memBasicInfo.FirstName);
                        dbSmartAspects.AddInParameter(commandMember, "@MiddleName", DbType.String, memBasicInfo.MiddleName);
                        dbSmartAspects.AddInParameter(commandMember, "@LastName", DbType.String, memBasicInfo.LastName);
                        dbSmartAspects.AddInParameter(commandMember, "@FullName", DbType.String, memBasicInfo.FullName);
                        dbSmartAspects.AddInParameter(commandMember, "@FatherName", DbType.String, memBasicInfo.FatherName);
                        dbSmartAspects.AddInParameter(commandMember, "@MotherName", DbType.String, memBasicInfo.MotherName);
                        dbSmartAspects.AddInParameter(commandMember, "@BirthDate", DbType.DateTime, memBasicInfo.BirthDate);
                        dbSmartAspects.AddInParameter(commandMember, "@MemberGender", DbType.Int32, memBasicInfo.MemberGender);
                        dbSmartAspects.AddInParameter(commandMember, "@BloodGroup", DbType.Int32, memBasicInfo.BloodGroup);
                        dbSmartAspects.AddInParameter(commandMember, "@MaritalStatus", DbType.Int32, memBasicInfo.MaritalStatus);
                        dbSmartAspects.AddInParameter(commandMember, "@Nationality", DbType.Int32, memBasicInfo.Nationality);
                        dbSmartAspects.AddInParameter(commandMember, "@PassportNumber", DbType.String, memBasicInfo.PassportNumber);
                        dbSmartAspects.AddInParameter(commandMember, "@Occupation", DbType.String, memBasicInfo.Occupation);
                        dbSmartAspects.AddInParameter(commandMember, "@Organization", DbType.String, memBasicInfo.Organization);
                        dbSmartAspects.AddInParameter(commandMember, "@Designation", DbType.String, memBasicInfo.Designation);
                        dbSmartAspects.AddInParameter(commandMember, "@MonthlyIncome", DbType.Decimal, memBasicInfo.MonthlyIncome);
                        dbSmartAspects.AddInParameter(commandMember, "@AnnualTurnover", DbType.Decimal, memBasicInfo.AnnualTurnover);
                        dbSmartAspects.AddInParameter(commandMember, "@SecurityDeposit", DbType.Decimal, memBasicInfo.SecurityDeposit);
                        dbSmartAspects.AddInParameter(commandMember, "@RegistrationDate", DbType.DateTime, memBasicInfo.RegistrationDate);
                        dbSmartAspects.AddInParameter(commandMember, "@ExpiryDate", DbType.DateTime, memBasicInfo.ExpiryDate);
                        dbSmartAspects.AddInParameter(commandMember, "@MemberAddress", DbType.String, memBasicInfo.MemberAddress);
                        dbSmartAspects.AddInParameter(commandMember, "@MailAddress", DbType.String, memBasicInfo.MailAddress);
                        dbSmartAspects.AddInParameter(commandMember, "@MobileNumber", DbType.String, memBasicInfo.MobileNumber);
                        dbSmartAspects.AddInParameter(commandMember, "@OfficePhone", DbType.String, memBasicInfo.OfficePhone);
                        dbSmartAspects.AddInParameter(commandMember, "@HomeFax", DbType.String, memBasicInfo.HomeFax);
                        dbSmartAspects.AddInParameter(commandMember, "@OfficeFax", DbType.String, memBasicInfo.OfficeFax);
                        dbSmartAspects.AddInParameter(commandMember, "@PersonalEmail", DbType.String, memBasicInfo.PersonalEmail);
                        dbSmartAspects.AddInParameter(commandMember, "@OfficeEmail", DbType.String, memBasicInfo.OfficeEmail);
                        dbSmartAspects.AddInParameter(commandMember, "@LastModifiedBy", DbType.Int32, memBasicInfo.LastModifiedBy);

                        dbSmartAspects.AddInParameter(commandMember, "@OfficeAddress", DbType.String, memBasicInfo.OfficeAddress);
                        dbSmartAspects.AddInParameter(commandMember, "@Height", DbType.Int64, memBasicInfo.Height);
                        dbSmartAspects.AddInParameter(commandMember, "@Weight", DbType.Int64, memBasicInfo.Weight);
                        dbSmartAspects.AddInParameter(commandMember, "@ReligionId", DbType.Int32, memBasicInfo.ReligionId);
                        dbSmartAspects.AddInParameter(commandMember, "@NomineeRelationId", DbType.Int32, memBasicInfo.NomineeRelationId);
                        dbSmartAspects.AddInParameter(commandMember, "@NomineeDOB", DbType.DateTime, memBasicInfo.NomineeDOB);
                        dbSmartAspects.AddInParameter(commandMember, "@NomineeName", DbType.String, memBasicInfo.NomineeName);
                        dbSmartAspects.AddInParameter(commandMember, "@NomineeFather", DbType.String, memBasicInfo.NomineeFather);
                        dbSmartAspects.AddInParameter(commandMember, "@NomineeMother", DbType.String, memBasicInfo.NomineeMother);

                        dbSmartAspects.AddInParameter(commandMember, "@MemberPassword", DbType.String, memBasicInfo.MemberPassword);

                        dbSmartAspects.AddInParameter(commandMember, "@IsNewChartOfAccountsHeadCreateForMember", DbType.Int32, isNewChartOfAccountsHeadCreateForMember);

                        update = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);
                        tmpMemberId = memBasicInfo.MemberId;

                        if (update > 0)
                        {
                            int countReference = 0;
                            int countFamilyMember = 0;
                            int countEducation = 0;

                            // Reference........................
                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("SaveMemMemberReferenceInfo_SP"))
                                {
                                    foreach (MemMemberReferenceBO bo in referenceList)
                                    {
                                        if (bo.MemberId == 0)
                                        {
                                            commandReference.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandReference, "@MemberId", DbType.Int32, tmpMemberId);
                                            dbSmartAspects.AddInParameter(commandReference, "@Arbitrator", DbType.String, bo.Arbitrator);
                                            dbSmartAspects.AddInParameter(commandReference, "@ArbitratorMode", DbType.String, bo.ArbitratorMode);
                                            dbSmartAspects.AddInParameter(commandReference, "@Relationship", DbType.String, bo.Relationship);

                                            countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transaction);
                                        }
                                    }
                                }
                            }
                            if (referenceList != null)
                            {
                                using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("UpdateMemMemberReferenceInfo_SP"))
                                {
                                    foreach (MemMemberReferenceBO bo in referenceList)
                                    {
                                        if (bo.MemberId != 0)
                                        {
                                            commandReference.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandReference, "@ReferenceId", DbType.Int32, bo.ReferenceId);
                                            dbSmartAspects.AddInParameter(commandReference, "@MemberId", DbType.Int32, tmpMemberId);
                                            dbSmartAspects.AddInParameter(commandReference, "@Arbitrator", DbType.String, bo.Arbitrator);
                                            dbSmartAspects.AddInParameter(commandReference, "@ArbitratorMode", DbType.String, bo.ArbitratorMode);
                                            dbSmartAspects.AddInParameter(commandReference, "@Relationship", DbType.String, bo.Relationship);

                                            countReference += dbSmartAspects.ExecuteNonQuery(commandReference, transaction);
                                        }
                                    }
                                }
                            }
                            if (arrayReferenceDelete.Count > 0)
                            {
                                foreach (int delId in arrayReferenceDelete)
                                {
                                    using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandReference, "@TableName", DbType.String, "MemMemberReference");
                                        dbSmartAspects.AddInParameter(commandReference, "@TablePKField", DbType.String, "ReferenceId");
                                        dbSmartAspects.AddInParameter(commandReference, "@TablePKId", DbType.String, delId);

                                        countReference = dbSmartAspects.ExecuteNonQuery(commandReference);
                                    }
                                }
                            }

                            // Family Member.........................
                            if (familyMemberList != null)
                            {
                                using (DbCommand commandFamilyMember = dbSmartAspects.GetStoredProcCommand("SaveMemFamilyMemberInfo_SP"))
                                {
                                    foreach (MemMemberFamilyMemberBO bo in familyMemberList)
                                    {
                                        if (bo.MemberId == 0)
                                        {
                                            commandFamilyMember.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberId", DbType.Int32, tmpMemberId);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberName", DbType.String, bo.MemberName);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberDOB", DbType.DateTime, bo.MemberDOB);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@Occupation", DbType.String, bo.Occupation);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@UsageMode", DbType.String, bo.UsageMode);

                                            countFamilyMember += dbSmartAspects.ExecuteNonQuery(commandFamilyMember, transaction);
                                        }
                                    }
                                }
                            }
                            if (familyMemberList != null)
                            {
                                using (DbCommand commandFamilyMember = dbSmartAspects.GetStoredProcCommand("UpdateMemFamilyMemberInfo_SP"))
                                {
                                    foreach (MemMemberFamilyMemberBO bo in familyMemberList)
                                    {
                                        if (bo.MemberId != 0)
                                        {
                                            commandFamilyMember.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@FamilyMemberId", DbType.Int32, bo.Id);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberId", DbType.Int32, tmpMemberId);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberName", DbType.String, bo.MemberName);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@MemberDOB", DbType.DateTime, bo.MemberDOB);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@Occupation", DbType.String, bo.Occupation);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@Relationship", DbType.String, bo.Relationship);
                                            dbSmartAspects.AddInParameter(commandFamilyMember, "@UsageMode", DbType.String, bo.UsageMode);

                                            countFamilyMember += dbSmartAspects.ExecuteNonQuery(commandFamilyMember, transaction);
                                        }
                                    }
                                }
                            }
                            if (arrayFamilyMemberDelete.Count > 0)
                            {
                                foreach (int delId in arrayFamilyMemberDelete)
                                {
                                    using (DbCommand commandFamilyMember = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@TableName", DbType.String, "MemMemberFamilyMember");
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@TablePKField", DbType.String, "Id");
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@TablePKId", DbType.String, delId);

                                        countFamilyMember = dbSmartAspects.ExecuteNonQuery(commandFamilyMember);
                                    }
                                }
                            }

                            // Education
                            if (educationList.Count > 0)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("SaveOnlineMemEducation_SP"))
                                {
                                    foreach (var item in educationList)
                                    {
                                        if (item.MemberId == 0)
                                        {
                                            commandEducation.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandEducation, "@MemberId", DbType.Int32, tmpMemberId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@Degree", DbType.String, item.Degree);
                                            dbSmartAspects.AddInParameter(commandEducation, "@Institution", DbType.String, item.Institution);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassingYear", DbType.Int32, item.PassingYear);
                                            dbSmartAspects.AddInParameter(commandEducation, "@CreatedBy", DbType.Int32, item.CreatedBy);

                                            countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transaction);
                                        }

                                    }
                                }
                            }
                            if (educationList.Count > 0)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("UpdateMemOnlineMemEducation_SP"))
                                {
                                    foreach (var item in educationList)
                                    {
                                        if (item.MemberId != 0)
                                        {
                                            commandEducation.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandEducation, "@MemberId", DbType.Int32, tmpMemberId);
                                            dbSmartAspects.AddInParameter(commandEducation, "@Degree", DbType.String, item.Degree);
                                            dbSmartAspects.AddInParameter(commandEducation, "@Institution", DbType.String, item.Institution);
                                            dbSmartAspects.AddInParameter(commandEducation, "@PassingYear", DbType.Int32, item.PassingYear);
                                            dbSmartAspects.AddInParameter(commandEducation, "@Id", DbType.Int32, item.Id);

                                            countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transaction);
                                        }

                                    }

                                }
                            }
                            if (arrayEducationDelete.Count > 0)
                            {
                                foreach (int delId in arrayEducationDelete)
                                {
                                    using (DbCommand commandFamilyMember = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    {
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@TableName", DbType.String, "MemOnlineMemEducation");
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@TablePKField", DbType.String, "Id");
                                        dbSmartAspects.AddInParameter(commandFamilyMember, "@TablePKId", DbType.String, delId);

                                        countFamilyMember += dbSmartAspects.ExecuteNonQuery(commandFamilyMember);
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                        status = true;
                    }
                }
            }

            return status;
        }
        public List<MemMemberBasicsBO> GetMemberInfoBySearchCriteriaForPaging(string memName, string code, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();
            DataSet memberDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberInfoBySearchCriteriaForPaging_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(memName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemName", DbType.String, memName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemCode", DbType.String, code);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemCode", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, memberDS, "MemberInfo");
                    DataTable table = memberDS.Tables["MemberInfo"];

                    memberList = table.AsEnumerable().Select(r =>
                                   new MemMemberBasicsBO
                                   {
                                       MemberId = r.Field<int>("MemberId"),
                                       MembershipNumber = r.Field<string>("MembershipNumber"),
                                       FullName = r.Field<string>("FullName"),
                                       FatherName = r.Field<string>("FatherName"),
                                       MotherName = r.Field<string>("MotherName")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return memberList;
        }

        //Online Member
        public bool SaveOnlineMember(OnlineMemberBO onlineMemberObj, List<OnlineMemberFamilyBO> memberFamilyList, List<OnlineMemberEducationBO> educationList, out int tmpMemberId)
        {
            bool status = false;
            int membersave = 0;
            int countEducation = 0;
            int countFamilyMember = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("SaveOnlineMemberInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@MemberType", DbType.Int32, onlineMemberObj.TypeId);
                            dbSmartAspects.AddInParameter(commandMember, "@FullName", DbType.String, onlineMemberObj.FullName);
                            dbSmartAspects.AddInParameter(commandMember, "@NameBangla", DbType.String, onlineMemberObj.NameBangla);
                            dbSmartAspects.AddInParameter(commandMember, "@FatherName", DbType.String, onlineMemberObj.FatherName);
                            dbSmartAspects.AddInParameter(commandMember, "@MotherName", DbType.String, onlineMemberObj.MotherName);
                            dbSmartAspects.AddInParameter(commandMember, "@BirthDate", DbType.DateTime, onlineMemberObj.BirthDate);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberGender", DbType.Int32, onlineMemberObj.MemberGender);
                            dbSmartAspects.AddInParameter(commandMember, "@BloodGroup", DbType.Int32, onlineMemberObj.BloodGroup);
                            dbSmartAspects.AddInParameter(commandMember, "@MaritalStatus", DbType.Int32, onlineMemberObj.MaritalStatus);
                            dbSmartAspects.AddInParameter(commandMember, "@CountryId", DbType.Int32, onlineMemberObj.CountryId);
                            dbSmartAspects.AddInParameter(commandMember, "@NationalitySt", DbType.String, onlineMemberObj.NationalitySt);
                            dbSmartAspects.AddInParameter(commandMember, "@PassportNumber", DbType.String, onlineMemberObj.PassportNumber);
                            dbSmartAspects.AddInParameter(commandMember, "@ReligionId", DbType.Int32, onlineMemberObj.ReligionId);
                            dbSmartAspects.AddInParameter(commandMember, "@ProfessionId", DbType.Int32, onlineMemberObj.ProfessionId);
                            dbSmartAspects.AddInParameter(commandMember, "@ResidencePhone", DbType.String, onlineMemberObj.ResidencePhone);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberAddress", DbType.String, onlineMemberObj.MemberAddress);//permanent
                            dbSmartAspects.AddInParameter(commandMember, "@MailAddress", DbType.String, onlineMemberObj.MailAddress);//present
                            dbSmartAspects.AddInParameter(commandMember, "@Hobbies", DbType.String, onlineMemberObj.Hobbies);
                            dbSmartAspects.AddInParameter(commandMember, "@Awards", DbType.String, onlineMemberObj.Awards);
                            dbSmartAspects.AddInParameter(commandMember, "@MobileNumber", DbType.String, onlineMemberObj.MobileNumber);
                            dbSmartAspects.AddInParameter(commandMember, "@PersonalEmail", DbType.String, onlineMemberObj.PersonalEmail);

                            dbSmartAspects.AddInParameter(commandMember, "@Occupation", DbType.String, onlineMemberObj.Occupation);
                            dbSmartAspects.AddInParameter(commandMember, "@Organization", DbType.String, onlineMemberObj.Organization);
                            dbSmartAspects.AddInParameter(commandMember, "@Designation", DbType.String, onlineMemberObj.Designation);
                            dbSmartAspects.AddInParameter(commandMember, "@OfficeAddress", DbType.String, onlineMemberObj.OfficeAddress);
                            dbSmartAspects.AddInParameter(commandMember, "@CreatedBy", DbType.Int32, onlineMemberObj.CreatedBy);
                            dbSmartAspects.AddInParameter(commandMember, "@Introducer_1_id", DbType.Int32, onlineMemberObj.Introducer_1_id);
                            dbSmartAspects.AddInParameter(commandMember, "@Introducer_2_id", DbType.Int32, onlineMemberObj.Introducer_2_id);

                            dbSmartAspects.AddInParameter(commandMember, "@BirthPlace", DbType.String, onlineMemberObj.BirthPlace);
                            dbSmartAspects.AddInParameter(commandMember, "@Height", DbType.Int32, onlineMemberObj.Height);
                            dbSmartAspects.AddInParameter(commandMember, "@Weight", DbType.Int32, onlineMemberObj.Weight);
                            dbSmartAspects.AddInParameter(commandMember, "@NationalID", DbType.String, onlineMemberObj.NationalID);

                            dbSmartAspects.AddInParameter(commandMember, "@NomineeName", DbType.String, onlineMemberObj.NomineeName);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeFather", DbType.String, onlineMemberObj.NomineeFather);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeMother", DbType.String, onlineMemberObj.NomineeMother);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeDOB", DbType.DateTime, onlineMemberObj.NomineeDOB);
                            dbSmartAspects.AddInParameter(commandMember, "@NomineeRelationId", DbType.Int32, onlineMemberObj.NomineeRelationId);
                            dbSmartAspects.AddInParameter(commandMember, "@PathPersonalImg", DbType.String, onlineMemberObj.PathPersonalImg);
                            dbSmartAspects.AddInParameter(commandMember, "@PathNIdImage", DbType.String, onlineMemberObj.PathNIdImage);

                            dbSmartAspects.AddOutParameter(commandMember, "@MemberId", DbType.Int32, sizeof(Int32));
                            membersave = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);
                            tmpMemberId = Convert.ToInt32(commandMember.Parameters["@MemberId"].Value);
                        }

                        if (membersave > 0)
                        {
                            if (educationList.Count > 0)
                            {
                                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("SaveOnlineMemEducation_SP"))
                                {
                                    foreach (var item in educationList)
                                    {
                                        commandEducation.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandEducation, "@MemberId", DbType.Int32, tmpMemberId);
                                        dbSmartAspects.AddInParameter(commandEducation, "@Degree", DbType.String, item.Degree);
                                        dbSmartAspects.AddInParameter(commandEducation, "@Institution", DbType.String, item.Institution);
                                        dbSmartAspects.AddInParameter(commandEducation, "@PassingYear", DbType.Int32, item.PassingYear);
                                        dbSmartAspects.AddInParameter(commandEducation, "@CreatedBy", DbType.Int32, item.CreatedBy);

                                        countEducation += dbSmartAspects.ExecuteNonQuery(commandEducation, transaction);
                                    }
                                }
                            }
                            if (memberFamilyList.Count > 0)
                            {
                                using (DbCommand commandFamMem = dbSmartAspects.GetStoredProcCommand("SaveOnlineMemFamily_SP"))
                                {
                                    foreach (var item in memberFamilyList)
                                    {
                                        commandFamMem.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandFamMem, "@MemberId", DbType.Int32, tmpMemberId);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@MemberName", DbType.String, item.MemberName);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@FamMemMarriageDate", DbType.DateTime, item.FamMemMarriageDate);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@Relationship", DbType.String, item.Relationship);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@Occupation", DbType.String, item.Occupation);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@FamMemBloodGroupId", DbType.Int32, item.FamMemBloodGroupId);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@CreatedBy", DbType.Int32, item.CreatedBy);
                                        dbSmartAspects.AddInParameter(commandFamMem, "@MemberDOB", DbType.DateTime, item.MemberDOB);

                                        countFamilyMember += dbSmartAspects.ExecuteNonQuery(commandFamMem, transaction);
                                    }
                                }
                            }
                            transaction.Commit();
                            status = true;
                        }
                        else
                        {
                            transaction.Rollback();
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
        public List<OnlineMemberBO> GetOnlineMemberInfoBySearchCriteriaForReport(int typeId, string memberNo, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<OnlineMemberBO> memberList = new List<OnlineMemberBO>();
            DataSet memberDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberTypeId", DbType.Int32, typeId);
                    if (!string.IsNullOrWhiteSpace(memberNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MembershipNumber", DbType.String, memberNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MembershipNumber", DbType.String, DBNull.Value);
                    }


                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, memberDS, "OnlineMemberInfo");
                    DataTable table = memberDS.Tables["OnlineMemberInfo"];

                    memberList = table.AsEnumerable().Select(r =>
                                   new OnlineMemberBO
                                   {
                                       MemberId = r.Field<int>("MemberId"),
                                       TypeName = r.Field<string>("MemberType"),
                                       FullName = r.Field<string>("MemberName"),
                                       MembershipNumber = r.Field<string>("MembershipNumber"),
                                       Country = r.Field<string>("CountryName"),
                                       MobileNumber = r.Field<string>("MobileNumber"),
                                       Occupation = r.Field<string>("Occupation"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       Introducer_1_Name = r.Field<string>("Introducer_1_Name"),
                                       Introducer_2_Name = r.Field<string>("Introducer_2_Name")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return memberList;
        }
        public List<OnlineMemberBO> GetOnlineMemberInfoBySearchCriteriaForPaging(int typeId, string name, string mobile, string email, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<OnlineMemberBO> memberList = new List<OnlineMemberBO>();
            DataSet memberDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberTypeId", DbType.Int32, typeId);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemberName", DbType.String, name);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemberName", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MobileNumber", DbType.String, mobile);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MobileNumber", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, email);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, memberDS, "OnlineMemberInfo");
                    DataTable table = memberDS.Tables["OnlineMemberInfo"];

                    memberList = table.AsEnumerable().Select(r =>
                                   new OnlineMemberBO
                                   {
                                       MemberId = r.Field<int>("MemberId"),
                                       TypeName = r.Field<string>("MemberType"),
                                       FullName = r.Field<string>("MemberName"),
                                       Country = r.Field<string>("CountryName"),
                                       MobileNumber = r.Field<string>("MobileNumber"),
                                       Occupation = r.Field<string>("Occupation"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       Introducer_1_Name = r.Field<string>("Introducer_1_Name"),
                                       Introducer_2_Name = r.Field<string>("Introducer_2_Name")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return memberList;
        }
        //add nominee here
        public OnlineMemberBO GetOnlineMemberInfoById(int memberId, int introId1, int introId2)
        {
            OnlineMemberBO bo = new OnlineMemberBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberBasicInfoById"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);
                    if (introId1 != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@IntroId1", DbType.Int32, introId1);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@IntroId1", DbType.Int32, DBNull.Value);
                    }
                    if (introId2 != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@IntroId2", DbType.Int32, introId2);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@IntroId2", DbType.Int32, DBNull.Value);
                    }
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    bo = Table.AsEnumerable().Select(r => new OnlineMemberBO
                    {
                        MemberId = r.Field<int>("MemberId"),
                        TypeId = r.Field<int>("TypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        FullName = r.Field<string>("FullName"),
                        NameBangla = r.Field<string>("NameBangla"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime>("BirthDate"),
                        MemberGenderSt = r.Field<string>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        MobileNumber = r.Field<string>("MobileNumber"),
                        MaritalSt = r.Field<string>("MaritalSt"),
                        BloodGroupName = r.Field<string>("BloodGroupName"),
                        Country = r.Field<string>("Country"),
                        NationalitySt = r.Field<string>("NationalitySt"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        Religion = r.Field<string>("Religion"),
                        Profession = r.Field<string>("Profession"),
                        Hobbies = r.Field<string>("Hobbies"),
                        Awards = r.Field<string>("Awards"),
                        Introducer_1_Name = r.Field<string>("Introducer_1_Name"),
                        Introducer_1_id = r.Field<Int32?>("Introducer_1_id"),
                        IntroducerMemNo1 = r.Field<string>("IntroducerMemNo1"),
                        IntroducerMemType1 = r.Field<string>("IntroducerMemType1"),
                        IntroducerEmail1 = r.Field<string>("IntroducerEmail1"),
                        IntroducerMobileNo1 = r.Field<string>("IntroducerMobileNo1"),

                        Introducer_2_id = r.Field<Int32?>("Introducer_2_id"),
                        Introducer_2_Name = r.Field<string>("Introducer_2_Name"),
                        IntroducerMemNo2 = r.Field<string>("IntroducerMemNo2"),
                        IntroducerMemType2 = r.Field<string>("IntroducerMemType2"),
                        IntroducerEmail2 = r.Field<string>("IntroducerEmail2"),
                        IntroducerMobileNo2 = r.Field<string>("IntroducerMobileNo2"),
                        PassportNumber = r.Field<string>("PassportNumber"),

                        Remarks = r.Field<string>("Remarks"),
                        IsAccepted = r.Field<bool>("IsAccepted"),
                        IsRejected = r.Field<bool>("IsRejected"),
                        IsDeferred = r.Field<bool>("IsDeferred"),
                        IsAccepted1 = r.Field<bool>("IsAccepted1"),
                        IsRejected1 = r.Field<bool>("IsRejected1"),
                        IsDeferred1 = r.Field<bool>("IsDeferred1"),
                        IsAccepted2 = r.Field<bool>("IsAccepted2"),
                        IsRejected2 = r.Field<bool>("IsRejected2"),
                        IsDeferred2 = r.Field<bool>("IsDeferred2"),
                        BirthPlace = r.Field<string>("BirthPlace"),
                        Height = r.Field<double?>("Height"),
                        Weight = r.Field<double?>("Weight"),
                        NationalID = r.Field<string>("NationalID"),
                        NomineeName = r.Field<string>("NomineeName"),
                        NomineeFather = r.Field<string>("NomineeFather"),
                        NomineeMother = r.Field<string>("NomineeMother"),
                        NomineeDOB = r.Field<DateTime?>("NomineeDOB"),
                        NomineeRelation = r.Field<string>("NomineeRelation"),
                        Remarks1 = r.Field<string>("Remarks1"),
                        Remarks2 = r.Field<string>("Remarks2"),
                        PathPersonalImg = r.Field<string>("PathPersonalImg"),
                        PathNIdImage = r.Field<string>("PathNIdImage"),

                        MeetingDate = r.Field<DateTime?>("MeetingDate"),
                        MeetingDateEC = r.Field<DateTime?>("MeetingDateEC"),
                        MeetingDecision = r.Field<string>("MeetingDecision"),
                        MeetingDecisionEC = r.Field<string>("MeetingDecisionEC")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public OnlineMemberBO GetOnlineMemberInfoByMemNumber(string memNumber)
        {
            OnlineMemberBO bo = new OnlineMemberBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberInfoByMemNumber"))
                {
                    if (memNumber != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MembershipNumber ", DbType.String, memNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MembershipNumber", DbType.String, DBNull.Value);
                    }
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    bo = Table.AsEnumerable().Select(r => new OnlineMemberBO
                    {
                        MemberId = r.Field<Int32>("MemberId"),
                        FullName = r.Field<string>("FullName")
                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public List<OnlineMemberFamilyBO> GetOnlineMemFamilyMemberByMemberId(int memId)
        {
            List<OnlineMemberFamilyBO> boList = new List<OnlineMemberFamilyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberFamilyInfoById"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "FamilyMemberInfo");
                    DataTable Table = incrementDS.Tables["FamilyMemberInfo"];

                    boList = Table.AsEnumerable().Select(r => new OnlineMemberFamilyBO
                    {
                        MemberId = r.Field<int>("MemberId"),
                        MemberName = r.Field<string>("MemberName"),
                        MemberDOB = r.Field<DateTime?>("MemberDOB"),
                        Occupation = r.Field<string>("Occupation"),
                        Relationship = r.Field<string>("Relationship"),
                        FamMemBloodGroup = r.Field<string>("BloodGroup"),
                        FamMemMarriageDate = r.Field<DateTime?>("FamMemMarriageDate")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<OnlineMemberEducationBO> GetOnlineMemberEducationsById(int memId)
        {
            List<OnlineMemberEducationBO> educationBOs = new List<OnlineMemberEducationBO>();
            using (DbConnection con = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberEducationInfoById"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memId);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EducationInfo");
                    DataTable table = ds.Tables["EducationInfo"];

                    educationBOs = table.AsEnumerable().Select(r => new OnlineMemberEducationBO
                    {
                        Id = r.Field<Int32>("Id"),
                        MemberId = r.Field<Int32>("MemberId"),
                        Degree = r.Field<string>("Degree"),
                        Institution = r.Field<string>("Institution"),
                        PassingYear = r.Field<Int32?>("PassingYear")
                    }).ToList();
                }
            }

            return educationBOs;
        }
        public bool UpdateAndAcceptMemByIntroducer(OnlineMemberBO onlineMember, int memberId, int introId)
        {
            bool status = false;
            int memberUpdate = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("UpdateAndAcceptMemByIntroducer_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@IsAccepted1", DbType.Boolean, onlineMember.IsAccepted1);
                            dbSmartAspects.AddInParameter(commandMember, "@IsRejected1", DbType.Boolean, onlineMember.IsRejected1);
                            dbSmartAspects.AddInParameter(commandMember, "@IsDeferred1", DbType.Boolean, onlineMember.IsDeferred1);
                            dbSmartAspects.AddInParameter(commandMember, "@IsAccepted2", DbType.Boolean, onlineMember.IsAccepted2);
                            dbSmartAspects.AddInParameter(commandMember, "@IsRejected2", DbType.Boolean, onlineMember.IsRejected2);
                            dbSmartAspects.AddInParameter(commandMember, "@IsDeferred2", DbType.Boolean, onlineMember.IsDeferred2);

                            dbSmartAspects.AddInParameter(commandMember, "@MemberId", DbType.Int32, memberId);
                            //dbSmartAspects.AddInParameter(commandMember, "@IntroducerId", DbType.Int32, introId);
                            dbSmartAspects.AddInParameter(commandMember, "@Remarks1", DbType.String, onlineMember.Remarks1);
                            dbSmartAspects.AddInParameter(commandMember, "@Remarks2", DbType.String, onlineMember.Remarks2);

                            memberUpdate = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);
                        }
                        if (memberUpdate > 0)
                        {
                            transaction.Commit();
                            status = true;
                        }
                        else
                        {
                            transaction.Rollback();
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
        public bool UpdateAndAcceptMember(OnlineMemberBO onlineMember)
        {
            bool status = false;
            int memberUpdate = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("UpdateAndAcceptMemberById_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@IsAccepted", DbType.Boolean, onlineMember.IsAccepted);
                            dbSmartAspects.AddInParameter(commandMember, "@IsRejected", DbType.Boolean, onlineMember.IsRejected);
                            dbSmartAspects.AddInParameter(commandMember, "@IsDeferred", DbType.Boolean, onlineMember.IsDeferred);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberId", DbType.Int32, onlineMember.MemberId);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberTypeId", DbType.Int32, onlineMember.TypeId);
                            dbSmartAspects.AddInParameter(commandMember, "@Remarks", DbType.String, onlineMember.Remarks);

                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDate", DbType.DateTime, onlineMember.MeetingDate);
                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDateEC", DbType.DateTime, onlineMember.MeetingDateEC);
                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDecision", DbType.String, onlineMember.MeetingDecision);
                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDecisionEC", DbType.String, onlineMember.MeetingDecisionEC);

                            memberUpdate = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);
                        }
                        if (memberUpdate > 0)
                        {
                            transaction.Commit();
                            status = true;
                        }
                        else
                        {
                            transaction.Rollback();
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
        public bool UpdateAndRejectMemByIntroducer(OnlineMemberBO onlineMember, int memberId)
        {
            bool status = false;
            int memberUpdate = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("UpdateAndRejectMemByIntroducer_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@IsAccepted1", DbType.Boolean, onlineMember.IsAccepted1);
                            dbSmartAspects.AddInParameter(commandMember, "@IsRejected1", DbType.Boolean, onlineMember.IsRejected1);
                            dbSmartAspects.AddInParameter(commandMember, "@IsDeferred1", DbType.Boolean, onlineMember.IsDeferred1);
                            dbSmartAspects.AddInParameter(commandMember, "@IsAccepted2", DbType.Boolean, onlineMember.IsAccepted2);
                            dbSmartAspects.AddInParameter(commandMember, "@IsRejected2", DbType.Boolean, onlineMember.IsRejected2);
                            dbSmartAspects.AddInParameter(commandMember, "@IsDeferred2", DbType.Boolean, onlineMember.IsDeferred2);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberId", DbType.Int32, memberId);

                            dbSmartAspects.AddInParameter(commandMember, "@Remarks1", DbType.String, onlineMember.Remarks1);
                            dbSmartAspects.AddInParameter(commandMember, "@Remarks2", DbType.String, onlineMember.Remarks2);

                            memberUpdate = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);
                        }
                        if (memberUpdate > 0)
                        {
                            transaction.Commit();
                            status = true;
                        }
                        else
                        {
                            transaction.Rollback();
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
        public bool UpdateAndRejectMember(OnlineMemberBO onlineMember)
        {
            bool status = false;
            int memberUpdate = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        using (DbCommand commandMember = dbSmartAspects.GetStoredProcCommand("UpdateAndRejectMemberById_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMember, "@IsAccepted", DbType.Boolean, onlineMember.IsAccepted);
                            dbSmartAspects.AddInParameter(commandMember, "@IsRejected", DbType.Boolean, onlineMember.IsRejected);
                            dbSmartAspects.AddInParameter(commandMember, "@IsDeferred", DbType.Boolean, onlineMember.IsDeferred);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberId", DbType.Int32, onlineMember.MemberId);
                            dbSmartAspects.AddInParameter(commandMember, "@MemberTypeId", DbType.Int32, onlineMember.TypeId);
                            dbSmartAspects.AddInParameter(commandMember, "@Remarks", DbType.String, onlineMember.Remarks);

                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDate", DbType.DateTime, onlineMember.MeetingDate);
                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDateEC", DbType.DateTime, onlineMember.MeetingDateEC);
                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDecision", DbType.String, onlineMember.MeetingDecision);
                            dbSmartAspects.AddInParameter(commandMember, "@MeetingDecisionEC", DbType.String, onlineMember.MeetingDecisionEC);

                            memberUpdate = dbSmartAspects.ExecuteNonQuery(commandMember, transaction);
                        }
                        if (memberUpdate > 0)
                        {
                            transaction.Commit();
                            status = true;
                        }
                        else
                        {
                            transaction.Rollback();
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

        public List<OnlineMemberBO> GetOnlineMemberInfoByIntroducerId(int introId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<OnlineMemberBO> introducers = new List<OnlineMemberBO>();
            DataSet memberDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOnlineMemberInfoByIntroducerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IntroducerId", DbType.Int32, introId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    dbSmartAspects.LoadDataSet(cmd, memberDS, "OnlineMemberInfo");
                    DataTable table = memberDS.Tables["OnlineMemberInfo"];
                    introducers = table.AsEnumerable().Select(r =>
                                   new OnlineMemberBO
                                   {
                                       MemberId = r.Field<int>("MemberId"),
                                       MembershipNumber = r.Field<string>("MembershipNumber"),
                                       TypeName = r.Field<string>("MemberType"),
                                       FullName = r.Field<string>("MemberName"),
                                       Country = r.Field<string>("CountryName"),
                                       MobileNumber = r.Field<string>("MobileNumber"),
                                       Occupation = r.Field<string>("Occupation"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       Introducer_1_id = r.Field<Int32>("Introducer_1_id"),
                                       Introducer_2_id = r.Field<Int32>("Introducer_2_id")
                                   }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;

                }
            }
            return introducers;

        }
        // end online member
        public MemMemberBasicsBO GetMemberInfoById(int memberId)
        {
            MemMemberBasicsBO bo = new MemMemberBasicsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberInfoById_SP"))
                {
                    if (memberId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                //bo.MemberId = int.Parse(reader["MemberId"].ToString());
                                //bo.CompanyId = int.Parse(reader["CompanyId"].ToString());

                                bo.MemberId = int.Parse(reader["MemberId"].ToString());
                                bo.CompanyId = int.Parse(reader["CompanyId"].ToString());
                                bo.NodeId = int.Parse(reader["NodeId"].ToString());
                                bo.TypeId = int.Parse(reader["TypeId"].ToString());
                                bo.TypeName = reader["TypeName"].ToString();
                                bo.MembershipNumber = reader["MembershipNumber"].ToString();
                                bo.NameTitle = reader["NameTitle"].ToString();
                                bo.FullName = reader["FullName"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.MiddleName = reader["MiddleName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FatherName = reader["FatherName"].ToString();
                                bo.MotherName = reader["MotherName"].ToString();
                                bo.BirthDate = DateTime.Parse(reader["BirthDate"].ToString());
                                bo.MemberGender = int.Parse(reader["MemberGender"].ToString());
                                bo.MemberAddress = reader["MemberAddress"].ToString();
                                bo.MailAddress = reader["MailAddress"].ToString();
                                bo.PersonalEmail = reader["PersonalEmail"].ToString();
                                bo.OfficeEmail = reader["OfficeEmail"].ToString();
                                bo.HomeFax = reader["HomeFax"].ToString();
                                bo.OfficeFax = reader["OfficeFax"].ToString();
                                bo.MaritalStatus = int.Parse(reader["MaritalStatus"].ToString());
                                bo.BloodGroup = int.Parse(reader["BloodGroup"].ToString());
                                bo.Nationality = int.Parse(reader["Nationality"].ToString());
                                if (reader["RegistrationDate"] != DBNull.Value)
                                {
                                    bo.RegistrationDate = DateTime.Parse(reader["RegistrationDate"].ToString());
                                }
                                if (reader["ExpiryDate"] != DBNull.Value)
                                {
                                    bo.ExpiryDate = DateTime.Parse(reader["ExpiryDate"].ToString());
                                }
                                bo.PassportNumber = reader["PassportNumber"].ToString();
                                bo.Organization = reader["Organization"].ToString();
                                bo.Occupation = reader["Occupation"].ToString();
                                bo.Designation = reader["Designation"].ToString();
                                if (reader["AnnualTurnover"] != DBNull.Value)
                                {
                                    bo.AnnualTurnover = decimal.Parse(reader["AnnualTurnover"].ToString());
                                }
                                if (reader["MonthlyIncome"] != DBNull.Value)
                                {
                                    bo.MonthlyIncome = decimal.Parse(reader["MonthlyIncome"].ToString());
                                }
                                if (reader["SecurityDeposit"] != DBNull.Value)
                                {
                                    bo.SecurityDeposit = decimal.Parse(reader["SecurityDeposit"].ToString());
                                }

                                bo.MemberGenderSt = reader["MemberGenderSt"].ToString();
                                bo.MaritalSt = reader["MaritalStatusSt"].ToString();
                                bo.BloodGroupName = reader["BloodGroup"].ToString();
                                bo.NationalitySt = reader["NationalitySt"].ToString();

                                bo.OfficeAddress = reader["OfficeAddress"].ToString();
                                bo.NomineeName = reader["NomineeName"].ToString();
                                bo.NomineeFather = reader["NomineeFather"].ToString();
                                bo.NomineeMother = reader["NomineeMother"].ToString();
                                if (reader["NomineeDOB"] != DBNull.Value)
                                {
                                    bo.NomineeDOB = DateTime.Parse(reader["NomineeDOB"].ToString());
                                }
                                bo.NomineeRelationId = int.Parse(reader["NomineeRelationId"].ToString());
                                //NomineeRelation = r.Field<string>("NomineeRelation"),
                                if (reader["Height"] != DBNull.Value)
                                {
                                    bo.Height = double.Parse(reader["Height"].ToString());
                                }
                                if (reader["Weight"] != DBNull.Value)
                                {
                                    bo.Weight = double.Parse(reader["Weight"].ToString());
                                }
                                bo.ReligionId = int.Parse(reader["ReligionId"].ToString());
                                bo.MobileNumber = reader["MobileNumber"].ToString();
                            }
                        }
                    }

                    /*DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    

                    bo = Table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        MemberId = r.Field<int>("MemberId"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        NodeId = r.Field<int?>("NodeId"),
                        TypeId = r.Field<int?>("TypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        NameTitle = r.Field<string>("NameTitle"),
                        FullName = r.Field<string>("FullName"),
                        FirstName = r.Field<string>("FirstName"),
                        MiddleName = r.Field<string>("MiddleName"),
                        LastName = r.Field<string>("LastName"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime?>("BirthDate"),
                        MemberGender = r.Field<int?>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        OfficeEmail = r.Field<string>("OfficeEmail"),
                        HomeFax = r.Field<string>("HomeFax"),
                        OfficeFax = r.Field<string>("OfficeFax"),
                        MaritalStatus = r.Field<int>("MaritalStatus"),
                        BloodGroup = r.Field<int?>("BloodGroup"),
                        Nationality = r.Field<int?>("Nationality"),
                        RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                        ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                        MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                        SecurityDeposit = r.Field<decimal?>("SecurityDeposit"),

                        MemberGenderSt = r.Field<string>("MemberGenderSt"),
                        MaritalSt = r.Field<string>("MaritalStatusSt"),
                        BloodGroupName = r.Field<string>("BloodGroup"),
                        NationalitySt = r.Field<string>("NationalitySt"),

                        OfficeAddress = r.Field<string>("OfficeAddress"),
                        NomineeName = r.Field<string>("NomineeName"),
                        NomineeFather = r.Field<string>("NomineeFather"),
                        NomineeMother = r.Field<string>("NomineeMother"),
                        NomineeDOB = r.Field<DateTime?>("NomineeDOB"),
                        NomineeRelationId = r.Field<Int32>("NomineeRelationId"),
                        //NomineeRelation = r.Field<string>("NomineeRelation"),

                        Height = r.Field<double?>("Height"),
                        Weight = r.Field<double?>("Weight"),
                        ReligionId = r.Field<Int32>("ReligionId"),
                        MobileNumber = r.Field<string>("MobileNumber")
                    }).FirstOrDefault();*/
                }
            }
            return bo;
        }
        public List<MemMemberBasicsBO> GetMemberInfoByIdForReport(int memberId)
        {
            List<MemMemberBasicsBO> basicsBOs = new List<MemMemberBasicsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberInfoByIdForReport_SP"))
                {
                    if (memberId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    basicsBOs = Table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        MemberId = r.Field<int>("MemberId"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        NodeId = r.Field<int?>("NodeId"),
                        TypeId = r.Field<int?>("TypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        NameTitle = r.Field<string>("NameTitle"),
                        FullName = r.Field<string>("FullName"),
                        FirstName = r.Field<string>("FirstName"),
                        MiddleName = r.Field<string>("MiddleName"),
                        LastName = r.Field<string>("LastName"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime?>("BirthDate"),
                        MemberGender = r.Field<int?>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        OfficeEmail = r.Field<string>("OfficeEmail"),
                        HomeFax = r.Field<string>("HomeFax"),
                        OfficeFax = r.Field<string>("OfficeFax"),
                        MaritalStatus = r.Field<int?>("MaritalStatus"),
                        BloodGroup = r.Field<int?>("BloodGroup"),
                        Nationality = r.Field<int?>("Nationality"),
                        RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                        ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                        MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                        SecurityDeposit = r.Field<decimal?>("SecurityDeposit"),

                        MemberGenderSt = r.Field<string>("MemberGenderSt"),
                        MaritalSt = r.Field<string>("MaritalSt"),
                        BloodGroupName = r.Field<string>("BloodGroupName"),
                        NationalitySt = r.Field<string>("NationalitySt"),

                        OfficeAddress = r.Field<string>("OfficeAddress"),
                        NomineeName = r.Field<string>("NomineeName"),
                        NomineeFather = r.Field<string>("NomineeFather"),
                        NomineeMother = r.Field<string>("NomineeMother"),
                        NomineeDOB = r.Field<DateTime?>("NomineeDOB"),
                        NomineeRelationId = r.Field<Int32?>("NomineeRelationId"),
                        NomineeRelation = r.Field<string>("NomineeRelation"),

                        Height = r.Field<double?>("Height"),
                        Weight = r.Field<double?>("Weight"),
                        //ReligionId = r.Field<Int32>("ReligionId"),
                        MobileNumber = r.Field<string>("MobileNumber")
                    }).ToList();
                }
            }
            return basicsBOs;
        }
        public MemMemberBasicsBO GetMemberInfoByMembershipNo(string membershipNo)
        {
            MemMemberBasicsBO bo = new MemMemberBasicsBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberInfoByMembershipNo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MembershipNo", DbType.String, membershipNo);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    bo = Table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        MemberId = r.Field<int>("MemberId"),
                        CompanyId = r.Field<int>("CompanyId"),
                        NodeId = r.Field<int>("NodeId"),
                        TypeId = r.Field<int>("TypeId"),
                        TypeName = r.Field<string>("TypeName"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        NameTitle = r.Field<string>("NameTitle"),
                        FullName = r.Field<string>("FullName"),
                        FirstName = r.Field<string>("FirstName"),
                        MiddleName = r.Field<string>("MiddleName"),
                        LastName = r.Field<string>("LastName"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime>("BirthDate"),
                        MemberGender = r.Field<int>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        OfficeEmail = r.Field<string>("OfficeEmail"),
                        HomeFax = r.Field<string>("HomeFax"),
                        OfficeFax = r.Field<string>("OfficeFax"),
                        MaritalStatus = r.Field<int>("MaritalStatus"),
                        BloodGroup = r.Field<int?>("BloodGroup"),
                        Nationality = r.Field<int?>("Nationality"),
                        RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                        ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                        MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                        SecurityDeposit = r.Field<decimal>("SecurityDeposit"),
                        MobileNumber = r.Field<string>("MobileNumber"),
                        DiscountPercent = r.Field<decimal>("DiscountPercent")

                    }).FirstOrDefault();
                }
            }
            return bo;
        }
        public List<MemMemberBasicsBO> GetMemMemberList()
        {
            List<MemMemberBasicsBO> boList = new List<MemMemberBasicsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemMemberList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    boList = Table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        TypeId = r.Field<int?>("TypeId"),
                        MemberId = r.Field<int>("MemberId"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        NameTitle = r.Field<string>("NameTitle"),
                        FullName = r.Field<string>("FullName"),
                        FirstName = r.Field<string>("FirstName"),
                        MiddleName = r.Field<string>("MiddleName"),
                        LastName = r.Field<string>("LastName"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime>("BirthDate"),
                        MemberGender = r.Field<int?>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        OfficeEmail = r.Field<string>("OfficeEmail"),
                        HomeFax = r.Field<string>("HomeFax"),
                        OfficeFax = r.Field<string>("OfficeFax"),
                        MaritalStatus = r.Field<int?>("MaritalStatus"),
                        BloodGroup = r.Field<int?>("BloodGroup"),
                        Nationality = r.Field<int?>("Nationality"),
                        RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                        ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                        MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                        SecurityDeposit = r.Field<decimal?>("SecurityDeposit")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<MemMemberBasicsBO> GetMemActiveMemberListInfo()
        {
            List<MemMemberBasicsBO> boList = new List<MemMemberBasicsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemActiveMemberListInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    boList = Table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        TypeId = r.Field<int>("TypeId"),
                        MemberId = r.Field<int>("MemberId"),
                        NodeId = r.Field<int>("NodeId"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        NameTitle = r.Field<string>("NameTitle"),
                        FullName = r.Field<string>("FullName"),
                        FirstName = r.Field<string>("FirstName"),
                        MiddleName = r.Field<string>("MiddleName"),
                        LastName = r.Field<string>("LastName"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime>("BirthDate"),
                        MemberGender = r.Field<int>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        OfficeEmail = r.Field<string>("OfficeEmail"),
                        HomeFax = r.Field<string>("HomeFax"),
                        OfficeFax = r.Field<string>("OfficeFax"),
                        MaritalStatus = r.Field<int>("MaritalStatus"),
                        BloodGroup = r.Field<int>("BloodGroup"),
                        Nationality = r.Field<int>("Nationality"),
                        RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                        ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                        MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                        SecurityDeposit = r.Field<decimal>("SecurityDeposit"),
                        DiscountPercent = r.Field<decimal>("DiscountPercent"),
                        MemberIdNDiscount = (r.Field<int>("NodeId").ToString() + "," + r.Field<decimal>("DiscountPercent").ToString())

                    }).ToList();
                }
            }
            return boList;
        }
        public List<MemMemberBasicsBO> GetMemActiveMemberListInfoForReport(int memberId)
        {
            List<MemMemberBasicsBO> boList = new List<MemMemberBasicsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemActiveMemberListInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberInfo");
                    DataTable Table = ds.Tables["MemberInfo"];

                    boList = Table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        TypeId = r.Field<int>("TypeId"),
                        MemberId = r.Field<int>("MemberId"),
                        NodeId = r.Field<int>("NodeId"),
                        MembershipNumber = r.Field<string>("MembershipNumber"),
                        NameTitle = r.Field<string>("NameTitle"),
                        FullName = r.Field<string>("FullName"),
                        FirstName = r.Field<string>("FirstName"),
                        MiddleName = r.Field<string>("MiddleName"),
                        LastName = r.Field<string>("LastName"),
                        FatherName = r.Field<string>("FatherName"),
                        MotherName = r.Field<string>("MotherName"),
                        BirthDate = r.Field<DateTime>("BirthDate"),
                        MemberGender = r.Field<int>("MemberGender"),
                        MemberAddress = r.Field<string>("MemberAddress"),
                        MailAddress = r.Field<string>("MailAddress"),
                        PersonalEmail = r.Field<string>("PersonalEmail"),
                        OfficeEmail = r.Field<string>("OfficeEmail"),
                        MobileNumber = r.Field<string>("MobileNumber"),
                        HomeFax = r.Field<string>("HomeFax"),
                        OfficeFax = r.Field<string>("OfficeFax"),
                        MaritalStatus = r.Field<int>("MaritalStatus"),
                        BloodGroup = r.Field<int>("BloodGroup"),
                        Nationality = r.Field<int>("Nationality"),
                        RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                        ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                        ExpiryDateInfo = r.Field<string>("ExpiryDateInfo"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        Organization = r.Field<string>("Organization"),
                        Occupation = r.Field<string>("Occupation"),
                        Designation = r.Field<string>("Designation"),
                        AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                        MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                        SecurityDeposit = r.Field<decimal>("SecurityDeposit")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<MemMemberReferenceBO> GetMemberReferenceByMemberId(int memId)
        {
            List<MemMemberReferenceBO> boList = new List<MemMemberReferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemReferenceByMemberId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "ReferenceInfo");
                    DataTable Table = incrementDS.Tables["ReferenceInfo"];

                    boList = Table.AsEnumerable().Select(r => new MemMemberReferenceBO
                    {
                        ReferenceId = r.Field<int>("ReferenceId"),
                        MemberId = r.Field<int>("MemberId"),
                        Arbitrator = r.Field<string>("Arbitrator"),
                        ArbitratorMode = r.Field<string>("ArbitratorMode"),
                        Relationship = r.Field<string>("Relationship")
                    }).ToList();
                }
            }
            return boList;
        }
        public List<MemMemberFamilyMemberBO> GetMemFamilyMemberByMemberId(int memId)
        {
            List<MemMemberFamilyMemberBO> boList = new List<MemMemberFamilyMemberBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemFamilyMemberByMemberId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "FamilyMemberInfo");
                    DataTable Table = incrementDS.Tables["FamilyMemberInfo"];

                    boList = Table.AsEnumerable().Select(r => new MemMemberFamilyMemberBO
                    {
                        Id = r.Field<int>("Id"),
                        MemberId = r.Field<int>("MemberId"),
                        MemberName = r.Field<string>("MemberName"),
                        MemberDOB = r.Field<DateTime?>("MemberDOB"),
                        Occupation = r.Field<string>("Occupation"),
                        Relationship = r.Field<string>("Relationship"),
                        UsageMode = r.Field<string>("UsageMode")
                    }).ToList();
                }
            }
            return boList;
        }

        public List<MemMemberTypeBO> GetMemMemberTypeList()
        {
            List<MemMemberTypeBO> boList = new List<MemMemberTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemMemberTypeList_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "MemberTypeInfo");
                    DataTable Table = ds.Tables["MemberTypeInfo"];

                    boList = Table.AsEnumerable().Select(r => new MemMemberTypeBO
                    {
                        TypeId = r.Field<int>("TypeId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        SubscriptionFee = r.Field<decimal?>("SubscriptionFee"),
                        DiscountPercent = r.Field<decimal?>("DiscountPercent")
                    }).ToList();
                }
            }
            return boList;
        }

        public Boolean UpdateMemberNAccountsInfo(int tmpMemberId, int tmpNodeId, int isNewChartOfAccountsHeadCreateForMember)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberNAccountsInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MemberId", DbType.String, tmpMemberId);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.String, tmpNodeId);
                    dbSmartAspects.AddInParameter(command, "@IsCOAEnable", DbType.Int32, isNewChartOfAccountsHeadCreateForMember);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<MemMemberBasicsBO> GetMemberInfoByName(string memberName)
        {
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();
            DataSet memberDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGetMemberInfoByName_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberName", DbType.String, memberName);

                    dbSmartAspects.LoadDataSet(cmd, memberDS, "MemberInfo");
                    DataTable table = memberDS.Tables["MemberInfo"];

                    memberList = table.AsEnumerable().Select(r =>
                                   new MemMemberBasicsBO
                                   {
                                       MemberId = r.Field<int>("MemberId"),
                                       CompanyId = r.Field<int>("CompanyId"),
                                       NodeId = r.Field<int>("NodeId"),
                                       TypeId = r.Field<int>("TypeId"),
                                       TypeName = r.Field<string>("TypeName"),
                                       MembershipNumber = r.Field<string>("MembershipNumber"),
                                       NameTitle = r.Field<string>("NameTitle"),
                                       FullName = r.Field<string>("FullName"),
                                       FirstName = r.Field<string>("FirstName"),
                                       MiddleName = r.Field<string>("MiddleName"),
                                       LastName = r.Field<string>("LastName"),
                                       FatherName = r.Field<string>("FatherName"),
                                       MotherName = r.Field<string>("MotherName"),
                                       BirthDate = r.Field<DateTime>("BirthDate"),
                                       MemberGender = r.Field<int>("MemberGender"),
                                       MemberAddress = r.Field<string>("MemberAddress"),
                                       MailAddress = r.Field<string>("MailAddress"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       OfficeEmail = r.Field<string>("OfficeEmail"),
                                       HomeFax = r.Field<string>("HomeFax"),
                                       OfficeFax = r.Field<string>("OfficeFax"),
                                       MaritalStatus = r.Field<int>("MaritalStatus"),
                                       BloodGroup = r.Field<int>("BloodGroup"),
                                       Nationality = r.Field<int>("Nationality"),
                                       RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                                       ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                                       PassportNumber = r.Field<string>("PassportNumber"),
                                       Organization = r.Field<string>("Organization"),
                                       Occupation = r.Field<string>("Occupation"),
                                       Designation = r.Field<string>("Designation"),
                                       AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                                       MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                                       SecurityDeposit = r.Field<decimal>("SecurityDeposit"),
                                       MobileNumber = r.Field<string>("MobileNumber")

                                   }).ToList();
                }
            }
            return memberList;
        }
        public List<MemMemberBasicsBO> GetMemberDetailInfoForCostcenter(int costCenterId, string memberName)
        {
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();
            DataSet memberDS = new DataSet();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberDetailInfoForCostcenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@MemberName", DbType.String, memberName);

                    dbSmartAspects.LoadDataSet(cmd, memberDS, "MemberInfo");
                    DataTable table = memberDS.Tables["MemberInfo"];

                    memberList = table.AsEnumerable().Select(r =>
                                   new MemMemberBasicsBO
                                   {
                                       MemberId = r.Field<int>("MemberId"),
                                       CompanyId = r.Field<int>("CompanyId"),
                                       NodeId = r.Field<int>("NodeId"),
                                       TypeId = r.Field<int>("TypeId"),
                                       TypeName = r.Field<string>("TypeName"),
                                       MembershipNumber = r.Field<string>("MembershipNumber"),
                                       NameTitle = r.Field<string>("NameTitle"),
                                       FullName = r.Field<string>("FullName"),
                                       FirstName = r.Field<string>("FirstName"),
                                       MiddleName = r.Field<string>("MiddleName"),
                                       LastName = r.Field<string>("LastName"),
                                       FatherName = r.Field<string>("FatherName"),
                                       MotherName = r.Field<string>("MotherName"),
                                       BirthDate = r.Field<DateTime>("BirthDate"),
                                       MemberGender = r.Field<int>("MemberGender"),
                                       MemberAddress = r.Field<string>("MemberAddress"),
                                       MailAddress = r.Field<string>("MailAddress"),
                                       PersonalEmail = r.Field<string>("PersonalEmail"),
                                       OfficeEmail = r.Field<string>("OfficeEmail"),
                                       HomeFax = r.Field<string>("HomeFax"),
                                       OfficeFax = r.Field<string>("OfficeFax"),
                                       MaritalStatus = r.Field<int>("MaritalStatus"),
                                       BloodGroup = r.Field<int>("BloodGroup"),
                                       Nationality = r.Field<int>("Nationality"),
                                       RegistrationDate = r.Field<DateTime?>("RegistrationDate"),
                                       ExpiryDate = r.Field<DateTime?>("ExpiryDate"),
                                       PassportNumber = r.Field<string>("PassportNumber"),
                                       Organization = r.Field<string>("Organization"),
                                       Occupation = r.Field<string>("Occupation"),
                                       Designation = r.Field<string>("Designation"),
                                       AnnualTurnover = r.Field<decimal?>("AnnualTurnover"),
                                       MonthlyIncome = r.Field<decimal?>("MonthlyIncome"),
                                       SecurityDeposit = r.Field<decimal>("SecurityDeposit"),
                                       MobileNumber = r.Field<string>("MobileNumber"),
                                       Balance = r.Field<decimal>("Balance"),
                                       DiscountPercent = r.Field<decimal>("DiscountPercent")

                                   }).ToList();
                }
            }
            return memberList;
        }

        //--------- Bill Generation, Received, Payment
        public MemberBillGenerationBO GetMemberBillGeneration(Int64 MemberBillId)
        {
            MemberBillGenerationBO paymentInfo = new MemberBillGenerationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberBillGeneration_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberBillId", DbType.Int64, MemberBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new MemberBillGenerationBO
                    {
                        MemberBillId = r.Field<Int64>("MemberBillId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        MemberBillNumber = r.Field<string>("MemberBillNumber"),
                        BillCurrencyId = r.Field<Int32>("BillCurrencyId"),
                        Remarks = r.Field<string>("Remarks"),
                        FullName = r.Field<string>("FullName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }

        public List<MemberBillGenerationDetailsBO> GetMemberBillGenerationDetails(Int64 MemberBillId)
        {
            List<MemberBillGenerationDetailsBO> paymentInfo = new List<MemberBillGenerationDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberBillGenerationDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberBillId", DbType.Int64, MemberBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new MemberBillGenerationDetailsBO
                    {
                        MemberBillDetailsId = r.Field<Int64>("MemberBillDetailsId"),
                        MemberBillId = r.Field<Int64>("MemberBillId"),
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        BillId = r.Field<int>("BillId"),
                        Amount = r.Field<decimal>("Amount")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public List<MemberPaymentLedgerVwBo> GetMemberBillForBillGenerationEdit(int MemberId, Int64 MemberBillId)
        {
            List<MemberPaymentLedgerVwBo> supplierInfo = new List<MemberPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberBillForBillGenerationEdit_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId);
                    dbSmartAspects.AddInParameter(cmd, "@MemberBillId", DbType.Int64, MemberBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new MemberPaymentLedgerVwBo
                    {
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        MemberName = r.Field<string>("FullName"),
                        //ContactName = r.Field<string>("ContactName"),
                        ContactNumber = r.Field<string>("MobileNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefMemberPaymentId = r.Field<Int64>("RefMemberPaymentId"),
                        MemberBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public bool DeleteMemberBillGeneration(Int64 memberBillId)
        {
            int status = 0;
            string query = string.Empty;

            query = string.Format(@"
                              UPDATE cpl SET BillGenerationId = 0 
                              FROM PMMemberPaymentLedger cpl 
                                INNER JOIN PMMemberBillGenerationDetails cbgd 
                                    ON cpl.BillGenerationId = cbgd.MemberBillId
                              WHERE cbgd.MemberBillId = {0}

                           DELETE FROM PMMemberBillGenerationDetails WHERE MemberBillId = {0}
                           DELETE FROM PMMemberBillGeneration WHERE MemberBillId = {0}", memberBillId);

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
        public List<MemberPaymentLedgerVwBo> MemberBillBySearch(int memberId)
        {
            List<MemberPaymentLedgerVwBo> supplierInfo = new List<MemberPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberBillBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new MemberPaymentLedgerVwBo
                    {
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        FullName = r.Field<string>("FullName"),
                        //ContactNumber = r.Field<string>("ContactNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        UsdConversionRate = r.Field<decimal>("UsdConversionRate"),
                        UsdBillAmount = r.Field<decimal>("UsdBillAmount"),
                        BillGenerationId = r.Field<Int64>("BillGenerationId"),
                        RefMemberPaymentId = r.Field<Int64>("RefMemberPaymentId"),
                        MemberBillDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public bool SaveMemberBillGeneration(MemberBillGenerationBO billGeneration, List<MemberBillGenerationDetailsBO> billGenerationDetails, out Int64 memberBillId)
        {
            int status = 0;
            memberBillId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberBillGeneration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@MemberId", DbType.Int64, billGeneration.MemberId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, billGeneration.BillDate);
                            dbSmartAspects.AddInParameter(command, "@BillCurrencyId", DbType.Int32, billGeneration.BillCurrencyId);

                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, billGeneration.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, billGeneration.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@MemberBillId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            memberBillId = Convert.ToInt64(command.Parameters["@MemberBillId"].Value);
                        }
                        if (billGenerationDetails.Count > 0 && status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberBillGenerationDetails_SP"))
                            {
                                foreach (MemberBillGenerationDetailsBO cpl in billGenerationDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.Int64, memberBillId);
                                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);

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

        public bool UpdateMemberBillGeneration(MemberBillGenerationBO billGeneration, List<MemberBillGenerationDetailsBO> billGenerationDetails,
                      List<MemberBillGenerationDetailsBO> billGenerationDetailsEdited, List<MemberBillGenerationDetailsBO> billGenerationDetailsDeleted)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberBillGeneration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.String, billGeneration.MemberBillId);
                            dbSmartAspects.AddInParameter(command, "@MemberId", DbType.Int64, billGeneration.MemberId);
                            dbSmartAspects.AddInParameter(command, "@BillDate", DbType.DateTime, billGeneration.BillDate);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, billGeneration.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, billGeneration.CreatedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                        }

                        if (status > 0 && billGenerationDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberBillGenerationDetails_SP"))
                            {
                                foreach (MemberBillGenerationDetailsBO cpl in billGenerationDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.Int64, billGeneration.MemberBillId);
                                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int32, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && billGenerationDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberBillGenerationDetails_SP"))
                            {
                                foreach (MemberBillGenerationDetailsBO cpl in billGenerationDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@MemberBillDetailsId", DbType.Int64, cpl.MemberBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.Int64, billGeneration.MemberBillId);
                                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.Int64, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@Amount", DbType.Decimal, cpl.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && billGenerationDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteMemberBillGenerationDetails_SP"))
                            {
                                foreach (MemberBillGenerationDetailsBO cpl in billGenerationDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@MemberBillDetailsId", DbType.Int64, cpl.MemberBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.Int64, cpl.MemberBillId);
                                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);

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

        public List<MemberBillGenerationBO> GetMemberBillGenerationBySearch(DateTime? dateFrom, DateTime? dateTo, int memberId)
        {
            List<MemberBillGenerationBO> paymentInfo = new List<MemberBillGenerationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberBillGenerationBySearch_SP"))
                {
                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    if (memberId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new MemberBillGenerationBO
                    {
                        MemberBillId = r.Field<Int64>("MemberBillId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        MemberBillNumber = r.Field<string>("MemberBillNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        MemberName = r.Field<string>("FullName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public List<MemMemberBasicsBO> GetMemberInfoForAutoSearch(string memberName)
        {
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberInfoForAutoSearch_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(memberName))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemberName", DbType.String, memberName);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@MemberName", DbType.String, DBNull.Value);
                    }

                    DataSet memberDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, memberDS, "MemberInfo");
                    DataTable table = memberDS.Tables["MemberInfo"];
                    if (table != null)
                        memberList = table.AsEnumerable().Select(r =>
                                       new MemMemberBasicsBO
                                       {
                                           MemberId = r.Field<int>("MemberId"),
                                           FullName = r.Field<string>("FullName"),
                                           MembershipNumber = r.Field<string>("MembershipNumber"),
                                           NameWithMembershipNumber = r.Field<string>("NameWithMembershipNumber"),
                                       }).ToList();
                }
            }
            return memberList;
        }

        public List<MemberPaymentLedgerVwBo> MemberBillByMemberIdAndBillGenerationFlag(int MemberId, Int64 MemberBillId, int currencyId)
        {
            List<MemberPaymentLedgerVwBo> supplierInfo = new List<MemberPaymentLedgerVwBo>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("MemberBillByMemberIdAndBillGenerationFlag_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId);
                    dbSmartAspects.AddInParameter(cmd, "@MemberBillId", DbType.Int64, MemberBillId);
                    dbSmartAspects.AddInParameter(cmd, "@CurrencyId", DbType.Int64, currencyId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new MemberPaymentLedgerVwBo
                    {
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        FullName = r.Field<string>("FullName"),
                        //ContactName = r.Field<string>("ContactName"),
                        MobileNumber = r.Field<string>("MobileNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        MemberBillNumber = r.Field<string>("MemberBillNumber"),
                        BillId = r.Field<Int32>("BillId"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        Remarks = r.Field<string>("Remarks"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        CurrencyId = r.Field<Int32>("BillCurrencyId")

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        //------- Member Bill Receive
        public MemberPaymentBO GetMemberPayment(Int64 paymentId)
        {
            MemberPaymentBO paymentInfo = new MemberPaymentBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new MemberPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        MemberBillId = r.Field<Int64>("MemberBillId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        MemberId = r.Field<int>("MemberId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        ConvertionRate = r.Field<decimal?>("ConvertionRate"),
                        PaymentType = r.Field<string>("PaymentType"),
                        PaymentFor = r.Field<string>("PaymentFor"),
                        AccountingPostingHeadId = r.Field<int>("AccountingPostingHeadId"),
                        AdjustmentType = r.Field<string>("AdjustmentType"),
                        MemberPaymentAdvanceId = r.Field<Int64>("MemberPaymentAdvanceId"),
                        CurrencyType = r.Field<string>("CurrencyType"),
                        AdjustmentAmount = r.Field<decimal>("AdjustmentAmount"),
                        AdjustmentAccountHeadId = r.Field<int?>("AdjustmentAccountHeadId"),
                        PaymentAdjustmentAmount = r.Field<decimal?>("PaymentAdjustmentAmount")

                    }).FirstOrDefault();
                }
            }

            return paymentInfo;
        }

        public List<MemberPaymentDetailsBO> GetMemberPaymentDetails(Int64 paymentId)
        {
            List<MemberPaymentDetailsBO> supplierInfo = new List<MemberPaymentDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.Int64, paymentId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new MemberPaymentDetailsBO
                    {
                        MemberBillId = r.Field<long>("MemberBillId"),
                        PaymentDetailsId = r.Field<long>("PaymentDetailsId"),
                        PaymentId = r.Field<long>("PaymentId"),
                        MemberBillDetailsId = r.Field<long?>("MemberBillDetailsId"),
                        MemberPaymentId = r.Field<long>("MemberPaymentId"),

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
        public List<MemberBillGenerationBO> GetMemberBillForBillReceive(int MemberId, Int64 MemberBillId)
        {
            List<MemberBillGenerationBO> supplierInfo = new List<MemberBillGenerationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberBillForReceivedPayment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId);
                    dbSmartAspects.AddInParameter(cmd, "@MemberBillId", DbType.Int64, MemberBillId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new MemberBillGenerationBO
                    {
                        MemberBillId = r.Field<Int64>("MemberBillId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        MemberBillNumber = r.Field<string>("MemberBillNumber"),
                        MemberBillDetailsId = r.Field<Int64>("MemberBillDetailsId"),
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        BillId = r.Field<Int32>("BillId"),
                        Amount = r.Field<decimal>("Amount"),
                        BillNumber = r.Field<string>("BillNumber"),
                        ModuleName = r.Field<string>("ModuleName"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        BillCurrencyId = r.Field<int>("BillCurrencyId"),
                        //PaymentDetailsId = 0

                    }).ToList();
                }
            }

            return supplierInfo;
        }

        public List<MemberBillGenerationBO> GetMemberGeneratedBillByBillStatus(int MemberId)
        {
            List<MemberBillGenerationBO> paymentInfo = new List<MemberBillGenerationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberGeneratedBill_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new MemberBillGenerationBO
                    {
                        MemberBillId = r.Field<Int64>("MemberBillId"),
                        MemberId = r.Field<Int32>("MemberId"),
                        BillDate = r.Field<DateTime>("BillDate"),
                        MemberBillNumber = r.Field<string>("MemberBillNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        BillCurrencyId = r.Field<int>("BillCurrencyId")

                    }).ToList();
                }
            }

            return paymentInfo;
        }

        public List<MemberPaymentBO> GetMemberPaymentBySearch(int MemberId, DateTime? dateFrom, DateTime? dateTo, string paymentFor)
        {
            List<MemberPaymentBO> paymentInfo = new List<MemberPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentBySearch_SP"))
                {
                    if (MemberId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@PaymentFor", DbType.String, paymentFor);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new MemberPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        LedgerNumber = r.Field<string>("LedgerNumber"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        MemberId = r.Field<int>("MemberId"),
                        AdvanceAmount = r.Field<decimal>("AdvanceAmount"),
                        Remarks = r.Field<string>("Remarks"),
                        FullName = r.Field<string>("FullName"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AdjustmentType = r.Field<string>("AdjustmentType")

                    }).ToList();
                }
            }

            return paymentInfo;
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ApprovedMemberPaymentLedger_SP"))
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
                                DELETE FROM PMMemberPaymentDetails
                                WHERE PaymentId = {0}

                                DELETE FROM PMMemberPayment
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

        public bool SaveMemberBillPayment(MemberPaymentBO MemberPayment, List<MemberPaymentDetailsBO> MemberPaymentDetails)
        {
            int status = 0;
            Int64 MemberPaymentId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.String, MemberPayment.MemberBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, MemberPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, MemberPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@MemberPaymentAdvanceId", DbType.String, MemberPayment.MemberPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@MemberId", DbType.String, MemberPayment.MemberId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, MemberPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.Decimal, MemberPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, MemberPayment.AdjustmentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, MemberPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, MemberPayment.AccountingPostingHeadId);

                            if (!string.IsNullOrEmpty(MemberPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, MemberPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(MemberPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, MemberPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (MemberPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, MemberPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (MemberPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, MemberPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (MemberPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, MemberPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (MemberPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, MemberPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@PaymentId", DbType.Int32, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            MemberPaymentId = Convert.ToInt64(command.Parameters["@PaymentId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberPaymentDetails_SP"))
                        {
                            foreach (MemberPaymentDetailsBO cpl in MemberPaymentDetails)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, MemberPaymentId);
                                dbSmartAspects.AddInParameter(command, "@MemberBillDetailsId", DbType.Int64, cpl.MemberBillDetailsId);
                                dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);
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

        public bool UpdateMemberBillPayment(MemberPaymentBO MemberPayment, List<MemberPaymentDetailsBO> MemberPaymentDetails,
            List<MemberPaymentDetailsBO> MemberPaymentDetailsEdited, List<MemberPaymentDetailsBO> MemberPaymentDetailsDeleted)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberPayment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.String, MemberPayment.PaymentId);
                            dbSmartAspects.AddInParameter(command, "@MemberBillId", DbType.String, MemberPayment.MemberBillId);
                            dbSmartAspects.AddInParameter(command, "@PaymentFor", DbType.String, MemberPayment.PaymentFor);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentType", DbType.String, MemberPayment.AdjustmentType);
                            dbSmartAspects.AddInParameter(command, "@MemberPaymentAdvanceId", DbType.String, MemberPayment.MemberPaymentAdvanceId);
                            dbSmartAspects.AddInParameter(command, "@MemberId", DbType.String, MemberPayment.MemberId);
                            dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, MemberPayment.PaymentDate);
                            dbSmartAspects.AddInParameter(command, "@AdvanceAmount", DbType.String, MemberPayment.AdvanceAmount);
                            dbSmartAspects.AddInParameter(command, "@AdjustmentAmount", DbType.Decimal, MemberPayment.AdjustmentAmount);

                            dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, MemberPayment.PaymentType);
                            dbSmartAspects.AddInParameter(command, "@AccountingPostingHeadId", DbType.Int32, MemberPayment.AccountingPostingHeadId);

                            if (!string.IsNullOrEmpty(MemberPayment.Remarks))
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, MemberPayment.Remarks);
                            else
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(MemberPayment.ChequeNumber))
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, MemberPayment.ChequeNumber);
                            else
                                dbSmartAspects.AddInParameter(command, "@ChequeNumber", DbType.String, DBNull.Value);

                            if (MemberPayment.CurrencyId != null)
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, MemberPayment.CurrencyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, DBNull.Value);

                            if (MemberPayment.ConvertionRate != null)
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, MemberPayment.ConvertionRate);
                            else
                                dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Int32, DBNull.Value);

                            if (MemberPayment.AdjustmentAccountHeadId != null)
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, MemberPayment.AdjustmentAccountHeadId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AdjustmentAccountHeadId", DbType.Int32, DBNull.Value);

                            if (MemberPayment.PaymentAdjustmentAmount != null)
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Decimal, MemberPayment.PaymentAdjustmentAmount);
                            else
                                dbSmartAspects.AddInParameter(command, "@PaymentAdjustmentAmount", DbType.Int32, DBNull.Value);

                            status = dbSmartAspects.ExecuteNonQuery(command, transaction);

                            supplierIdPaymentId = Convert.ToInt32(command.Parameters["@PaymentId"].Value);
                        }

                        if (status > 0 && MemberPaymentDetails.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberPaymentDetails_SP"))
                            {
                                foreach (MemberPaymentDetailsBO cpl in MemberPaymentDetails)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, MemberPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@MemberBillDetailsId", DbType.Int64, cpl.MemberBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && MemberPaymentDetailsEdited.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberPaymentDetails_SP"))
                            {
                                foreach (MemberPaymentDetailsBO cpl in MemberPaymentDetailsEdited)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@PaymentDetailsId", DbType.Int64, cpl.PaymentDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentId", DbType.Int64, MemberPayment.PaymentId);
                                    dbSmartAspects.AddInParameter(command, "@MemberBillDetailsId", DbType.Int64, cpl.MemberBillDetailsId);
                                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, cpl.MemberPaymentId);
                                    dbSmartAspects.AddInParameter(command, "@BillId", DbType.String, cpl.BillId);
                                    dbSmartAspects.AddInParameter(command, "@PaymentAmount", DbType.Decimal, cpl.PaymentAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transaction);
                                }
                            }
                        }

                        if (status > 0 && MemberPaymentDetailsDeleted.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (MemberPaymentDetailsBO cpl in MemberPaymentDetailsDeleted)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "PMMemberPaymentDetails");
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
    }
}
