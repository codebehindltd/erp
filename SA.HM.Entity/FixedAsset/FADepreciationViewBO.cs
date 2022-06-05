using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.FixedAsset
{
    public class FADepreciationViewBO
    {
        public List<FADepreciationBO> DepreciationBOList { get; set; }
        public List<FADepreciationDetailsBO> DepreciationDetailsBOList { get; set; }
        public FADepreciationBO DepreciationBO { get; set; }
        public FADepreciationDetailsBO DepreciationDetailsBO { get; set; }
        public string DepreciationTableString { get; set; }
    }
}
