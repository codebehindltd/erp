using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestHouseCheckOutBO
    {
        public int CheckOutId { get; set; }
        public string RoomNumber { get; set; } 
        public int RegistrationId { get; set; }
        public string BillNumber { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string PayMode { get; set; }
        public int AccountsPostingHeadId { get; set; }
        public int BankId { get; set; }
        public string BranchName { get; set; }
        public string ChecqueNumber { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public DateTime ?ExpireDate { get; set; }
        public string CardHolderName { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int BillPaidBy { get; set; }
        public string RebateRemarks { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string CheckOutProcessType { get; set; }
        public decimal Balance { get; set; }

        public string CardReference { get; set; }
        public int? DealId { get; set; }
        public bool? IsDayClosed { get; set; }

        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? LastModifiedDate { get; set; }
        public DateTime? CheckOutDateForSync { get; set; }

    }
}
