using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetCancelationDA: BaseService
    {
        public List<BanquetReservationBO> GetCanceledReservationList(DateTime fromDate, DateTime toDate)
        {
            List<BanquetReservationBO> list = new List<BanquetReservationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCanceledReservationList_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CanceledReservations");
                    DataTable Table = ds.Tables["CanceledReservations"];

                    list = Table.AsEnumerable().Select(r => new BanquetReservationBO
                    {
                        Id = r.Field<long>("Id"),
                        BanquetId = r.Field<long>("BanquetId"),
                        ReservationNumber = r.Field<string>("ReservationNumber"),
                        CancellationReason = r.Field<string>("CancellationReason"),
                        BillVoidByName = r.Field<string>("BillVoidByName"),
                        BillVoidDateTime = r.Field<string>("BillVoidDateTime"),
                    }).ToList();                    
                }
            }
            return list;
        }
    }
}
