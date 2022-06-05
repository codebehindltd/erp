using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.DocumentManagement
{
    public class DMDocumentViewBO
    {
        public DMDocumentBO DocumentBO { get; set; }
        public List<DocumentsForDocManagementBO> DocumentsForDocList { get; set; }

    }
}
