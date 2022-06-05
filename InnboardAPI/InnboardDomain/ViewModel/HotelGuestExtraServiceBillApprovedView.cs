using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.ViewModel
{
    public class HotelGuestExtraServiceBillApprovedView: HotelGuestExtraServiceBillApproved
    {
        [NotMapped]
        public Guid? GuidId { get; set; }
    }
}
