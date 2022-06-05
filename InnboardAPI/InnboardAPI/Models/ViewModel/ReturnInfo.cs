using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnboardAPI.Models
{
    public class ReturnInfo
    {
        public bool IsSuccess { get; set; }
        public object AlertMessage { get; set; }
        public object Data { get; set; }
        public ArrayList Arr { get; set; }
        public object Pk { get; set; }
        public ArrayList PKey { get; set; }
        public bool IsDirectPrint { get; set; }
        public int BillPrintAndPreview { get; set; }
        public int KotPrintAndPreview { get; set; }
        public bool IsBillHoldUp { get; set; }
        public string RedirectUrl { get; set; }
        public string DataStr { get; set; }
        public bool IsReservationCheckInDateValidation { get; set; }
    }
}