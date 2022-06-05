using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class PermissionBO
    {
        public bool IsEdit { get; set; }
        public bool IsCancel { get; set; }
        public bool IsChecked { get; set; }
        public bool IsApproved { get; set; }
        public bool IsView { get; set; }

        public PermissionBO()
        {
            IsEdit = false;
            IsCancel = false;
            IsChecked = false;
            IsApproved = false;
            IsView = false;
        }
    }
}
