using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.SalesAndMarketing;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class AccountManagerSalesTargetDA : BaseService
    {
        public bool SaveAccountManagerSalesTarget(AccountManagerSalesTargetBO Budget, List<AccountManagerSalesTargetDetailsBO> BudgetDetails, out Int64 targetId)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveAccountManagerSalesTarget_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, Budget.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, Budget.ProjectId);
                            dbSmartAspects.AddInParameter(commandMaster, "@FiscalYearId", DbType.Int32, Budget.FiscalYearId);

                            if (Budget.CheckedBy != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, Budget.CheckedBy);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, DBNull.Value);


                            if (Budget.ApprovedBy != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, Budget.ApprovedBy);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, Budget.ApprovedStatus);

                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int64, Budget.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandMaster, "@TargetId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            targetId = Convert.ToInt64(commandMaster.Parameters["@TargetId"].Value.ToString());
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveAccountManagerSalesTargetDetails_SP"))
                            {
                                foreach (AccountManagerSalesTargetDetailsBO DetailBO in BudgetDetails)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@TargetId", DbType.Int64, targetId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@MonthId", DbType.Int64, DetailBO.MonthId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@AccountManagerId", DbType.Int32, DetailBO.AccountManagerId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, DetailBO.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateAccountManagerSalesTarget(AccountManagerSalesTargetBO Budget, List<AccountManagerSalesTargetDetailsBO> BudgetDetailsNew, List<AccountManagerSalesTargetDetailsBO> BudgetDetailsEdited)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateAccountManagerSalesTarget_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@TargetId", DbType.Int32, Budget.TargetId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, Budget.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, Budget.ProjectId);
                            dbSmartAspects.AddInParameter(commandMaster, "@FiscalYearId", DbType.Int32, Budget.FiscalYearId);

                            if (Budget.CheckedBy != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, Budget.CheckedBy);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, DBNull.Value);


                            if (Budget.ApprovedBy != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, Budget.ApprovedBy);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, Budget.ApprovedStatus);

                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int64, Budget.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        }

                        if (status > 0 && BudgetDetailsNew.Count > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveAccountManagerSalesTargetDetails_SP"))
                            {
                                foreach (AccountManagerSalesTargetDetailsBO DetailBO in BudgetDetailsNew)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@TargetId", DbType.Int64, Budget.TargetId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@MonthId", DbType.Int64, DetailBO.MonthId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@AccountManagerId", DbType.Int32, DetailBO.AccountManagerId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, DetailBO.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && BudgetDetailsEdited.Count > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateAccountManagerSalesTargetDetails_SP"))
                            {
                                foreach (AccountManagerSalesTargetDetailsBO DetailBO in BudgetDetailsEdited)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@TargetDetailsId", DbType.Int64, DetailBO.TargetDetailsId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@TargetId", DbType.Int64, Budget.TargetId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@MonthId", DbType.Int64, DetailBO.MonthId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@AccountManagerId", DbType.Int32, DetailBO.AccountManagerId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, DetailBO.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public AccountManagerSalesTargetBO GetSalesTargetByCompanyIdNProjectIdNFiscalYearId(int glCompanyId, int glProjectId, Int64 fiscalYearId)
        {
            AccountManagerSalesTargetBO budget = new AccountManagerSalesTargetBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesTargetByCompanyIdNProjectIdNFiscalYearId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, glCompanyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, glProjectId);
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesTarget");
                    DataTable Table = ds.Tables["SalesTarget"];

                    budget = Table.AsEnumerable().Select(r => new AccountManagerSalesTargetBO
                    {
                        TargetId = r.Field<Int64>("TargetId"),
                        FiscalYearId = r.Field<Int64>("FiscalYearId"),
                        CheckedBy = r.Field<Int64?>("CheckedBy"),
                        ApprovedBy = r.Field<Int64?>("ApprovedBy"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).FirstOrDefault();
                }
            }
            return budget;
        }
        public List<AccountManagerSalesTargetDetailsBO> GetSalesTargetDetailsByFiscalYearId(int glCompanyId, int glProjectId, Int64 fiscalYearId)
        {
            List<AccountManagerSalesTargetDetailsBO> budgetDetails = new List<AccountManagerSalesTargetDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesTargetDetailsByFiscalYearId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, glCompanyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, glProjectId);
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SalesTarget");
                    DataTable Table = ds.Tables["SalesTarget"];

                    budgetDetails = Table.AsEnumerable().Select(r => new AccountManagerSalesTargetDetailsBO
                    {
                        TargetDetailsId = r.Field<Int64>("TargetDetailsId"),
                        TargetId = r.Field<Int64>("TargetId"),
                        MonthId = r.Field<Int16>("MonthId"),
                        AccountManagerId = r.Field<Int32>("AccountManagerId"),
                        Amount = r.Field<decimal>("Amount"),
                        AccountManager = r.Field<string>("AccountManager"),

                    }).ToList();
                }
            }
            return budgetDetails;
        }
        //public GLBudgetBO GetBudgetByFiscalYearAndStatus(Int64 fiscalYearId, string approvedStatus)
        //{
        //    GLBudgetBO budget = new GLBudgetBO();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBudgetByFiscalYearAndStatus_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);
        //            dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approvedStatus);

        //            DataSet ds = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, ds, "Budget");
        //            DataTable Table = ds.Tables["Budget"];

        //            budget = Table.AsEnumerable().Select(r => new GLBudgetBO
        //            {
        //                BudgetId = r.Field<Int64>("BudgetId"),
        //                FiscalYearId = r.Field<Int64>("FiscalYearId"),
        //                CheckedBy = r.Field<Int64?>("CheckedBy"),
        //                ApprovedBy = r.Field<Int64?>("ApprovedBy"),
        //                ApprovedStatus = r.Field<string>("ApprovedStatus"),
        //                FiscalYearName = r.Field<string>("FiscalYearName"),

        //            }).FirstOrDefault();
        //        }
        //    }
        //    return budget;
        //}
        //public List<GLBudgetBO> GetBudgetBySearch(int glCompanyId, int glProjectId, Int64 fiscalYearId, string approvedStatus, int recordPerPage, int pageIndex, out int totalRecords)
        //{
        //    List<GLBudgetBO> budget = new List<GLBudgetBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBudgetBySearch_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, glCompanyId);
        //            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, glProjectId);
        //            dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);
        //            dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approvedStatus);

        //            dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
        //            dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
        //            dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

        //            DataSet ds = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, ds, "Budget");
        //            DataTable Table = ds.Tables["Budget"];

        //            budget = Table.AsEnumerable().Select(r => new GLBudgetBO
        //            {
        //                BudgetId = r.Field<Int64>("BudgetId"),
        //                FiscalYearId = r.Field<Int64>("FiscalYearId"),
        //                CheckedBy = r.Field<Int64?>("CheckedBy"),
        //                ApprovedBy = r.Field<Int64?>("ApprovedBy"),
        //                ApprovedStatus = r.Field<string>("ApprovedStatus"),
        //                FiscalYearName = r.Field<string>("FiscalYearName"),

        //            }).ToList();

        //            totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
        //        }
        //    }
        //    return budget;
        //}
        //public List<GLBudgetDetailsReportBO> GetBudgetDetailsForReportByFiscalYearId(int glCompanyId, int glProjectId, Int64 fiscalYearId)
        //{
        //    List<GLBudgetDetailsReportBO> budgetDetails = new List<GLBudgetDetailsReportBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBudgetDetailsByFiscalYearId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, glCompanyId);
        //            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, glProjectId);
        //            dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);

        //            DataSet ds = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, ds, "Budget");
        //            DataTable Table = ds.Tables["Budget"];

        //            budgetDetails = Table.AsEnumerable().Select(r => new GLBudgetDetailsReportBO
        //            {
        //                NodeNumber = r.Field<string>("NodeNumber"),
        //                NodeType = r.Field<string>("NodeType"),
        //                GroupNodeNumber = r.Field<string>("GroupNodeNumber"),
        //                GroupNodeHead = r.Field<string>("GroupNodeHead"),
        //                NodeLevel = r.Field<int>("NodeLevel"),
        //                NodeOrder = r.Field<int>("NodeOrder"),
        //                GroupOrder = r.Field<int>("GroupOrder"),
        //                GroupLevel = r.Field<int>("GroupLevel"),
        //                GroupId = r.Field<Int64?>("GroupId"),

        //                BudgetDetailsId = r.Field<Int64>("BudgetDetailsId"),
        //                BudgetId = r.Field<Int64>("BudgetId"),
        //                MonthId = r.Field<Int16>("MonthId"),
        //                NodeId = r.Field<Int64>("NodeId"),
        //                Amount = r.Field<decimal>("Amount"),
        //                NodeHead = r.Field<string>("NodeHead")

        //            }).ToList();
        //        }
        //    }
        //    return budgetDetails;
        //}
        //public List<GLBudgetDetailsReportBO> GetBudgetDetailsForReportByBudgetId( Int64 budgetId)
        //{
        //    List<GLBudgetDetailsReportBO> budgetDetails = new List<GLBudgetDetailsReportBO>();

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBudgetDetailsByBudgetId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@BudgetId", DbType.Int64, budgetId);

        //            DataSet ds = new DataSet();
        //            dbSmartAspects.LoadDataSet(cmd, ds, "Budget");
        //            DataTable Table = ds.Tables["Budget"];

        //            budgetDetails = Table.AsEnumerable().Select(r => new GLBudgetDetailsReportBO
        //            {
        //                NodeNumber = r.Field<string>("NodeNumber"),
        //                NodeType = r.Field<string>("NodeType"),
        //                GroupNodeNumber = r.Field<string>("GroupNodeNumber"),
        //                GroupNodeHead = r.Field<string>("GroupNodeHead"),
        //                NodeLevel = r.Field<int>("NodeLevel"),
        //                NodeOrder = r.Field<int>("NodeOrder"),
        //                GroupOrder = r.Field<int>("GroupOrder"),
        //                GroupLevel = r.Field<int>("GroupLevel"),
        //                GroupId = r.Field<Int64?>("GroupId"),

        //                BudgetDetailsId = r.Field<Int64>("BudgetDetailsId"),
        //                BudgetId = r.Field<Int64>("BudgetId"),
        //                MonthId = r.Field<Int16>("MonthId"),
        //                NodeId = r.Field<Int64>("NodeId"),
        //                Amount = r.Field<decimal>("Amount"),
        //                NodeHead = r.Field<string>("NodeHead")

        //            }).ToList();
        //        }
        //    }
        //    return budgetDetails;
        //}
        //public Boolean BudgetApproval(GLBudgetBO budgetApproval)
        //{
        //    bool retVal = false;
        //    int status = 0;

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();
        //        using (DbTransaction transction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ApprovedBudget_SP"))
        //                {
        //                    dbSmartAspects.AddInParameter(commandMaster, "@BudgetId", DbType.Int64, budgetApproval.BudgetId);
        //                    dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, budgetApproval.ApprovedStatus);
        //                    dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int64, budgetApproval.ApprovedBy);

        //                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
        //                }

        //                if (status > 0)
        //                {
        //                    transction.Commit();
        //                    retVal = true;
        //                }
        //                else
        //                {
        //                    transction.Rollback();
        //                    retVal = false;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transction.Rollback();
        //                retVal = false;
        //                throw ex;
        //            }
        //        }
        //    }
        //    return retVal;
        //}
    }
}
