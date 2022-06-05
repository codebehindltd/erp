using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.Banquet
{
    public class RestaurantSalesTransactionDA: BaseService
    {
        public List<SalesTransactionReportViewBO> GetBanquetTransactionInfoForReport( string paymentMode, string serviceIdList, DateTime serviceFromDate, DateTime serviceToDate, int receivedBy, string moduleName)
        {
            List<SalesTransactionReportViewBO> salesList = new List<SalesTransactionReportViewBO>();
            //DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetSalesTransactionForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    
                    if (paymentMode == "All")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PaymentMode", DbType.String, paymentMode);
                    }
                    if (serviceIdList == "-1")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ServiceIdList", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ServiceIdList", DbType.String, serviceIdList);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@ServiceFromDate", DbType.DateTime, serviceFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceToDate", DbType.DateTime, serviceToDate);
                    if (receivedBy == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceivedBy", DbType.Int32, receivedBy);
                    }
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
