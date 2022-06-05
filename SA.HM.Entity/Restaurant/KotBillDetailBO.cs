using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Restaurant
{
    public class KotBillDetailBO
    {
        public int KotDetailId { get; set; }
        public DateTime BillDate { get; set; }
        public int KotId { get; set; }
        public String ItemType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Remarks { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public int BagQuantity { get; set; }
        public decimal kgQuantity { get; set; }
        public decimal ItemUnit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal Amount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalVatAmount { get; set; }
        public int KitchenId { get; set; }
        public string PrinterName { get; set; }
        public Boolean PrintFlag { get; set; }
        public Boolean IsChanged { get; set; }
        public Boolean IsDispatch { get; set; }
        public decimal ItemWiseDiscount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal ServiceRate { get; set; }
        public bool IsDeleted { get; set; }
        public decimal ItemCost { get; set; }
        public decimal ItemLineTotal { get; set; }
        public int NewItemCount { get; set; }
        public int VoidOrDeletedItemCount { get; set; }
        public int EditedOrChangedItemCount { get; set; }
        public int EmpId { get; set; }
        public string ItemWiseDiscountType { get; set; }
        public decimal ItemWiseIndividualDiscount { get; set; }
        public int ClassificationId { get; set; }
        public string DeliveryStatus { get; set; }
        public int PaxQuantity { get; set; }
        public bool IsItemEditable { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public string LastModifiedUser { get; set; }
        public int StockBy { get; set; }
        public string UnitHead { get; set; }

        public bool IsBillPreviewButtonEnable { get; set; }

        public decimal CitySDCharge { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal InvoiceDiscount { get; set; }
    }
}
