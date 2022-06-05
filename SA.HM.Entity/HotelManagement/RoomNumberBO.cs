using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity
{
    public class RoomNumberBO
    {
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string TypeCode { get; set; }
        public string RoomTypeOrCode { get; set; }
        public string RoomNumber { get; set; }
        public string RoomName { get; set; }
        public int StatusId { get; set; }
        public long HKRoomStatusId { get; set; }
        public string ActiveStatus { get; set; }
        public decimal RoomRate { get; set; }
        public decimal RoomRateUSD { get; set; }
        public Boolean IsSmokingRoom { get; set; }
        public string CleanupStatus { get; set; }
        public string CleanDate { get; set; }
        public string CleanTime { get; set; }
        public string LastCleanDate { get; set; }
        public DateTime LastCleanDate2 { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string RoomInformation { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string StatusName { get; set; }
        public string HKRoomStatusName { get; set; }
        public string CSSClassName { get; set; }
        public string ColorCodeName { get; set; }
        public Boolean IsRestaurantItemExist { get; set; }
        public string detailRowId { get; set; }
        public string GuestName { get; set; }
        public string FloorName { get; set; }
        public string BlockName { get; set; }
        public string FloorAndBlockName { get; set; }
        public long GroupByRowNo { get; set; }

        //----For Validation
        public int IndexId { get; set; }
        public int IsBillLockedAndPreview { get; set; }
        public long RegistrationId { get; set; }
        public Boolean IsLinkedRoom { get; set; }
    }
}
