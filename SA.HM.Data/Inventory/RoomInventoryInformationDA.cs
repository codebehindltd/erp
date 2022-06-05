using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class RoomInventoryInformationDA : BaseService
    {
        public List<RoomInvInfoReportViewBO> GetRoomInventoryInfo(DateTime fromDate, DateTime toDate, int itemId, int roomId)
        {
            List<RoomInvInfoReportViewBO> roomInvInfoList = new List<RoomInvInfoReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRoomInventoryInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.Int32, roomId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    roomInvInfoList = Table.AsEnumerable().Select(r => new RoomInvInfoReportViewBO
                    {
                        InventoryId = r.Field<int>("InventoryId"),
                        TransectionDate = r.Field<DateTime>("TransectionDate"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemCategory = r.Field<string>("ItemCategory"),
                        Quantity = r.Field<decimal>("Quantity"),
                        RoomId = r.Field<int>("RoomId"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        RoomType = r.Field<string>("RoomType"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();
                }
            }

            return roomInvInfoList;
        }
    }
}
