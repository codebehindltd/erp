using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Banquet
{
    public class BanquetReservationBillGenerateReportBO
    {
        public long ReservationId { get; set; }
        public string ReservationNumber { get; set; }
        public int CompanyId { get; set; }
        public int NodeId { get; set; }
        public string Organization { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string BookingFor { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public Nullable<System.DateTime> ArriveDate { get; set; }
        public string ArriveTimeStamp { get; set; }
        public string ArriveDateStamp { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public string DepartureTimeStamp { get; set; }
        public string DepartureDateStamp { get; set; }
        public Nullable<long> BanquetId { get; set; }
        public string BanquetName { get; set; }
        public Nullable<long> OccessionTypeId { get; set; }
        public string OccessionName { get; set; }
        public Nullable<long> SeatingId { get; set; }
        public string SeatingName { get; set; }
        public int NumberOfPersonAdult { get; set; }
        public int NumberOfPersonChild { get; set; }
        public Nullable<long> RefferenceId { get; set; }
        public string RefferenceName { get; set; }
        public string CancellationReason { get; set; }
        public string SpecialInstructions { get; set; }
        public string Remarks { get; set; }
        public decimal BanquetRate { get; set; }
        public long DetailId { get; set; }
        public string ItemType { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionNumber { get; set; }
        public Nullable<long> ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CalculatedDiscountAmount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public Nullable<long> IsBanquetBillInclusive { get; set; }
        public decimal ServiceCharge { get; set; }
        public bool IsInvoiceServiceChargeEnable { get; set; }
        public decimal InvoiceServiceCharge { get; set; }
        public decimal CitySDCharge { get; set; }
        public bool IsInvoiceCitySDChargeEnable { get; set; }
        public decimal InvoiceCitySDCharge { get; set; }
        public decimal VatAmount { get; set; }
        public bool IsInvoiceVatAmountEnable { get; set; }
        public decimal InvoiceVatAmount { get; set; }
        public string AdditionalChargeType { get; set; }
        public decimal AdditionalCharge { get; set; }
        public bool IsInvoiceAdditionalChargeEnable { get; set; }
        public decimal InvoiceAdditionalCharge { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public int IsFNBPanelVisible { get; set; }
        public int IsRequisitesPanelVisible { get; set; }
        public int IsInnboardVatServiceChargeEnable { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal OthersBillTotal { get; set; }
        public string InnboardVatAmount { get; set; }
        public string InnboardServiceChargeAmount { get; set; }
    }
}
