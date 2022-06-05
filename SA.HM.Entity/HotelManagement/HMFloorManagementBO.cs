using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class HMFloorManagementBO
    {
        public int FloorManagementId { get; set; }
        public int FloorId { get; set; }
        public int BlockId { get; set; }
        public int RoomId { get; set; }
        public string TypeCode { get; set; }        
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public decimal RoomRate { get; set; }
        public Boolean ActiveStat { get; set; }
        public string ActiveStatus { get; set; }
        public string CSSClassName { get; set; }
        public string ColorCodeName { get; set; }
        public decimal XCoordinate { get; set; }
        public decimal YCoordinate { get; set; }
        public decimal RoomWidth { get; set; }
        public decimal RoomHeight { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public int IsBillLockedAndPreview { get; set; }
    }
}
