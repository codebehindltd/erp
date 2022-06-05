using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesAndMarketing;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class SalesCallDA : BaseService
    {
        public Boolean SaveSalesCallInfo(SMCompanySalesCallBO salesCallBO, List<SMCompanySalesCallDetailBO> salesCallDetailList, out int tmpSalesCallId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalesCallInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, salesCallBO.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@SiteId", DbType.Int32, salesCallBO.SiteId);
                        dbSmartAspects.AddInParameter(command, "@InitialDate", DbType.DateTime, salesCallBO.InitialDate);
                        dbSmartAspects.AddInParameter(command, "@FollowupDate", DbType.DateTime, salesCallBO.FollowupDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, salesCallBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, salesCallBO.LocationId);
                        dbSmartAspects.AddInParameter(command, "@CityId", DbType.Int32, salesCallBO.CityId);
                        //dbSmartAspects.AddInParameter(command, "@IndustryId", DbType.Int32, salesCallBO.IndustryId);
                        dbSmartAspects.AddInParameter(command, "@FollowupTypeId", DbType.Int32, salesCallBO.FollowupTypeId);
                        dbSmartAspects.AddInParameter(command, "@FollowupType", DbType.String, salesCallBO.FollowupType);
                        dbSmartAspects.AddInParameter(command, "@PurposeId", DbType.Int32, salesCallBO.PurposeId);
                        dbSmartAspects.AddInParameter(command, "@Purpose", DbType.String, salesCallBO.Purpose);

                        dbSmartAspects.AddInParameter(command, "@CITypeId", DbType.Int32, salesCallBO.CITypeId);
                        dbSmartAspects.AddInParameter(command, "@ActionPlanId", DbType.Int32, salesCallBO.ActionPlanId);
                        dbSmartAspects.AddInParameter(command, "@OpportunityStatusId", DbType.Int32, salesCallBO.OpportunityStatusId);

                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salesCallBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@SalesCallId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpSalesCallId = Convert.ToInt32(command.Parameters["@SalesCallId"].Value);
                    }

                    if (status)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesCallDetailsInfo_SP"))
                        {
                            foreach (SMCompanySalesCallDetailBO detailBO in salesCallDetailList)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int32, tmpSalesCallId);
                                dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, detailBO.EmpId);
                                dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, detailBO.CreatedBy);

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
        public Boolean UpdateSalesCallInfo(SMCompanySalesCallBO salesCallBO, List<SMCompanySalesCallDetailBO> deleteDetailList, List<SMCompanySalesCallDetailBO> addDetailList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalesCallInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SalesCallId", DbType.Int32, salesCallBO.SalesCallId);
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, salesCallBO.CompanyId);
                        dbSmartAspects.AddInParameter(command, "@SiteId", DbType.Int32, salesCallBO.SiteId);
                        dbSmartAspects.AddInParameter(command, "@InitialDate", DbType.DateTime, salesCallBO.InitialDate);
                        dbSmartAspects.AddInParameter(command, "@FollowupDate", DbType.DateTime, salesCallBO.FollowupDate);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, salesCallBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, salesCallBO.LocationId);
                        dbSmartAspects.AddInParameter(command, "@CityId", DbType.Int32, salesCallBO.CityId);
                        //dbSmartAspects.AddInParameter(command, "@IndustryId", DbType.Int32, salesCallBO.IndustryId);
                        dbSmartAspects.AddInParameter(command, "@FollowupTypeId", DbType.Int32, salesCallBO.FollowupTypeId);
                        dbSmartAspects.AddInParameter(command, "@FollowupType", DbType.String, salesCallBO.FollowupType);
                        dbSmartAspects.AddInParameter(command, "@PurposeId", DbType.Int32, salesCallBO.PurposeId);
                        dbSmartAspects.AddInParameter(command, "@Purpose", DbType.String, salesCallBO.Purpose);
                        dbSmartAspects.AddInParameter(command, "@CITypeId", DbType.String, salesCallBO.CITypeId);
                        dbSmartAspects.AddInParameter(command, "@ActionPlanId", DbType.String, salesCallBO.ActionPlanId);
                        dbSmartAspects.AddInParameter(command, "@OpportunityStatusId", DbType.String, salesCallBO.OpportunityStatusId);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, salesCallBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }

                    if (deleteDetailList != null)
                    {
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteSalesCallDetailsInfo_SP"))
                            {
                                foreach (SMCompanySalesCallDetailBO detailBO in deleteDetailList)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@SalesCallDetailId", DbType.Int32, detailBO.SalesCallDetailId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                }
                            }
                        }
                    }

                    if (addDetailList != null)
                    {
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesCallDetailsInfo_SP"))
                            {
                                foreach (SMCompanySalesCallDetailBO detailBO in addDetailList)
                                {
                                    cmd.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int32, salesCallBO.SalesCallId);
                                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, detailBO.EmpId);
                                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, detailBO.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                }
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
        public SMCompanySalesCallBO GetSalesCallInfoById(int salesCallId)
        {
            int count = 0;
            SMCompanySalesCallBO salesCall = new SMCompanySalesCallBO();
            List<SMCompanySalesCallDetailBO> salesCallDetailList = new List<SMCompanySalesCallDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCallInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int32, salesCallId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                salesCall.SalesCallId = salesCallId;
                                salesCall.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                salesCall.SiteId = Convert.ToInt32(reader["SiteId"]);
                                salesCall.CompanyName = reader["CompanyName"].ToString();
                                salesCall.InitialDate = Convert.ToDateTime(reader["InitialDate"]);
                                salesCall.FollowupDate = Convert.ToDateTime(reader["FollowupDate"]);
                                salesCall.Remarks = reader["Remarks"].ToString();
                                salesCall.LocationId = Convert.ToInt32(reader["LocationId"]);
                                salesCall.CityId = Convert.ToInt32(reader["CityId"]);
                                //salesCall.IndustryId = Convert.ToInt32(reader["IndustryId"]);
                                salesCall.FollowupTypeId = Convert.ToInt32(reader["FollowupTypeId"]);
                                salesCall.FollowupType = reader["FollowupType"].ToString();
                                salesCall.PurposeId = Convert.ToInt32(reader["PurposeId"]);
                                salesCall.Purpose = reader["Purpose"].ToString();

                                if (!string.IsNullOrEmpty(reader["CITypeId"].ToString()))
                                    salesCall.CITypeId = Convert.ToInt32(reader["CITypeId"]);

                                if (!string.IsNullOrEmpty(reader["ActionPlanId"].ToString()))
                                    salesCall.ActionPlanId = Convert.ToInt32(reader["ActionPlanId"]);

                                if (!string.IsNullOrEmpty(reader["OpportunityStatusId"].ToString()))
                                    salesCall.OpportunityStatusId = Convert.ToInt32(reader["OpportunityStatusId"]);

                                count++;
                            }
                        }
                    }
                }
                if (count > 0)
                {

                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCallDeatilInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int32, salesCallId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMCompanySalesCallDetailBO salesCallDetailBO = new SMCompanySalesCallDetailBO();

                                    salesCallDetailBO.SalesCallDetailId = Convert.ToInt32(reader["SalesCallDetailId"]);
                                    salesCallDetailBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                    salesCallDetailBO.EmpName = reader["DisplayName"].ToString();

                                    salesCallDetailList.Add(salesCallDetailBO);
                                }
                            }
                        }
                    }
                }
                salesCall.SMCompanySalesCallDetailList = salesCallDetailList;
            }
            return salesCall;
        }
        public List<SMCompanySalesCallBO> GetSalesCallInfoBySearchCriteriaForPaging(string companyName, DateTime? fromIniDate, DateTime? toIniDate, DateTime? fromFolupDate, DateTime? toFolupDate, int? foluptypeId, int? purposeId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMCompanySalesCallBO> salesCallList = new List<SMCompanySalesCallBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCallInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyName", DbType.String, companyName);
                    dbSmartAspects.AddInParameter(cmd, "@FromIniDate", DbType.DateTime, fromIniDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToIniDate", DbType.DateTime, toIniDate);
                    dbSmartAspects.AddInParameter(cmd, "@FromFolupDate", DbType.DateTime, fromFolupDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToFolupDate", DbType.DateTime, toFolupDate);
                    if (foluptypeId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FolupTypeId", DbType.Int32, foluptypeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FolupTypeId", DbType.Int32, DBNull.Value);
                    }
                    if (purposeId != null)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PurposeId", DbType.Int32, purposeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PurposeId", DbType.Int32, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMCompanySalesCallBO salesCall = new SMCompanySalesCallBO();
                                salesCall.SalesCallId = Convert.ToInt32(reader["SalesCallId"]);
                                salesCall.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                salesCall.CompanyName = reader["CompanyName"].ToString();
                                salesCall.InitialDate = Convert.ToDateTime(reader["InitialDate"]);
                                salesCall.FollowupDate = Convert.ToDateTime(reader["FollowupDate"]);
                                salesCallList.Add(salesCall);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return salesCallList;
        }
        public Boolean DeleteSalesCallDetailsInfo(int salesCallDetailId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSalesCallDetailInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SalesCallDetailId", DbType.Int32, salesCallDetailId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public Boolean DeleteSalesCallInfo(int salesCallId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSalesCallInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SalesCallId", DbType.Int32, salesCallId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }

                if (status)
                {
                    var detailList = GetSalesCallDetailInfoBySalesCallId(salesCallId);
                    if (detailList.Count != 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSalesCallDetailsInfoBySalesCallId_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@SalesCallId", DbType.Int32, salesCallId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            return status;
        }
        public List<SMCompanySalesCallDetailBO> GetSalesCallDetailInfoBySalesCallId(int salesCallId)
        {
            int count = 0;
            List<SMCompanySalesCallDetailBO> salesCallDetailList = new List<SMCompanySalesCallDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCallDeatilInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int32, salesCallId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMCompanySalesCallDetailBO salesCallDetailBO = new SMCompanySalesCallDetailBO();

                                salesCallDetailBO.SalesCallDetailId = Convert.ToInt32(reader["SalesCallDetailId"]);
                                salesCallDetailBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                salesCallDetailBO.EmpName = reader["DisplayName"].ToString();

                                salesCallDetailList.Add(salesCallDetailBO);
                            }
                        }
                    }
                }
            }
            return salesCallDetailList;
        }
        public List<SalesNMarketingScheduleViewBO> GetSalesNMarketingScheduleInfo(DateTime fromDate, DateTime toDate, int salesPerson, int salesPurpose)
        {
            List<SalesNMarketingScheduleViewBO> viewList = new List<SalesNMarketingScheduleViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesNMarketingScheduleInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (salesPerson > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesPersonId", DbType.Int32, salesPerson);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesPersonId", DbType.Int32, DBNull.Value);
                    }
                    if (salesPurpose > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesPurpose", DbType.Int32, salesPurpose);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SalesPurpose", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesNMarketingSchedule");
                    DataTable Table = ds.Tables["SalesNMarketingSchedule"];

                    viewList = Table.AsEnumerable().Select(r => new SalesNMarketingScheduleViewBO
                    {
                        InitialDate = r.Field<string>("InitialDate"),
                        FollowUpDate = r.Field<string>("FollowUpDate"),
                        SalesPerson = r.Field<string>("SalesPerson"),
                        Participants = r.Field<string>("Participants"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ContactPerson = r.Field<string>("ContactPerson"),
                        ContactNumber = r.Field<string>("ContactNumber"),
                        Email = r.Field<string>("Email"),
                        FollowupType = r.Field<string>("FollowupType"),
                        Purpose = r.Field<string>("Purpose"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return viewList;
        }
        public List<SalesPersonViewBO> GetSalesPersonInfo()
        {
            List<SalesPersonViewBO> salesPersonList = new List<SalesPersonViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesPersonInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesPerson");
                    DataTable Table = ds.Tables["SalesPerson"];

                    salesPersonList = Table.AsEnumerable().Select(r => new SalesPersonViewBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        EmpName = r.Field<string>("EmpName")
                    }).ToList();
                }
            }
            return salesPersonList;

        }
        public List<CompanyWiseGuestInfoViewBO> GetGuestInformationForReport()
        {
            List<CompanyWiseGuestInfoViewBO> list = new List<CompanyWiseGuestInfoViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestInformation");
                    DataTable Table = ds.Tables["GuestInformation"];

                    list = Table.AsEnumerable().Select(r => new CompanyWiseGuestInfoViewBO
                    {
                        GuestName = r.Field<string>("GuestName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        GuestDOB = r.Field<string>("GuestDOB"),
                        GuestAge = r.Field<string>("GuestAge"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        TelephoneNumber = r.Field<string>("TelephoneNumber")
                    }).ToList();
                }
            }
            return list;
        }
        public List<CompanyWiseGuestInfoViewBO> GetGuestInformationBySearchCriteriaForReport(string reportType, string filterType, int? company, int? reference, int? country, DateTime fromDate, DateTime toDate)
        {
            List<CompanyWiseGuestInfoViewBO> list = new List<CompanyWiseGuestInfoViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestInformationBySearchCriteriaForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@FilterType", DbType.String, filterType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, company);
                    dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.Int32, reference);
                    dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, country);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CompanyWiseGuestInfo");
                    DataTable Table = ds.Tables["CompanyWiseGuestInfo"];

                    list = Table.AsEnumerable().Select(r => new CompanyWiseGuestInfoViewBO
                    {
                        RegistrationNumber = r.Field<string>("RegistrationNumber"),
                        GuestName = r.Field<string>("GuestName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        EmailAddress = r.Field<string>("EmailAddress"),
                        WebAddress = r.Field<string>("WebAddress"),
                        TelephoneNumber = r.Field<string>("TelephoneNumber"),
                        FaxNumber = r.Field<string>("FaxNumber"),
                        CompanyRemarks = r.Field<string>("CompanyRemarks"),
                        CountryId = r.Field<int>("CountryId"),
                        CountryName = r.Field<string>("CountryName"),
                        GuestNationality = r.Field<string>("GuestNationality"),
                        PassportNumber = r.Field<string>("PassportNumber"),
                        GuestDOB = r.Field<string>("GuestDOB"),
                        TotalPerson = r.Field<int>("TotalPerson"),
                        DateIn = r.Field<string>("DateIn"),
                        DateOut = r.Field<string>("DateOut"),
                        ReferenceId = r.Field<int>("ReferenceId"),
                        GuestReferance = r.Field<string>("GuestReferance"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        CurrencyHead = r.Field<string>("CurrencyHead"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        RoomRate = r.Field<decimal>("RoomRate"),
                        NoOfNight = r.Field<decimal>("NoOfNight"),
                        TotalBill = r.Field<decimal>("TotalBill"),
                        AirportPickUp = r.Field<string>("AirportPickUp"),
                        AirportDrop = r.Field<string>("AirportDrop"),
                        ArrivalFlightName = r.Field<string>("ArrivalFlightName"),
                        ArrivalFlightNumber = r.Field<string>("ArrivalFlightNumber"),
                        ArrivalTimeString = r.Field<string>("ArrivalTimeString"),
                        UserName = r.Field<string>("UserName"),
                        CheckOutBy = r.Field<string>("CheckOutBy"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }
            return list;
        }
        public List<SummaryGuestInfoViewBO> GetGuestInformationBySearchCriteriaForSummaryReport(string reportType, int? company, int? reference, int? country, DateTime fromDate, DateTime toDate)
        {
            List<SummaryGuestInfoViewBO> list = new List<SummaryGuestInfoViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestSummaryInformationBySearchCriteriaForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, company);
                    dbSmartAspects.AddInParameter(cmd, "@ReferenceId", DbType.Int32, reference);
                    dbSmartAspects.AddInParameter(cmd, "@CountryId", DbType.Int32, country);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CompanyWiseGuestInfo");
                    DataTable Table = ds.Tables["CompanyWiseGuestInfo"];

                    list = Table.AsEnumerable().Select(r => new SummaryGuestInfoViewBO
                    {
                        TransactionHead = r.Field<string>("TransactionHead"),
                        TransactionDate = r.Field<DateTime>("TransactionDate"),
                        DisplayTransactionDate = r.Field<string>("DisplayTransactionDate"),
                        StayedNights = r.Field<int>("StayedNights")

                    }).ToList();
                }
            }
            return list;
        }
        public List<SalesCallViewBO> GetAllSalesCallInfo()
        {
            List<SalesCallViewBO> viewList = new List<SalesCallViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllSalesCallInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                    DataTable Table = ds.Tables["SalesCall"];

                    viewList = Table.AsEnumerable().Select(r => new SalesCallViewBO
                    {
                        SalesCallId = r.Field<Int64>("SalesCallId"),
                        EmpId = r.Field<int>("EmpId"),
                        FollowupDate = r.Field<DateTime>("FollowupDate"),
                        CompanyName = r.Field<string>("CompanyName"),
                        CompanyAddress = r.Field<string>("CompanyAddress"),
                        FollowupType = r.Field<string>("FollowupType"),
                        Purpose = r.Field<string>("Purpose"),
                        ShowFollowupDate = r.Field<string>("ShowFollowupDate")

                    }).ToList();
                }
            }
            return viewList;

        }

        //--** Sales Call Analysis
        public List<SalesCallViewBO> GetSalesCallByOpportunityStatus(int companyId, int siteId, int opportunityStatus)
        {
            List<SalesCallViewBO> roomTypeList = new List<SalesCallViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesCallByLastStatus_SP"))
                {
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (siteId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SiteId", DbType.Int32, siteId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SiteId", DbType.Int32, DBNull.Value);

                    if (opportunityStatus != 0)
                        dbSmartAspects.AddInParameter(cmd, "@OpportunityStatusId", DbType.Int32, opportunityStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@OpportunityStatusId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalesCallViewBO salesCall = new SalesCallViewBO();

                                salesCall.SalesCallId = Convert.ToInt32(reader["SalesCallId"]);
                                salesCall.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                salesCall.SiteId = Convert.ToInt32(reader["SiteId"]);
                                salesCall.CompanyName = reader["CompanyName"].ToString();
                                salesCall.InitialDate = Convert.ToDateTime(reader["InitialDate"]);
                                salesCall.FollowupDate = Convert.ToDateTime(reader["FollowupDate"]);
                                salesCall.Remarks = reader["Remarks"].ToString();
                                salesCall.LocationId = Convert.ToInt32(reader["LocationId"]);
                                salesCall.CityId = Convert.ToInt32(reader["CityId"]);
                                salesCall.FollowupTypeId = Convert.ToInt32(reader["FollowupTypeId"]);
                                salesCall.FollowupType = reader["FollowupType"].ToString();
                                salesCall.PurposeId = Convert.ToInt32(reader["PurposeId"]);
                                salesCall.Purpose = reader["Purpose"].ToString();

                                if (!string.IsNullOrEmpty(reader["CITypeId"].ToString()))
                                    salesCall.CITypeId = Convert.ToInt32(reader["CITypeId"]);

                                if (!string.IsNullOrEmpty(reader["ActionPlanId"].ToString()))
                                    salesCall.ActionPlanId = Convert.ToInt32(reader["ActionPlanId"]);

                                if (!string.IsNullOrEmpty(reader["OpportunityStatusId"].ToString()))
                                    salesCall.OpportunityStatusId = Convert.ToInt32(reader["OpportunityStatusId"]);

                                salesCall.CompanyName = reader["CompanyName"].ToString();
                                salesCall.SiteName = reader["SiteName"].ToString();

                                salesCall.CIType = reader["CIType"].ToString();
                                salesCall.ActionPlan = reader["ActionPlan"].ToString();
                                salesCall.OpportunityStatus = reader["OpportunityStatus"].ToString();
                                salesCall.LocationName = reader["LocationName"].ToString();

                                roomTypeList.Add(salesCall);
                            }
                        }
                    }
                }
            }
            return roomTypeList;
        }

        //--**Sales Call Entry
        public Boolean SaveSalesCallEntry(SalesCallEntryBO salesCall, List<SalesCallParticipantBO> participantFromCompany,
                                              List<SalesCallParticipantBO> participantFromClient, out long salesCallEntryId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalesCallEntry_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@LogType", DbType.String, salesCall.LogType);

                        if (salesCall.MeetingDate != null)
                            dbSmartAspects.AddInParameter(command, "@MeetingDate", DbType.DateTime, salesCall.MeetingDate);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingDate", DbType.DateTime, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingLocation))
                            dbSmartAspects.AddInParameter(command, "@MeetingLocation", DbType.String, salesCall.MeetingLocation);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingLocation", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.ParticipantFromParty))
                            dbSmartAspects.AddInParameter(command, "@ParticipantFromParty", DbType.String, salesCall.ParticipantFromParty);
                        else
                            dbSmartAspects.AddInParameter(command, "@ParticipantFromParty", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingAgenda))
                            dbSmartAspects.AddInParameter(command, "@MeetingAgenda", DbType.String, salesCall.MeetingAgenda);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingAgenda", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.Decission))
                            dbSmartAspects.AddInParameter(command, "@Decission", DbType.String, salesCall.Decission);
                        else
                            dbSmartAspects.AddInParameter(command, "@Decission", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingAfterAction))
                            dbSmartAspects.AddInParameter(command, "@MeetingAfterAction", DbType.String, salesCall.MeetingAfterAction);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingAfterAction", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.EmailType))
                            dbSmartAspects.AddInParameter(command, "@EmailType", DbType.String, salesCall.EmailType);
                        else
                            dbSmartAspects.AddInParameter(command, "@EmailType", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingType))
                            dbSmartAspects.AddInParameter(command, "@MeetingType", DbType.String, salesCall.MeetingType);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingType", DbType.String, DBNull.Value);

                        if (salesCall.SocialMediaId > 0)
                            dbSmartAspects.AddInParameter(command, "@SocialMediaId", DbType.Int32, salesCall.SocialMediaId);
                        else
                            dbSmartAspects.AddInParameter(command, "@SocialMediaId", DbType.Int32, DBNull.Value);

                        if (salesCall.AccountManagerId > 0)
                            dbSmartAspects.AddInParameter(command, "@AccountManagerId", DbType.Int32, salesCall.AccountManagerId);
                        else
                            dbSmartAspects.AddInParameter(command, "@AccountManagerId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.CallStatus))
                            dbSmartAspects.AddInParameter(command, "@CallStatus", DbType.String, salesCall.CallStatus);
                        else
                            dbSmartAspects.AddInParameter(command, "@CallStatus", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.LogBody))
                            dbSmartAspects.AddInParameter(command, "@LogBody", DbType.String, salesCall.LogBody);
                        else
                            dbSmartAspects.AddInParameter(command, "@LogBody", DbType.String, DBNull.Value);

                        if (salesCall.CompanyId != null)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, salesCall.CompanyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (salesCall.DealId != null)
                            dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, salesCall.DealId);
                        else
                            dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, DBNull.Value);

                        if (salesCall.ContactId != null)
                            dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, salesCall.ContactId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, DBNull.Value);

                        if (salesCall.LogDate != null)
                            dbSmartAspects.AddInParameter(command, "@LogDate", DbType.DateTime, salesCall.LogDate);
                        else
                            dbSmartAspects.AddInParameter(command, "@LogDate", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salesCall.CreatedBy);

                        dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));
                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                        salesCallEntryId = Convert.ToInt64(command.Parameters["@Id"].Value);
                    }

                    if (status)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesCallParticipant_SP"))
                        {
                            foreach (SalesCallParticipantBO detailBO in participantFromCompany)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallEntryId", DbType.Int64, salesCallEntryId);
                                dbSmartAspects.AddInParameter(cmd, "@PrticipantType", DbType.String, "CompanyParticipant");
                                dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, detailBO.ContactId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesCallParticipant_SP"))
                        {
                            foreach (SalesCallParticipantBO detailBO in participantFromClient)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallEntryId", DbType.Int64, salesCallEntryId);
                                dbSmartAspects.AddInParameter(cmd, "@PrticipantType", DbType.String, "ClientParticipant");
                                dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, detailBO.ContactId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
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
        public Boolean UpdateSalesCallEntry(SalesCallEntryBO salesCall, List<SalesCallParticipantBO> participantFromCompany,
                                             List<SalesCallParticipantBO> participantFromClient, StringBuilder companyParticipantIdList,
                                                    StringBuilder clientParticipantIdList)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalesCallEntry_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, salesCall.Id);
                        dbSmartAspects.AddInParameter(command, "@LogType", DbType.String, salesCall.LogType);
                        if (salesCall.AccountManagerId > 0)
                            dbSmartAspects.AddInParameter(command, "@AccountManagerId", DbType.Int32, salesCall.AccountManagerId);
                        else
                            dbSmartAspects.AddInParameter(command, "@AccountManagerId", DbType.Int32, DBNull.Value);

                        if (salesCall.MeetingDate != null)
                            dbSmartAspects.AddInParameter(command, "@MeetingDate", DbType.DateTime, salesCall.MeetingDate);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingDate", DbType.DateTime, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingLocation))
                            dbSmartAspects.AddInParameter(command, "@MeetingLocation", DbType.String, salesCall.MeetingLocation);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingLocation", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.ParticipantFromParty))
                            dbSmartAspects.AddInParameter(command, "@ParticipantFromParty", DbType.String, salesCall.ParticipantFromParty);
                        else
                            dbSmartAspects.AddInParameter(command, "@ParticipantFromParty", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingAgenda))
                            dbSmartAspects.AddInParameter(command, "@MeetingAgenda", DbType.String, salesCall.MeetingAgenda);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingAgenda", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.Decission))
                            dbSmartAspects.AddInParameter(command, "@Decission", DbType.String, salesCall.Decission);
                        else
                            dbSmartAspects.AddInParameter(command, "@Decission", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingAfterAction))
                            dbSmartAspects.AddInParameter(command, "@MeetingAfterAction", DbType.String, salesCall.MeetingAfterAction);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingAfterAction", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.EmailType))
                            dbSmartAspects.AddInParameter(command, "@EmailType", DbType.String, salesCall.EmailType);
                        else
                            dbSmartAspects.AddInParameter(command, "@EmailType", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.MeetingType))
                            dbSmartAspects.AddInParameter(command, "@MeetingType", DbType.String, salesCall.MeetingType);
                        else
                            dbSmartAspects.AddInParameter(command, "@MeetingType", DbType.String, DBNull.Value);

                        if (salesCall.SocialMediaId > 0)
                            dbSmartAspects.AddInParameter(command, "@SocialMediaId", DbType.Int32, salesCall.SocialMediaId);
                        else
                            dbSmartAspects.AddInParameter(command, "@SocialMediaId", DbType.Int32, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.CallStatus))
                            dbSmartAspects.AddInParameter(command, "@CallStatus", DbType.String, salesCall.CallStatus);
                        else
                            dbSmartAspects.AddInParameter(command, "@CallStatus", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(salesCall.LogBody))
                            dbSmartAspects.AddInParameter(command, "@LogBody", DbType.String, salesCall.LogBody);
                        else
                            dbSmartAspects.AddInParameter(command, "@LogBody", DbType.String, DBNull.Value);

                        if (salesCall.CompanyId != null)
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, salesCall.CompanyId);
                        else
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);

                        if (salesCall.DealId != null)
                            dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, salesCall.DealId);
                        else
                            dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, DBNull.Value);

                        if (salesCall.ContactId != null)
                            dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, salesCall.ContactId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, DBNull.Value);

                        if (salesCall.LogDate != null)
                            dbSmartAspects.AddInParameter(command, "@LogDate", DbType.DateTime, salesCall.LogDate);
                        else
                            dbSmartAspects.AddInParameter(command, "@LogDate", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, salesCall.CreatedBy);


                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;

                    }

                    if (status)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesCallParticipant_SP"))
                        {
                            foreach (SalesCallParticipantBO detailBO in participantFromCompany)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallEntryId", DbType.Int64, salesCall.Id);
                                dbSmartAspects.AddInParameter(cmd, "@PrticipantType", DbType.String, "CompanyParticipant");
                                dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, detailBO.ContactId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }

                    if (status)
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveSalesCallParticipant_SP"))
                        {
                            foreach (SalesCallParticipantBO detailBO in participantFromClient)
                            {
                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallEntryId", DbType.Int64, salesCall.Id);
                                dbSmartAspects.AddInParameter(cmd, "@PrticipantType", DbType.String, "ClientParticipant");
                                dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, detailBO.ContactId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                            }
                        }
                    }
                    if (status)
                    {
                        if (companyParticipantIdList.Length > 0)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteSalesCallParticipant_SP"))
                            {

                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallEntryId", DbType.Int64, salesCall.Id);
                                dbSmartAspects.AddInParameter(cmd, "@PrticipantType", DbType.String, "CompanyParticipant");
                                dbSmartAspects.AddInParameter(cmd, "@ContactIdList", DbType.String, companyParticipantIdList.ToString());

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

                            }
                        }
                        if (clientParticipantIdList.Length > 0)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DeleteSalesCallParticipant_SP"))
                            {

                                cmd.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmd, "@SalesCallEntryId", DbType.Int64, salesCall.Id);
                                dbSmartAspects.AddInParameter(cmd, "@PrticipantType", DbType.String, "ClientParticipant");
                                dbSmartAspects.AddInParameter(cmd, "@ContactIdList", DbType.String, clientParticipantIdList.ToString());

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;

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

        public SalesCallEntryView GetSalesCallById(long id)
        {
            SalesCallEntryView salesCall = new SalesCallEntryView();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetSalesCallById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, id);


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(command, ds, "SalesCall");

                    salesCall = ds.Tables[0].AsEnumerable().Select(r => new SalesCallEntryView()
                    {
                        Id = r.Field<long>("Id"),
                        LogType = r.Field<string>("LogType"),
                        MeetingDate = r.Field<DateTime?>("MeetingDate"),
                        MeetingLocation = r.Field<string>("MeetingLocation"),
                        ParticipantFromParty = r.Field<string>("ParticipantFromParty"),
                        MeetingAgenda = r.Field<string>("MeetingAgenda"),
                        Decission = r.Field<string>("Decission"),
                        MeetingAfterAction = r.Field<string>("MeetingAfterAction"),
                        EmailType = r.Field<string>("EmailType"),
                        CallStatus = r.Field<string>("CallStatus"),
                        LogBody = r.Field<string>("LogBody"),
                        CompanyId = r.Field<int?>("CompanyId"),
                        DealId = r.Field<long?>("DealId"),
                        ContactId = r.Field<long?>("ContactId"),
                        LogDate = r.Field<DateTime>("LogDate"),

                        CompanyName = r.Field<string>("CompanyName"),
                        MeetingType = r.Field<string>("MeetingType"),
                        SocialMediaId = r.Field<int?>("SocialMediaId"),
                        AccountManagerId = r.Field<int?>("AccountManagerId"),                        

                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        LastModifiedBy = r.Field<int?>("LastModifiedBy"),
                        LastModifiedDate = r.Field<DateTime?>("LastModifiedDate"),
                        CallToActionId = r.Field<long?>("CallToActionId"),

                    }).FirstOrDefault();

                    salesCall.participants = ds.Tables[1].AsEnumerable().Select(r => new SalesCallParticipantBO()
                    {
                        Id = r.Field<long>("Id"),
                        SalesCallEntryId = r.Field<long>("SalesCallEntryId"),
                        PrticipantType = r.Field<string>("PrticipantType"),
                        ContactId = r.Field<long>("ContactId")

                    }).ToList();
                }
            }

            return salesCall;
        }
        public bool DeleteLog(long id)
        {
            bool status = false;
            int executeValue = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                try
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteLog_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, id);

                        executeValue = dbSmartAspects.ExecuteNonQuery(command);
                    }
                    status = (executeValue > 0);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return status;
        }

        public List<SalesCallEntryBO> GetLogActivityInformationForSearch(string logtype, string fromDate, string toDate, int company, int accountManager, int industry,
                                                                        int deal, int contact, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SalesCallEntryBO> list = new List<SalesCallEntryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLogActivityInformationForGridPaging_SP"))
                {
                    //cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(logtype))
                        dbSmartAspects.AddInParameter(cmd, "@LogType", DbType.String, logtype);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LogType", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(fromDate))
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(toDate))
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (industry != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Industry", DbType.Int32, industry);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Industry", DbType.Int32, DBNull.Value);

                    if (accountManager != 0)
                        dbSmartAspects.AddInParameter(cmd, "@AccountManager", DbType.Int32, accountManager);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AccountManager", DbType.Int32, DBNull.Value);

                    if (company != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Company", DbType.Int32, company);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Company", DbType.Int32, DBNull.Value);

                    if (contact != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Contact", DbType.Int32, contact);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Contact", DbType.Int32, DBNull.Value);

                    if (deal != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Deal", DbType.Int32, deal);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Deal", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CompanyWiseGuestInfo");
                    DataTable Table = ds.Tables["CompanyWiseGuestInfo"];

                    list = Table.AsEnumerable().Select(r => new SalesCallEntryBO
                    {
                        Id = r.Field<long>("Id"),
                        LogDate = r.Field<DateTime>("LogDate"),
                        LogType = r.Field<string>("LogType"),
                        IndustryName = r.Field<string>("IndustryName"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ParticipantFromParty = r.Field<string>("ClientParticipant"),
                        CompanyParticipant = r.Field<string>("CompanyParticipant"),
                        AccountManager = r.Field<string>("AccountManger"),
                    }).ToList();
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return list;
        }

        public List<SalesCallEntryBO> GetLogActivityInformationForReport(string logtype, DateTime? fromDate, DateTime? toDate, string company, string accountManager, string industry, string contact)
        {
            List<SalesCallEntryBO> list = new List<SalesCallEntryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLogActivityInformationForReport_SP"))
                {
                    //cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(logtype))
                        dbSmartAspects.AddInParameter(cmd, "@LogType", DbType.String, logtype);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LogType", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(industry))
                        dbSmartAspects.AddInParameter(cmd, "@Industry", DbType.String, industry);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Industry", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(accountManager))
                        dbSmartAspects.AddInParameter(cmd, "@AccountManager", DbType.String, accountManager);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@AccountManager", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(company))
                        dbSmartAspects.AddInParameter(cmd, "@Company", DbType.String, company);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Company", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(contact))
                        dbSmartAspects.AddInParameter(cmd, "@Contact", DbType.String, contact);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Contact", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CompanyWiseGuestInfo");
                    DataTable Table = ds.Tables["CompanyWiseGuestInfo"];

                    list = Table.AsEnumerable().Select(r => new SalesCallEntryBO
                    {
                        Id = r.Field<long>("Id"),
                        LogDateString = r.Field<string>("LogDateString"),
                        LogType = r.Field<string>("LogType"),
                        IndustryName = r.Field<string>("IndustryName"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ParticipantFromParty = r.Field<string>("ParticipantFromParty"),
                        CompanyParticipant = r.Field<string>("CompanyParticipant"),
                        AccountManager = r.Field<string>("AccountManager"),

                    }).ToList();
                }
            }
            return list;
        }

        public Boolean DeleteSMLogKeepingBySalesCallEntryIdNContactId( long SalesCallEntryId, long ContactId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSMLogKeepingBySalesCallEntryIdNContactId_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, ContactId);
                        dbSmartAspects.AddInParameter(command, "@SalesCallEntryId", DbType.Int64, SalesCallEntryId);

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
