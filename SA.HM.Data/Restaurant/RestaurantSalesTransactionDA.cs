using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantSalesTransactionDA: BaseService
    {
        public List<SalesTransactionReportViewBO> GetRestaurantTransactionInfoForReport(string reportType, string paymentMode, string CostCenterIdLst, DateTime serviceFromDate, DateTime serviceToDate, int receivedBy, string moduleName)
        {
            List<SalesTransactionReportViewBO> salesList = new List<SalesTransactionReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantSalesTransactionForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    if (reportType == "All")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    }

                    if (paymentMode == "All")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, paymentMode);
                    }

                    if (receivedBy == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, receivedBy);
                    }

                    if (!string.IsNullOrEmpty(CostCenterIdLst))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, CostCenterIdLst);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdList", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@ServiceFromDate", DbType.DateTime, serviceFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceToDate", DbType.DateTime, serviceToDate);                    
                    dbSmartAspects.AddInParameter(cmd, "@IndividualModuleName", DbType.String, moduleName);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RestaurantTransaction");
                    DataTable table = ds.Tables["RestaurantTransaction"];
                    salesList = table.AsEnumerable().Select(r =>
                                new SalesTransactionReportViewBO
                                {
                                    ServiceDate = r.Field<string>("ServiceDate"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    BillNumber = r.Field<string>("BillNumber"),
                                    PaymentDescription = r.Field<string>("PaymentDescription"),
                                    PaymentMode = r.Field<string>("PaymentMode"),
                                    POSTerminalBank = r.Field<string>("POSTerminalBank"),
                                    ReceivedAmount = r.Field<decimal>("ReceivedAmount"),
                                    PaidAmount = r.Field<decimal>("PaidAmount"),
                                    OperatedBy = r.Field<string>("OperatedBy")                                    
                                }).ToList();
                }
            }
            return salesList;
        }
    }
}
