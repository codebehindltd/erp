using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public static class AlertMessage
    {
        public const string Success = "Operation Successfull.";
        public const string Save = "Saved Operation Successfull.";
        public const string TaskCreate = "Task Created Operation Successfull.";
        public const string HoldUp = "HoldUp Operation Successfull.";
        public const string Update = "Update Operation Successfull.";
        public const string Delete = "Delete Operation Successfull.";
        public const string Error = "Here Something Wrong. Please Contact With Admin.";
        public const string Cancel = "Cancel Operation Successfull";
        public const string Approved = "Approved Operation Successfull";
        public const string Checked = "Checked Operation Successfull";
        public const string Received = "Item Received Operation Successfull";
        public const string SucceesfulLogin = "Login Operation Successfull";
        public const string ErrorFulLogin = "Login Operation Un-Successfull";
        public const string ReservationCancel = "Reservation Cancel Successfull.";
        public const string ReservationActive = "Reservation Active Successfull.";
        public const string BillReSettlement = "Bill Re-settlement Successfull";
        public const string BillSettlement = "Bill Settlement Successfull";
        public const string BillGenerate = "Bill Generate Successfull";
        public const string CashAdjustment = "Cash Adjustment Successfull";
        //Added by Arif [06-11-2017]
        public const string Duplicate = "The Entry is Duplicate!";
        public const string BillRefund = "Bill Fully Refund Successful.";
        public const string Delivery = "Delivery Process Successful.";
        public const string MessageSent = "Message Sent Successfully.";

        public const string MaxRefundAmount = "Maximum refund amount is";
        public const string TextTypeValidation = "Please Provide ";
        public const string DropDownValidation = "Please Select ";
        public const string DuplicateValidation = " Already Exists.";
        public const string FormatValidation = " is not in correct format.";
        public const string ForeignKeyValidation = "Cannot Delete. Has relation to other data. Please delete related first then try.";
        public const string Transfer = "Transfer Operation Successfull.";

    }
}