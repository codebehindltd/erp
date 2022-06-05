using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace HotelManagement.Entity.HMCommon
{
    public class ActivityTypeEnum
    {
        public enum ActivityType
        {
            [Description("Login")]
            Login,

            [Description("Logout")]
            Logout,

            [Description("View")]
            View,

            [Description("Edit")]
            Edit,

            [Description("Delete")]
            Delete,

            [Description("Add")]
            Add,

            [Description("Approved")]
            Approve,

            [Description("CheckOut")]
            CheckOut,

            [Description("Received After Out")]
            ReceivedAfterOut,

            [Description("Complete Requisition")]
            CompleteRequisition,

            [Description("Complete Transfer From Requisition")]
            CompleteRequisitionTransfer,

            [Description("Transfer")]
            Transfer

        }
    }
}
