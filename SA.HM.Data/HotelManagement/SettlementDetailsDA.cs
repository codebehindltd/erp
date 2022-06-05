using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class SettlementDetailsDA:BaseService
    {
        public List<SettlementDetailsReportViewBO> GetSettlementDetailsForReport(string filterType,DateTime searchDate)
        {
            List<SettlementDetailsReportViewBO> detailsList = new List<SettlementDetailsReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSettlementDetailsInfomationForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    dbSmartAspects.AddInParameter(cmd, "@FilterBy", DbType.String, filterType);
                    dbSmartAspects.AddInParameter(cmd, "@SearchDate", DbType.DateTime, searchDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomInfo");
                    DataTable Table = ds.Tables["RoomInfo"];
                    detailsList = Table.AsEnumerable().Select(r =>
                                new SettlementDetailsReportViewBO
                                {
                                    PaymentMode = r.Field<string>("PaymentMode"),
                                    Description = r.Field<string>("Description"),
                                    ColumnGroupId = r.Field<int>("ColumnGroupId"),
                                    ColumnGroup = r.Field<string>("ColumnGroup"),
                                    SubGroupId = r.Field<int>("SubGroupId"),
                                    SubGroup = r.Field<string>("SubGroup"),
                                    QtNValue = r.Field<decimal?>("QtNValue")

                                }).ToList();
                }
            }
            return detailsList;
        }
    }
}
