using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace HotelManagement.Presentation.Website.Common
{
    public class ReturnInfo
    {
        public bool IsSuccess { get; set; }
        public object AlertMessage { get; set; }
        public object Data { get; set; }
        public ArrayList Arr { get; set; }
        public object Pk { get; set; }
        public ArrayList PKey { get; set; }
        public ArrayList ObjectList { get; set; }
        public ArrayList ObjectList1 { get; set; }
        public bool IsDirectPrint { get; set; }
        public int BillPrintAndPreview { get; set; }
        public int KotPrintAndPreview { get; set; }
        public string PrimaryKeyValue { get; set; }
        public string TransactionType { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionStatus { get; set; }
        public bool IsBillHoldUp { get; set; }
        public string RedirectUrl { get; set; }
        public string DataStr { get; set; }
        public bool IsReservationCheckInDateValidation { get; set; }
        public bool IsBillResettled { get; set; }
        public Int64? Id { get; set; }
    }
}