using HotelManagement.Entity.SupportAndTicket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SupportAndTicket
{
    public class SupportNTicketReportDA : BaseService
    {
        public List<BillClaimedNTotalBillClaimedBO> GetBillClaimedNTotalBillClaimeForReport(DateTime fromDate, DateTime toDate)
        {
            List<BillClaimedNTotalBillClaimedBO> BillClaimedNTotalBillClaimedList = new List<BillClaimedNTotalBillClaimedBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillClaimedNTotalBillClaimed_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));

                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    BillClaimedNTotalBillClaimedBO BillClaimedNTotalBillClaimedBO = new BillClaimedNTotalBillClaimedBO();
                                    BillClaimedNTotalBillClaimedBO.BillClaimed = Convert.ToInt32(reader["BillClaimed"]);
                                    BillClaimedNTotalBillClaimedBO.TotalBillClaimed = Convert.ToDecimal(reader["TotalBillClaimed"]);
                                    BillClaimedNTotalBillClaimedList.Add(BillClaimedNTotalBillClaimedBO);
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
            return BillClaimedNTotalBillClaimedList;
        }
        public List<CaseNameAndCountNumberBO> GetCaseNameAndCountNumberForReport(DateTime fromDate, DateTime toDate)
        {
            List<CaseNameAndCountNumberBO> CaseNameAndCountNumberList = new List<CaseNameAndCountNumberBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCaseNameAndCountNumber_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));

                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    CaseNameAndCountNumberBO CaseNameAndCountNumber = new CaseNameAndCountNumberBO();
                                    CaseNameAndCountNumber.CaseName = reader["CaseName"].ToString();
                                    CaseNameAndCountNumber.Total = Convert.ToInt32(reader["Total"]);
                                    CaseNameAndCountNumberList.Add(CaseNameAndCountNumber);
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
            return CaseNameAndCountNumberList;
        }
        public List<CityNCompanyNameAndCountNumberBO> GetCityNCompanyNameAndCountNumberForReport(string transactiontype, DateTime fromDate, DateTime toDate)
        {
            List<CityNCompanyNameAndCountNumberBO> CityNCompanyNameAndCountNumberList = new List<CityNCompanyNameAndCountNumberBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCityNCompanyNameAndCountNumber_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactiontype);
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    CityNCompanyNameAndCountNumberBO CityNCompanyNameAndCountNumber = new CityNCompanyNameAndCountNumberBO();
                                    CityNCompanyNameAndCountNumber.TransactionName = reader["TransactionName"].ToString();
                                    CityNCompanyNameAndCountNumber.TransactionTotal = Convert.ToInt32(reader["TransactionTotal"]);
                                    CityNCompanyNameAndCountNumberList.Add(CityNCompanyNameAndCountNumber);
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
            return CityNCompanyNameAndCountNumberList;
        }
        public List<WarrantyAndCountNumberBO> GetWarrantyAndCountNumberForReport(DateTime fromDate, DateTime toDate)
        {
            List<WarrantyAndCountNumberBO> ItemWarrantyAndCountNumberList = new List<WarrantyAndCountNumberBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemWarrantyAndCountNumber_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));

                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    WarrantyAndCountNumberBO WarrantyAndCountNumbe = new WarrantyAndCountNumberBO();
                                    WarrantyAndCountNumbe.WarrantyType = reader["WarrantyType"].ToString();
                                    WarrantyAndCountNumbe.Total = Convert.ToInt32(reader["Total"]);
                                    ItemWarrantyAndCountNumberList.Add(WarrantyAndCountNumbe);
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
            return ItemWarrantyAndCountNumberList;
        }
        public List<SupportCategoryAndCountNumberBO> GetSupportCategoryAndCountNumberForReport(DateTime fromDate, DateTime toDate)
        {
            List<SupportCategoryAndCountNumberBO> SupportCategoryAndCountNumberList = new List<SupportCategoryAndCountNumberBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportCategoryAndCountNumber_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));

                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SupportCategoryAndCountNumberBO SupportCategoryAndCountNumber = new SupportCategoryAndCountNumberBO();
                                    SupportCategoryAndCountNumber.SupportCategory = reader["SupportCategory"].ToString();
                                    SupportCategoryAndCountNumber.Total = Convert.ToInt32(reader["Total"]);
                                    SupportCategoryAndCountNumberList.Add(SupportCategoryAndCountNumber);
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
            return SupportCategoryAndCountNumberList;
        }
        public List<SupportTypeAndCountNumberBO> GetSupportTypeAndCountNumberForReport(DateTime fromDate, DateTime toDate)
        {
            List<SupportTypeAndCountNumberBO> SupportTypeAndCountNumberList = new List<SupportTypeAndCountNumberBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportTypeAndCountNumber_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));

                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SupportTypeAndCountNumberBO SupportTypeAndCountNumber = new SupportTypeAndCountNumberBO();
                                    SupportTypeAndCountNumber.SupportType = reader["SupportType"].ToString();
                                    SupportTypeAndCountNumber.Total = Convert.ToInt32(reader["Total"]);
                                    SupportTypeAndCountNumberList.Add(SupportTypeAndCountNumber);
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
            return SupportTypeAndCountNumberList;
        }
        public List<ItemCategoryAndCountNumberBO> GetSupportItemCategoryAndCountNumberForReport(DateTime fromDate, DateTime toDate)
        {
            List<ItemCategoryAndCountNumberBO> ItemCategoryAndCountNumberList = new List<ItemCategoryAndCountNumberBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportItemCategoryAndCountNumber_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));

                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    ItemCategoryAndCountNumberBO ItemCategoryAndCountNumber = new ItemCategoryAndCountNumberBO();
                                    ItemCategoryAndCountNumber.ItemCategory = reader["ItemCategory"].ToString();
                                    ItemCategoryAndCountNumber.Total = Convert.ToInt32(reader["Total"]);
                                    ItemCategoryAndCountNumberList.Add(ItemCategoryAndCountNumber);
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
            return ItemCategoryAndCountNumberList;
        }
        public List<SupportNTicketViewBO> GetSupportNTicketDetailsForReport(DateTime fromDate, DateTime toDate, string TicketNumber,
                                            int Case, int ItemCategory, int SupportCategory, int SupportType, int WarentyType,
                                            int Company, int Country, int State, string Status, decimal BillClaimedFrom, decimal BillClaimedTo)
        {
            List<SupportNTicketViewBO> SupportNTicketViewList = new List<SupportNTicketViewBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportNTicketDetailsForReport_SP"))
                    {
                        
                        if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                        }

                        if (!string.IsNullOrEmpty(TicketNumber))
                            dbSmartAspects.AddInParameter(cmd, "@TicketNumber", DbType.String, Convert.ToString(TicketNumber));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TicketNumber", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(Status))
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Convert.ToString(Status));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                        if (Case != 0)
                            dbSmartAspects.AddInParameter(cmd, "@Case", DbType.Int32, Convert.ToInt32(Case));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Case", DbType.Int32, DBNull.Value);
                        if (ItemCategory != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.Int32, Convert.ToInt32(ItemCategory));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.Int32, DBNull.Value);
                        if (SupportCategory != 0)
                            dbSmartAspects.AddInParameter(cmd, "@SupportCategory", DbType.Int32, Convert.ToInt32(SupportCategory));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SupportCategory", DbType.Int32, DBNull.Value);
                        if (SupportType != 0)
                            dbSmartAspects.AddInParameter(cmd, "@SupportType", DbType.Int32, Convert.ToInt32(SupportType));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SupportType", DbType.Int32, DBNull.Value);
                        if (WarentyType != 0)
                            dbSmartAspects.AddInParameter(cmd, "@WarentyType", DbType.Int32, Convert.ToInt32(WarentyType));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@WarentyType", DbType.Int32, DBNull.Value);
                        if (Company != 0)
                            dbSmartAspects.AddInParameter(cmd, "@Company", DbType.Int32, Convert.ToInt32(Company));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Company", DbType.Int32, DBNull.Value);

                        if (Country != 0)
                            dbSmartAspects.AddInParameter(cmd, "@Country", DbType.Int32, Convert.ToInt32(Country));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Country", DbType.Int32, DBNull.Value);

                        if (State != 0)
                            dbSmartAspects.AddInParameter(cmd, "@State", DbType.Int32, Convert.ToInt32(State));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@State", DbType.Int32, DBNull.Value);
                        
                        if (BillClaimedFrom != 0)
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedFrom", DbType.Decimal, Convert.ToDecimal(BillClaimedFrom));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedFrom", DbType.Decimal, DBNull.Value);
                        if (BillClaimedTo != 0)
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedTo", DbType.Decimal, Convert.ToDecimal(BillClaimedTo));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedTo", DbType.Decimal, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SupportNTicketViewBO SupportNTicketView = new SupportNTicketViewBO();
                                    SupportNTicketView.Id = Convert.ToInt32(reader["Id"]);
                                    SupportNTicketView.SupportCategory = reader["SupportCategory"].ToString();
                                    SupportNTicketView.CompanyName = reader["CompanyName"].ToString();
                                    SupportNTicketView.BillingAddress = reader["BillingAddress"].ToString();
                                    SupportNTicketView.CityName = reader["CityName"].ToString();
                                    SupportNTicketView.CaseName = reader["CaseName"].ToString();
                                    SupportNTicketView.WarrantyType = reader["WarrantyType"].ToString();                                    
                                    SupportNTicketView.SupportType = reader["SupportType"].ToString();
                                    SupportNTicketView.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    SupportNTicketView.CaseNumber = reader["CaseNumber"].ToString();
                                    SupportNTicketView.SupportStatus = reader["SupportStatus"].ToString();
                                    SupportNTicketView.BillStatus = reader["BillStatus"].ToString();
                                    SupportNTicketView.FeedbackStatus = reader["FeedbackStatus"].ToString();
                                    SupportNTicketView.CreatedByName = reader["CreatedByName"].ToString();
                                    SupportNTicketView.CreatedDateDisplay = reader["CreatedDateDisplay"].ToString();
                                    SupportNTicketView.AssignedTo = reader["AssignedTo"].ToString();
                                    SupportNTicketView.CaseCloseByName = reader["CaseCloseByName"].ToString();
                                    SupportNTicketView.CaseCloseDateDisplay = reader["CaseCloseDateDisplay"].ToString();
                                    SupportNTicketView.PassDay = Convert.ToInt16(reader["PassDay"]);
                                    SupportNTicketViewList.Add(SupportNTicketView);
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
            return SupportNTicketViewList;
        }
        public List<SupportNTicketViewBO> GetSupportNTicketDetailsForReportFormat02(DateTime fromDate, DateTime toDate, string TicketNumber,
                                            int Case, int ItemCategory, int SupportCategory, int SupportType, int WarentyType,
                                            int Company, int Country, int State, string Status, decimal BillClaimedFrom, decimal BillClaimedTo)
        {
            List<SupportNTicketViewBO> SupportNTicketViewList = new List<SupportNTicketViewBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportNTicketDetailsForReportFormat02_SP"))
                    {

                        if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                        }

                        if (!string.IsNullOrEmpty(TicketNumber))
                            dbSmartAspects.AddInParameter(cmd, "@TicketNumber", DbType.String, Convert.ToString(TicketNumber));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TicketNumber", DbType.String, DBNull.Value);
                        if (!string.IsNullOrEmpty(Status))
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Convert.ToString(Status));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                        if (Case != 0)
                            dbSmartAspects.AddInParameter(cmd, "@Case", DbType.Int32, Convert.ToInt32(Case));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Case", DbType.Int32, DBNull.Value);
                        if (ItemCategory != 0)
                            dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.Int32, Convert.ToInt32(ItemCategory));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ItemCategory", DbType.Int32, DBNull.Value);
                        if (SupportCategory != 0)
                            dbSmartAspects.AddInParameter(cmd, "@SupportCategory", DbType.Int32, Convert.ToInt32(SupportCategory));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SupportCategory", DbType.Int32, DBNull.Value);
                        if (SupportType != 0)
                            dbSmartAspects.AddInParameter(cmd, "@SupportType", DbType.Int32, Convert.ToInt32(SupportType));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@SupportType", DbType.Int32, DBNull.Value);
                        if (WarentyType != 0)
                            dbSmartAspects.AddInParameter(cmd, "@WarentyType", DbType.Int32, Convert.ToInt32(WarentyType));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@WarentyType", DbType.Int32, DBNull.Value);
                        if (Company != 0)
                            dbSmartAspects.AddInParameter(cmd, "@Company", DbType.Int32, Convert.ToInt32(Company));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Company", DbType.Int32, DBNull.Value);

                        if (Country != 0)
                            dbSmartAspects.AddInParameter(cmd, "@Country", DbType.Int32, Convert.ToInt32(Country));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Country", DbType.Int32, DBNull.Value);

                        if (State != 0)
                            dbSmartAspects.AddInParameter(cmd, "@State", DbType.Int32, Convert.ToInt32(State));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@State", DbType.Int32, DBNull.Value);

                        if (BillClaimedFrom != 0)
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedFrom", DbType.Decimal, Convert.ToDecimal(BillClaimedFrom));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedFrom", DbType.Decimal, DBNull.Value);
                        if (BillClaimedTo != 0)
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedTo", DbType.Decimal, Convert.ToDecimal(BillClaimedTo));
                        else
                            dbSmartAspects.AddInParameter(cmd, "@BillClaimedTo", DbType.Decimal, DBNull.Value);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SupportNTicketViewBO SupportNTicketView = new SupportNTicketViewBO();
                                    SupportNTicketView.BranchCode = reader["BranchCode"].ToString();
                                    SupportNTicketView.CompanyName = reader["CompanyName"].ToString();
                                    SupportNTicketView.BillingAddress = reader["BillingAddress"].ToString();
                                    SupportNTicketView.CityName = reader["CityName"].ToString();
                                    SupportNTicketView.CreatedDateDisplay = reader["CreatedDateDisplay"].ToString();
                                    SupportNTicketView.CaseNumber = reader["CaseNumber"].ToString();
                                    SupportNTicketView.CaseName = reader["CaseName"].ToString();
                                    SupportNTicketView.CaseDeltails = reader["CaseDeltails"].ToString();
                                    SupportNTicketView.ItemName = reader["ItemName"].ToString();
                                    SupportNTicketView.UnitQuantity = Convert.ToDecimal(reader["UnitQuantity"]);
                                    SupportNTicketView.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    SupportNTicketView.LineTotal = Convert.ToDecimal(reader["LineTotal"]);
                                    SupportNTicketView.VatPercent = Convert.ToDecimal(reader["VatPercent"]);
                                    SupportNTicketView.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                    SupportNTicketView.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                                    SupportNTicketView.SupportStatus = reader["SupportStatus"].ToString();
                                    SupportNTicketView.BillStatus = reader["BillStatus"].ToString();
                                    SupportNTicketView.PassDay = Convert.ToInt16(reader["PassDay"]);                                    
                                    SupportNTicketViewList.Add(SupportNTicketView);
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
            return SupportNTicketViewList;
        }

    }
}
