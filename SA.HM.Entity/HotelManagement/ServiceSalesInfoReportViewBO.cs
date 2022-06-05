using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HotelManagement
{
    public class ServiceSalesInfoReportViewBO
    {
        public string ServiceDate { get; set; }
        public string ReferenceNo { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string RoomNumber { get; set; }        
        public decimal? DiscountAmount { get; set; }
        public decimal? ServiceRate { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? CitySDCharge { get; set; }
        public decimal? VatAmount { get; set; }        
        public decimal? AdditionalCharge { get; set; }
        public string SalesType { get; set; }
        public int? IsDiscountHead { get; set; }
        public List<ServiceSalesInfoReportViewBO> salesInfo1 = new List<ServiceSalesInfoReportViewBO>();
        public List<ServiceSalesInfoReportViewBO> salesInfo2 = new List<ServiceSalesInfoReportViewBO>();

        public string FOne_ID { get; set; }
        public string FOne_Branch_Code { get; set; }
        public string FOne_CustomerGroup { get; set; }
        public string FOne_Customer_Name { get; set; }
        public string FOne_Customer_Code { get; set; }
        public string FOne_Delivery_Address { get; set; }
        public DateTime FOne_Invoice_Date_Time { get; set; }
        public DateTime FOne_Delivery_Date_Time { get; set; }
        public string FOne_Reference_No { get; set; }
        public string FOne_Comments { get; set; }
        public string FOne_Sale_Type { get; set; }
        public string FOne_Previous_Invoice_No { get; set; }
        public string FOne_Is_Print { get; set; }
        public string FOne_Tender_Id { get; set; }
        public string FOne_Post { get; set; }
        public string FOne_LC_Number { get; set; }
        public string FOne_Currency_Code { get; set; }
        public string FOne_CommentsD { get; set; }
        public string FOne_Item_Code { get; set; }
        public string FOne_Item_Name { get; set; }
        public decimal? FOne_Quantity { get; set; }
        public decimal? FOne_NBR_Price { get; set; }
        public string FOne_UOM { get; set; }
        public decimal? FOne_VAT_Rate { get; set; }
        public decimal? FOne_SD_Rate { get; set; }
        public string FOne_Non_Stock { get; set; }
        public string FOne_Trading_MarkUp { get; set; }
        public string FOne_Type { get; set; }
        public decimal? FOne_Discount_Amount { get; set; }
        public decimal? FOne_Promotional_Quantity { get; set; }
        public string FOne_VAT_Name { get; set; }
        public decimal? FOne_SubTotal { get; set; }
        public string FOne_Vehicle_No { get; set; }
        public string FOne_ExpDescription { get; set; }
        public decimal? FOne_ExpQuantity { get; set; }
        public decimal? FOne_ExpGrossWeight { get; set; }
        public decimal? FOne_ExpNetWeight { get; set; }
        public string FOne_ExpNumberFrom { get; set; }
        public string FOne_ExpNumberTo { get; set; }
    }
}
