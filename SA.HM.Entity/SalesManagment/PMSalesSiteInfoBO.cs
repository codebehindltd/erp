using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.SalesManagment
{
  public  class PMSalesSiteInfoBO
  {
      public int SiteInfoId { get; set; }
      public int SelectedSiteInfoId { get; set; }
      public string SiteId { get; set; }

      public int CustomerId { get; set; }
      public string SiteName { get; set; }
      public string SiteAddress { get; set; }

      public string SiteContactPerson { get; set; }
      public string SitePhoneNumber { get; set; }
      public string SiteEmail { get; set; }

      public int CreatedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public int LastModifiedBy { get; set; }
      public DateTime LastModifiedDate { get; set; }

  }
}
