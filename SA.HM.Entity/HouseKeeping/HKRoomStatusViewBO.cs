using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class HKRoomStatusViewBO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string ArriveDateString { get; set; }
        public string ExpectedCheckOutDateString { get; set; }
        public string RoomType { get; set; }
        public string FORoomStatus { get; set; }

        //Room Status
        public string OldHKRoomStatus { get; set; }
        public string HKRoomStatus { get; set; }
        public string Floor { get; set; }
        public string Description { get; set; }
        public DateTime LastCleanDate { get; set; }
        public string ShowLastCleanDate { get; set; }
        public string Remarks { get; set; }
        public string DateIn { get; set; }
        public string DateOut { get; set; }
        public DateTime ToDate { get; set; }
        public string ShowToDate { get; set; }

        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }

        //Room Conditions
        public long DailyRoomConditionId { get; set; }
        public string Features { get; set; }
        public string Conditions { get; set; }
        public List<HotelDailyRoomConditionBO> RoomConditionList { get; set; }

        //Room Discrepancies
        public long RoomDiscrepancyId { get; set; }
        public int? FOPersons { get; set; }
        public int? HKPersons { get; set; }
        public string DiscrepanciesDetails { get; set; }
        public string Reason { get; set; }

        //Task Assignment
        public string TaskDetails { get; set; }
        public long TaskId { get; set; }
        public long RoomTaskId { get; set; }
        public string Feedbacks { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }

        //Report
        public int TotalRoomNo { get; set; }
        public string ShowFeedbackTime { get; set; }
        public string GuestName { get; set; }

        public string ReservationId { get; set; }
        public string ReservationStatus { get; set; }

        public string DisplayName { get; set; }
        public string AssignedEmployees { get; set; }

        public HKRoomStatusViewBO()
        {
            RoomConditionList = new List<HotelDailyRoomConditionBO>();
        }
    }
}
