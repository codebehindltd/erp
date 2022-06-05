using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class InhouseGuestLedgerBO
    {
        public string HMCompanyProfile { get; set; }
        public string HMCompanyAddress { get; set; }
        public string HMCompanyWeb { get; set; }

        public int RegistrationId { get; set; }

        public int RoomNumber { get; set; }

        public string Service1 { get; set; }
        public decimal Service1Amount { get; set; }

        public string Service2 { get; set; }
        public decimal Service2Amount { get; set; }

        public string Service3 { get; set; }
        public decimal Service3Amount { get; set; }

        public string Service4 { get; set; }
        public decimal Service4Amount { get; set; }

        public string Service5 { get; set; }
        public decimal Service5Amount { get; set; }

        public string Service6 { get; set; }
        public decimal Service6Amount { get; set; }

        public string Service7 { get; set; }
        public decimal Service7Amount { get; set; }

        public string Service8 { get; set; }
        public decimal Service8Amount { get; set; }

        public string Service9 { get; set; }
        public decimal Service9Amount { get; set; }

        public string Service10 { get; set; }
        public decimal Service10Amount { get; set; }

        public string Service11 { get; set; }
        public decimal Service11Amount { get; set; }

        public string Service12 { get; set; }
        public decimal Service12Amount { get; set; }

        public string Service13 { get; set; }
        public decimal Service13Amount { get; set; }

        public string Service14 { get; set; }
        public decimal Service14Amount { get; set; }

        public string Service15 { get; set; }
        public decimal Service15Amount { get; set; }

        public string Service16 { get; set; }
        public decimal Service16Amount { get; set; }

        public string Service17 { get; set; }
        public decimal Service17Amount { get; set; }

        public string Service18 { get; set; }
        public decimal Service18Amount { get; set; }

        public string Service19 { get; set; }
        public decimal Service19Amount { get; set; }

        public string Service20 { get; set; }
        public decimal Service20Amount { get; set; }

        public string Service21 { get; set; }
        public decimal Service21Amount { get; set; }

        public string Service22 { get; set; }
        public decimal Service22Amount { get; set; }

        public string Service23 { get; set; }
        public decimal Service23Amount { get; set; }

        public string Service24 { get; set; }
        public decimal Service24Amount { get; set; }

        public string Service25 { get; set; }
        public decimal Service25Amount { get; set; }

        public string Service26 { get; set; }
        public decimal Service26Amount { get; set; }

        public decimal AdvanceCashOut { get; set; }
        public decimal TodaysBillAmount { get; set; }
        public decimal PreviousDueAmount { get; set; }
        public decimal BalanceAmount { get; set; }

        public DateTime PrintDate { get; set; }        
        public string GuestName { get; set; }
        public string CompanyName { get; set; }
        public string GuestReference { get; set; }        
        public string Pay { get; set; }
        public string Araival { get; set; }
        public string Departure { get; set; }
        public decimal RoomRent { get; set; }
        public decimal Amount { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public int? SatyedNights { get; set; }
        public DateTime InhouseGuestLedgerDate { get; set; }

        public int TotalRoom { get; set; }
        public int TotalPerson { get; set; }
    }
}
