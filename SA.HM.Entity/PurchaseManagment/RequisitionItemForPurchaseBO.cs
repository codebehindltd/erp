using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.PurchaseManagment
{
    public class RequisitionItemForPurchaseBO
    {
        public int RequisitionId { get; set; }
        public long RequisitionDetailsId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal RequisitionQuantity { get; set; }
        public decimal ApprovedQuantity { get; set; }
        public string UnitName { get; set; }
        public Nullable<decimal> UnitPriceLocal { get; set; }
        public Nullable<decimal> UnitPriceUsd { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public Nullable<System.DateTime> LastPurchaseDate { get; set; }


        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public string ColorText { get; set; }
        public string SizeText { get; set; }
        public string StyleText { get; set; }


        public string Remarks { get; set; }
        public int StockById { get; set; }
        public int POrderId { get; set; }
        public int PODetailsId { get; set; }
        public int? SupplierId { get; set; }
        public decimal ApprovedPOQuantity { get; set; }
        public decimal RemainingPOQuantity { get; set; }
    }
}
