using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.GeneralLedger
{
    public class GLNotesConfigurationBO
    {
        public int ConfigurationId { get; set; }
        public string ConfigurationType { get; set; }
        public string NotesNumber { get; set; }
        public int NodeId { get; set; }
    }
}
