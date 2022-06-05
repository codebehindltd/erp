using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
   public class GuestDocumentsBO
    {
        public int DocumentsId { get; set; }
        public int GuestId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
