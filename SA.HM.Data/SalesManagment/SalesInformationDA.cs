using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesManagment
{
    public class SalesInformationDA: BaseService
    {
        public List<DateWiseSalesReportViewBO> GetSalesInfoForDateWiseSalesReport(DateTime fromDate, DateTime toDate, int itemId, int customerId)
        {
            List<DateWiseSalesReportViewBO> empList = new List<DateWiseSalesReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesInformationForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@CustomerId", DbType.Int32, customerId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMSales");
                    DataTable Table = ds.Tables["PMSales"];

                    empList = Table.AsEnumerable().Select(r => new DateWiseSalesReportViewBO
                    {
                        SalesId = r.Field<int>("SalesId"),
                        SalesDate = r.Field<string>("SalesDate"),
                        CustomerId = r.Field<int>("CustomerId"),
                        CustomerName = r.Field<string>("CustomerName"),
                        CustomerCode = r.Field<string>("CustomerCode"),
                        SalesAmount = r.Field<decimal>("SalesAmount"),
                        VatAmount = r.Field<decimal>("VatAmount"),
                        GrandTotal = r.Field<decimal>("GrandTotal"),
                        DetailId = r.Field<int>("DetailId"),
                        ServiceType = r.Field<string>("ServiceType"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        FieldValue1 = r.Field<string>("FieldValue1"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        FieldValue2 = r.Field<string>("FieldValue2")                        
                    }).ToList();
                }
            }

            return empList;
        }

        public List<SalesReturnReportViewBO> GetSalesReturnInfoForReport(DateTime fromDate, DateTime toDate, string returnType)
        {
            List<SalesReturnReportViewBO> empList = new List<SalesReturnReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReturnInformationForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ReturnType", DbType.String, returnType);
                    

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReturn");
                    DataTable Table = ds.Tables["PMProductReturn"];

                    empList = Table.AsEnumerable().Select(r => new SalesReturnReportViewBO
                    {
                        ReturnId = r.Field<int>("ReturnId"),
                        ReturnDate = r.Field<string>("ReturnDate"),
                        ReturnType = r.Field<string>("ReturnType"),
                        TransactionId = r.Field<int>("TransactionId"),
                        InvoiceNumber = r.Field<string>("InvoiceNumber"),
                        ProductId = r.Field<int>("ProductId"),
                        ProductName = r.Field<string>("ProductName"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        Quantity = r.Field<decimal>("Quantity"),
                        Remarks = r.Field<string>("Remarks")                       
                    }).ToList();
                }
            }

            return empList;
        }
    }
}
