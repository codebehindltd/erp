using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
    public class SalesServiceViewBO
    {
        public string SubscriberName { get; set; }
        public string PostalOrPhysicalAddress { get; set; }
        public string ContactPersont1 { get; set; }
        public string ContactPersont2 { get; set; }
        public string TechnicalContactPerson { get; set; }
        public string ClientId { get; set; }
        public string ConnectionType { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public string ContactPersonInSite { get; set; }
        public string Category { get; set; }
        public int? ItemId { get; set; }
        public string BandwidthType { get; set; }
        public string Bandwidth { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? BillExpireDate { get; set; }
        public decimal? ItemUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? EquipmentBalance { get; set; }
        public int InstallationFee { get; set; }
        public string Remarks { get; set; }
    }
}
