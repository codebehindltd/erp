﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
   public class BanquetOccessionTypeBO
   {
       public long Id { get; set; }
       public string Name { get; set; }
       public string Code { get; set; }
       public string Description { get; set; }

       public long CreatedBy { get; set; }
       public long LastModifiedBy { get; set; }

   }
}
