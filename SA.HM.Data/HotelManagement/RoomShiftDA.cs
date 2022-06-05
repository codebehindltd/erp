using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomShiftDA: BaseService
    {
        public List<RoomShiftReportViewBO> GetRoomShiftInfo(DateTime fromDate, DateTime toDate)
        {
            List<RoomShiftReportViewBO> roomShift = new List<RoomShiftReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomShiftInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RoomShift");
                    DataTable Table = ds.Tables["RoomShift"];
                    roomShift = Table.AsEnumerable().Select(r =>
                                new RoomShiftReportViewBO
                                {
                                    FromDate = r.Field<DateTime>("FromDate"),
                                    ToDate = r.Field<DateTime>("ToDate"),
                                    ShiftDate = r.Field<string>("ShiftDate"),
                                    Remarks = r.Field<string>("Remarks"),
                                    RegistrationNumber = r.Field<string>("RegistrationNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    PreviousRoom = r.Field<string>("PreviousRoom"),
                                    ShiftedRoom = r.Field<string>("ShiftedRoom"),
                                    ShiftedBy = r.Field<string>("ShiftedBy")                                    
                                }).ToList();
                }
            }
            return roomShift;
        }
    }
}
