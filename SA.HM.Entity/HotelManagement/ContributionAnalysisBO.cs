using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ContributionAnalysisBO
    {
        public DateTime? SummaryDate { get; set; }
        public string ContributionType { get; set; }
        public long? ContributionTypeWiseId { get; set; }
        public string Name { get; set; }
        public decimal? TotalRoomSale { get; set; }
        public decimal? AverageRoomRate { get; set; }
        public decimal? OccupencyPercent { get; set; }
        public decimal? NoOfNight { get; set; }
        public decimal? Pax { get; set; }

    }
}
