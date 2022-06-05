using HotelManagement.Entity.Restaurant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Restaurant
{
    public class BillDetailsDA: BaseService
    {
        public List<BillDetailsBO> GetBillDetailsReportBySearchCriteria(DateTime fromDate, DateTime toDate, int costCenterIds,  int catagoryId, int itemId, string billNumber, int cashierId, string paymentType)
        {
            List<BillDetailsBO> billDetailsList = new List<BillDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBillDetailsReportBySearchCriteria_SP"))
                {
                    if (catagoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, catagoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }
                    if (costCenterIds > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterIds);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    }
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(billNumber))
                    {
                        if (billNumber == "All")
                        {
                            dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, DBNull.Value);
                        } 
                        else  
                            dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, billNumber); 
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BillNumber", DbType.String, DBNull.Value);
                    }
                    if (cashierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CashierInfoId", DbType.Int32, cashierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CashierInfoId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(paymentType))
                    {
                        if (paymentType == "All")
                        {
                            dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, DBNull.Value);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, paymentType);
                        }
                        
                    }  
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentType", DbType.String, DBNull.Value);

                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BillDetails");
                    DataTable Table = ds.Tables["BillDetails"];

                    billDetailsList = Table.AsEnumerable().Select(r => new BillDetailsBO
                    {
                        CategoryName = r.Field<string>("CategoryName"),
                        BillNumber = r.Field<string>("BillNumber"),
                        BillingSession = r.Field<string>("BillingSession"),
                        BillDate = r.Field<string>("BillDate"),
                        BillTime = r.Field<string>("BillTime"),
                        CostCenter = r.Field<string>("CostCenter"),
                        ItemName = r.Field<string>("ItemName"),
                        PaymentType = r.Field<string>("PaymentType"),
                        Quantity = r.Field<decimal?>("Quantity"),
                        Cashier = r.Field<string>("Cashier"),
                        RoundedGrandTotal = r.Field<decimal?>("RoundedGrandTotal"),
                        KotId = r.Field<int>("KotId"),
                        KotTime = r.Field<string>("KotTime"),
                        KotDetailId = r.Field<int>("KotDetailId"),
                        SettlementDetails = r.Field<string>("SettlementDetails"),
                        CostCenterId = r.Field<int?>("CostCenterId")

                    }).ToList();
                }
            }

                return billDetailsList;
        }
    }
}
