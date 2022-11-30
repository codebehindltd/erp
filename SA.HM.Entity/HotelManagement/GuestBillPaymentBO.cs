using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class GuestBillPaymentBO
    {
        public int PaymentId { get; set; }
        public string LedgerNumber { get; set; }

        public string ModuleName { get; set; }
        public int? ServiceBillId { get; set; }
        public string PaymentType { get; set; }
        public int RegistrationId { get; set; }
        public int RegistrationMergeId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int FieldId { get; set; }
        public int CurrencyTypeId { get; set; }
        public string CurrencyType { get; set; }
        public decimal ConvertionRate { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMode { get; set; }
        public int PaymentModeId { get; set; }
        public string TransactionType { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public decimal ReceiveAmount { get; set; }

        public string ChequeNumber { get; set; }
        public string ChecqueNumber { get; set; }
        public DateTime? ChecqueDate { get; set; }
        public int CardTypeId { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string CardReference { get; set; }

        public decimal ConversionRate { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public int LastModifiedBy { get; set; }
        public int DealId { get; set; }

        public Int64 NodeId { get; set; }
        public Int64 AccountsPostingHeadId { get; set; }
        public int RefundAccountHead { get; set; }
        public string Remarks { get; set; }

        public int BillPaidBy { get; set; }
        public int BillPaidByRoomNumber { get; set; }
        public string PaymentDescription { get; set; }
        public int RoomNumber { get; set; }

        public int PaidServiceId { get; set; }
        public Boolean IsUSDTransaction { get; set; }
        public Boolean IsSameBillWithPaidService { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public int BillId { get; set; }
        public string BillNumber { get; set; }
        public int LCBankAccountHeadId { get; set; }
        public string LCBankAccountHead { get; set; }
        public decimal DRAmount { get; set; }
        public decimal CRAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        public decimal? ServiceRate { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? CitySDCharge { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? AdditionalCharge { get; set; }
        public decimal? CurrencyExchangeRate { get; set; }
        public bool IsAdvancePayment { get; set; }
        public long ReservationId { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? LastModifiedDate { get; set; }

        public bool IsBillSettlement { get; set; }
        public bool IsBillEditable { get; set; }

    }
}
