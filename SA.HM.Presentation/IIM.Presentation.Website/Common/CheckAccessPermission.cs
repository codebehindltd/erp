using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Common
{
    public class CheckAccessPermission<T>
        where T : class
    {
        public PermissionBO Permission { get; set; }
        public string ActionLinks { get; set; }

        public CheckAccessPermission()
        {
            Permission = new PermissionBO();

            Permission.IsEdit = false;
            Permission.IsCancel = false;
            Permission.IsChecked = false;
            Permission.IsApproved = false;
            Permission.IsView = false;
        }

        public void CheckApprovedPermission(T data, UserInformationBO userInfo)
        {
            PropertyInfo pApprovedBy = data.GetType().GetProperty("ApprovedBy");
            PropertyInfo pCheckedBy = data.GetType().GetProperty("CheckedBy");
            PropertyInfo pApprovedStatus = data.GetType().GetProperty("ApprovedStatus");
            PropertyInfo pCreatedBy = data.GetType().GetProperty("CreatedBy");
            PropertyInfo pStatus = data.GetType().GetProperty("Status");

            int approvedBy = (int)pApprovedBy.GetValue(data, null);
            int checkedBy = (int)pCheckedBy.GetValue(data, null);
            int createdBy = (int)pCreatedBy.GetValue(data, null);
            bool status = (bool)pStatus.GetValue(data, null);

            string approvedStatus = (string)pApprovedStatus.GetValue(data, null);

            if (userInfo.UserInfoId == 1)
            {
                Permission.IsEdit = true;
                Permission.IsCancel = true;
                Permission.IsChecked = true;
                Permission.IsApproved = true;
                Permission.IsView = true;
            }
            else if (userInfo.UserInfoId == checkedBy)
            {
                if (approvedStatus == HMConstants.ApprovalStatus.Pending.ToString() && !status)
                {
                    Permission.IsEdit = true;
                    Permission.IsCancel = true;
                    Permission.IsChecked = true;
                    Permission.IsApproved = false;
                    Permission.IsView = true;
                }
                else if (approvedStatus == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    Permission.IsEdit = false;
                    Permission.IsCancel = false;
                    Permission.IsChecked = false;
                    Permission.IsApproved = false;
                    Permission.IsView = true;
                }
                else if ((approvedStatus == HMConstants.ApprovalStatus.Pending.ToString() && status) || (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString()))
                {
                    Permission.IsEdit = true;
                    Permission.IsCancel = true;
                    Permission.IsChecked = true;
                    Permission.IsApproved = false;
                    Permission.IsView = true;
                }
            }
            else if (userInfo.UserInfoId == approvedBy)
            {
                if (approvedStatus == HMConstants.ApprovalStatus.Pending.ToString() && !status)
                {
                    Permission.IsEdit = false;
                    Permission.IsCancel = false;
                    Permission.IsChecked = false;
                    Permission.IsApproved = false;
                    Permission.IsView = false;
                }
                else if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString() && !status)
                {
                    Permission.IsEdit = false;
                    Permission.IsCancel = false;
                    Permission.IsChecked = false;
                    Permission.IsApproved = false;
                    Permission.IsView = false;
                }
                else if ((approvedStatus == HMConstants.ApprovalStatus.Checked.ToString() && status) || (approvedStatus == HMConstants.ApprovalStatus.Approved.ToString()))
                {
                    Permission.IsEdit = true;
                    Permission.IsCancel = true;
                    Permission.IsChecked = true;
                    Permission.IsApproved = false;
                    Permission.IsView = true;
                }
            }
            else if (userInfo.UserInfoId == createdBy)
            {
                if (approvedStatus == HMConstants.ApprovalStatus.Pending.ToString())
                {
                    Permission.IsEdit = true;
                    Permission.IsCancel = true;
                    Permission.IsChecked = false;
                    Permission.IsApproved = false;
                    Permission.IsView = true;
                }
                else if (approvedStatus == HMConstants.ApprovalStatus.Checked.ToString() || approvedStatus == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    Permission.IsEdit = false;
                    Permission.IsCancel = false;
                    Permission.IsChecked = false;
                    Permission.IsApproved = false;
                    Permission.IsView = true;
                }
            }

        }
    }
}