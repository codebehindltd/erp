using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HouseKeeping
{
    public class LostFoundBO
    {
        public long Id { get; set; }
        public string TransectionType { get; set; }
        public int? TransectionId { get; set; }
        public string OtherArea { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string ItemType { get; set; }
        public DateTime? FoundDateTime { get; set; }
        public string FoundTime { get; set; }
        public string FoundDate { get; set; }
        public int? WhoFoundIt { get; set; }
        public string WhoFoundItName { get; set; }
        public string FoundPlace { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string WhomToReturn { get; set; }
        public string ReturnDescription { get; set; }
        public bool? HasItemReturned { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? Todate { get; set; }
    }
}
