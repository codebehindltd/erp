using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.Inventory
{
    public class PMSupplierProductReturnDetailsBO
    {
        public long ReturnDetailsId { get; set; }
        public long ReturnId { get; set; }
        public int StockById { get; set; }
        public int ItemId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int StyleId { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal Quantity { get; set; }
        public Nullable<decimal> ReturnQuantity { get; set; }
        public Nullable<decimal> AverageCost { get; set; }
        public string ItemName { get; set; }
        public string HeadName { get; set; }
        public string ProductType { get; set; }
        public int ReceivedId { get; set; }
        public int ReceiveDetailsId { get; set; }
        public int CostCenterId { get; set; }
        public int LocationId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime PODate { get; set; }
        public string PONumber { get; set; }
        public string ReceivedByName { get; set; }
        public string PurchedByName { get; set; }
        public string ReturnByName { get; set; }
        public string CheckedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string ReturnNumber { get; set; }
        public string ReceiveNumber { get; set; }        
        public string ReturnDate { get; set; }
        public string ReceivedDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string MessureUnit { get; set; }
        public decimal ReturnAmount { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string Serial { get; set; }
    }
}
